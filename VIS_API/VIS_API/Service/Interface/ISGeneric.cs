using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.API;
using VIS_API.Models;

namespace VIS_API.Service.Interface
{
    public interface ISGeneric
    { /// <summary>
      /// 定義一個 ViewOrder資料型別
      /// </summary>
      /// <param name="parameter"></param>
      /// <returns></returns>
        Task<List<Order>> GetList(OrderParameter? parameter = null);
    }
}
