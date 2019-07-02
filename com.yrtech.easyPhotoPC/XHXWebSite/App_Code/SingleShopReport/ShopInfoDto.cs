using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO.SingleShopReport
{
    /// <summary>
    /// 经销商得分明细
    /// </summary>
    public class ShopInfoDto
    {
        public string ProjectCode { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string AreaName { get; set; }
        public string StartDate { get; set; }
        public DateTime sellStartDate { get; set; }
        public DateTime sellEndDate { get; set; }
        // 销售区间
        public string Invoiceregion { get; set; }
        public DateTime AfterEndDate { get;set; }
        public DateTime AfterStartDate { get; set; }
        //售后区间
        public string AfterInvoiceregion { get; set; }
        public string SellInvoiceCode { get; set; }
        public string AfterInvoiceCode { get; set; }
      
    }
}
