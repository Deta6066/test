namespace VisLibrary.Models.ViewModel
{
    /// <summary>
    /// 未交易客戶
    /// </summary>
    public class MUntradedCustomerModel
    {
        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// 最後交易日
        /// </summary>
        public string? LastTransactionDate { get; set; }
        /// <summary>
        /// 未交易天數
        /// </summary>
        public string? UntradedDays { get; set; }
    }
}
