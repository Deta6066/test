using System.Threading;
using VIS_API.Models;
using VIS_API.Models.API;
using VIS_API.UnitWork;

namespace VIS_API.Service.Interface
{
    public interface IOrder
    {
        Task<VOrder?> GetList(OrderParameter parameter);

        //Task<List<Order>?> GetOrderOverdueList(MCompanyDataSource companyDataSource,OrderParameter? parameter = null);
        //Task<List<Order>?> GetOrderList(MCompanyDataSource companyDataSource, OrderParameter? parameter = null);

    }
}
