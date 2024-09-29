using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.IO;
using ClTool;
namespace ClTool2
{

    public class WebClient : ClTool.WebClient
    {



        public WebClient(string baseurl) : base(baseurl)
        {




        }
        public override void SaveCookie()
        {
            //await JsRuntime.InvokeAsync<string>("blazorExtensions.SetCookie", new[] { "isAuthenticated", "true" });
        }
        public override void LoadCookie()
        {
            
        }
        public override async Task<MyHttpResponse> fetch014(string url, string payload, HttpMethod method)
        {
            HttpClientHandler handler = new HttpClientHandler();

            Console.WriteLine("CLtool2 fetch url " + url);
            using (var client = new HttpClient(handler))
            {

                Console.WriteLine("baseUrl " + baseUrl);
                client.BaseAddress = new Uri(baseUrl);
                HttpContent content = null;
                if (payload != null)
                {
                    content = new StringContent(payload, Encoding.UTF8, "application/json");
                }

                var request = new HttpRequestMessage(method, baseUrl + url)
                {
                    Content = content
                };

                request.Headers.Add("Access-Control-Allow-Credentials", "include");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);





                Console.WriteLine("go to client.SendAsync(request)");
                var result = await client.SendAsync(request);
                Console.WriteLine("come from client.SendAsync(request)");
                //TODO issue_7
                // result.StatusCode=403  set Callback from Uiside and call
                if (result.StatusCode == HttpStatusCode.Unauthorized && onLogout != null)
                {
                    onLogout.Invoke();
                }


                var ss = new MyHttpResponse()
                {
                    body = await result.Content.ReadAsStringAsync(),
                    header = new()
                };
                
                foreach (var i in result.Content.Headers)
                {
                    Console.WriteLine(i.Key);
                    Console.WriteLine(i.Value.ToList()[0]);
                    ss.header[i.Key] = i.Value.ToList()[0];
                }
                return ss;
            }
            


        }


        public override async Task<string> fetch(string url, string payload, HttpMethod method)
        {
            HttpClientHandler handler = new HttpClientHandler();

            Console.WriteLine("CLtool2 fetch url " + url);
            using (var client = new HttpClient(handler))
            {

                Console.WriteLine("baseUrl " + baseUrl);
                client.BaseAddress = new Uri(baseUrl);
                HttpContent content = null;
                if (payload != null)
                {
                    content = new StringContent(payload, Encoding.UTF8, "application/json");
                }

                var request = new HttpRequestMessage(method, baseUrl + url)
                {
                    Content = content
                };
               
                request.Headers.Add("Access-Control-Allow-Credentials", "include");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);





                Console.WriteLine("go to client.SendAsync(request)");
                var result = await client.SendAsync(request);
                Console.WriteLine("come from client.SendAsync(request)");
                //TODO issue_7
                // result.StatusCode=403  set Callback from Uiside and call
                if (result.StatusCode == HttpStatusCode.Unauthorized && onLogout!=null)
                {
                    onLogout.Invoke();
                }

                
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }


        }
        
           public override async Task<UploadResult> uploadFileSection(string url, string sId, int chunkNumber, byte[] fileContent,int l)
        {
            using var content = new MultipartFormDataContent();
            content.Add(
                       content: new ByteArrayContent(fileContent,0,l),
                       name: "\"inputFile\"",
                       fileName: "file.Name");

            HttpClientHandler handler = new HttpClientHandler() { };
            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Console.WriteLine("fetch url " + url);
            using (var client = new HttpClient(handler))
            {


                client.BaseAddress = new Uri(baseUrl);





                var request = new HttpRequestMessage(HttpMethod.Put, baseUrl + url + "/" + sId + "/" + chunkNumber)
                {
                    Content = content,

                };
                request.Headers.Add("Access-Control-Allow-Credentials", "include");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);



                var result = await client.SendAsync(request);

                string resultContent = await result.Content.ReadAsStringAsync();
                return JToken.Parse(resultContent).ToObject<UploadResult>();

            }


        }


        
        public override async Task<ClTool.UploadResult> sendFile(string url, MultipartFormDataContent content)
        {
            HttpClientHandler handler = new HttpClientHandler();
            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Console.WriteLine("fetch url " + url);
            using (var client = new HttpClient(handler))
            {


                client.BaseAddress = new Uri(baseUrl);





                var request = new HttpRequestMessage(HttpMethod.Post, baseUrl + url)
                {
                    Content = content
                };
                request.Headers.Add("adminToken", WebClient.pass);

                request.Headers.Add("Access-Control-Allow-Credentials", "include");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var result = await client.SendAsync(request);
                 //TODO issue_7
                 // result.StatusCode=403  set Callback from Uiside and call
                string resultContent = await result.Content.ReadAsStringAsync();
                return JToken.Parse(resultContent).ToObject<ClTool.UploadResult>();

            }


        }


    }


}