using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Repositories.Interface;

/// <summary>
/// 提供異常處理的幫助類。
/// </summary>
namespace VIS_API.Utilities
{
    public class ExceptionHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(IServiceProvider serviceProvider, ILogger<ExceptionHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<IActionResult> ExecuteWithExceptionHandlingAsync(Func<Task<IActionResult>> func, Action<Exception> exceptionHandler)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                LogException(ex);
                await LogExceptionToDatabase(ex);
                exceptionHandler(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<T?> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (TimeoutException ex)
            {
                LogException(ex);
                await LogExceptionToDatabase(ex);
                return default;
            }
            catch (Exception ex)
            {
                LogException(ex);
                await LogExceptionToDatabase(ex);
                return default;
            }
            
        }

        public async Task ExecuteWithExceptionHandlingAsync(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                LogException(ex);
                await LogExceptionToDatabase(ex);
            }
        }

        /// <summary>
        /// 執行包含異常處理的異步操作，並返回結果。
        /// </summary>
        /// <typeparam name="T">結果類型。</typeparam>
        /// <param name="func">要執行的異步操作。</param>
        /// <param name="exceptionHandler">處理異常的委託。</param>
        /// <returns>操作結果。如果發生異常，返回默認值。</returns>
        public async Task<T?> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> func, Action<Exception> exceptionHandler)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                LogException(ex);
                await LogExceptionToDatabase(ex);
               // exceptionHandler(ex);
                return default;
            }
        }

        /// <summary>
        /// 執行包含異常處理的同步操作。
        /// </summary>
        /// <param name="action">要執行的操作。</param>
        /// <param name="exceptionHandler">處理異常的委託。</param>
        public void ExecuteWithExceptionHandling(Action action, Action<Exception> exceptionHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogException(ex);
                LogExceptionToDatabase(ex).Wait();
                exceptionHandler(ex);
            }
        }

        /// <summary>
        /// 執行包含異常處理的異步操作，不返回結果。
        /// </summary>
        /// <param name="func">要執行的異步操作。</param>
        /// <param name="exceptionHandler">處理異常的委託。</param>
        /// <returns>Task。</returns>
        public async Task ExecuteWithExceptionHandlingAsync(Func<Task> func, Action<Exception> exceptionHandler)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                LogException(ex);
                await LogExceptionToDatabase(ex);
                exceptionHandler(ex);
            }
        }

        /// <summary>
        /// 記錄異常信息到文件。
        /// </summary>
        /// <param name="ex">捕獲的異常。</param>
        private void LogException(Exception ex)
        {

            LogLevel logLevel = ex switch
            {
                ArgumentNullException => LogLevel.Error,
                ArgumentException => LogLevel.Error,
                InvalidOperationException => LogLevel.Warning,
                NullReferenceException => LogLevel.Error,
                FileNotFoundException => LogLevel.Error,
                UnauthorizedAccessException => LogLevel.Warning,
                NotSupportedException => LogLevel.Warning,
                TimeoutException => LogLevel.Warning,
                OperationCanceledException => LogLevel.Information,
                _ => LogLevel.Error,
            };

            _logger.Log(logLevel, ex, "An exception occurred: {Message}", ex.Message);

        }

        /// <summary>
        /// 將異常信息記錄到資料庫。
        /// </summary>
        /// <param name="ex">捕獲的異常。</param>
        private async Task LogExceptionToDatabase(Exception ex)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var errorLogRepository = scope.ServiceProvider.GetRequiredService<IErrorLogRepository>();
                    var errorLog = new ErrorLog
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Timestamp = DateTime.Now,
                        SourceIp = "Unknown" // 根據需要設定 SourceIp
                    };
                    await errorLogRepository.LogErrorAsync(errorLog);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error logging exception to database");
            }
        }

        /// <summary>
        /// 判斷屬性值是否為其類型的默認值。
        /// </summary>
        /// <param name="value">要檢查的屬性值。</param>
        /// <returns>如果屬性值為默認值，返回 true；否則返回 false。</returns>
        public static bool IsDefaultValue(object value)
        {
            var type = value.GetType();
            return value.Equals(Activator.CreateInstance(type));
        }

        /// <summary>
        /// 執行包含異常處理的操作。
        /// </summary>
        /// <param name="action">要執行的操作。</param>
        public void ExecuteWithExceptionHandling(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// 執行包含異常處理的操作，並返回結果。
        /// </summary>
        /// <typeparam name="T">結果類型。</typeparam>
        /// <param name="func">要執行的操作。</param>
        /// <returns>操作結果。</returns>
        public T? ExecuteWithExceptionHandling<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                LogException(ex);
                return default;
            }
        }
    }
}
