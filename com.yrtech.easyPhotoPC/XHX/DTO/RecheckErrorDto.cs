using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO
{
    public class RecheckErrorDto
    {
        public string ErrorTypeCode { get; set; }
        public string ErrorTypeName { get; set; }
        public string Remark { get; set; }
        public string InUserID { get; set; }
        public DateTime InDateTime { get; set; }
        public char StatusType { get; set; }
    }
}
