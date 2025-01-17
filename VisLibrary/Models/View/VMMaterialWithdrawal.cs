﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.PCD;

namespace VisLibrary.Models.View
{
    public class VMMaterialWithdrawal
    {
        /// <summary>
        /// 領料單列表
        /// </summary>
        public List<MMaterialWithdrawal> MaterialWithdrawalList { get; set; }
        /// <summary>
        /// 領料單明細列表
        /// </summary>
        public List<MMaterialWithdrawalDetail> MMaterialWithdrawalDetails { get; set; }
    }
}
