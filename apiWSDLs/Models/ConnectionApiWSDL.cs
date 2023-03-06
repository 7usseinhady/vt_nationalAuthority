using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace apiWSDLs.Models
{
    public class ConnectionApiWSDL
    {
        object oResult;
        public T connectionApiGetWithLocalHost<T>(string path)
        {
            try
            {
                using (var vClient = new HttpClient())
                {
                    string sPath = path;
                    var vGetDataTask = vClient.GetAsync(sPath)
                        .ContinueWith(response =>
                        {
                            var responseResult = response.Result;
                            if (responseResult.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var readResult = responseResult.Content.ReadAsAsync<T>();
                                readResult.Wait();
                                oResult = readResult.Result;
                            }
                        });

                    vGetDataTask.Wait();
                    return (T)oResult;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}