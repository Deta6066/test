using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int CompanyFk { get; set; }
        public string Token { get; set; } // Refresh Token
        public string Account { get; set; } // 關聯的使用者
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

    }
}
