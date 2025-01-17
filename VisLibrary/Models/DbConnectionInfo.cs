namespace VisLibrary.Models
{
    public class DbConnectionInfo
    {
        // 通用屬性
        public  string? Server { get; set; }           // 伺服器地址
        public int? Port { get; set; }               // 端口號 (選填，SQL Server 通常不需要手動指定)
        public  string? Database { get; set; }         // 資料庫名稱
        public  string? Id { get; set; }         // 用戶名
        public  string? Password { get; set; }         // 密碼
        public  string? ConnectionString { get; set; } // 完整的連線字串 (選填，如果提供則直接使用此連線字串)

        // 特定於 MySQL 的屬性
        public string? Charset { get; set; }          // 字符集
        public bool ConvertZeroDatetime { get; set; } // 是否將零日期時間轉換
        public bool AllowUserVariables { get; set; } // 是否允許用戶變量

        // 特定於 SQL Server 的屬性
        public bool TrustServerCertificate { get; set; } // 是否信任伺服器證書

        // 生成連線字串的方法
        public string BuildConnectionString()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            var builder = new System.Data.Common.DbConnectionStringBuilder();

            if (!string.IsNullOrEmpty(Server))
                builder["Server"] = Port.HasValue ? $"{Server},{Port}" : Server;

            if (!string.IsNullOrEmpty(Database))
                builder["Database"] = Database;

            if (!string.IsNullOrEmpty(Id))
                builder["User ID"] = Id;

            if (!string.IsNullOrEmpty(Password))
                builder["Password"] = Password;

            if (!string.IsNullOrEmpty(Charset) && DbType == 0)
                builder["Charset"] = Charset;

            if (ConvertZeroDatetime && DbType == 0)
                builder["Convert Zero Datetime"] = true;

            if (AllowUserVariables && DbType ==0)
                builder["Allow User Variables"] = true;

            if (DbType == 1 && TrustServerCertificate)
                builder["TrustServerCertificate"] = true;

            return builder.ConnectionString;
        }

        /// <summary>
        /// 資料庫類型，如 "MySQL" 或 "SQLServer"
        /// </summary>
        public required int DbType { get; set; }
    }

}
