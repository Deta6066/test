using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// ���k�ٶ���Model
    /// </summary>
    public class MUnreturnedTransportRack
    {
        //id,�Ȥ�W��,�~��,�ƶq
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// �Ȥ�W��
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// �~��
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// ���`�ƶq
        /// </summary>
        public int NormalQuantity { get; set; }
        /// <summary>
        /// ���`�ƶq
        /// </summary>
        public int AbnormalQuantity { get; set; }
    }
}