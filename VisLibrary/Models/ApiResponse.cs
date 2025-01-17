using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public ResponseStatusCode StatusCode { get; set; }

        public T? Data { get; set; }
        public string? Message { get; set; }

        // 可以提供建構子以便於快速建立回傳物件
        public ApiResponse(T data, bool success = true, ResponseStatusCode statusCode = ResponseStatusCode.Ok, string? message = null)
        {
            Success = success;
            StatusCode = statusCode;
            Data = data;
            Message = message;
        }
    }
    public enum ResponseStatusCode
    {
        // 成功類
        Ok = 200,                 // 請求成功
        Created = 201,            // 資源已成功建立
        Accepted = 202,           // 請求已接收但尚未處理完成
        NoContent = 204,          // 請求成功但無內容可返回

        // 客戶端錯誤類
        BadRequest = 400,         // 無效的請求，可能是參數有誤
        Unauthorized = 401,       // 未經授權或身份驗證失敗
        Forbidden = 403,          // 用戶已認證但無權存取該資源
        NotFound = 404,           // 資源不存在
        Conflict = 409,           // 請求衝突（如資料重複）
        UnprocessableEntity = 422,// 請求格式正確，但語意有問題(如驗證失敗)
        TooManyRequests = 429,    // 請求次數過多，達到頻率限制

        // 自定義邏輯相關
        ValidationError = 450,    // 自訂的驗證錯誤 (如欄位不符商業規則)
        BusinessLogicError = 460, // 商業邏輯錯誤（如庫存不足、預定單不符條件）
                                  // 您可依業務需求新增更多特定錯誤代碼

        // 伺服器錯誤類
        InternalServerError = 500,  // 伺服器端錯誤
        NotImplemented = 501,       // 尚未實作的功能
        BadGateway = 502,           // 網關或代理遇到問題
        ServiceUnavailable = 503,   // 服務暫時不可用（例如系統維護中）
        GatewayTimeout = 504        // 網關超時
    }
}
