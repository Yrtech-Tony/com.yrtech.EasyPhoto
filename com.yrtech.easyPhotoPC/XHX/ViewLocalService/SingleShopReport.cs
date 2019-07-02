using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.DTO.SingleShopReport;
using XHX.DTO;
using XHX.Common;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;

namespace XHX.ViewLocalService
{
    public partial class SingleShopReport : BaseForm
    {
        public static localhost.Service service = new localhost.Service();
        //LocalService service = new LocalService();
        MSExcelUtil msExcelUtil = new MSExcelUtil();
        List<ShopDto> shopList = new List<ShopDto>();
        List<ShopDto> shopLeft = new List<ShopDto>();
        public List<ShopDto> ShopList
        {
            get { return shopList; }
            set { shopList = value; }
        }
        GridCheckMarksSelection selection;
        internal GridCheckMarksSelection Selection
        {

            get
            {
                return selection;
            }
        }
        public SingleShopReport()
        {
            InitializeComponent();
            service.Url = "http://192.168.13.240/XHX.BMWServer/service.asmx";
            XHX.Common.BindComBox.BindProject(cboProjects);
            tbnFilePath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnModule.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            SearchAllShopByProjectCode(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString());
            selection = new GridCheckMarksSelection(gridView1);
            selection.CheckMarkColumn.VisibleIndex = 0;
        }

        public override List<BaseForm.ButtonType> CreateButton()
        {
            List<XHX.BaseForm.ButtonType> list = new List<XHX.BaseForm.ButtonType>();
            return list;
        }

        private List<ShopDto> SearchAllShopByProjectCode(string projectCode)
        {
            DataSet ds = service.SearchShopByProjectCode(projectCode);
            List<ShopDto> shopDtoList = new List<ShopDto>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopDto shopDto = new ShopDto();
                    shopDto.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    shopDto.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    shopDtoList.Add(shopDto);
                }
            }
            grcShop.DataSource = shopDtoList;
            return shopDtoList;
        }

        private ShopReportDto GetShopReportDto(string projectCode, string shopCode)
        {
            DataSet[] dataSetList = service.GetShopReportDto(projectCode, shopCode);
            ShopReportDto shopReportDto = new ShopReportDto();
            List<PerTypeFailCountDto> chaptersScoreDtoList = new List<PerTypeFailCountDto>();
            List<ShopInfoDto> linkScoreDtoList = new List<ShopInfoDto>();
            List<SubjectsScoreDto> subjectsScoreDtoList = new List<SubjectsScoreDto>();
            List<AllScoreDto> allScoreDtoList = new List<AllScoreDto>();
            shopReportDto.ChaptersScoreDtoList = chaptersScoreDtoList;
            shopReportDto.LinkScoreDtoList = linkScoreDtoList;
            shopReportDto.SubjectsScoreDtoList = subjectsScoreDtoList;
            shopReportDto.AllScoreDtoList = allScoreDtoList;

            DataSet ds = dataSetList[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    shopReportDto.ProjectCode = Convert.ToString(ds.Tables[0].Rows[i]["ProjectCode"]);
                    shopReportDto.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    shopReportDto.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    shopReportDto.AreaName = Convert.ToString(ds.Tables[0].Rows[i]["AreaName"]);
                    shopReportDto.Province = Convert.ToString(ds.Tables[0].Rows[i]["Province"]);
                    shopReportDto.City = Convert.ToString(ds.Tables[0].Rows[i]["City"]);
                    shopReportDto.ShopNamePY = Convert.ToString(ds.Tables[0].Rows[i]["ShopNamePY"]);
                    shopReportDto.ProvincePY = Convert.ToString(ds.Tables[0].Rows[i]["ProvincePY"]);
                    shopReportDto.CityPY = Convert.ToString(ds.Tables[0].Rows[i]["CityPY"]);
                    shopReportDto.AreaNameEn = Convert.ToString(ds.Tables[0].Rows[i]["AreaNameEn"]);
                }
            }
            ds = dataSetList[4];
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AllScoreDto allScoreDto = new AllScoreDto();
                    //allScoreDto.Weight = Convert.ToDecimal(ds.Tables[0].Rows[i]["Weight"]);
                    allScoreDto.ScoreShop = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreShop"]);
                    allScoreDto.ScoreArea_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_AVG"]);
                    allScoreDto.ScoreArea_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_MAX"]);
                    allScoreDto.ScoreAll_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_AVG"]);
                    allScoreDto.ScoreAll_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_MAX"]);
                    allScoreDtoList.Add(allScoreDto);
                }
            }
            ds = dataSetList[1];
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    PerTypeFailCountDto chaptersScoreDto = new PerTypeFailCountDto();
                    chaptersScoreDto.CharterCode = Convert.ToInt32(ds.Tables[0].Rows[i]["CharterCode"]);
                    //chaptersScoreDto.Weight = Convert.ToDecimal(ds.Tables[0].Rows[i]["Weight"]);
                    chaptersScoreDto.ScoreShop = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreShop"]);
                    chaptersScoreDto.ScoreArea_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_AVG"]);
                    chaptersScoreDto.ScoreArea_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_MAX"]);
                    chaptersScoreDto.ScoreAll_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_AVG"]);
                    chaptersScoreDto.ScoreAll_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_MAX"]);
                    chaptersScoreDtoList.Add(chaptersScoreDto);
                }
            }

            ds = dataSetList[2];
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopInfoDto linkScoreDto = new ShopInfoDto();
                    //linkScoreDto.CharterCode = Convert.ToInt32(ds.Tables[0].Rows[i]["CharterCode"]);
                    //linkScoreDto.CharterName = Convert.ToString(ds.Tables[0].Rows[i]["CharterName"]);
                    linkScoreDto.LinkCode = Convert.ToString(ds.Tables[0].Rows[i]["LinkCode"]);
                    linkScoreDto.LinkName = Convert.ToString(ds.Tables[0].Rows[i]["LinkName"]);
                    linkScoreDto.ScoreShop = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreShop"]);
                    linkScoreDto.ScoreArea_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_AVG"]);
                    linkScoreDto.ScoreArea_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_MAX"]);
                    linkScoreDto.ScoreAll_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_AVG"]);
                    linkScoreDto.ScoreAll_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_MAX"]);
                    linkScoreDtoList.Add(linkScoreDto);
                }
            }

            ds = dataSetList[3];
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SubjectsScoreDto subjectsScoreDto = new SubjectsScoreDto();
                    subjectsScoreDto.SubjectCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectCode"]);
                    subjectsScoreDto.FullScore = Convert.ToDecimal(ds.Tables[0].Rows[i]["FullScore"]);
                    subjectsScoreDto.Score = Convert.ToDecimal(ds.Tables[0].Rows[i]["Score"]);
                    subjectsScoreDto.LostDesc = Convert.ToString(ds.Tables[0].Rows[i]["LossDesc"]);
                    subjectsScoreDto.PicName = Convert.ToString(ds.Tables[0].Rows[i]["PicName"]);
                    subjectsScoreDtoList.Add(subjectsScoreDto);
                }
            }

            return shopReportDto;
        }

        private void WriteDataToExcel(ShopReportDto shopReportDto)
        {
          //  Workbook workbook = msExcelUtil.OpenExcelByMSExcel(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\Template\SingleShopReportTemplate_20130812.xlsx");
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);

            #region 封面
            {
                Worksheet worksheet_FengMian = workbook.Worksheets["封面"] as Worksheet;
                msExcelUtil.SetCellValue(worksheet_FengMian, "E9", shopReportDto.ShopCode);
                msExcelUtil.SetCellValue(worksheet_FengMian, "I9", shopReportDto.ShopName);
                msExcelUtil.SetCellValue(worksheet_FengMian, "E11", shopReportDto.AreaName);
                msExcelUtil.SetCellValue(worksheet_FengMian, "I11", shopReportDto.Province);
                msExcelUtil.SetCellValue(worksheet_FengMian, "E13", shopReportDto.ProjectCode);
                msExcelUtil.SetCellValue(worksheet_FengMian, "I13", shopReportDto.City);

                msExcelUtil.SetCellValue(worksheet_FengMian, "E10", shopReportDto.ShopCode);
                msExcelUtil.SetCellValue(worksheet_FengMian, "I10", shopReportDto.ShopNamePY);
                msExcelUtil.SetCellValue(worksheet_FengMian, "E12", shopReportDto.AreaNameEn);
                msExcelUtil.SetCellValue(worksheet_FengMian, "I12", shopReportDto.ProvincePY);
                msExcelUtil.SetCellValue(worksheet_FengMian, "E14", shopReportDto.ProjectCode);
                msExcelUtil.SetCellValue(worksheet_FengMian, "I14", shopReportDto.CityPY);

                //设置单元格的格式
                /*虽然模板设置了格式，但是在赋值之后格式消失了，所以需要重新设置*/

                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 9, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 9, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 9, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 9, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 11, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 11, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 11, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 11, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 13, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 13, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 13, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 13, "Arial");



                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 10, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 10, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 10, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 10, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 12, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 12, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 12, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 12, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 14, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "E", 14, "Arial");

                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 14, "微软雅黑");
                msExcelUtil.SetCellFont(worksheet_FengMian, "I", 14, "Arial");
            }
            #endregion

            #region 经销商得分概况
            {
                Worksheet worksheet_ShopScore = workbook.Worksheets["经销商得分概况"] as Worksheet;
                List<PerTypeFailCountDto> chaptersScoreDtoList = shopReportDto.ChaptersScoreDtoList;
                /*Modify by 20130831*/
                int rowIndex = 3;
                foreach (AllScoreDto allscoreDto in shopReportDto.AllScoreDtoList)
                {
                    //msExcelUtil.SetCellValue(worksheet_ShopScore, "C", rowIndex, allscoreDto.Weight);
                    msExcelUtil.SetCellValue(worksheet_ShopScore, "D", rowIndex, allscoreDto.ScoreShop);
                    //如果勾选的话只查询经销商的得分，
                    //如果没有勾选的话则区域和全国都查询
                    if (!checkBox1.Checked)
                    {
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "E", rowIndex, allscoreDto.ScoreArea_AVG);
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "F", rowIndex, allscoreDto.ScoreArea_MAX);
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "G", rowIndex, allscoreDto.ScoreAll_AVG);
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "H", rowIndex, allscoreDto.ScoreAll_MAX);
                    }
                  
                    rowIndex++;
                }

                foreach (PerTypeFailCountDto chaptersScoreDto in chaptersScoreDtoList)
                {
                   // msExcelUtil.SetCellValue(worksheet_ShopScore, "C", rowIndex, chaptersScoreDto.Weight);
                    msExcelUtil.SetCellValue(worksheet_ShopScore, "D", rowIndex, chaptersScoreDto.ScoreShop);
                    
                    //如果勾选的话只查询经销商的得分，
                    //如果没有勾选的话则区域和全国都查询
                    if (!checkBox1.Checked)
                    {
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "E", rowIndex, chaptersScoreDto.ScoreArea_AVG);
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "F", rowIndex, chaptersScoreDto.ScoreArea_MAX);
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "G", rowIndex, chaptersScoreDto.ScoreAll_AVG);
                        msExcelUtil.SetCellValue(worksheet_ShopScore, "H", rowIndex, chaptersScoreDto.ScoreAll_MAX);
                    }
                   
                    rowIndex++;
                }

                List<ShopInfoDto> linkScoreDtoList = shopReportDto.LinkScoreDtoList;
                foreach (ShopInfoDto linkScoreDto in linkScoreDtoList)
                {
                    int aa = linkScoreDtoList.IndexOf(linkScoreDto);
                    for (int i = 25; i < 80; i++)
                    {
                        string cellValue = msExcelUtil.GetCellValue(worksheet_ShopScore, "B", i);
                        if (cellValue != "" &&cellValue.Trim().Contains((linkScoreDto.LinkCode + linkScoreDto.LinkName).Trim()))
                        {
                            if (linkScoreDto.ScoreShop == Convert.ToDecimal(9999))
                            {
                                msExcelUtil.SetCellValue(worksheet_ShopScore, "D", i, "N/A");
                            }
                            else
                            {
                                msExcelUtil.SetCellValue(worksheet_ShopScore, "D", i, linkScoreDto.ScoreShop);
                            }

                            //如果勾选的话只查询经销商的得分，
                            //如果没有勾选的话则区域和全国都查询
                            if (!checkBox1.Checked)
                            {
                                //if (linkScoreDto.ScoreShop == Convert.ToDecimal(9999))
                                //{
                                //    msExcelUtil.SetCellValue(worksheet_ShopScore, "E", i, "N/A");
                                //    msExcelUtil.SetCellValue(worksheet_ShopScore, "F", i, "N/A");
                                //    msExcelUtil.SetCellValue(worksheet_ShopScore, "G", i, "N/A");
                                //    msExcelUtil.SetCellValue(worksheet_ShopScore, "H", i, "N/A");
                                //}
                                //else
                                //{
                                    msExcelUtil.SetCellValue(worksheet_ShopScore, "E", i, linkScoreDto.ScoreArea_AVG);
                                    msExcelUtil.SetCellValue(worksheet_ShopScore, "F", i, linkScoreDto.ScoreArea_MAX);
                                    msExcelUtil.SetCellValue(worksheet_ShopScore, "G", i, linkScoreDto.ScoreAll_AVG);
                                    msExcelUtil.SetCellValue(worksheet_ShopScore, "H", i, linkScoreDto.ScoreAll_MAX);
                                //}
                            }
                             
                        }
                    }
                }
            }
            #endregion

            #region 指标点得分详情
            List<SubjectsScoreDto> subjectsScoreDtoListDetail = shopReportDto.SubjectsScoreDtoList;
            {
                Worksheet worksheet_ShopScoreDetail = workbook.Worksheets["指标点得分详情"] as Worksheet;
                int rowIndex1 = 3;
                foreach (SubjectsScoreDto subjectsScoreDto in subjectsScoreDtoListDetail)
                {

                   // msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "E", rowIndex1, subjectsScoreDto.FullScore);
                    if (subjectsScoreDto.Score == 9999 || subjectsScoreDto.Score == Convert.ToDecimal(9999.00))
                    {
                        msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "F", rowIndex1, "N/A");
                    }
                    else
                    {
                        msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "F", rowIndex1, subjectsScoreDto.Score);
                    }
                    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "G", rowIndex1, subjectsScoreDto.LostDesc);
                    //设置单元格的格式
                    /*虽然模板设置了格式，但是在赋值之后格式消失了，所以需要重新设置*/
                    msExcelUtil.SetCellFont(worksheet_ShopScoreDetail, "G", rowIndex1, "微软雅黑");
                    msExcelUtil.SetCellFont(worksheet_ShopScoreDetail, "G", rowIndex1, "Arial");
                    rowIndex1++;
                }
               
                //foreach (SubjectsScoreDto subjectsScoreDto in subjectsScoreDtoListDetail)
                //{

                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "H", rowIndex1, subjectsScoreDto.FullScore);
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "I", rowIndex1, subjectsScoreDto.Score);
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "J", rowIndex1, subjectsScoreDto.LostDesc);
                //    rowIndex1++;
                //}
            }
            #endregion
            #region 失分照片
            {
                List<SubjectsScoreDto> subjectsScoreDtoList = shopReportDto.SubjectsScoreDtoList;
                Worksheet worksheet_ShopScoreDetail2 = workbook.Worksheets["失分照片"] as Worksheet;
                int rowIndex = 3;
                foreach (SubjectsScoreDto subjectsScoreDto in subjectsScoreDtoList)
                {
                    if (String.IsNullOrEmpty(subjectsScoreDto.PicName))
                    {
                        msExcelUtil.DeleteRow(worksheet_ShopScoreDetail2, rowIndex);
                        continue;
                    }
                    else
                    {
                        //msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "F", rowIndex, subjectsScoreDto.LostDesc);
                        string[] picNameArray = subjectsScoreDto.PicName.Split(';');
                        int picIndex = 0;
                        foreach (string picName in picNameArray)
                        {
                            if (picIndex != 0 && picIndex % 3 == 0)
                            {
                                msExcelUtil.AddRow(worksheet_ShopScoreDetail2, ++rowIndex);
                            }
                            if (string.IsNullOrEmpty(picName)) continue;
                            byte[] bytes = service.SearchAnswerDtl2Pic(picName.Replace(".jpg",""), shopReportDto.ProjectCode + shopReportDto.ShopName, subjectsScoreDto.SubjectCode, "", "");
                            if (bytes == null || bytes.Length == 0) continue;
                            Image.FromStream(new MemoryStream(bytes)).Save(Path.Combine(Path.GetTempPath(), picName + ".jpg"));
                            int colIndex = 3 + picIndex % 3;

                            msExcelUtil.InsertPicture(worksheet_ShopScoreDetail2, worksheet_ShopScoreDetail2.Cells[rowIndex, colIndex] as Microsoft.Office.Interop.Excel.Range, Path.Combine(Path.GetTempPath(), picName + ".jpg"), rowIndex);
                            picIndex++;
                        }
                    }

                    rowIndex++;
                }
            }
            #endregion
            //workbook.Save(Path.Combine(tbnFilePath.Text,shopReportDto.ProjectCode+"_"+shopReportDto.ShopName+".xls"));
            workbook.Close(true, Path.Combine(tbnFilePath.Text, shopReportDto.AreaName + "_" + shopReportDto.ShopCode+"_"+ shopReportDto.ShopName + ".xlsx"), Type.Missing);
        }

        private void GenerateReport()
        {
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            _shopDtoList = new List<ShopDto>();
            //_shopDtoList = SearchAllShopByProjectCode(projectCode);
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (gridView1.GetRowCellValue(i, "CheckMarkSelection") != null && gridView1.GetRowCellValue(i, "CheckMarkSelection").ToString() == "True")
                {
                    _shopDtoList.Add(gridView1.GetRow(i) as ShopDto);
                }
            }
            _shopDtoListCount = _shopDtoList.Count;
            this.Enabled = false;
            _bw = new BackgroundWorker();
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.WorkerReportsProgress = true;
            _bw.RunWorkerAsync(new object[] { projectCode });
        }

        BackgroundWorker _bw;
        List<ShopDto> _shopDtoList;
        int _shopDtoListCount = 0;
        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbrProgress.Value = (e.ProgressPercentage) * 100 / _shopDtoListCount;
            System.Windows.Forms.Application.DoEvents();
        }
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] shopNames;
            int currentShopDtoIndex = 0;
            foreach (ShopDto shopDto in _shopDtoList)
            {
                try
                {
                    object[] arguments = e.Argument as object[];
                    ShopReportDto shopReportDto = GetShopReportDto(arguments[0] as string, shopDto.ShopCode);
                    WriteDataToExcel(shopReportDto);
                    _bw.ReportProgress(currentShopDtoIndex++);
                }
                catch (Exception ex)
                {
                    shopLeft.Add(shopDto);
                   // MessageBox.Show(shopDto.ShopCode);
                    WriteErrorLog(shopDto.ShopCode + shopDto.ShopName + ex.Message.ToString());
                    continue;
                }

            }
        }
        void WriteErrorLog(string errMessage)
        {
            string path = tbnFilePath.Text + "\\"+"Error.txt";

            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                AddText(fs, errMessage + "\r\n");
            }

        }
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.Enabled = true;
            List<ShopDto> gridSource = grcShop.DataSource as List<ShopDto>;

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                gridView1.SetRowCellValue(i, "CheckMarkSelection", false);
                foreach (ShopDto shop in shopLeft)
                {
                    if (shop.ShopCode == gridSource[i].ShopCode)
                    {
                        gridView1.SetRowCellValue(i, "CheckMarkSelection", true);
                    }
                    //else
                    //{
                    //    gridView1.SetRowCellValue(i, "CheckMarkSelection", false);
                    //}
                }
            }
            //if (shopLeft.Count > 0)
            //{
            //    string str = string.Empty;
            //    foreach (ShopDto shop in shopLeft)
            //    {
            //        str += shop.ShopCode + ":" + shop.ShopName + ";";
            //    }
            //    CommonHandler.ShowMessage(MessageType.Information, "报告生成完毕未生成报告经销商如下:" + str);
            //}
            //else
            //{
            CommonHandler.ShowMessage(MessageType.Information, "报告生成完毕");
            //}
            
        }

        private void tbnFilePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbnFilePath.Text = fbd.SelectedPath;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbnFilePath.Text))
            {
                CommonHandler.ShowMessage(MessageType.Information, "请选择报告生成路径");
                return;
            }
            GenerateReport();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SearchAllShopByProjectCode(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString());
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ShopNotInScore shop = new ShopNotInScore(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString());
            shop.ShowDialog();
                
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
    }
}
