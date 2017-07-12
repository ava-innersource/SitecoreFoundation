using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;

namespace SF.Foundation.Resources
{

    public class SyncController : ServicesApiController
    {
        //Get /sitecore/api/avn/sync
        [HttpGet]
        [ActionName("Sync")]
        public string Index()
        {
            throwError(HttpStatusCode.MethodNotAllowed, "Get Not Allowed", "Post is Supported");

            return "";
        }

        //Post: /sitecore/api/sf/sync?p=/CSS/Sites/etc.
        [HttpPost]
        public string Sync()
        {
            try
            {
                var folderName = "global";
                var relativePath = System.Web.HttpContext.Current.Request.QueryString["p"];

                if (!relativePath.StartsWith("css") &&
                    !relativePath.StartsWith("less") &&
                    !relativePath.StartsWith("sass") &&
                    !relativePath.StartsWith("scripts"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Path (p querystring) needs to target css, less, sass or styles folders"));
                }

                var fullPath = System.Web.HttpContext.Current.Server.MapPath("~/") + folderName + "/" + relativePath;


                if (Request.Content.IsMimeMultipartContent())
                {
                    //Create Director if doesn't exist
                    Directory.CreateDirectory(fullPath);

                    //Will write out file name as is to path dir
                    var streamProvider = new CustomMultipartFormDataStreamProvider(fullPath);

                    //var task = Request.Content.ReadAsMultipartAsync(streamProvider);

                    IEnumerable<HttpContent> parts = null;
                    Task.Factory
                        .StartNew(() => parts = Request.Content.ReadAsMultipartAsync(streamProvider).Result.Contents,
                            System.Threading.CancellationToken.None,
                            TaskCreationOptions.LongRunning, // guarantees separate thread
                            TaskScheduler.Default)
                        .Wait();
                                        
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }

                return "OK";
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error Syncing Resource", ex, this);
                return "ERR";
            }
        }

       

        private void throwError(HttpStatusCode status, string content, string reasonPhrase)
        {
            var resp = new HttpResponseMessage(status)
            {
                Content = new StringContent(content),
                ReasonPhrase = reasonPhrase
            };
            throw new HttpResponseException(resp);
        }
    }


    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private string basePath { get; set; }
	    public CustomMultipartFormDataStreamProvider(string path) : base(path)
	    {
            basePath = path;
        }
 
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            name =  name.Replace(@"""",string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped

            //ensure file doesn't exist before returning.
            var fullPath = basePath + "/" + name;
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return name;
        }

    }

}
