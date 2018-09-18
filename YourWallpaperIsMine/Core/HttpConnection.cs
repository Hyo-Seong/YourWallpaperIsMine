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

        private JArray jsonResult;

        public void GetImageURL()
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

            jsonResult = JArray.Parse(responseText);


            GetRandomCount(jsonResult);

        }

        private void GetRandomCount(JArray jsonResult)
        {
            int count = jsonResult.Count();
            Random random = new Random();
            int randomNumber = random.Next(1, count);

            //Console.WriteLine(jsonResult[1]["imageUrl"]);
            imagePath = jsonResult[randomNumber]["imageUrl"].ToString();
        }
        private int errorCount = 0;

        public bool DownloadImage()
        {
            if(!Directory.Exists(Path.GetTempPath() + "YourWallpaperIsMine"))
            {
                Directory.CreateDirectory(Path.GetTempPath() + "YourWallpaperIsMine");
            }
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(@imagePath, Path.GetTempPath() + "YourWallpaperIsMine\\temp.jpg");
                    return true;
                }
            }
            catch
            {
                GetRandomCount(jsonResult);
                errorCount++;
                if (errorCount == 5)
                {
                    return false;
                }
                DownloadImage();
            }
            return true;
        }
    }
}
