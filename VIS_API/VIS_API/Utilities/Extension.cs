using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace VIS_API.Utilities
{
    public static partial class Extension
    {
        #region object

        public static string Text(this object? obj)
        {
            return (obj ?? "").ToString() ?? "";
        }

        public static string Left(this object? obj, int length)
        {
            if (length > 0)
            {
                string text = Text(obj);
                return text.Length > length ? text.Substring(0, length) : text;
            }
            else
                return "";
        }

        public static string Right(this object? obj, int length)
        {
            string text = Text(obj);
            return text.Length > length ? text.Substring(text.Length - length) : text;
        }

        public static bool IsEmpty(this object? obj)
        {
            return Text(obj).Trim() == "";
        }

        public static int Int(this object? obj)
        {
            return decimal.TryParse(Text(obj), out decimal ret) ? (int)ret : 0;
        }

        public static uint UInt(this object? obj)
        {
            return decimal.TryParse(Text(obj), out decimal ret) ? (uint)ret : 0;
        }

        public static string Int(this object? obj, string format)
        {
            return Int(obj).ToString(format);
        }

        public static decimal Decimal(this object? obj)
        {
            return decimal.TryParse(Text(obj), out decimal ret) ? ret : 0;
        }

        public static string Decimal(this object? obj, string format)
        {
            // #,##0.00
            // 0.##
            return Decimal(obj).ToString(format);
        }

        public static DateTime? Date(this object? obj)
        {
            return DateTime.TryParse(Text(obj), out DateTime ret) ? (DateTime?)ret : null;
        }

        public static string Date(this object? obj, string format)
        {
            // yyyy-MM-dd HH:mm:ss
            return (System.DateTime.TryParse(Text(obj), out DateTime ret) ? ret.ToString(format) : "");
        }

        public static string MD5(this object? obj)
        {
            var dataBytes = Encoding.UTF8.GetBytes(Text(obj));
            var hashBytes = System.Security.Cryptography.MD5.HashData(dataBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static string Json(this object? obj)
        {
            if (obj is Task)
                throw new InvalidOperationException("Object of type \"Task\" cannot be converted to JSON format.");
            else
                return JsonConvert.SerializeObject(obj);
        }

        public static string JavaScriptStringEncode(this object? obj)
        {
            return HttpUtility.JavaScriptStringEncode(Text(obj));
        }

        public static string UrlEncode(this object? obj)
        {
            return HttpUtility.UrlEncode(Text(obj));
        }

        #endregion

        #region Aes

        /// <summary>加密</summary>
        public static string Encrypt(this Aes obj, object value)
        {
            if (value.IsEmpty())
                return "";
            else
            {
                using (MemoryStream ms = new())
                using (CryptoStream cs = new(ms, obj.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new(cs))
                {
                    sw.Write(value);
                    sw.Flush();

                    cs.FlushFinalBlock();

                    return string.Join("", ms.ToArray().Select(x => x.ToString("x2")));
                }
            }
        }

        /// <summary>解密</summary>
        public static string Decrypt(this Aes obj, string value)
        {
            if (value.IsEmpty())
                return "";
            else
            {
                byte[] bytes = Enumerable.Range(0, value.Length)
                    .Where(i => i % 2 == 0)
                    .Select(i => Convert.ToByte(value.Substring(i, 2), 16)).ToArray();

                using (MemoryStream ms = new(bytes))
                using (CryptoStream cs = new(ms, obj.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        #endregion
    }

    //-------------------------------------------------------------------------------------------------
    // 常用範例
    //-------------------------------------------------------------------------------------------------
    // list1.Join(list2,
    //    t1 => t1.pk, t2 => t2.fk,
    //    (t1, t2) => t1).ToList(); // (t1, t2) => new {...}).ToList();
    //-------------------------------------------------------------------------------------------------
    //Enumerable.Range(0, 100).Select(x => ...).ToList();
    //-------------------------------------------------------------------------------------------------
    //var regex = new Regex(@"[a-zA-Z][1-2][0-9]{8}$");
    //if (!regex.IsMatch(uid))
    //    throw new MyException("身分證字號格式不正確", "第一碼必須是英文字母，第二碼必須是1或2，其餘八碼必須是數字");
    //-------------------------------------------------------------------------------------------------
    //public static bool IsUnique(int pk, string uid)
    //{
    //    string sql = @"
    //SELECT COUNT(*)
    //FROM `member`
    //WHERE `pk` <> ?pk 
    //AND `uid` = ?uid
    //;";
    //    using (MySqlConnection conn = new MySqlConnection(Db.ConnectionString))
    //    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
    //    {
    //        cmd.Parameters.AddWithValue("?pk", pk);
    //        cmd.Parameters.AddWithValue("?uid", uid);
    //        conn.Open();
    //        return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
    //    }
    //}
    // 自動產生流水編 -----------------------------------------------------------------------------------
    //SET @length = 4;
    //SET @date = DATE_FORMAT(NOW(), '%Y%m%d');
    //SET @sn = (SELECT IFNULL(MAX(RIGHT(`id`, @length)),0) FROM `project` WHERE LEFT(`id`,8) = binary @date);
    //SET @id = CONCAT(@date, RIGHT(CONCAT('0000', @sn + 1), @length));
}