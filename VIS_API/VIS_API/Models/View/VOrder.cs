namespace VIS_API.Models.API
{
    public class VOrder
    {
        public List<Order>? OrderList { get; set; }
        public List<Order>? OrderOverdue { get; set; }
        public List<MAssembleCenter>? Assemblecenter { get; set; }
        public List<string>? assembleGroup { get; set; }

        public string? Factory { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string? OrderType { get; set; }


    }
}
