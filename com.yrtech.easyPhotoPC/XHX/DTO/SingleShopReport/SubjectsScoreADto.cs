using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO.SingleShopReport
{
    /// <summary>
    /// 指标得分详情
    /// </summary>
    public class SubjectsScoreADto
    {
        /// <summary>
        /// 体系编号
        /// </summary>
        public string SubjectCode { get; set; }
        /// <summary>
        /// 失分说明
        /// </summary>
        public string LoseDesc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 照片名称~
        /// </summary>
        public string PicName { get; set; }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string SpCode { get; set; }
        public string SellInvoiceCode { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }
        public string A5 { get; set; }
        public string SellCustomerName { get; set; }
        public string SellVINCode{get;set;}
        public string SellInvoiceDate{get;set;}
        public string SellInvoiceDMSDate { get; set; }
        public string Score { get; set; }
    }
}
