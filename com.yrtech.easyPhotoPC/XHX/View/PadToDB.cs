using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using XHX.Common;
using System.Data.Common;
using XHX.DTO;
using DbAccess;
using Microsoft.Office.Interop.Excel;

namespace XHX.View
{
    public partial class PadToDB : BaseForm
    {
        public static localhost.Service service = new localhost.Service();
        string ProjectCode_Golbal = "";
        string ShopCode_Golbal = "";
        MSExcelUtil msExcelUtil = new MSExcelUtil();

        public PadToDB()
        {
            InitializeComponent();
            XHX.Common.BindComBox.BindProject(cboProjects);
            XHX.Common.BindComBox.BindSubjectExamType(cboExamType);
        }

        public override List<XHX.BaseForm.ButtonType> CreateButton()
        {
            List<XHX.BaseForm.ButtonType> list = new List<XHX.BaseForm.ButtonType>();
            return list;
        }

        private void btnShopCode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Shop_Popup pop = new Shop_Popup("", "", false);
            pop.ShowDialog();
            ShopDto dto = pop.Shopdto;
            if (dto != null)
            {
                btnShopCode.Text = dto.ShopCode;
                txtShopName.Text = dto.ShopName;
            }
            ProjectCode_Golbal = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            ShopCode_Golbal = btnShopCode.Text;

            //�����ı��ʱ���Ӧ���Ծ�����Ҳ���иı�

            //List<ShopSubjectExamTypeDto> list = new List<ShopSubjectExamTypeDto>();
            ShopSubjectExamTypeDto shop = new ShopSubjectExamTypeDto();
            DataSet ds = service.SearchShopExamTypeByProjectCodeAndShopCode(ProjectCode_Golbal, ShopCode_Golbal);
            if (ds.Tables[0].Rows.Count > 0)
            {
                shop.ExamTypeCode = ds.Tables[0].Rows[0]["SubjectTypeCodeExam"] == null ? "" : ds.Tables[0].Rows[0]["SubjectTypeCodeExam"].ToString();
            }
            else
            {
                shop.ExamTypeCode = "";
            }
            CommonHandler.SetComboBoxSelectedValue(cboExamType, shop.ExamTypeCode);
        }

        #region UploadData

        private void btnDataPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                btnDataPath.Text = fbd.SelectedPath;
            }
        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            //if (CommonHandler. == 0)
            //{
            //    CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"��Ŀ\"");
            //    cboProjects.Focus();
            //    return;
            //}
            if (txtShopName.Text == "")
            {
                CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"������\"");
                txtShopName.Focus();
                return;
            }
            if (btnDataPath.Text == "")
            {
                CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"����·��\"");
                btnDataPath.Focus();
                return;
            }

            ProjectCode_Golbal = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            ShopCode_Golbal = btnShopCode.Text;

            DirectoryInfo dataDir = new DirectoryInfo(btnDataPath.Text);
            FileInfo[] filesInfo = dataDir.GetFiles();

            bool isExistDBFile = false;
            foreach (FileInfo fileInfo in filesInfo)
            {
                if (fileInfo.Name == "writeable.db")
                {
                    isExistDBFile = true;
                    SqliteHelper.SetConnectionString("Data Source=" + fileInfo.FullName, "");
                }
            }
            if (!isExistDBFile)
            {
                CommonHandler.ShowMessage(MessageType.Information, "·���в��������ݿ��ļ�'writeable.db'");
                return;
            }

            #region �ϴ�Answer������
            {
                List<String> dataList = SqliteHelper.Search("SELECT ProjectCode,SubjectCode,ShopCode,Score,Remark,ImageName,InUserID,'0','',AssessmentDate,InDateTime,SPCode,SpType from Answer WHERE Flag=0 AND ProjectCode='" + ProjectCode_Golbal + "' AND ShopCode='" + ShopCode_Golbal + "'");
                List<String> updateStringList = new List<string>();
                foreach (String data in dataList)
                {
                    String[] properties = data.Split('$');
                    String updateString = @"update Answer Set Flag=1 WHERE ProjectCode='{0}' " +
                                               "AND SubjectCode='{1}' " +
                                               "AND ShopCode='{2}'";
                    updateString = String.Format(updateString, properties[0], properties[1], properties[2]);
                    updateStringList.Add(updateString);

                }
                service.SaveAnswerList(dataList.ToArray());
                SqliteHelper.InsertOrUpdata(updateStringList);
            }
            #endregion

            #region �ϴ�AnswerLog������
            {
                List<String> dataList = SqliteHelper.Search("SELECT ProjectCode,SubjectCode,ShopCode,Score,Desc,InUserID,StatusCode from AnswerLog WHERE Flag=0 AND ProjectCode='" + ProjectCode_Golbal + "' AND ShopCode='" + ShopCode_Golbal + "'");
                List<String> updateStringList = new List<string>();
                foreach (String data in dataList)
                {
                    String[] properties = data.Split('$');
                    String updateString = @"update AnswerLog Set Flag=1 WHERE ProjectCode='{0}' " +
                                           "AND SubjectCode='{1}' " +
                                           "AND ShopCode='{2}'" +
                                           "AND StatusCode='{3}'";
                    updateString = String.Format(updateString, properties[0], properties[1], properties[2], properties[6]);
                    updateStringList.Add(updateString);

                }
                service.SaveAnswerLogList(dataList.ToArray());
                SqliteHelper.InsertOrUpdata(updateStringList);
            }
            #endregion

            #region �ϴ�AnswerDtl������
            {
                List<String> dataList = SqliteHelper.Search("SELECT ProjectCode,SubjectCode,ShopCode,SeqNO,InUserID,CheckOptionCode,PicNameList from AnswerDtl WHERE Flag=0 AND ProjectCode='" + ProjectCode_Golbal + "' AND ShopCode='" + ShopCode_Golbal + "'");
                List<String> updateStringList = new List<string>();
                foreach (String data in dataList)
                {
                    String[] properties = data.Split('$');
                    String updateString = @"update AnswerDtl Set Flag=1,PicNameList='{4}' WHERE ProjectCode='{0}' " +
                                               "AND SubjectCode='{1}' " +
                                               "AND ShopCode='{2}' " +
                                               "AND SeqNO={3}"; ;
                    updateString = String.Format(updateString, properties[0], properties[1], properties[2], properties[3], properties[6]);
                    updateStringList.Add(updateString);

                }
                service.SaveAnswerDtlList(dataList.ToArray());
                SqliteHelper.InsertOrUpdata(updateStringList);
            }
            #endregion

            #region �ϴ�AnswerDtl2������
            {
                List<String> dataList = SqliteHelper.Search("SELECT ProjectCode,SubjectCode,ShopCode,SeqNO,InUserID,CheckOptionCode from AnswerDtl2 WHERE Flag=0 AND ProjectCode='" + ProjectCode_Golbal + "' AND ShopCode='" + ShopCode_Golbal + "'");
                List<String> updateStringList = new List<string>();
                foreach (String data in dataList)
                {
                    String[] properties = data.Split('$');
                    String updateString = @"update AnswerDtl2 Set Flag=1 WHERE ProjectCode='{0}' " +
                                               "AND SubjectCode='{1}' " +
                                               "AND ShopCode='{2}' " +
                                               "AND SeqNO={3}";
                    updateString = String.Format(updateString, properties[0], properties[1], properties[2], properties[3]);
                    updateStringList.Add(updateString);

                }
                service.SaveAnswerDtl2StreamList(dataList.ToArray());
                SqliteHelper.InsertOrUpdata(updateStringList);
            }
            #endregion
            #region �ϴ�AnswerDtl3������
            {
                List<String> dataList = SqliteHelper.Search("SELECT ProjectCode,SubjectCode,ShopCode,SeqNO,LossDesc,PicName from AnswerDtl3 WHERE Flag=0 AND ProjectCode='" + ProjectCode_Golbal + "' AND ShopCode='" + ShopCode_Golbal + "'");
                List<String> updateStringList = new List<string>();
                foreach (String data in dataList)
                {
                    String[] properties = data.Split('$');
                    String updateString = @"update AnswerDtl3 Set Flag=1 WHERE ProjectCode='{0}' " +
                                               "AND SubjectCode='{1}' " +
                                               "AND ShopCode='{2}' " +
                                               "AND SeqNO={3}";
                    updateString = String.Format(updateString, properties[0], properties[1], properties[2], properties[3]);
                    updateStringList.Add(updateString);

                }
                service.SaveAnswerDtl3StringList(dataList.ToArray());
                SqliteHelper.InsertOrUpdata(updateStringList);
            }
            #endregion


            //#region �ϴ�ͼƬ�ļ�
            //{
            //    DirectoryInfo[] dirInfos = dataDir.GetDirectories();
            //    foreach (DirectoryInfo dirInfo in dirInfos)
            //    {
            //        if (dirInfo.Name == ProjectCode_Golbal + txtShopName.Text)
            //        {
            //            FileInfo[] fileList = dirInfo.GetFiles("Thumds.db");
            //            if (fileList != null && fileList.Length != 0)
            //            {
            //                foreach (FileInfo file in fileList)
            //                {
            //                    if (file.Name == "Thumds.db")
            //                    {
            //                        file.Delete();
            //                        break;
            //                    }
            //                }
            //            }

            //            string tempFile = Path.Combine(Path.GetTempPath(), dirInfo.Name + ".zip");
            //            if (ZipHelper.Zip(dirInfo.FullName, tempFile, ""))
            //            {
            //                FileStream fs = new FileStream(tempFile, FileMode.Open);
            //                byte[] zipFile = new byte[fs.Length];
            //                fs.Read(zipFile, 0, zipFile.Length);
            //                fs.Close();
            //                service.UploadImgZipFile(zipFile);
            //            }
            //            else
            //            {
            //                CommonHandler.ShowMessage(MessageType.Information, "ѹ��ͼƬ�ļ���\"" + dirInfo.FullName + "\"ʧ�ܡ�");
            //            }
            //        }
            //    }
            //}
            //#endregion
            #region �ϴ�ͼƬ�ļ�
            {
                DirectoryInfo[] dirInfos = dataDir.GetDirectories();
                foreach (DirectoryInfo dirInfo in dirInfos)
                {
                    if (dirInfo.Name == ProjectCode_Golbal + txtShopName.Text)
                    {
                        FileInfo[] fileList = dirInfo.GetFiles("Thumbs.db");
                        if (fileList != null && fileList.Length != 0)
                        {
                            foreach (FileInfo file in fileList)
                            {
                                if (file.Name == "Thumbs.db")
                                {
                                    file.Delete();
                                    break;
                                }
                            }
                        }
                        UploadImgZipFileBySubDirectory(dirInfo.FullName);
                    }
                }
            }
            #endregion

            CommonHandler.ShowMessage(MessageType.Information, "�����ϴ���ϡ�");
        }

        #endregion
        void UploadImgZipFileBySubDirectory(string dirPath)
        {
            DirectoryInfo shopDir = new DirectoryInfo(dirPath);
            double shopDirSize = 0;
            foreach (DirectoryInfo dir in shopDir.GetDirectories())
            {
                foreach (FileInfo fi in dir.GetFiles())
                {
                    shopDirSize += fi.Length;
                }

            }
            DirectoryInfo[] dirInfos = shopDir.GetDirectories();

            for (int i = 0; i < dirInfos.Length; i++)
            {
                DirectoryInfo subjectDir = dirInfos[i];
                double subjectDirSize = 0;
                foreach (FileInfo fi in subjectDir.GetFiles())
                {
                    subjectDirSize += fi.Length;
                }
                string tempFile = Path.Combine(Path.GetTempPath(), subjectDir.Name + ".zip");
                if (ZipHelper.Zip(subjectDir.FullName, tempFile, ""))
                {
                    FileStream fs = new FileStream(tempFile, FileMode.Open);
                    byte[] zipFile = new byte[fs.Length];
                    fs.Read(zipFile, 0, zipFile.Length);
                    fs.Close();
                    service.UploadImgZipFile(shopDir.Name, zipFile);
                    try
                    {
                        pbrProgressForUpload.Value += (int)((subjectDirSize / shopDirSize) * 100D);
                    }
                    catch (Exception)
                    {

                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                else
                {
                    CommonHandler.ShowMessage(MessageType.Information, "ѹ��ͼƬ�ļ���\"" + subjectDir.FullName + "\"ʧ�ܡ�");
                }
            }
            CommonHandler.ShowMessage(MessageType.Information, "�����ϴ���ϡ�");
            pbrProgressForUpload.Value = 0;
        }
        #region DownloadData

        private void tbnSQLitePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbnSQLitePath.Text = fbd.SelectedPath;
            }
        }

        private void btnDownloadData_Click(object sender, EventArgs e)
        {
            if (tbnSQLitePath.Text == "")
            {
                CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"����·��\"");
                tbnSQLitePath.Focus();
                return;
            }

            string sqlConnString = GetSqlServerConnectionString("123.57.229.128", "Infiniti_StockCheck", "sa", "mxT1@mfb");
            string sqlitePath = Path.Combine(tbnSQLitePath.Text.Trim(), "yfnd.db");
            this.Cursor = Cursors.WaitCursor;
            SqlConversionHandler handler = new SqlConversionHandler(delegate(bool done,
                bool success, int percent, string msg)
            {
                Invoke(new MethodInvoker(delegate()
                {
                    pbrProgress.Value = percent;

                    if (done)
                    {
                        this.Cursor = Cursors.Default;

                        if (success)
                        {
                           // File.Copy(sqlitePath, Path.Combine(Path.GetDirectoryName(sqlitePath), "writeable.db"), true);
                            CommonHandler.ShowMessage(MessageType.Information, "���سɹ�");
                            pbrProgress.Value = 0;
                        }
                        else
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "����ʧ��\r\n" + msg);
                            pbrProgress.Value = 0;
                        }
                    }
                }));
            });
            SqlTableSelectionHandler selectionHandler = new SqlTableSelectionHandler(delegate(List<TableSchema> schema)
            {
                return schema;
            });

            FailedViewDefinitionHandler viewFailureHandler = new FailedViewDefinitionHandler(delegate(ViewSchema vs)
            {
                return null;
            });

            string password = null;
            SqlServerToSQLite.ConvertSqlServerToSQLiteDatabase(sqlConnString, sqlitePath, password, handler,
                selectionHandler, viewFailureHandler, false, false);
        }

        private static string GetSqlServerConnectionString(string address, string db, string user, string pass)
        {
            string res = @"Data Source=" + address.Trim() +
                ";Initial Catalog=" + db.Trim() + ";User ID=" + user.Trim() + ";Password=" + pass.Trim();
            return res;
        }

        #endregion

        #region UpdateData

        private void tbnSQLitePathForUpdate_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbnSQLitePathForUpdate.Text = fbd.SelectedPath;
            }
        }

        private void btnDownloadDataForUpdate_Click(object sender, EventArgs e)
        {
            if (tbnSQLitePathForUpdate.Text == "")
            {
                CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"����·��\"");
                tbnSQLitePathForUpdate.Focus();
                return;
            }

            string sqlConnString = GetSqlServerConnectionString("123.57.229.128", "Infiniti_StockCheck", "sa", "mxT1@mfb");
            string sqlitePath = Path.Combine(tbnSQLitePathForUpdate.Text.Trim(), "readonly.db");
            this.Cursor = Cursors.WaitCursor;
            SqlConversionHandler handler = new SqlConversionHandler(delegate(bool done,
                bool success, int percent, string msg)
            {
                Invoke(new MethodInvoker(delegate()
                {
                    pbrProgressForUpdate.Value = percent;

                    if (done)
                    {
                        this.Cursor = Cursors.Default;

                        if (success)
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "���سɹ�");
                            pbrProgressForUpdate.Value = 0;
                        }
                        else
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "����ʧ��\r\n" + msg);
                            pbrProgressForUpdate.Value = 0;
                        }
                    }
                }));
            });
            SqlTableSelectionHandler selectionHandler = new SqlTableSelectionHandler(delegate(List<TableSchema> schema)
            {
                return schema;
            });

            FailedViewDefinitionHandler viewFailureHandler = new FailedViewDefinitionHandler(delegate(ViewSchema vs)
            {
                return null;
            });

            string password = null;
            SqlServerToSQLite.ConvertSqlServerToSQLiteDatabase(sqlConnString, sqlitePath, password, handler,
                selectionHandler, viewFailureHandler, false, false);
        }

        #endregion

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

        private string getState(string value)
        {
            string result = "";
            if (value == "Y")
            {
                result = "01";
            }
            else if (value == "N")
            {
                result = "02";
            }
            else
            {
                result = "03";
            }
            return result;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //if (txtShopName.Text == "")
            //{
            //    CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"������\"");
            //    txtShopName.Focus();
            //    return;
            //}
            if (btnModule.Text == "")
            {
                CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"Excel\"");
                return;
            }

            ProjectCode_Golbal = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            ShopCode_Golbal = btnShopCode.Text;

            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);

            #region �ϴ�Answer������
            {
                //����
                Worksheet worksheet_ShopScoreADetail = workbook.Worksheets["���۲�����ϸ"] as Worksheet;
                string inDateTime = DateTime.Now.ToShortDateString(); ;
                for (int i = 3; i < 5000; i++)
                {
                    string subjectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "H", i);
                    if (!string.IsNullOrEmpty(subjectCode) && subjectCode.Contains("A"))
                    {
                        
                            string SpCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "G", i);
                            string A1 = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "Q", i);
                            string A2 = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "O", i);
                            string A3 = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "P", i);
                            string A4 = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "R", i);
                            string A5 = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "T", i);
                            string customer = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "I", i);
                            string vinCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "J", i);
                            string sellInvoiceDate = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "K", i);
                            string sellInvoiceDmsDate = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "L", i);
                            string shopCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "B", i);
                            string scoreChk = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "Y", i);


                            decimal? score = 0;
                            if (scoreChk == "Y")
                            {
                                score = 1;
                            }
                            else if (scoreChk == "N")
                            {
                                score = 0;
                            }
                            else
                            {
                                score = 9999;
                            }
                            //if (!string.IsNullOrEmpty(sellInvoiceDate) && sellInvoiceDate.Substring(0, 1) == "4")
                            //{
                            //    sellInvoiceDate = DateTime.FromOADate(Convert.ToInt32(sellInvoiceDate)).ToString("d");
                            //}
                            //if (!string.IsNullOrEmpty(sellInvoiceDmsDate) && sellInvoiceDmsDate.Substring(0, 1) == "4")
                            //{
                            //    sellInvoiceDmsDate = DateTime.FromOADate(Convert.ToInt32(sellInvoiceDmsDate)).ToString("d");
                            //}
                            //if (sellInvoiceDate == "-")
                            //{
                            //    sellInvoiceDate = "";

                            //}
                            //if (sellInvoiceDmsDate == "-")
                            //{
                            //    sellInvoiceDmsDate = "";
                            //}


                            string remark = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "V", i);
                            string lossDesc = "";
                            if (A1 == "N")
                            {
                                lossDesc += "a1�걨ʱ�䳬48Сʱ;";
                            }
                            if (A2 == "N")
                            {
                                lossDesc += "a2�ͻ����Ʋ�һ��;";
                            } if (A3 == "N")
                            {
                                lossDesc += "a3VIN�Ų�һ��;";
                            } if (A4 == "N")
                            {
                                lossDesc += "a4��Ʊδ¼��DMS;";
                            } if (A5 == "N")
                            {
                                lossDesc += "a5DMS��Ʊʶ��ʧ��;";
                            }
                        try
                        {
                            //�ϴ�Answer����
                            service.SaveAnswer(ProjectCode_Golbal, subjectCode, shopCode, score, remark, "", "excel", '0', "", inDateTime, inDateTime, SpCode, "");
                            //service.UpdateSellTool(ProjectCode_Golbal, shopCode, subjectCode, customer, vinCode, sellInvoiceDate, sellInvoiceDmsDate);
                            //�ϴ�Answerdtl����
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 1, "excel", getState(A1), "");
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 2, "excel", getState(A2), "");
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 3, "excel", getState(A3), "");
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 4, "excel", getState(A4), "");
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 5, "excel", getState(A5), "");
                            //�ϴ�Answerdtl3����
                            service.SaveAnswerDtl3(ProjectCode_Golbal, subjectCode, shopCode, 1, lossDesc, "");
                        }
                        catch (Exception ex)
                        {
                            CommonHandler.ShowMessage(MessageType.Information, shopCode + "-" + subjectCode+"-"+lossDesc);
                        }
                    }
                }

                //�ۺ�
                Worksheet worksheet_ShopScoreBDetail = workbook.Worksheets["�ͻ����񲿷���ϸ"] as Worksheet;
                for (int i = 3; i < 8700; i++)
                {
                    string subjectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "A", i);
                    if (!string.IsNullOrEmpty(subjectCode) && subjectCode.Contains("B"))
                    {
                        
                        string SpCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "F", i);
                        string B1 = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "R", i);
                        string B2 = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "S", i);
                        string B3 = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "T", i);
                        string afterInvoiceDate = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "I", i);
                        string afterInvoiceDmsDate = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "M", i);
                        string invoiceMony = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "H", i);
                        string invoiceDMSMony = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "L", i);
                        string shopCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "B", i);
                        string scoreChk = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "U", i);
                        

                        decimal? score = 0;

                        if (scoreChk == "Y"
                                )
                        {
                            score = 1;
                        }
                        else if (scoreChk == "N")
                        {
                            score = 0;
                        }
                        else
                        {
                            score = 9999;
                        }
                        //if (!string.IsNullOrEmpty(afterInvoiceDate) && afterInvoiceDate.Substring(0, 1) == "4")
                        //{
                        //    afterInvoiceDate = DateTime.FromOADate(Convert.ToInt32(afterInvoiceDate)).ToString("d");
                        //}
                        //if (!string.IsNullOrEmpty(afterInvoiceDmsDate) && afterInvoiceDmsDate.Substring(0, 1) == "4")
                        //{
                        //    afterInvoiceDmsDate = DateTime.FromOADate(Convert.ToInt32(afterInvoiceDmsDate)).ToString("d");
                        //}
                        //if (afterInvoiceDate == "-")
                        //{
                        //    afterInvoiceDate = "";
                        //}
                        //if (afterInvoiceDmsDate == "-")
                        //{
                        //    afterInvoiceDmsDate = "";
                        //}

                        //if (invoiceMony == "-" || invoiceMony == "_")
                        //{
                        //    invoiceMony = "0";
                        //}
                        //if (invoiceDMSMony == "-" || invoiceDMSMony == "_")
                        //{
                        //    invoiceDMSMony = "0";
                        //}
                       
                        string remark = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "X", i);
                        string lossDesc = "";
                        if (B1 == "N")
                        {
                            lossDesc += "b1��Ʊ�걨�������ڽ��㵥;";
                        }
                        if (B2 == "N")
                        {
                            lossDesc += "b2���������5%;";
                        }
                        if (B3 == "N")
                        {
                            lossDesc += "b3��Ʊ�޶�ӦDMS��¼;";
                        }
                        try
                        {
                            service.SaveAnswer(ProjectCode_Golbal, subjectCode, shopCode, score, remark, "", "excel", '0', "",inDateTime, inDateTime, SpCode, "");
                            //service.UpdateAfterTool(ProjectCode_Golbal, shopCode, subjectCode, afterInvoiceDate, afterInvoiceDmsDate, invoiceMony, invoiceDMSMony);
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 1, "excel", getState(B1), "");
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 2, "excel", getState(B2), "");
                            service.SaveAnswerDtl(ProjectCode_Golbal, subjectCode, shopCode, 3, "excel", getState(B3), "");

                            service.SaveAnswerDtl3(ProjectCode_Golbal, subjectCode, shopCode, 1, lossDesc, "");
                        }
                        catch (Exception ex )
                        {

                            CommonHandler.ShowMessage(MessageType.Information, subjectCode + "-" + shopCode + "-" + lossDesc);
                        }
                        //�ϴ�Answer����
                       
                    }
                }
            }
            #endregion
            CommonHandler.ShowMessage(MessageType.Information, "�ϴ����");
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.Filter = "Excel(*.xlsx)|";
            ofp.FilterIndex = 2;
            if (ofp.ShowDialog() == DialogResult.OK)
            {
                buttonEdit1.Text = ofp.FileName;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(buttonEdit1.Text);
            Worksheet worksheet1 = workbook.Worksheets["Sheet1"] as Worksheet;
            for (int i = 2; i < 120; i++)
            {
                string projectCode = msExcelUtil.GetCellValue(worksheet1, "A", i);
                if (!string.IsNullOrEmpty(projectCode))
                {
                    string shopCode = msExcelUtil.GetCellValue(worksheet1, "B", i);
                    string startDate = DateTime.FromOADate(Convert.ToInt32(msExcelUtil.GetCellValue(worksheet1, "D", i))).ToString("d");
                    string sellstartDate = DateTime.FromOADate(Convert.ToInt32(msExcelUtil.GetCellValue(worksheet1, "E", i))).ToString("d");
                    string sellendDate = DateTime.FromOADate(Convert.ToInt32(msExcelUtil.GetCellValue(worksheet1, "F", i))).ToString("d");
                    string sellInvoiceCount = msExcelUtil.GetCellValue(worksheet1, "G", i).ToString();
                    string afterInvoiceCount = msExcelUtil.GetCellValue(worksheet1, "H", i).ToString();
                    string sellLocalCount = msExcelUtil.GetCellValue(worksheet1, "I", i).ToString();
                    string afterLocalCount = msExcelUtil.GetCellValue(worksheet1, "J", i).ToString();
                    service.AnswerStartInfoSave(projectCode, shopCode, "", "sysadmin", startDate, sellstartDate, sellendDate, sellstartDate, sellendDate, sellInvoiceCount, afterInvoiceCount, sellLocalCount, afterLocalCount);

                }
            }
            CommonHandler.ShowMessage(MessageType.Information, "�ϴ����");

        }

        private void buttonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.Filter = "Excel(*.xlsx)|";
            ofp.FilterIndex = 2;
            if (ofp.ShowDialog() == DialogResult.OK)
            {
                buttonEdit2.Text = ofp.FileName;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ProjectCode_Golbal = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();

            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);

            #region �ϴ�Answer������
            {
                //����
                Worksheet worksheet_ShopScoreADetail = workbook.Worksheets["���߱��-����"] as Worksheet;
                for (int i = 2; i < 10000; i++)
                {
                    string subjectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "C", i);
                    if (!string.IsNullOrEmpty(subjectCode) && subjectCode.Contains("A"))
                    {
                        string projectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "A", i);
                        string shopCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "B", i);
                        string customer = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "D", i);
                        string vinCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "E", i);
                        string sellInvoiceDate = DateTime.FromOADate(Convert.ToInt32(msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "F", i))).ToString("d");
                        string sellInvoiceDmsDate = DateTime.FromOADate(Convert.ToInt32(msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "G", i))).ToString("d");

                        //�ϴ�Answer����
                        service.UpdateSellTool(projectCode, shopCode, subjectCode, customer, vinCode, sellInvoiceDate, sellInvoiceDmsDate);
                    }
                }

                //�ۺ�
                Worksheet worksheet_ShopScoreBDetail = workbook.Worksheets["���߱��-�ۺ�"] as Worksheet;
                for (int i = 2; i < 10000; i++)
                {
                    string subjectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "C", i);
                    if (!string.IsNullOrEmpty(subjectCode) && subjectCode.Contains("B"))
                    {
                        string projectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "A", i);
                        string shopCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "B", i);
                        string afterInvoiceDate = DateTime.FromOADate(Convert.ToInt32(msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "D", i))).ToString("d");
                        string afterInvoiceDmsDate = DateTime.FromOADate(Convert.ToInt32(msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "E", i))).ToString("d");
                        string invoiceMony = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "F", i);
                        string invoiceDMSMony = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "G", i);
                        //�ϴ�Answer����
                        service.UpdateAfterTool(projectCode, shopCode, subjectCode, afterInvoiceDate, afterInvoiceDmsDate, invoiceMony, invoiceDMSMony);
                    }
                }
            }
            #endregion
            CommonHandler.ShowMessage(MessageType.Information, "�ϴ����");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //if (txtShopName.Text == "")
            //{
            //    CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"������\"");
            //    txtShopName.Focus();
            //    return;
            //}
            if (btnModule.Text == "")
            {
                CommonHandler.ShowMessage(MessageType.Information, "��ѡ��\"Excel\"");
                return;
            }

            ProjectCode_Golbal = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            ShopCode_Golbal = btnShopCode.Text;

            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);

            #region �ϴ�Answer������
            {
                //����
                Worksheet worksheet_ShopScoreADetail = workbook.Worksheets["���۲�����ϸ"] as Worksheet;
                string inDateTime = DateTime.Now.ToShortDateString(); ;
                for (int i = 3; i < 5000; i++)
                {
                    string subjectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "H", i);
                    if (!string.IsNullOrEmpty(subjectCode) && subjectCode.Contains("A"))
                    {
                        // ���µ÷�
                        string scoreChk = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "V", i);
                        string shopCode = msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "B", i);


                        decimal? score = 0;
                        if (scoreChk == "Y"
                                )
                        {
                            score = 1;
                        }
                        else if (scoreChk == "N")
                        {
                            score = 0;
                        }
                        else
                        {
                            score = 9999;
                        }

                        //�ϴ�Answer����
                        service.AnswerScoreUpdate(ProjectCode_Golbal, shopCode, subjectCode, Convert.ToString(score));
                    }
                }

                //�ۺ�
                Worksheet worksheet_ShopScoreBDetail = workbook.Worksheets["�ͻ����񲿷���ϸ"] as Worksheet;
                for (int i = 3; i < 8700; i++)
                {
                    string subjectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "A", i);
                    if (!string.IsNullOrEmpty(subjectCode) && subjectCode.Contains("B"))
                    {

                        string scoreChk = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "U", i);
                        string shopCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "B", i);

                        decimal? score = 0;

                        if (scoreChk == "Y"
                                )
                        {
                            score = 1;
                        }
                        else if (scoreChk == "N")
                        {
                            score = 0;
                        }
                        else
                        {
                            score = 9999;
                        }
                        //�ϴ�Answer����
                        service.AnswerScoreUpdate(ProjectCode_Golbal, shopCode
                            , subjectCode, Convert.ToString(score));
                    }
                }
            }
            #endregion
            CommonHandler.ShowMessage(MessageType.Information, "�ϴ����");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            ProjectCode_Golbal = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            //�ۺ�
                Worksheet worksheet_ShopScoreBDetail = workbook.Worksheets["�ͻ����񲿷���ϸ"] as Worksheet;
                for (int i = 3; i < 8700; i++)
                {
                    string subjectCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "A", i);
                    if (!string.IsNullOrEmpty(subjectCode) && subjectCode.Contains("B"))
                    {

                        string remark1 = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "V", i);
                        string remark2 = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "X", i);
                        string shopCode = msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "B", i);
                        //�ϴ�Answer����
                        service.AnswerRemarkUpdate(ProjectCode_Golbal, shopCode
                            , subjectCode, remark1+"_"+remark2);
                    }
                }
           
            CommonHandler.ShowMessage(MessageType.Information, "�ϴ����");
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}