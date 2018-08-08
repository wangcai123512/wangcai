using System.IO;
using System.Net;
using System.Text;

namespace Common.CommonFunction
{
    public class HttpRequest
    {
        #region HTTP-Request
        public string GenerateHttpRequest(string data, string url)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            WebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            request.Timeout = 10000;
            string resPage = string.Empty;
            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();
            WebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            resPage = reader.ReadToEnd();
            return resPage;
        }

        #endregion
    }
}