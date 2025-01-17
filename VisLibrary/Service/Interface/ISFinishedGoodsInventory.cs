﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.View;
using VisLibrary.Models.WHE;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 成品庫存服務介面
    /// </summary>
    public interface ISFinishedGoodsInventory
    {
        /// <summary>
        /// 取得成品庫存明細
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public  Task<List<VMFinishGoodDetail>> GetFinishedGoodsInventoryDetail(int companyID, MInventoryParameter filter);
    }
}
