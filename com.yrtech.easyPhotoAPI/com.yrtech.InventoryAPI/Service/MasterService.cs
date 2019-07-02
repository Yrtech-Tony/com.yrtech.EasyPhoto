using com.yrtech.InventoryAPI.Common;
using com.yrtech.InventoryAPI.DTO;
using com.yrtech.InventoryDAL;
using com.yrtech.SurveyAPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace com.yrtech.InventoryAPI.Service
{
    public class MasterService
    {
        com.yrtech.InventoryDAL.InventoryDAL db = new InventoryDAL.InventoryDAL();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Projects> GetProject()
        {
            SqlParameter[] para = new SqlParameter[] { };
            Type t = typeof(Projects);
            string sql = "";
            sql = @"SELECT  [ProjectCode]
                  ,[ProjectName]
                  ,[InUserID]
                  ,[InDateTime]
                  ,[Year]
                  ,[Quarter]
                  ,[OrderNO]
                    FROM [Projects]
                    WHERE 1=1 ORDER BY OrderNO DESC  
                    ";
            return db.Database.SqlQuery(t, sql, para).Cast<Projects>().ToList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopCode"></param>
        /// <returns></returns>
        public List<Shop> GetShop(string shopCode)
        {
            if(shopCode==null)shopCode="";
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ShopCode", shopCode)};
            Type t = typeof(Shop);
            string sql = "";
            sql = @"SELECT [ShopCode]
                        ,[ShopName]
                        ,[InUserID]
                        ,[InDateTime]
                        ,[Password]
                        ,AreaCode
                      FROM [Shop]
                    WHERE  1=1 ";
          
            if (!string.IsNullOrEmpty(shopCode))
            {
                sql += " AND ShopCode = @ShopCode";
            }
            return db.Database.SqlQuery(t, sql, para).Cast<Shop>().ToList();
        }
        public List<Note> GetNote(string projectCode,string type)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectCode", projectCode)
                                                        , new SqlParameter("@Type", type) };
            Type t = typeof(Note);
            string sql = "";
            sql = @"SELECT *
                    FROM [Note]
                    WHERE 1=1   
                    ";
            if (!string.IsNullOrEmpty(projectCode))
            {
                sql += " AND ProjectCode = @ProjectCode";
            }
            if (!string.IsNullOrEmpty(type))
            {
                sql += " AND Type = @Type";
            }
            return db.Database.SqlQuery(t, sql, para).Cast<Note>().ToList();
        }
       
    }
}