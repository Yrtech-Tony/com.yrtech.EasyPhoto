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
using System.IO;
//using XHX.WebService;

namespace XHX.View
{
    public partial class ShopVinList : BaseForm
    {
        XtraGridDataHandler<ShopVinListDto> dataHandler = null;
        localhost.Service webService = new localhost.Service();
        MSExcelUtil msExcelUtil = new MSExcelUtil();

        public ShopVinList()
        {
            InitializeComponent();
            OnLoadView();
        }

        public void OnLoadView()
        {
            BindComBox.BindProject(cboProjects);
            dataHandler = new XtraGridDataHandler<ShopVinListDto>(grvShop);
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
            ShopVinListDto shop = new ShopVinListDto();
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
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<ShopVinListDto> shopList = dataHandler.DataList;
                foreach (ShopVinListDto shop in shopList)
                {
                    if (shop.StatusType == 'I' || shop.StatusType == 'U')
                    {
                        webService.ShopVinListSave(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString(),shop.VinCode,shop.ShopCode,shop.Type);
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
            List<ShopVinListDto> shopList = new List<ShopVinListDto>();
            DataSet ds = webService.ShopVinListSearch(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString(),txtVin.Text.Trim(),txtShopName.Text.Trim());
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopVinListDto shop = new ShopVinListDto();
                    shop.ProjectCode = Convert.ToString(ds.Tables[0].Rows[i]["ProjectCode"]);
                    shop.VinCode = Convert.ToString(ds.Tables[0].Rows[i]["VinCode"]);
                    shop.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                  
                    shop.AreaCode = Convert.ToString(ds.Tables[0].Rows[i]["AreaCode"]);
                    shop.Type = Convert.ToString(ds.Tables[0].Rows[i]["Type"]);
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
            try
            {
                //ShopVinListDto shop = grvShop.GetRow(grvShop.FocusedRowHandle) as ShopVinListDto;
                //if (grvShop.FocusedColumn == gcShopCode)
                //{
                //    if (shop.StatusType != 'I')
                //    {
                //        e.Cancel = true;

                //    }
                //    else
                //    {
                //        e.Cancel = false;
                //    }
                //}
            }
            catch (Exception ex)
            { 
            
            }
            
        }
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (grcShop.DataSource == null || grvShop.RowCount == 0) return;
                ShopVinListDto shop = grvShop.GetRow(e.RowHandle) as ShopVinListDto;
                if (e.Column == gcShopCode && shop.StatusType != 'I')
                {
                    e.Appearance.BackColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                
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
                Worksheet worksheet_FengMian = workbook.Worksheets["CheckList"] as Worksheet;

                DateTime dtStart = DateTime.Now;
                for (int i = 2; i < 1000000; i++)
                {
                    string shopCode = msExcelUtil.GetCellValue(worksheet_FengMian, "A", i).Trim();
                    if (string.IsNullOrEmpty(shopCode)) break; ;
                    if (!string.IsNullOrEmpty(shopCode))
                    {
                        string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString().Trim();
                        string vinCode = msExcelUtil.GetCellValue(worksheet_FengMian, "B", i).Trim();
                        string type = msExcelUtil.GetCellValue(worksheet_FengMian, "C", i).Trim();
                        webService.ShopVinListSave(projectCode, vinCode, shopCode, type);
                    }
                }
                DateTime dtEnd = DateTime.Now;
                TimeSpan ts = dtEnd - dtStart;
                MessageBox.Show("上传耗费时间:"+ts.TotalMinutes.ToString());
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
