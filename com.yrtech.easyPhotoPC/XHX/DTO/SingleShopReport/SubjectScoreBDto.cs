using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO.SingleShopReport
{
    /// <summary>
    /// 指标得分详情
    /// </summary>
    public class SubjectsScoreBDto
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
        /// 照片名称
        /// </summary>
        public string PicName { get; set; }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string SpCode { get; set; }
        public string AfterInvoiceCode { get; set; }
        public string B1 { get; set; }
        public string B2 { get; set; }
        public string B3 { get; set; }
        public string AfterInvoiceDate { get; set; }
        public string AfterInvoiceDMSDate { get; set; }
        public string AfterInvoiceMony { get; set; }
        public string AfterDMSMony { get; set; }
        public string Score { get; set; }
        
    }
}
