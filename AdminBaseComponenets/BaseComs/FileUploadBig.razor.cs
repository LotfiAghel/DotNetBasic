using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
using System.IO;
using System.Threading;
using Blazorise;
using Models;

namespace AdminBaseComponenets.BaseComs
{
    public class FileUploader
    {
        public static FileUploader instnace = new FileUploader();
        public class State
        {
            public int ProgressPercentage { get; set; }
            public long totalBytesRead=0,fileSize=0;

            public bool Completed=false;
        }

        public static long maxFileSize = 1024L * 1024L * 1024L * 2L;
        public Dictionary<string, State> Files { get; set; } = new Dictionary<string, State>();

        public State upload(SessionCreationStatusResponse path, IBrowserFile selectedFile, Action<State> onUploadSection)
        {
            State upload1;
            if(Files.TryGetValue(path.FileName, out upload1))
                return upload1;
            upload1 = Files[path.FileName] = new State();            
            Stream stream = selectedFile.OpenReadStream(maxFileSize);
            //var path = $"env.WebRootPath/{selectedFile.Name}";
            //using FileStream fs = File.Create(path);

            // Set buffer size to 512 KB.
           
            upload1.fileSize = selectedFile.Size;
            
            int bufferSize = 512 * 1024;
            byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(bufferSize);
            int bytesRead;
            
            
           
            
            Task task = Task.Run(async () =>
            {
                try
                {
                    while ((bytesRead = await stream.ReadAsync(buffer)) != 0)
                    {

                        //await fs.WriteAsync(buffer, 0, bytesRead);
                        await ClTool.WebClient.webClient.uploadFileSection("api/file/upload", path.SessionId, 1, buffer,
                            bytesRead);
                        upload1.totalBytesRead += bytesRead;
                        upload1.ProgressPercentage = (int)(100 * upload1.totalBytesRead / upload1.fileSize);
                        onUploadSection(upload1);
                    }
                    upload1.Completed = true;
                    onUploadSection(upload1);
                }
                catch (Exception ex)
                {

                }
                
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);    
                
                
            });
            return upload1;

        }


        public State getState(string path)
        {
            if (Files.TryGetValue(path, out var upload1) && !upload1.Completed)
                return upload1;
            return null;
        } 
    }
    public partial class FileUploadBig : ValueInput<string> 
    {

        
        MarkupString AlertMessage = new MarkupString("<strong>No file selected</strong>");
    string AlertClass = "alert alert-info";
    int ProgressPercentage = 0;
    IBrowserFile selectedFile = null;
    
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

    }


}