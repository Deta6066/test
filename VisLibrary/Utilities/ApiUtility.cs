using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;

namespace VisLibrary.Utilities
{
    public static class ApiUtility
    {
        /// <summary>
        /// 透過API取得資料
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="method"> 
        /// http method的種類 :Get or Post 預設為Get
        /// </param>
        /// <returns></returns>
        /// <param name="headers"></param>
        public static async Task<T?> GetDataByAPIAsync<T>(string apiUrl, string method = "Get", Dictionary<string, object>? headers = null, HttpContent? content = null) where T : new()
        {
            T? result = new();
            headers = headers ?? new Dictionary<string, object>();
            // 建立一個 HttpClient 實例
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //如果有header
                    //加入headers的資料
                    if (headers != null && headers.Count > 0)
                    {
                        foreach (var item in headers)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
                        }
                    }

                    //依照method設定 取得資料
                    //HttpResponseMessage response = await client.GetAsync(apiUrl);\
                    HttpResponseMessage response = new HttpResponseMessage();
                    //method轉成首字大寫，後面字串小寫
                    method = method.Substring(0, 1).ToUpper() + method.Substring(1).ToLower();
                    switch (method)
                    {
                        case "Get":
                            response = await client.GetAsync(apiUrl);
                            break;
                        case "Post":
                            response = await client.PostAsync(apiUrl, content);
                            break;
                        default:
                            break;
                    }

                    // 確認回應是否成功
                    if (response.IsSuccessStatusCode)
                    {
                        // 讀取回應內容
                        string responseData = await response.Content.ReadAsStringAsync();

                        // 轉成 Order List
                        result = JsonConvert.DeserializeObject<T>(responseData);

                        //if (result != null)
                        //{
                        //    result = orders;
                        //}
                        Console.WriteLine(responseData);
                    }
                    else
                    {
                        // 處理請求失敗的情況
                        Console.WriteLine($"無法取得資料。狀態碼：{response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // 處理異常情況
                    Console.WriteLine($"發生錯誤：{ex.Message}");
                }
            }

            return result;
        }
        /// <summary>
        /// 透過API取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiInfo">
        /// api資訊
        /// </param>
        /// <param name="headers"></param>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        public static async Task<T?> GetDataByAPIAsync<T>(MApiInfo apiInfo, Dictionary<string, object>? headers = null, object? content = null) where T : new()
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            return await GetDataByAPIAsync<T>(apiInfo.ApiUrl, apiInfo.ApiMethod, headers, httpContent);
        }
        /// <summary>
        /// 取得API資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiInfo">
        /// api資訊
        /// </param>
        /// <param name="companyID">公司ID</param>
        /// <param name="content">api要傳輸的內容</param>
        /// <returns></returns>
        public static async Task<T?> GetDataByAPIAsync<T>(MApiInfo apiInfo,int companyID, object? content) where T : new()
        {
            T? result = new T();
            Dictionary<string, object> header = new Dictionary<string, object>();
            header.Add("companyID", companyID);
            //取得api資料
            if (apiInfo != null)
            {
                //取得api資料
                result = await GetDataByAPIAsync<T>(apiInfo, header, content);
            }
            return result;
        }
    }
}
