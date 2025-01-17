using System.Threading;
using VisLibrary.Models;
using VisLibrary.Models.API;
using VisLibrary.UnitWork;

namespace VisLibrary.Service.Interface
{
    public interface IOrder
    {
        Task<VOrder?> GetList(OrderParameter parameter);

        //Task<List<Order>?> GetOrderOverdueList(MCompanyDataSource companyDataSource,OrderParameter? parameter = null);
        //Task<List<Order>?> GetOrderList(MCompanyDataSource companyDataSource, OrderParameter? parameter = null);

    }
}
