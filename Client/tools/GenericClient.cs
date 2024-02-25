using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;



using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using Tools;
using Models;
using System.Collections;
using NSubstitute;
using ChunkedUploadWebApi.Model;
using System.Web;
using Microsoft.Net.Http.Headers;

namespace ClTool
{



    public class WebClient
    {
        public static WebClient webClient {
            get; 
            set; } = null;
        public static string pass;
        public string baseUrl { get; 
            set; 
        }
        public Action onLogout;
        public void setUrl(string s)
        {
            baseUrl = s;
        }
        


        public JsonSerializer settings1;
        CookieCollection cookie = null;
        object cookie2 = null;
        public WebClient(string baseurl)
        {

            this.baseUrl = baseurl;
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                SerializationBinder = TypeNameSerializationBinder.gloabl,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = MyContractResolver.admin,

            };


            settings1 = JsonSerializer.Create(settings);
            settings1.Converters.Add(new RialConverter());
            settings1.Converters.Add(new PerimitveContainerConvertor());
            settings1.Converters.Add(new ForeignKeyConverter());
            /*settings1.Converters.Add(new ForeignKeyConverter2());
            settings1.Converters.Add(new ForeignKeyConverter3());
            settings1.Converters.Add(new DateConverter());*/
            




        }
        private static void fixCookies(HttpWebRequest request, HttpWebResponse response)
        {
            for (int i = 0; i < response.Headers.Count; i++)
            {
                string name = response.Headers.GetKey(i);
                if (name != "Set-Cookie")
                    continue;

                string value = response.Headers.Get(i);
                value=value.Replace("expires=Fri,", "expires=Fri ");
            //var cc = new CookieHeaderValue(value);
            //cookie2 = CookieHeaderValue.Parse(value);
            foreach (var singleCookie in value.Split(','))
                {
                    System.Text.RegularExpressions.Match match = Regex.Match(singleCookie, "(.+?)=(.+?);");
                    if (match.Captures.Count == 0)
                        continue;
                    response.Cookies.Add(
                        new Cookie(
                            match.Groups[1].ToString().Replace(" ", ""),
                            match.Groups[2].ToString(),
                            "/",
                            request.Host.Split(':')[0]
                            )
                        );
                }
            }
        }
        public class MyHttpResponse
        {
            public string body;
            public Dictionary<string,string> header;
        }
        public Dictionary<string, string> headres = null;
        public virtual async Task<MyHttpResponse> fetch014(string url, string payload, HttpMethod method)
        {
            Console.WriteLine("fetch url =" + baseUrl + url);
            if(payload!=null)
                Console.WriteLine("body =" + payload);
            HttpWebRequest request = HttpWebRequest.Create(baseUrl + url) as HttpWebRequest;
            request.Method = method.ToString();
            if (request.CookieContainer == null && cookie != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookie);
                Console.WriteLine("cookie =");
                foreach (Cookie cook in cookie)
                    Console.WriteLine($"  - {cook.Name} ={cook.Value};");
            }
            
                
            if (headres != null)
            {
                Console.WriteLine("Headers =");
                foreach (var e in headres)
                {
                    request.Headers[e.Key] = e.Value;
                    Console.WriteLine($"  - {e.Key}=\"{e.Value}\"");
                }
                
                //foreach (var h in request.Headers)
                    
            }
            
            if (payload != null)
            {
                request.ContentType = "application/json";
                request.ContentLength = payload.Length;
                byte[] sentData = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = sentData.Length;
                request.Headers["adminToken"] = pass;







                using (System.IO.Stream sendStream = request.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();

                }
            }

            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                onLogout?.Invoke();
                return null;
            }

            HttpWebResponse responseWithLoginCookies = (HttpWebResponse)response;
            cookie = responseWithLoginCookies.Cookies;
            
            
            CookieContainer cookieJar = new CookieContainer();
            cookieJar.GetCookies(request.RequestUri);
            fixCookies(request, responseWithLoginCookies);
            /*{
                Console.WriteLine($"Cookie  : {responseWithLoginCookies.Headers["Set-Cookie"]}");


            }
            foreach (Cookie cook in cookie)
            {
                Console.WriteLine("Cookie:");
                Console.WriteLine("{0} = {1}", cook.Name, cook.Value);
                Console.WriteLine("Domain: {0}", cook.Domain);
                Console.WriteLine("Path: {0}", cook.Path);
                Console.WriteLine("Port: {0}", cook.Port);
                Console.WriteLine("Secure: {0}", cook.Secure);
                Console.WriteLine("When issued: {0}", cook.TimeStamp);
                Console.WriteLine("Expires: {0} (expired? {1})",
                    cook.Expires, cook.Expired);
                Console.WriteLine("Don't save: {0}", cook.Discard);
                Console.WriteLine("Comment: {0}", cook.Comment);
                Console.WriteLine("Uri for comments: {0}", cook.CommentUri);
                Console.WriteLine("Version: RFC {0}", cook.Version == 1 ? "2109" : "2965");
                // Show the string representation of the cookie.
                Console.WriteLine("String: {0}", cook.ToString());
            }*/
            var z=new StreamReader(response.GetResponseStream());
            var ss=new MyHttpResponse()
            {
                body = await z.ReadToEndAsync(),
                header = new()
            };

            for (int i = 0; i < response.Headers.Count; i++)
            {
                string name = response.Headers.GetKey(i);
                string value = response.Headers.Get(i)!;
                ss.header[name] = value;
            }
            return ss;
        }


        public virtual async Task<string> fetch(string url, string payload, HttpMethod method)
        {
            
            var response= await  fetch014(url,payload, method);

            return response.body;
        }


        public virtual async Task<string> fetchW(string url, string payload, HttpMethod method)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Console.WriteLine("fetch url " + url);
            using (var client = new HttpClient(handler))
            {


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
                request.Headers.Add("adminToken", WebClient.pass);




                var result = await client.SendAsync(request);


                {

                    Console.WriteLine("------headers-------" + result.Headers.Count());
                    Console.WriteLine("------headers-------" + result.Headers.ToString());
                    foreach (var z in result.Headers)
                    {
                        Console.WriteLine("..");
                        Console.WriteLine(z.Key + " : " + z.Value.ToString());
                    }


                }

                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }


        }
        
        public virtual async Task<SessionCreationStatusResponse> createFileSession(string url,int chS,int tS)
        {
            
            
           

            var cookieContainer = new CookieContainer();
            if(cookie!=null)
                cookieContainer.Add(cookie);
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Console.WriteLine("fetch url " + url);
            using (var client = new HttpClient(handler))
            {


                client.BaseAddress = new Uri(baseUrl);


                var csps=new CreateSessionParams()
                {
                    ChunkSize = chS,
                    TotalSize = tS
                };


                var request = new HttpRequestMessage(HttpMethod.Post, baseUrl + url);




                var result = await client.SendAsync(request);

                string resultContent = await result.Content.ReadAsStringAsync();
                return JToken.Parse(resultContent).ToObject<SessionCreationStatusResponse>();

            }


        }

        public virtual async Task<UploadResult> uploadFileSection(string url,string sId,int chunkNumber, byte[] fileContent)
        {
            using var content = new MultipartFormDataContent();
            content.Add(
                       content: new ByteArrayContent(fileContent),
                       name: "\"inputFile\"",
                       fileName: "file.Name");
            
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(cookie);
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Console.WriteLine("fetch url " + url);
            using (var client = new HttpClient(handler))
            {


                client.BaseAddress = new Uri(baseUrl);





                var request = new HttpRequestMessage(HttpMethod.Put, baseUrl + url+"/"+sId+"/"+ chunkNumber)
                {
                    Content = content,
                    
                };




                var result = await client.SendAsync(request);

                string resultContent = await result.Content.ReadAsStringAsync();
                return JToken.Parse(resultContent).ToObject<UploadResult>();

            }


        }

        public virtual async Task<UploadResult> sendFile(string url, MultipartFormDataContent content)
        {
           var cookieContainer = new CookieContainer();
            cookieContainer.Add(cookie);
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Console.WriteLine("fetch url " + url);
            using (var client = new HttpClient(handler))
            {


                client.BaseAddress = new Uri(baseUrl);





                var request = new HttpRequestMessage(HttpMethod.Post, baseUrl + url)
                {
                    Content = content
                };
                
                
               

                var result = await client.SendAsync(request);

                string resultContent = await result.Content.ReadAsStringAsync();
                return JToken.Parse(resultContent).ToObject<UploadResult>();

            }


        }

        public async Task<JToken> fetch0<TIN>(string url, TIN payload, HttpMethod method)
        {
            var x = await fetch(url, payload != null ? JToken.FromObject(payload, settings1).ToString() : null, method);
            return JToken.Parse(x);
        }

        public async Task<object> fetch00(Type tout, string url, HttpMethod method, object payload)
        {
            var x = await fetch0(url, payload != null ? JToken.FromObject(payload, settings1).ToString() : null, method);
            return x.ToObject(tout, settings1);
        }
        public async Task<JToken> fetch00(string url, HttpMethod method, object payload)
        {
            var x = await fetch0(url, payload != null ? JToken.FromObject(payload, settings1).ToString() : null, method);
            return x;
        }
        public async Task<TOUT> fetch<TIN, TOUT>(string url, HttpMethod method, TIN payload)
        {
            var x = await fetch0(url, payload != null ? JToken.FromObject(payload, settings1).ToString() : null, method);
            return x.ToObject<TOUT>(settings1);
        }
        public async Task<TOUT> fetchWithHeader<TIN, TOUT>(string url, HttpMethod method, TIN payload)
        {
            var x = await fetch0(url, payload != null ? JToken.FromObject(payload, settings1).ToString() : null, method);
            return x.ToObject<TOUT>(settings1);
        }
        public async Task<object> fetch(string url, HttpMethod method, Type TOUT, object payload)
        {
            var x = await fetch0(url, payload != null ? JToken.FromObject(payload, settings1).ToString() : null, method);
            return x.ToObject(TOUT, settings1);

        }

        public async Task<TOUT> fetch2<TIN, TOUT>(string url, HttpMethod method, TIN payload)
        {
            var x = await fetch(url, payload != null ? JToken.FromObject(payload, settings1).ToString() : null, method);
            return JToken.Parse(x).ToObject<TOUT>(settings1);
        }

        internal Task<object> fetch<T1, T2>(string name, object pOST, object t)
        {
            throw new NotImplementedException();
        }
    }

    public class GenericClient00
    {
        public WebClient webClient;
        public async Task<object> get00(Type T, int id)
        {
            return await webClient.fetch00(T, T.Name + "/" + id, HttpMethod.Get, null);
        }

    }

    public class GenericClient0<T, TKEY> : GenericClient00
    where T : class
    {
        public string additinalUrl;

        public GenericClient0(WebClient webClient, string additinalUrl = null)
        {
            this.webClient = webClient;
            this.additinalUrl = additinalUrl;

        }
        public string getPath()
        {
            return additinalUrl + typeof(T).GetUrlEncodeName();
        }
        public string getSubTablePath<TMASTER, TMKEY>(string collectionName, TMKEY masterId)
        {
            return $"{typeof(TMASTER).GetUrlEncodeName()}/{masterId}/{collectionName}";
        }
        public async Task<List<T>> getAll()
        {
            if (cash != null)
                return cash;
            return cash = await webClient.fetch<T, List<T>>(getPath(), HttpMethod.Get, null);
        }
        public async Task<List<T>> getAll2(IQuery<T> inp)
        {
            return await webClient.fetch<IQueryContainer<T>, List<T>>(additinalUrl + typeof(T).GetUrlEncodeName() + "/getAll", HttpMethod.Post, new IQueryContainer<T>() {query= inp });
        }
        public async Task<T> sendAction(TKEY entityId, IAction<T> inp)
        {
            return await webClient.fetch<IAction<T>, T>($"{additinalUrl}{typeof(T).GetUrlEncodeName()}/{entityId}/runAction", HttpMethod.Post, inp);
        }
        public async Task<List<T>> getAll3<TMASTER, TMKEY>(string collectionName, TMKEY masterId)
        {
            return await webClient.fetch<T, List<T>>(getSubTablePath<TMASTER,TMKEY>(collectionName,masterId), HttpMethod.Get, null);
        }
        public async Task<List<T>> getAll(string itemName, TKEY value)
        {
            var filter = JToken.FromObject(new
            {
                int_eq = new
                {
                    field = itemName,
                    value = value
                }
            }).ToString();
            return cash = await webClient.fetch<T, List<T>>(additinalUrl + typeof(T).GetUrlEncodeName() + $"?filter={filter}", HttpMethod.Get, null);
        }
        List<T> cash = null;
        public async Task<List<object>> getAll2()
        {


            var ga = await getAll();
            return ga.ConvertAll<object>(x => x); ;
        }
        public async Task<T> get(TKEY id)
        {
            return await webClient.fetch<T, T>(additinalUrl + typeof(T).GetUrlEncodeName() + "/" + id, HttpMethod.Get, null);
        }


        public async Task<T> put(object id, T t)
        {
            return await webClient.fetch<T, T>(additinalUrl + typeof(T).GetUrlEncodeName() + "/" + id, HttpMethod.Put, t);

        }


        public async Task<T> post(T t)
        {
            return await webClient.fetch<T, T>(additinalUrl + typeof(T).GetUrlEncodeName(), HttpMethod.Post, t);
        }

    }
    public class GenericClientInt<T,TKEY> : GenericClient0<T, TKEY>
        where T :class, Models.IIdMapper<TKEY>
        where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        public GenericClientInt(ClTool.WebClient w, string additinalUrl = null) : base(w, additinalUrl) { }
    }
   

}