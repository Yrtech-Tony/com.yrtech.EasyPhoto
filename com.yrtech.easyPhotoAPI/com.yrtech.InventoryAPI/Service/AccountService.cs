using com.yrtech.InventoryAPI.DTO;
using com.yrtech.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace com.yrtech.InventoryAPI.Service
{
    public class AccountService
    {
        com.yrtech.InventoryDAL.InventoryDAL db = new InventoryDAL.InventoryDAL();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<ShopDto> LoginForMobile(string accountId, string password)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@AccountId", accountId),
                                                       new SqlParameter("@Password",password)};
            Type t = typeof(ShopDto);
            string sql = @"SELECT * FROM Shop WHERE ShopCode = @AccountId AND Password =  @Password";
            return db.Database.SqlQuery(t, sql, para).Cast<ShopDto>().ToList();
        }
    }
}