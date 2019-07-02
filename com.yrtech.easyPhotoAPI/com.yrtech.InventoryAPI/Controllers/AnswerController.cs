using System.Web.Http;
using com.yrtech.InventoryAPI.Service;
using com.yrtech.InventoryAPI.Common;
using System.Collections.Generic;
using System;
using com.yrtech.InventoryDAL;
using com.yrtech.InventoryAPI.Controllers;

namespace com.yrtech.SurveyAPI.Controllers
{

    [RoutePrefix("inventory/api")]
    public class AnswerController : ApiController
    {
        AnswerService answerService = new AnswerService();
        MasterService masterService = new MasterService();
        [HttpGet]
        [Route("Answer/GetShopAnswerList")]
        public APIResult GetShopAnswerList(string projectCode, string shopCode, string allChk, string vinCode)
        {
            try
            {
                List<Answer> answerList = answerService.GetShopAnswerList(projectCode, shopCode, allChk, vinCode);
                return new APIResult() { Status = true, Body = CommonHelper.Encode(answerList) };
            }
            catch (Exception ex)
            {
                return new APIResult() { Status = false, Body = ex.Message.ToString() };
            }

        }
        [HttpPost]
        [Route("Answer/SaveShopAnswer")]
        public APIResult SaveShopAnswer(Answer answer)
        {
            try
            {
                answerService.SaveShopAnswer(answer);
                return new APIResult() { Status = true, Body = "" };
            }
            catch (Exception ex)
            {
                return new APIResult() { Status = false, Body = ex.Message.ToString() };
            }
        }

        [HttpGet]
        [Route("Answer/DownloadAnswerList")]
        public APIResult DownloadAnswerList(string projectCode, string shopCode)
        {
            try
            {
                CommonController commonController = new CommonController();
                commonController.DownloadReport(projectCode, shopCode);
                return new APIResult() { Status = true, Body = "" };
            }
            catch (Exception ex)
            {
                return new APIResult() { Status = false, Body = ex.Message.ToString() };
            }

        }
    }
}
