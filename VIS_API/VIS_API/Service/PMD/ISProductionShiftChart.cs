﻿using VIS_API.Models.PMD;

namespace VIS_API.Service.PMD
{
    /// <summary>
    /// 產品推移圖服務介面
    /// </summary>
    public interface ISProductionShiftChart
    {
        /// <summary>
        /// 取得產品推移圖明細
        /// </summary>
        /// <returns></returns>
        List<Mwaitingfortheproduction_details> GetProductionShiftDetailList(WaitingfortheproductionParamater paramater);
    }
}