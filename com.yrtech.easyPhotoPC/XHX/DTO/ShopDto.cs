using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO
{
    public class ShopDto
    {
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string SaleSmall { get; set; }
        public string SaleBig { get; set; }
        public string AfterSmall { get; set; }
        public string AfterBig { get; set; }
        public bool UseChk { get; set; }
        public char StatusType { get; set; }
        public string UserName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public bool LastRecheck { get; set; }
        public string Password { get; set; }
        public string ShopNamePY { get; set; }
        public string ProvincePY { get; set; }
        public string CityPY { get; set; }
        public string Email { get; set; }
        public string InvoiceCode { get; set; }
    }
}
