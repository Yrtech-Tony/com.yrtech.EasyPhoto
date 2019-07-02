using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO.SingleShopReport
{
    public class ShopReportDto
    {
        public List<ShopInfoDto> ShopInfoDtoList { get; set; }
        public List<AnswerInfoDto> AnswerInfoDtoList { get; set; }
        public List<PerTypeFailCountDto> PerTypeFailCountDtoList { get; set; }
        public List<SubjectsScoreBDto> SubjectsScoreBDtoList { get; set; }
        public List<SubjectsScoreADto> SubjectsScoreADtoList { get; set; }
        
    }
}
