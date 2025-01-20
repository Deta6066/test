using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VIS_API.Utilities
{
    public class SafeStringAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var input = value.ToString();

            // SQL Injection patterns (simple example)
            var sqlInjectionPattern = new Regex(@"('(\s|--|\w)*--)|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE)?|INSERT|MERGE|SELECT|UPDATE|UNION|USE)\b)", RegexOptions.IgnoreCase);
            if (sqlInjectionPattern.IsMatch(input))
            {
                return new ValidationResult("Potential SQL Injection attack detected.");
            }

            // XSS patterns (simple example)
            var xssPattern = new Regex(@"<script|<img|<iframe|<a\s+href|<link", RegexOptions.IgnoreCase);
            if (xssPattern.IsMatch(input))
            {
                return new ValidationResult("Potential XSS attack detected.");
            }

            return ValidationResult.Success;
        }
    }
}
