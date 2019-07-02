using System.Web.Http;
using com.yrtech.InventoryAPI.Service;
using com.yrtech.InventoryAPI.Common;
using System.Collections.Generic;
using System;
using com.yrtech.InventoryDAL;

namespace com.yrtech.InventoryAPI.Controllers
{
    [RoutePrefix("inventory/api")]
    public class MasterController : ApiController
    {
        AnswerService answerService = new AnswerService();
        MasterService masterService = new MasterService();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Master/GetProject")]
        public APIResult GetProject()
        {
            try
            {
                List<Projects> projectList = masterService.GetProject();
                return new APIResult() { Status = true, Body = CommonHelper.Encode(projectList) };
            }
            catch (Exception ex)
            {
                return new APIResult() { Status = false, Body = ex.Message.ToString() };
            }

        }
        [HttpGet]
        [Route("Master/GetShop")]
        public APIResult GetShop(string shopCode)
        {
            try
            {
                List<Shop> shopList = masterService.GetShop(shopCode); ;
                return new APIResult() { Status = true, Body = CommonHelper.Encode(shopList) };
            }
            catch (Exception ex)
            {
                return new APIResult() { Status = false, Body = ex.Message.ToString() };
            }

        }
        [HttpGet]
        [Route("Master/GetNote")]
        public APIResult GetNote(string projectCode,string type)
        {
            try
            {
                List<Note> noteList = masterService.GetNote(projectCode,type); ;
                return new APIResult() { Status = true, Body = CommonHelper.Encode(noteList) };
            }
            catch (Exception ex)
            {
                return new APIResult() { Status = false, Body = ex.Message.ToString() };
            }

        }
    }
}
