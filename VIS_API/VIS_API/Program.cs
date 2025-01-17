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

// �]�w JSON �ǦC�ƿﶵ�A���ϥιw�]���R�W�W�h
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// �]�w��x
builder.AddLogger();

// �[�J�ۭq���A��
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomSwagger();

// CORS �]�w
builder.Services.AddCors(options =>
{
    // �]�w���\�Ҧ��ӷ��� CORS ����
    options.AddPolicy("AllowAll",
        policy =>
        {       // ���w���\���ӷ�
            policy.WithOrigins("http://localhost:5173", "http://localhost:9898", "http://192.168.101.171:100", "http://192.168.101.171:5153") 
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// �]�w���v
builder.Services.AddAuthorization();

// �]�w API �欰�ﶵ
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // ���o���~��x�ܮw�A��
        var errorLogRepository = context.HttpContext.RequestServices.GetRequiredService<IErrorLogRepository>();
        var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
        var sourceIp = remoteIpAddress != null ? remoteIpAddress.ToString() : "Unknown";

        if (sourceIp == "::1")
        {
            sourceIp = "127.0.0.1"; // ���� IP
        }

        // �O�����~��x
        var errorLog = new ErrorLog
        {
            Message = "Invalid model state",
            StackTrace = string.Join("; ", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
            Timestamp = DateTime.Now,
            SourceIp = sourceIp
        };

        // �P�B�O�����~
        errorLogRepository.LogErrorAsync(errorLog).Wait();

        // �^�ǿ��~�T��
        var result = new BadRequestObjectResult(context.ModelState);
        return result;
    };
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();


// �ϥΦۭq�����~�B�z�����n��
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error(e.ExceptionObject as Exception, "���B�z�����`");
};

TaskScheduler.UnobservedTaskException += (sender, e) =>
{
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error(e.Exception, "���[��쪺���Ȳ��`");
    e.SetObserved(); // �аO���w�[��A�������ε{���Y��
};
//app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// �ϥ� Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

// �ϥ�CORS����
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();
//app.UseCors("AllowSpecificOrigin");
// �ϥΦۭq�� JwtCookieMiddleware�A�T�O�b Authentication �e
app.UseMiddleware<JwtCookieMiddleware>();
// �`�J SingleIPMiddleware
app.UseMiddleware<SingleIPMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    app.Run(); // �Ұ����ε{��
}
catch (Exception ex)
{
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error(ex, "�]���o�ͨҥ~���p�A����{��");
    throw;
}
finally
{
    LogManager.Shutdown(); // ������x�޲z
}
