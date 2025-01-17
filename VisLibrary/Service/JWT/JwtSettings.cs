using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Service.JWT
{
    /// <summary>
    /// 儲存 JWT 配置，如 Key, Issuer, Audience
    /// </summary>
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
