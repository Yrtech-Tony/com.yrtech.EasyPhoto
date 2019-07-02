using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.Common;
using XHX.DTO;
using DevExpress.XtraEditors.Repository;
using Microsoft.Office.Interop.Excel;
//using XHX.WebService;

namespace XHX.View
{
    public partial class Shop : BaseForm
    {
        XtraGridDataHandler<ShopDto> dataHandler = null;
        localhost.Service webService = new localhost.Service();
        MSExcelUtil msExcelUtil = new MSExcelUtil();
       
        public Shop()
        {
            InitializeComponent();
            OnLoadView();
        }

        public void OnLoadView()
        {
            dataHandler = new XtraGridDataHandler<ShopDto>(grvShop);
        }
        public void InitializeView()
        {
            txtShopName.Text = "";
            grcShop.DataSource = null;
        }

        public override List<ButtonType> CreateButton()
        {
            List<ButtonType> list = new List<ButtonType>();
            list.Add(ButtonType.InitButton);
            list.Add(ButtonType.SearchButton);
            list.Add(ButtonType.AddRowButton);
           
            list.Add(ButtonType.SaveButton);
            list.Add(ButtonType.ExcelDownButton);
            return list;
        }
        public override void InitButtonClick()
        {
            base.InitButtonClick();
            InitializeView();
        }
        public override void SearchButtonClick()
        {
            SearchShop();
            if (base.UserInfoDto.RoleType != "C")
            {
                this.CSParentForm.EnabelButton(ButtonType.AddRowButton, true);
                this.CSParentForm.EnabelButton(ButtonType.SaveButton, true);
            }
            else
            {
                this.CSParentForm.EnabelButton(ButtonType.AddRowButton, false);
                this.CSParentForm.EnabelButton(ButtonType.SaveButton, false);
            }
        }
        public override void AddRowButtonClick()
        {
            ShopDto shop = new ShopDto();
            shop.AreaCode = "";
            shop.Password = "1111";
            shop.UseChk = true;
            dataHandler.AddRow(shop);
        }
        public override void SaveButtonClick()
        {
            grvShop.CloseEditor();
            grvShop.UpdateCurrentRow();

            if (base.UserInfoDto.RoleType != "S")
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有权限");
            }
            foreach (ShopDto shop in grcShop.DataSource as List<ShopDto>)
            {
                if (string.IsNullOrEmpty(shop.ShopCode))
                {
                    CommonHandler.ShowMessage(MessageType.Information, "经销商代码不能为空");
                    grvShop.FocusedColumn = gcShopCode;
                    grvShop.FocusedRowHandle = (grcShop.DataSource as List<ShopDto>).IndexOf(shop);
                    return;
                }
                foreach (ShopDto s in dataHandler.DataList)
                {
                    if (s != shop)
                    {
                        if (s.ShopCode == shop.ShopCode)
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "经销商代码不能重复");
                            grvShop.FocusedColumn = gcShopCode;
                            grvShop.FocusedRowHandle = (grcShop.DataSource as List<ShopDto>).IndexOf(s);
                            return;
                        }
                    }
                }
            }
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<ShopDto> shopList = dataHandler.DataList;
                foreach (ShopDto shop in shopList)
                {
                    if (shop.StatusType == 'I' || shop.StatusType == 'U')
                    {
                        webService.SaveShop(shop.ShopCode,shop.ShopName,shop.UseChk,shop.Password,shop.AreaCode,this.UserInfoDto.UserID);
                    }
      
                }
            }
            SearchShop();
            CommonHandler.ShowMessage(MessageType.Information, "保存完毕");
        }
        public override void ExcelDownButtonClick()
        {
            CommonHandler.ExcelExport(grvShop);
        }
        private void SearchShop()
        {
            List<ShopDto> shopList = new List<ShopDto>();
            DataSet ds = webService.SearchShop("", txtShopName.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopDto shop = new ShopDto();
                    shop.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    shop.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    shop.UseChk = Convert.ToBoolean(ds.Tables[0].Rows[i]["UseChk"]);
                    shop.Password = Convert.ToString(ds.Tables[0].Rows[i]["Password"]);
                    shop.AreaCode = Convert.ToString(ds.Tables[0].Rows[i]["AreaCode"]);
                    shopList.Add(shop);
                }
            }
            grcShop.DataSource = shopList;
        }

        private void Shop_Load(object sender, EventArgs e)
        {
            this.CSParentForm.EnabelButton(ButtonType.AddRowButton, false);
            this.CSParentForm.EnabelButton(ButtonType.SaveButton, false);
        }
        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            ShopDto shop = grvShop.GetRow(grvShop.FocusedRowHandle) as ShopDto;
            if (grvShop.FocusedColumn == gcShopCode)
            {
                if (shop.StatusType != 'I')
                {
                    e.Cancel = true;

                }
                else
                {
                    e.Cancel = false;
                }
            }
        }
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (grcShop.DataSource == null || grvShop.RowCount == 0) return;
            ShopDto shop = grvShop.GetRow(e.RowHandle) as ShopDto;
            if (e.Column == gcShopCode && shop.StatusType != 'I')
            {
                e.Appearance.BackColor = Color.Gray;
            }
        }
        private void btnPassword_Click(object sender, EventArgs e)
        {
            List<string> str = new List<string>();
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要生成吗？") == DialogResult.Yes)
            {
                for (int i = 0; i < grvShop.RowCount; i++)
                {
                    Random r = new Random(i);
                    int value = r.Next(100000, 999999);
                    grvShop.SetRowCellValue(i, gcPassword, value.ToString());
                }
            }
        }

        private void btnModule_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.Filter = "Excel(*.xlsx)|";
            ofp.FilterIndex = 2;
            if (ofp.ShowDialog() == DialogResult.OK)
            {
                btnModule.Text = ofp.FileName;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
                Worksheet worksheet_FengMian = workbook.Worksheets["经销商"] as Worksheet;
                for (int i = 2; i < 1000; i++)
                {
                    string shopCode = msExcelUtil.GetCellValue(worksheet_FengMian, "A", i);
                    if (string.IsNullOrEmpty(shopCode)) break;
                    if (!string.IsNullOrEmpty(shopCode))
                    {
                        string shopName = msExcelUtil.GetCellValue(worksheet_FengMian, "B", i);
                        string areaCode = msExcelUtil.GetCellValue(worksheet_FengMian, "C", i);
                        webService.SaveShop(shopCode, shopName, true, "1111", areaCode, this.UserInfoDto.UserID);
                    }
                }
                SearchShop();
                CommonHandler.ShowMessage(MessageType.Information, "上传完毕");
            }
            catch (Exception ex)
            {

                CommonHandler.ShowMessage(ex);
            }
          
           
        }
    }


}
