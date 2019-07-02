using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO.SingleShopReport
{
    public class AnswerInfoDto
    {
       
        /// <summary>
        /// 发票数量
        /// </summary>
        public int InvoiceCount { get; set; }


        /// <summary>
        /// 发票不合格数量
        /// </summary>
        public int FailInvoicCount { get; set; }


        /// <summary>
        /// 不合格率
        /// </summary>
        public decimal FailInvoicePercent { get; set; }


        /// <summary>
        /// 类型:1.销售 2.售后
        /// </summary>
        public string InvoiceType { get; set; }
        public string LocalCount { get; set; } 

    }
}
