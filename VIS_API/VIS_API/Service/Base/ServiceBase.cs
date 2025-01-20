using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.API;
using VIS_API.Models.ViewModel;
using VIS_API.Models;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Interface;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace VIS_API.Service.Base
{
    public abstract class ServiceBase(IOptions<SqlCmdConfig> sqlCmdConfig)
    {
        private readonly SqlCmdConfig _sqlCmdConfig = sqlCmdConfig.Value;

        public string GetSqlCommand(string section, string command)
        {
            return section.ToLower() switch
            {
                "assemblecenter" => GetCommandFromSection(_sqlCmdConfig.assemblecenter, command),
                "productionlinegroup" => GetCommandFromSection(_sqlCmdConfig.productionLineGroup, command),
                "companydatasource" => GetCommandFromSection(_sqlCmdConfig.companydatasource, command),
                _ => throw new ArgumentException("Invalid section name")
            };
        }

        private string GetCommandFromSection(object sectionConfig, string command)
        {
            var property = sectionConfig.GetType().GetProperty(command, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return property != null ? property.GetValue(sectionConfig)?.ToString() ?? string.Empty : string.Empty;
        }
    }
}
