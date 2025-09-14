using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
using System.IO;
using System.Threading;
using Blazorise;

namespace AdminBaseComponenets.BaseComs
{
    public partial class FileUploadBig : ValueInput<string> 
    {

        
        // Modal state for FileBrowser
        
    
        MarkupString AlertMessage = new MarkupString("<strong>No file selected</strong>");
        string AlertClass = "alert alert-info";
        int ProgressPercentage = 0;
        IBrowserFile selectedFile = null;
    
        [Parameter]
        public string[] allowedExtensions { get; set; }= [".zip", ".rar", ".png", ".jpg",".jpeg", ".mp3" ,".mp4",".apk",".ogg"];
        bool IsUploadDisabled = true;
        private Guid inputFileId = Guid.NewGuid();
        private string cacheBuster = "";
    
        protected void setNull(){
            this.value=null;
            OnChange(value);
        }
    
        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            selectedFile = e.GetMultipleFiles()[0];
            ProgressPercentage = 0;
            IsUploadDisabled = true;
    
            if (selectedFile.Size > FileUploader.maxFileSize)
            {
                SetAlert("alert alert-danger", "oi oi-ban", $"File size exceeds the limit. Maximum allowed size is <strong>{FileUploader.maxFileSize / (1024 * 1024)} MB</strong>.");
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
    
        [Inject] IToastService ToastService { get; set; }
    
        // Handler for file selection from FileBrowser
        public Task OnFileSelectedFromBrowser(string filePath)
        {
            value = filePath;
            OnChange(value);
            
            return InvokeAsync(StateHasChanged);
        }
    
        // Handler for closing the FileBrowser modal
        

    public static byte[] ReadToEnd(System.IO.Stream stream)
    {
        long originalPosition = 0;

        if(stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }

        try
        {
            byte[] readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
        finally
        {
            if(stream.CanSeek)
            {
                stream.Position = originalPosition; 
            }
        }
    }
    private async void OnSubmit()
    {
        if (selectedFile != null)
        {
            IsUploadDisabled = true;
           
            
            // Use a timer to update the UI every few hundred milliseconds.
            using var timer = new Timer(_ => InvokeAsync(() => StateHasChanged()));
            timer.Change(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));
            string fd = null;
            if (FilesDirectory != null) ;
                fd = FilesDirectory;
            try
            {
                var oldBase = ClTool.WebClient.webClient;
                string checksum = "";
                //using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    //var arr=ReadToEnd(stream);
                    //checksum = System.Text.Encoding.Default.GetString( MMD5.ComputeHash(arr));
                }
                //stream.Position = 0;

                //await oldBase.fetch("", null, HttpMethod.Get);
                var tmp = await oldBase.fetch<Models.CreateSessionParams, Models.SessionCreationStatusResponse>("api/file/create", HttpMethod.Post, new Models.CreateSessionParams()
                    {
                        FileName = selectedFile.Name,
                        dir = fd,
                        ChunkSize = 1000,
                        TotalSize = 10000,
                        checkSum= checksum,
                        forceWrite = true
                    });
                value = tmp.FileName;


                OnChange(value);
                cacheBuster = DateTime.Now.Ticks.ToString();

                FileUploader.instnace.upload(tmp,selectedFile,onUploadSection);
                this.StateHasChanged();
                //OnChange(value);
                
                /*while ((bytesRead = await stream.ReadAsync(buffer)) != 0)
                {
                    totalBytesRead += bytesRead;
                    ProgressPercentage = (int)(100 * totalBytesRead / fileSize);
                    //await fs.WriteAsync(buffer, 0, bytesRead);
                    await oldBase.uploadFileSection("api/file/upload", tmp.SessionId, 1, buffer, bytesRead);
                }*/
               
            }
            finally
            {
                
            }

            // Stop the timer and update the UI with the final progress.
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            
            SetAlert("alert alert-success", "oi oi-check", $"<strong>{selectedFile.Name}</strong> ({selectedFile.Size} bytes) file uploaded on server.");
            inputFileId = Guid.NewGuid();
           
        }
    }

    private void onUploadSection(FileUploader.State state)
    {
        ProgressPercentage = state.ProgressPercentage;
        this.StateHasChanged();
    }
    private void SetAlert(string alertClass, string iconClass, string message)
    {
        AlertClass = alertClass;
        AlertMessage = new MarkupString($"<span class='{iconClass}' aria-hidden='true'></span> {message}");
    }
    // Opens the FileBrowser modal when "انتخاب از سرور" button is clicked
    
    
    [CascadingParameter(Name="FilesDirectory")]
    public string FilesDirectory { get; set; }

    [CascadingParameter(Name = "FilesDownloadDirectory")]
    public string FilesDownloadDirectory { get; set; } = "Upload";

    // Note: FileBrowser and FileUploadModal components are available in AdminClient project
    // These would be used when FileUploadBig is used in AdminClient context

    // Note: FileBrowser and FileUploadModal functionality is available in AdminClient project
    // When FileUploadBig is used in AdminClient context, these methods would be implemented
    [Inject] public IModalService ModalService { get; set; }

    private async Task ShowFileBrowserModal()
    {
        var options = new ModalInstanceOptions()
        {
            UseModalStructure = false
        };
        await ModalService.Show<FileBrowser>(x =>
        {
            x.Add(fileBrowser => fileBrowser.OnSuccess, OnFileSelectedFromBrowser);
        }, options);
    }

    }


}