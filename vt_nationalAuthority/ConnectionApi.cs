
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace vt_nationalAuthority
{
    public class ConnectionApi : Controller
    {
        //static string sLocalHostNumberAPI = "4773"; // API Mobile

        //////////RayaHost
        //////static string sLocalHostNumberAPI = "80"; // IP Raya

        //static string localhost = "http://localhost/api/api/";

        ////////////////localhost
        static string sLocalHostNumberAPI = "6245";
        static string localhost = "http://localhost:" + sLocalHostNumberAPI + "/api/";
        object oResult;
        /// <summary>
        /// Connection With Api , To Get List Of Data
        /// </summary>
        /// <typeparam name="T">Generic Type Of Data</typeparam>
        /// <param name="sApiControllerName">Api Controller Name</param>
        /// <param name="sFunctionName">Api Function Name</param>
        /// <param name="sStr">Parameters If I Need</param>
        /// <returns>Request With List Of Data</returns>
        public T connectionApiGetList<T>(string sApiControllerName, string sFunctionName, string sStr)
        {
            try
            {
                using (var vClient = new HttpClient())
                {
                    string sPath = localhost + sApiControllerName + "/" + sFunctionName;
                    if (!String.IsNullOrEmpty(sStr))
                        sPath += "/" + sStr;

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
            catch (Exception ex)
            {
throw;
            }
        }
        /// <summary>
        ///  Connection With Api , To Search For List Of Data
        /// </summary>
        /// <typeparam name="T">Generic Type Of Data</typeparam>
        /// <param name="sApiControllerName">Api Controller Name</param>
        /// <param name="sFunctionName">Api Function Name</param>
        /// <param name="lStr">List Of Parameters</param>
        /// <returns>Request With List Of Data</returns>
        public T connectionApiSearchList<T>(string sApiControllerName, string sFunctionName, List<string> lStr)
        {
            try
            {
                using (var vClient = new HttpClient())
                {
                    string sPath = localhost + sApiControllerName + "/" + sFunctionName;

                    // Assuming the API is in the same web application. 
                    string baseUrl = System.Web.HttpContext.Current
                                                .Request
                                                .Url
                                                .GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
                    vClient.BaseAddress = new Uri(baseUrl);
                    oResult = vClient.PostAsync(sPath,
                                                 lStr,
                                                 new JsonMediaTypeFormatter())
                                       .Result
                                       .Content
                                       .ReadAsAsync<T>()
                                       .Result;

                    return (T)oResult;
                }
            }
            catch (Exception ex)
            {
throw;
            }
        }
        /// <summary>
        /// Connection With Api , To Post Data In Database 
        /// </summary>
        /// <typeparam name="T">Generic Type Of Data</typeparam>
        /// <param name="sApiControllerName">Api Controller Name</param>
        /// <param name="sFunctionName">Api Function Name</param>
        /// <param name="modal">Data We Need To Post In Database</param>
        /// <param name="sStr">Parameter If I Need</param>
        /// <returns>Request For Result <returns>
        public T connectionApiPost<T>(string sApiControllerName, string sFunctionName, T modal, string sStr)
        {
            try
            {
                using (var vClient = new HttpClient())
                {
                    string sPath = localhost + sApiControllerName + "/" + sFunctionName;
                    if (!String.IsNullOrEmpty(sStr))
                        sPath += "/" + sStr;

                    // Assuming the API is in the same web application. 
                    string baseUrl = System.Web.HttpContext.Current
                                                .Request
                                                .Url
                                                .GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
                    vClient.BaseAddress = new Uri(baseUrl);
                    oResult = vClient.PostAsync(sPath,
                                                 modal,
                                                 new JsonMediaTypeFormatter())
                                       .Result
                                       .Content
                                       .ReadAsAsync<T>()
                                       .Result;

                    return (T)oResult;
                }
            }
            catch (Exception ex)
            {
throw;
            }
        }
        /// <summary>
        /// Connection With Api , To Delete Data In Database 
        /// </summary>
        /// <typeparam name="T">Generic Type Of Data</typeparam>
        /// <param name="sApiControllerName">Api Controller Name</param>
        /// <param name="sFunctionName">Api Function Name</param>
        /// <param name="sStr">Parameter If I Need</param>
        /// <returns>Request For Result <returns>
        public T connectionApiDelete<T>(string sApiControllerName, string sFunctionName, string sStr)
        {
            try
            {
                string sPath = localhost + sApiControllerName + "/" + sFunctionName;
                if (!String.IsNullOrEmpty(sStr))
                    sPath += "/" + sStr;

                var vClient = new HttpClient();
                oResult = vClient.DeleteAsync(sPath).Result
                            .Content
                            .ReadAsAsync<T>()
                            .Result;
                return (T)oResult;
            }
            catch (Exception ex)
            {
throw;
            }
        }
        /// <summary>
        /// Connection With Api , To Get Data With API URL Path
        /// </summary>
        /// <typeparam name="T">Generic Type Of Data</typeparam>
        /// <param name="path">Api URL</param>
        /// <returns>Request For Result <returns>
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
            catch (Exception ex)
            {
throw;
            }
        }
    }
}