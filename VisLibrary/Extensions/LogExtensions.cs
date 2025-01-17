using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Extensions
{
    /// <summary>
    /// 提供自定義日誌配置的擴展方法。
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// 添加自定義日誌配置，包括 NLog 配置。
        /// </summary>
        /// <param name="builder">Web 應用程序構建器。</param>
        public static void AddLogger(this WebApplicationBuilder builder)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();
        }
    }
}
