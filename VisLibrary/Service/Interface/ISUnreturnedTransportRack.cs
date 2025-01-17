﻿using VisLibrary.Models;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 未歸還項目介面
    /// </summary>
    public interface ISUnreturnedTransportRack
    {
        Task<List<MUnreturnedTransportRack>> GetMUnreturnedTransports(MUnreturnedTransportRackParamater filter);
    }
}