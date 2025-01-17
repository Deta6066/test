using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Repositories.Base
{
    /// <summary>
    /// 標記一個屬性為主鍵。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// 標記一個屬性在插入操作中應被忽略。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InsertIgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// 標記一個屬性在更新操作中應被忽略。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UpdateIgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// 標記一個屬性用於搜尋查詢，並指定查詢操作類型。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchKeyAttribute : Attribute
    {
        /// <summary>
        /// 查詢操作類型。
        /// </summary>
        public SearchOperator Operator { get; }

        /// <summary>
        /// 初始化一個新的 <see cref="SearchKeyAttribute"/> 實例。
        /// </summary>
        /// <param name="searchOperator">查詢操作類型，預設為 <see cref="SearchOperator.Equals"/>。</param>
        public SearchKeyAttribute(SearchOperator searchOperator = SearchOperator.Equals)
        {
            Operator = searchOperator;
        }
    }

    /// <summary>
    /// 定義查詢操作類型。
    /// </summary>
    public enum SearchOperator
    {
        /// <summary>
        /// 等於。
        /// </summary>
        Equals,

        /// <summary>
        /// 包含（LIKE）。
        /// </summary>
        Contains,

        /// <summary>
        /// 大於。
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 小於。
        /// </summary>
        LessThan,

        /// <summary>
        /// 大於或等於。
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// 小於或等於。
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// 不等於。
        /// </summary>
        NotEquals
    }
}
