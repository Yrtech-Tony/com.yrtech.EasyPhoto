using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO
{
    public class ShopVinListDto
    {
        public string ProjectCode { get; set; }
        public string VinCode { get; set; }
        public string ShopCode { get; set; }
        public string AreaCode { get; set; }
        public string Type { get; set; }
        public char StatusType { get; set; }
      
        public string PhotoName { get; set; }
        public string Remark { get; set; }
        public string AddChk { get; set; }
        public string InUserId { get; set; }
        public string InDateTime { get; set; }
    }
}
