using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System;
using System.Text;
using VisLibrary.Extensions;
using VisLibrary.Middleware;
using VisLibrary.Models;
using VisLibrary.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// 設定 JSON 序列化選項，不使用預設的命名規則
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// 設定日誌
builder.AddLogger();

// 加入自訂的服務
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomSwagger();

// CORS 設定
builder.Services.AddCors(options =>
{
    // 設定允許所有來源的 CORS 策略
    options.AddPolicy("AllowAll",
        policy =>
        {       // 指定允許的來源
            policy.WithOrigins("http://localhost:5173", "http://localhost:9898", "http://192.168.101.171:100", "http://192.168.101.171:5153") 
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// 設定授權
builder.Services.AddAuthorization();

// 設定 API 行為選項
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // 取得錯誤日誌倉庫服務
        var errorLogRepository = context.HttpContext.RequestServices.GetRequiredService<IErrorLogRepository>();
        var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
        var sourceIp = remoteIpAddress != null ? remoteIpAddress.ToString() : "Unknown";

        if (sourceIp == "::1")
        {
            sourceIp = "127.0.0.1"; // 本機 IP
        }

        // 記錄錯誤日誌
        var errorLog = new ErrorLog
        {
            Message = "Invalid model state",
            StackTrace = string.Join("; ", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
            Timestamp = DateTime.Now,
            SourceIp = sourceIp
        };

        // 同步記錄錯誤
        errorLogRepository.LogErrorAsync(errorLog).Wait();

        // 回傳錯誤訊息
        var result = new BadRequestObjectResult(context.ModelState);
        return result;
    };
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();


// 使用自訂的錯誤處理中介軟體
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error(e.ExceptionObject as Exception, "未處理的異常");
};

TaskScheduler.UnobservedTaskException += (sender, e) =>
{
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error(e.Exception, "未觀察到的任務異常");
    e.SetObserved(); // 標記為已觀察，防止應用程式崩潰
};
//app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 使用 Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

// 使用CORS策略
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();
//app.UseCors("AllowSpecificOrigin");
// 使用自訂的 JwtCookieMiddleware，確保在 Authentication 前
app.UseMiddleware<JwtCookieMiddleware>();
// 注入 SingleIPMiddleware
app.UseMiddleware<SingleIPMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    app.Run(); // 啟動應用程式
}
catch (Exception ex)
{
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error(ex, "因為發生例外狀況，停止程式");
    throw;
}
finally
{
    LogManager.Shutdown(); // 關閉日誌管理
}
