using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Models;

namespace AdminBaseComponenets.BaseComs;

public class FileUploader
{
    public static FileUploader instnace = new FileUploader();
    public class State
    {
        public int ProgressPercentage { get; set; }
        public long totalBytesRead=0,fileSize=0;

        public bool Completed=false;
    }
    public class ExpetainC
    {
        public int ProgressPercentage { get; set; }
        public long totalBytesRead=0,fileSize=0;

        public bool Completed=false;
    }
    public class ExpetainOut
    {
            
        public bool Retry { get; set; }

            
    }

    public static long maxFileSize = 1024L * 1024L * 1024L * 2L;
    public Dictionary<string, State> Files { get; set; } = new Dictionary<string, State>();
    public async Task<State> upload2(
        IBrowserFile selectedFile,
        string filesDirectory,
        Action<State> onUploadSection,
        Action<string> onSessionCreated = null,
        Func<Exception, Task<ExpetainOut>> onException = null)
    {
        State uploadState = new State();
        uploadState.fileSize = selectedFile.Size;

        try
        {
            // 1. Create upload session
            var sessionParams = new Models.CreateSessionParams()
            {
                FileName = selectedFile.Name,
                dir = filesDirectory,
                ChunkSize = 512 * 1024,
                TotalSize = selectedFile.Size,
                checkSum = "",
                forceWrite = true
            };

            var oldBase = ClTool.WebClient.webClient;
            var sessionResponse = await oldBase.fetch<Models.CreateSessionParams, Models.SessionCreationStatusResponse>(
                "api/file/create", HttpMethod.Post, sessionParams);

            if (onSessionCreated != null)
                onSessionCreated(sessionResponse.FileName);

            

            // 2. Upload in chunks
            
            upload(sessionResponse,selectedFile,onUploadSection,onException);
          

            
            
        }
        catch (Exception ex)
        {
            if (onException != null)
                await onException(ex);
        }

        return uploadState;
    }
   
    public State upload(SessionCreationStatusResponse fileUploadSesion, IBrowserFile selectedFile, Action<State> onUploadSection,Func<Exception,Task<ExpetainOut>> onException=null)
    {
        State upload1;
        if(Files.TryGetValue(fileUploadSesion.FileName, out upload1))
            return upload1;
        upload1 = Files[fileUploadSesion.FileName] = new State();            
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
            while(true)
                try
                {
                    while ((bytesRead = await stream.ReadAsync(buffer)) != 0)
                    {

                        //await fs.WriteAsync(buffer, 0, bytesRead);
                        await ClTool.WebClient.webClient.uploadFileSection("api/file/upload", fileUploadSesion.SessionId, 1, buffer,
                            bytesRead);
                        upload1.totalBytesRead += bytesRead;
                        upload1.ProgressPercentage = (int)(100 * upload1.totalBytesRead / upload1.fileSize);
                        onUploadSection(upload1);
                    }
                    upload1.Completed = true;
                    onUploadSection(upload1);
                    break;
                }
                catch (Exception ex)
                {
                    if (onException == null)
                        break;
                
                    var oo = await onException(ex);
                    if (!oo.Retry)
                    {
                        break;
                    }
                
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