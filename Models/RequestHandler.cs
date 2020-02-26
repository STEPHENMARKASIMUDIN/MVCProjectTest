using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DebitCreditMemo.Models
{
    internal class RequestHandler
    {
        private string _Method = string.Empty;
        private Uri _Url = null;
        private string _ContentType = string.Empty;
        private byte[] _jsonData = null;

        // Post Request Handler
        public RequestHandler(string method, Uri uri, string ContentType, byte[] jsonData)
        {
            this._Method = method;
            this._Url = uri;
            this._ContentType = ContentType;
            this._jsonData = jsonData;
        }
        public RequestHandler(Uri url, string Method, string ContentType)
        {
            _Url = url;
            _Method = Method;
            _ContentType = ContentType;
        }

        public virtual string HttpPostRequest()
        {
            int attempt = 0;
            do
            {
                try
                {

                    if (attempt > 0)
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        if (attempt == 1)
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        }
                        else
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                        }
                    }
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_Url) as HttpWebRequest;

                    request.Credentials = request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = _Method;
                    request.ContentType = _ContentType;
                    request.ContentLength = _jsonData.Length;
                    request.Timeout = Timeout.Infinite;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(_jsonData, 0, _jsonData.Length);
                        stream.Close();
                    }
                    WebResponse webresponse = request.GetResponse();
                    String res = null;
                    using (Stream response = webresponse.GetResponseStream())
                    {
                        if (webresponse != null)
                        {
                            using (StreamReader reader = new StreamReader(response))
                            {
                                res = reader.ReadToEnd();
                                reader.Close();
                                webresponse.Close();
                            }
                        }
                    }
                    return res;
                }
                catch (WebException ex)
                {
                    if (ex.ToString().ToLower().Contains("underlying") || ex.ToString().ToLower().Contains("ssl"))
                    {
                        if (attempt < 2)
                        {
                            attempt++;
                            Thread.Sleep(attempt * 2000);
                        }
                        else
                        {
                            try
                            {
                                using (WebResponse response = ex.Response)
                                {
                                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                                    using (Stream data = response.GetResponseStream())
                                    using (var reader = new StreamReader(data))
                                    {
                                        string text = reader.ReadToEnd();
                                        String respcode = httpResponse.StatusCode.ToString();
                                        if (respcode == "422")
                                        {
                                            return "ERROR" + "|" + text;
                                        }
                                        return "ERROR" + "|" + text;
                                    }
                                }
                            }
                            catch (Exception tex)
                            {

                                return "ERROR" + "|" + tex.ToString();
                            }
                        }
                    }
                    else
                    {
                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string text = reader.ReadToEnd();
                                String respcode = httpResponse.StatusCode.ToString();
                                if (respcode == "422")
                                {
                                    return "ERROR" + "|" + text;
                                }
                                return "ERROR" + "|" + text;
                            }
                        }
                    }

                }
            } while (true);

        }

        public virtual string HttpGetRequest()
        {
            int attempt = 0;
            do
            {
                try
                {

                    if (attempt > 0)
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        if (attempt == 1)
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        }
                        else
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                        }
                    }
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_Url) as HttpWebRequest;
                    request.Method = _Method;
                    request.ContentType = _ContentType;
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Timeout = Timeout.Infinite;
                    WebResponse webresponse = request.GetResponse();
                    String res = null;
                    using (Stream response = webresponse.GetResponseStream())
                    {
                        if (webresponse != null)
                        {
                            using (StreamReader reader = new StreamReader(response))
                            {
                                res = reader.ReadToEnd();
                                reader.Close();
                                webresponse.Close();
                            }
                        }
                    }
                    return res;
                }
                catch (WebException ex)
                {
                    if (ex.ToString().ToLower().Contains("underlying") || ex.ToString().ToLower().Contains("ssl"))
                    {
                        if (attempt < 2)
                        {
                            attempt++;
                            Thread.Sleep(attempt * 2000);
                        }
                        else
                        {
                            try
                            {
                                using (WebResponse response = ex.Response)
                                {
                                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                                    using (Stream data = response.GetResponseStream())
                                    using (var reader = new StreamReader(data))
                                    {
                                        string text = reader.ReadToEnd();
                                        String respcode = httpResponse.StatusCode.ToString();
                                        if (respcode == "422")
                                        {
                                            return "ERROR" + "|" + text;
                                        }
                                        return "ERROR" + "|" + text;
                                    }
                                }
                            }
                            catch (Exception tex)
                            {

                                return "ERROR" + "|" + tex.ToString();
                            }
                        }
                    }
                    else
                    {
                        return "ERROR" + "|" + ex.ToString();
                    }

                }
            } while (true);
        }
    }
}
