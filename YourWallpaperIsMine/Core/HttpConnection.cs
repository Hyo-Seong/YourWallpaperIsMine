using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YourWallpaperIsMine.Core
{
    class HttpConnection
    {
        const string SERVER_URL = @"http://192.168.0.12:8000/geturl";
        private string imagePath = @"";
        public bool GetImageURL()
        {
            string responseText = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SERVER_URL);
            request.Method = "GET";
            request.Timeout = 30 * 1000; // 30초

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                HttpStatusCode status = resp.StatusCode;

                Stream respStream = resp.GetResponseStream();
                using (StreamReader sr = new StreamReader(respStream))
                {
                    responseText = sr.ReadToEnd();
                }
            }

            
            //responseText = responseText.Replace("[", "{");
            //responseText = responseText.Replace("]", "}");
            Console.WriteLine(responseText);

            var jsonResult = JArray.Parse(responseText);


            Console.WriteLine(jsonResult[1]["imageUrl"]);
            imagePath = jsonResult[1]["imageUrl"].ToString();
            return true;
        }
        

        public void DownloadImage()
        {
            if(!Directory.Exists(Path.GetTempPath() + "YourWallpaperIsMine"))
            {
                Directory.CreateDirectory(Path.GetTempPath() + "YourWallpaperIsMine");
            }
            using (var client = new WebClient())
            {
                client.DownloadFile(@imagePath, Path.GetTempPath() + "YourWallpaperIsMine\\temp.jpg");
            }
        }
    }
}
