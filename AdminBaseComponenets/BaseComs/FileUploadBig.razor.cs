using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;


namespace AdminBaseComponenets.BaseComs
{
    
    public partial class FileUploadBig : ValueInput<string> 
    {


        
        MarkupString AlertMessage = new MarkupString("<strong>No file selected</strong>");
    string AlertClass = "alert alert-info";
    int ProgressPercentage = 0;
    IBrowserFile selectedFile = null;
    long maxFileSize = 1024L * 1024L * 1024L * 2L;
    string[] allowedExtensions = { ".zip", ".rar", ".png", ".jpg", ".mp3" };
    bool IsUploadDisabled = true;
    private Guid inputFileId = Guid.NewGuid();
    protected void setNull(){
            this.value=null;
            OnChange(value);
    }
    private void OnInputFileChange(InputFileChangeEventArgs e)
    {
        selectedFile = e.GetMultipleFiles()[0];
        ProgressPercentage = 0;
        IsUploadDisabled = true;

        if (selectedFile.Size > maxFileSize)
        {
            SetAlert("alert alert-danger", "oi oi-ban", $"File size exceeds the limit. Maximum allowed size is <strong>{maxFileSize / (1024 * 1024)} MB</strong>.");
            return;
        }

        if (!allowedExtensions.Contains(Path.GetExtension(selectedFile.Name).ToLowerInvariant()))
        {
            SetAlert("alert alert-danger", "oi oi-warning", $"Invalid file type. Allowed file types are <strong>{string.Join(", ", allowedExtensions)}</strong>.");
            return;
        }

        SetAlert("alert alert-info", "oi oi-info", $"<strong>{selectedFile.Name}</strong> ({selectedFile.Size} bytes) file selected.");
        IsUploadDisabled = false;
    }

    private async void OnSubmit()
    {
        if (selectedFile != null)
        {
            IsUploadDisabled = true;
            Stream stream = selectedFile.OpenReadStream(maxFileSize);
            //var path = $"env.WebRootPath/{selectedFile.Name}";
            //using FileStream fs = File.Create(path);

            // Set buffer size to 512 KB.
            int bufferSize = 512 * 1024;
            byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(bufferSize);
            int bytesRead;
            long totalBytesRead = 0;
            long fileSize = selectedFile.Size;

            // Use a timer to update the UI every few hundred milliseconds.
            using var timer = new Timer(_ => InvokeAsync(() => StateHasChanged()));
            timer.Change(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));

            try
            {
                var oldBase = ClTool.WebClient.webClient;
                //await oldBase.fetch("", null, HttpMethod.Get);
                var tmp = await oldBase.fetch<Models.CreateSessionParams, Models.SessionCreationStatusResponse>("api/file/create", HttpMethod.Post, new Models.CreateSessionParams()
                    {
                        FileName = selectedFile.Name,
                        ChunkSize = 1000,
                        TotalSize = 10000
                    });
                value = tmp.FileName;
                OnChange(value);



                //OnChange(value);

                while ((bytesRead = await stream.ReadAsync(buffer)) != 0)
                {
                    totalBytesRead += bytesRead;
                    ProgressPercentage = (int)(100 * totalBytesRead / fileSize);
                    //await fs.WriteAsync(buffer, 0, bytesRead);
                    await oldBase.uploadFileSection("api/file/upload", tmp.SessionId, 1, buffer);
                }
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
            }

            // Stop the timer and update the UI with the final progress.
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            ProgressPercentage = 100;
            SetAlert("alert alert-success", "oi oi-check", $"<strong>{selectedFile.Name}</strong> ({selectedFile.Size} bytes) file uploaded on server.");
            inputFileId = Guid.NewGuid();
            this.StateHasChanged();
        }
    }

    private void SetAlert(string alertClass, string iconClass, string message)
    {
        AlertClass = alertClass;
        AlertMessage = new MarkupString($"<span class='{iconClass}' aria-hidden='true'></span> {message}");
    }

    }


}