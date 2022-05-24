using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Mass_Release_Downloader
{
    public class Network
    {
        public static string DownloadString(string URL)
        {
            using (HttpClient client = new HttpClient())
            {
                var productValue = new ProductInfoHeaderValue("GitHubReleaseDownloader", "1.0");
                var commentValue = new ProductInfoHeaderValue("(+https://gs2012.xyz)");

                client.DefaultRequestHeaders.UserAgent.Add(productValue);
                client.DefaultRequestHeaders.UserAgent.Add(commentValue);
                using (var result = client.GetStringAsync(URL))
                {
                    result.Wait();
                    return result.Result; 


                }
            }
        }

        public static void DownloadFile(string downloadUrl, string filePath)
        {
            WebClient client = new WebClient();
            client.DownloadFile(downloadUrl, filePath);
        }
    }
}
