using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XHX.Common;

namespace XHX.DTO
{
    [Serializable]
    public class ShopInstandardInfo : BufferColumnDto,IShopInstandardInfo
    {
        public override DynamicData CopyKeyMembers(DynamicData data)
        {
            IShopInstandardInfo that = data as IShopInstandardInfo;
            if (that == null) return null;
            that.ShopCode = this.ShopCode;
            return data;
        }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
    }
    public interface IShopInstandardInfo
    {
        string ShopCode { get; set; }
        //string SeqNO { get; set; }
    }
    [Serializable]
    public class ShopInstandardBodyDto : TwoLevelColumnData, IShopInstandardInfo
    {
        public string ShopCode
        {
            get;
            set;
        }
        public string SeqNO { get; set; }
        public override DynamicData CopyKeyMembers(DynamicData data)
        {
            IShopInstandardInfo that = data as IShopInstandardInfo;
            if (that == null) return null;
            that.ShopCode = this.ShopCode;
            //that.SeqNO = this.SeqNO;
            return data;
        }

        public override bool IsSameRow(BufferColumnDto dto)
        {
            IShopInstandardInfo that = dto as IShopInstandardInfo;
            if (that == null) return false;
            return this.ShopCode.Equals(that.ShopCode);
        }
    }
}
