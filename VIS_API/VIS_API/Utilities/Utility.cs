using Dapper;
using DapperDataBase.Database.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

using VIS_API.Models;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Utilities
{
    public static class Utility
    {
        private static readonly Aes _aes = Aes.Create();

        static Utility()
        {
            _aes.GenerateKey();
            _aes.GenerateIV();
        }

        public static T? FromJson<T>(string? json)
        {
            return JsonConvert.DeserializeObject<T>(json.Text());
        }

        public static dynamic? FromJson(string? json)
        {
            return JsonConvert.DeserializeObject(json.Text());
        }

        /// <summary>加密</summary>
        public static string Encrypt(object value)
        {
            return _aes.Encrypt(value);
        }

        /// <summary>解密</summary>
        public static string Decrypt(string value)
        {
            return _aes.Decrypt(value);
        }
        /// <summary>
        /// 取得View的路徑
        /// </summary>
        /// <param name="viewName">
        /// view檔案的名稱
        /// 例如輸入"PCD/SupplierDeliveryRate"，會回傳"views/PCD/SupplierDeliveryRate.cshtml"
        /// </param>
        /// <returns></returns>
        public static string GetViewsPath(string viewName)
        {
            return $"views/{viewName}.cshtml";
        }
        /// <summary>
        /// 取得外部檔案資料
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="actionName"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GetFileData(string filePath, string actionName, ref IDb? db)
        {
            string sql = "";
            //檢查檔案類型
            //依不同檔案類型 進行不同處理
            string extension = Path.GetExtension(filePath);
            switch (extension)
            {
                case ".json":
                    //讀取json檔案
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        //讀取檔案內容
                        string json = sr.ReadToEnd();
                        //轉成sql語法
                        var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                        if (dic.ContainsKey(actionName))
                        {
                            sql = dic[actionName];
                        }
                        //檢查是否有設定db資料庫
                        if (dic.ContainsKey("DB_IP"))
                        {
                            //設定db資料庫
                            db = CreateNewDb(dic);
                        }
                    }
                    break;
                case ".sql":
                    //讀取sql檔案
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        //讀取檔案內容
                        sql = sr.ReadToEnd();
                    }
                    break;
                case ".txt":
                default:
                    //讀取txt檔案
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        //讀取檔案內容
                        sql = sr.ReadToEnd();
                    }
                    break;
            }
            return sql;
        }
       
        /// <summary>
        /// 取得公司API位址
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //public static string GetCompanyApi(int companyID, string actionName, out Dictionary<string, object> paramater)
        //{
        //    //依公司ID與actionName取得API位址
        //    //從json檔取得API位址和使用的method
        //    //讀取json檔案
        //    string filePath = $"wwwroot/content/api_json/{companyID}.test.json";
        //    string apiUrl = "";
        //    paramater = new Dictionary<string, object>();
        //    bool isExist = File.Exists(filePath);
        //    //if file is not exist
        //    if (!isExist)
        //    {
        //        return apiUrl;
        //    }
        //    else
        //    {
        //        using (StreamReader sr = new StreamReader(filePath))
        //        {
        //            //讀取檔案內容
        //            string json = sr.ReadToEnd();
        //            //轉成sql語法
        //            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
        //            if (dic.ContainsKey(actionName))
        //            {
        //                //var apiData = dic[actionName] as Dictionary<string, object>;
        //                var apiData = JsonConvert.DeserializeObject<Dictionary<string, object>>(dic[actionName].ToString());
        //                //  apiUrl = apiData["url"].ToString();
        //                //if apidata的key有url
        //                if (apiData.ContainsKey("url"))
        //                {
        //                    apiUrl = apiData["url"].ToString();
        //                }
        //                if (apiData.ContainsKey("paramater"))
        //                {
        //                    paramater = apiData["paramater"] as Dictionary<string, object>;
        //                }
        //            }
        //        }
        //    }

        //    return apiUrl;
        //}

        /// <summary>
        /// 建立新的資料庫連線
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        /// 
        private static IDb? CreateNewDb(Dictionary<string, string> dic)
        {
            //使用參數設定連線字串
            StringBuilder connectionString = new StringBuilder();

            try
            {
                if (dic.ContainsKey("DB_TYPE"))
                {
                    IDb? db = null;
                    //檢查db類型
                    //依不同db類型 進行不同處理
                    switch (dic["DB_TYPE"].ToLower())
                    {
                        case "mysql":
                            connectionString.Append("Data Source=");
                            connectionString.Append(dic["DB_IP"]);
                            connectionString.Append(";port=");
                            connectionString.Append(dic["DB_PORT"]);
                            connectionString.Append(";Initial Catalog=");
                            connectionString.Append(dic["DB_NAME"]);
                            connectionString.Append(";User Id=");
                            connectionString.Append(dic["DB_USER"]);
                            connectionString.Append(";Password=");
                            connectionString.Append(dic["DB_PASS"]);
                            connectionString.Append(";Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;");
                            //db = new MySqlDb(connectionString.ToString());
                            break;
                        case "mssql":
                            //"msSqlConnectionString": "Server=localhost;Database=vis_new;User ID=sa;Password=dek0306;TrustServerCertificate=true;"
                            connectionString.Append("Server=");
                            connectionString.Append(dic["DB_IP"]);
                            connectionString.Append(";Database=");
                            connectionString.Append(dic["DB_NAME"]);
                            connectionString.Append(";User ID=");
                            connectionString.Append(dic["DB_USER"]);
                            connectionString.Append(";Password=");
                            connectionString.Append(dic["DB_PASS"]);
                            connectionString.Append(";TrustServerCertificate=true;");
                            //db = new MsSqlDb(connectionString.ToString());
                            break;
                        default:
                            break;
                    }
                    return db;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤：{ex.Message}");
            }

            return null;
        }
        /// <summary>
        /// 取得物件的欄位名稱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CmdColStr<T>(T model)
        {
            string Cols = "";
            string json = JsonConvert.SerializeObject(model);
            JObject jsonObject = JObject.Parse(json);
            var properties = jsonObject.Properties();
            foreach (var property in properties)
            {
                if (property.Name.ToLower() == "id")
                {
                    if (property == properties.Last())
                        Cols += property.Name;
                    else
                        Cols += property.Name + ",";
                    string key = property.Name;
                    JToken value = property.Value;
                }
            }
            return Cols;
        }
        /// <summary>
        /// 取得物件的欄位值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CmdCValueStr<T>(T model)
        {
            string Cols = "";
            string json = JsonConvert.SerializeObject(model);
            JObject jsonObject = JObject.Parse(json);
            var properties = jsonObject.Properties();
            foreach (var property in properties)
            {
                if (property.Name.ToLower()=="id")
                {
                    if (property == properties.Last())
                        Cols += $"@{property.Name}";
                    else
                        Cols += $"@{property.Name},";
                }
            }
            return Cols;
        }
        /// <summary>
        /// 透過公司ID取得外部檔案路徑
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <returns></returns>
        public static string GetExternalFilePath(int companyID)
        {
            return $"wwwroot/content/Sql_json/{companyID}.json";
        }
        /// <summary>
        /// 取得資料庫連線資訊
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DbConnectionInfo GetDbInfoByDataSource(MCompanyDataSource com)
        {
            //透過資料來源取得來源類別
            // int dbType = GetDbType(com.dbParamater);
            int dbType = com.dbType;

            int sourceType = com.sourceType;
            //if type !=2 (不是透過api取得資料)
            if (sourceType != 2)
            {
                //來源類型不是外部Api
                //透過dbParamater取得資料庫連線資訊
                return new DbConnectionInfo { ConnectionString = com.dbParamater, DbType = dbType };
            }
            else
            {
                //來源類型是外部Api
                //取得api連線資訊
                //透過來源類別取得資料庫連線資訊
                return new DbConnectionInfo { ConnectionString = com.dbParamater, DbType = dbType };
            }
        }
        /// <summary>
        /// 取得資料庫類型
        /// </summary>
        /// <param name="dbParamater">資料庫連線參數</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int GetDbType(string dbParamater)
        {
            // 將連線字串參數轉換為小寫，以便不區分大小寫
            string dbParameterLower = dbParamater.ToLower();

            if (dbParameterLower.Contains("server") && dbParameterLower.Contains("initial catalog"))
            {
                return (int)VISDbType.mssql;
            }
            else if (dbParameterLower.Contains("server") && dbParameterLower.Contains("database"))
            {
                //return "Mysql";
                return (int)VISDbType.mysql;
            }
            else
            {
                //return "Other";
                return (int)VISDbType.other;
            }
        }


        public static DynamicParameters? CreateDynamicParameters(object parameter)
        {
            try
            {
                var dynamicParameters = new DynamicParameters();
                if (parameter != null)
                {
                    var properties = parameter.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        string key = "@" + property.Name; // 确保参数以 "@" 开头
                        object? value = property.GetValue(parameter) ?? DBNull.Value; // 若值为 null 则用 DBNull.Value
                        dynamicParameters.Add(key, value);
                    }
                }
                return dynamicParameters;
            }
            catch (Exception ex)
            {
                return null;
                //throw; // 可选择重新抛出异常或返回空 DynamicParameters
            }
        }
    }
}
