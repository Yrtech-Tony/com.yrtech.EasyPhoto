using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using XHX.DTO.SingleShopReport;
using Microsoft.Office.Interop.Excel;
using XHX.Common;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Service : System.Web.Services.WebService
{
    public Service()
    {

        CommonHandler.DBConnect();
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public string HelloWorld() {
    //    return "Hello World";
    //}

    #region mou.junsheng

    #region DSAT 1.0
    #region 查询所有的Project
    [WebMethod]
    public DataSet SearchProjectNameAndCode()
    {
        string sql = string.Format("SELECT ProjectCode,ProjectName FROM Projects");
        DataSet ds = CommonHandler.query(sql);
        return ds;

    }
    #endregion

    #region 保存问卷的信息
    [WebMethod]
    public void SaveAnswer(string projectCode, string subjectCode, string shopCode,
                        decimal? score, string remark, string imageName, string userid,
                        char checkType, string passReCheck, string date, string inDate, string spCode, string spType)
    {
        string sql = "";
        if (score == null)
        {
            sql = string.Format(@"EXEC up_DSAT_Answer_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@Score = null
                                                 ,@Remark = '{4}'
                                                 ,@ImageName = '{5}'
                                                 ,@UserID = '{6}'
                                                 ,@CheckType = '{7}'
                                                 ,@PassCheck = '{8}'
                                                 ,@AssessmentDate = '{9}'
                                                ,@InDateTime = '{10}'
                                                ,@SpCode = '{11}'
                                                ,@SpType = '{12}'",
                                                    projectCode, subjectCode, shopCode, score, remark, imageName, userid,
                                                    checkType, passReCheck, date, inDate, spCode, spType);
        }
        else
        {
            sql = string.Format(@"EXEC up_DSAT_Answer_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@Score = '{3}'
                                                 ,@Remark = '{4}'
                                                 ,@ImageName = '{5}'
                                                 ,@UserID = '{6}'
                                                 ,@CheckType = '{7}'
                                                 ,@PassCheck = '{8}'
                                                 ,@AssessmentDate = '{9}'
                                                ,@InDateTime = '{10}'
                                                ,@SpCode = '{11}'
                                                ,@SpType = '{12}'",
                                                    projectCode, subjectCode, shopCode, score, remark, imageName, userid,
                                                    checkType, passReCheck, date, inDate, spCode, spType);
        }
        CommonHandler.query(sql);
    }
    #endregion

    #region 保存问卷的信息AnswerDtl
    [WebMethod]
    public void SaveAnswerDtl(string projectCode, string subjectCode, string shopCode,
                            int SeqNO, string userid, string checkOptionCode, string picNameList)
    {
        string sql = string.Format(@"EXEC up_DSAT_AnswerDtl_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@SeqNO = {3}
                                                 ,@UserID = '{4}'
                                                 ,@CheckOptionCode = '{5}'
                                                 ,@PicNameList = '{6}'",
                                                  projectCode, subjectCode, shopCode,
                                                  SeqNO, userid, checkOptionCode, picNameList);
        CommonHandler.query(sql);
    }
    #endregion

    #region 保存问卷的信息AnswerDtl2
    [WebMethod]
    public void SaveAnswerDtl2(string projectCode, string subjectCode, string shopCode, int seqNo, string userID, string checkOption, string fileName)
    {
        FileStream fs = new FileStream(fileName, FileMode.Open);
        int streamLength = (int)fs.Length;
        byte[] image = new byte[streamLength];
        fs.Read(image, 0, streamLength);
        fs.Close();

        string cString = "Data Source =.;initial Catalog = DSAT_DEV;User ID = DSAT;Password = DSAT;";
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = cString;
        //cnn.Open();
        SqlCommand command = new SqlCommand("EXEC up_DSAT_AnswerDtl2_S @ProjectCode,@SubjectCode,@ShopCode,@SeqNO,@UserID,@CheckOptions,@Image", cnn);

        SqlParameter parProjectCode = new SqlParameter("@ProjectCode", SqlDbType.VarChar, 6);
        parProjectCode.Value = projectCode;
        command.Parameters.Add(parProjectCode);

        SqlParameter parSubjectCode = new SqlParameter("@SubjectCode", SqlDbType.VarChar, 20);
        parSubjectCode.Value = subjectCode;
        command.Parameters.Add(parSubjectCode);

        SqlParameter parShopCode = new SqlParameter("@ShopCode", SqlDbType.VarChar, 20);
        parShopCode.Value = shopCode;
        command.Parameters.Add(parShopCode);

        SqlParameter parSeqNO = new SqlParameter("@SeqNO", SqlDbType.Int);
        parSeqNO.Value = seqNo;
        command.Parameters.Add(parSeqNO);

        SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
        parUserID.Value = userID;
        command.Parameters.Add(parUserID);

        SqlParameter parCheckOptionCode = new SqlParameter("@CheckOptions", SqlDbType.VarChar, 2);
        parCheckOptionCode.Value = checkOption;
        command.Parameters.Add(parCheckOptionCode);

        SqlParameter parImage = new SqlParameter("@Image", SqlDbType.Image);
        parImage.Value = image;
        command.Parameters.Add(parImage);

        cnn.Open();
        int num = command.ExecuteNonQuery();
        cnn.Close();
    }

    [WebMethod]
    public void SaveAnswerDtl2Streampic(string userID, byte[] image, string shopName, string fileName, string method, string extend, string subjectCode)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string uploadImagePath = appDomainPath + @"UploadImage\" + shopName + @"\";

        if (!Directory.Exists(appDomainPath + @"UploadImage\" + shopName))
        {
            Directory.CreateDirectory(uploadImagePath);
        }
        if (!Directory.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode))
        {
            Directory.CreateDirectory(appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\");
        }
        if (method == "upload")
        {
            MemoryStream buf = new MemoryStream(image);


            if (image != null && !File.Exists(uploadImagePath + fileName + ".jpg"))
            {
                Image picimage = Image.FromStream(buf, true);
                picimage.Save(uploadImagePath + fileName + ".jpg");
            }
        }
        else
        {
            if (image != null)
            {
                MemoryStream buf = new MemoryStream(image);

                if (extend != ".doc" && extend != ".doc" && extend != ".docx" && extend != ".xls" && extend != ".xlsx"
                    && extend != ".ppt" && extend != ".pptx")
                {
                    Image picimage = Image.FromStream(buf, true);
                    picimage.Save(appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + fileName + ".jpg");
                }
                else
                {
                    //FileStream fs = new FileStream(uploadImagePath + fileName + extend, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    //buf.WriteTo(fs);
                    //fs.Close();
                    // buf.Close();

                    using (FileStream fs = new FileStream(uploadImagePath + fileName + extend, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        fs.Write(image, 0, image.Length);
                        fs.Close();
                    }
                    //StreamWriter str = new StreamWriter(fs);

                    //picimage.Save(uploadImagePath + fileName + extend);
                }
            }

        }

    }
    [WebMethod]
    public void SaveAnswerDtl2Stream(string projectCode, string subjectCode, string shopCode, int seqNo, string userID, string checkOption, byte[] image, string shopName, string fileName)
    {

        string sql = string.Format(@"EXEC up_DSAT_AnswerDtl2_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@SeqNO = {3}
                                                 ,@UserID = '{4}'
                                                 ,@CheckOptionCode = '{5}'
                                                 ,@FileExtend = '{6}'",
                                                 projectCode, subjectCode, shopCode,
                                                 seqNo, userID, checkOption, fileName);
        CommonHandler.query(sql);
    }
    #endregion

    #region 保存复核信息
    [WebMethod]
    public void SaveRecheck(string projectCode, string subjectCode, string shopCode,
                            string userid, string recheckContent, string recheckTypeCode, string passReCheck, decimal? score, bool adminModifyChk)
    {
        string sql = "";
        if (score == null)
        {
            sql = string.Format(@"[up_DSAT_ReCheck_S]      
	                                        @ProjectCode = '{0}',
	                                        @SubjectCode = '{1}',
	                                        @ShopCode='{2}',
	                                        @ReCheckUser= '{3}',
	                                        @ReCheckContent	= '{4}',
	                                        @ReCheckTypeCode = '{5}',
	                                        @PassReCheck = {6},
                                            @Score = null,
                                            @AdminModifyChk= {7}",
                                             projectCode, subjectCode, shopCode, userid,
                                             recheckContent, recheckTypeCode, passReCheck, adminModifyChk);
        }
        else
        {
            sql = string.Format(@"[up_DSAT_ReCheck_S]      
	                                        @ProjectCode = '{0}',
	                                        @SubjectCode = '{1}',
	                                        @ShopCode='{2}',
	                                        @ReCheckUser= '{3}',
	                                        @ReCheckContent	= '{4}',
	                                        @ReCheckTypeCode = '{5}',
	                                        @PassReCheck = {6},
                                            @Score = {7},
                                            @AdminModifyChk={8}",
                                               projectCode, subjectCode, shopCode, userid,
                                               recheckContent, recheckTypeCode, passReCheck, score, adminModifyChk);
        }
        CommonHandler.query(sql);
    }
    #endregion

    #region 查询下一个问卷信息并显示
    [WebMethod]
    public DataSet SearchNextSubject(string projectCode, string shopCode,
                                    int orderNo, char checkType, string examType)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectNext_R] @ProjectCode= '{0}',@ShopCode = '{1}',@OrderNO = {2},@Type='N',@CheckType= '{3}',@SubjectTypeCodeExam = '{4}' ",
                                   projectCode, shopCode,
                                   orderNo, checkType, examType);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 查询下一个问卷的检查标准信息
    [WebMethod]
    public DataSet SearchNextSubjectInsectionStardard(string projectCode, string subjectCode, string shopCode)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl_R] @ProjectCode= '{0}',@SubjectCode = '{1}',@ShopCode = '{2}' ",
                               projectCode, subjectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 查询上一个问卷信息并显示Type = p
    [WebMethod]
    public DataSet SearchPreSubject(string projectCode, string shopCode, int orderNO, char checkType, string examType)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectNext_R] @ProjectCode= '{0}',@ShopCode = '{1}',@OrderNO = {2},@Type='P',@CheckType= '{3}',@SubjectTypeCodeExam = '{4}' ",
                            projectCode, shopCode,
                            orderNO, checkType, examType);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 查询上一个问卷信息检查标准并显示
    [WebMethod]
    public DataSet SearchPreSubjectInsecptionStardard(string projectCode, string subjectCode, string shopCode)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl_R] @ProjectCode= '{0}',@SubjectCode = '{1}',@ShopCode = '{2}' ",
                                  projectCode, subjectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 查询上一个问卷信息并显示 type = o
    [WebMethod]
    public DataSet SearchPreSubjectTypeISO(string projectCode, string shopCode, int orderNO, char checkType, string examType)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectNext_R] @ProjectCode= '{0}',@ShopCode = '{1}',@OrderNO = {2},@Type='O',@CheckType = '{3}',@SubjectTypeCodeExam = '{4}' ",
               projectCode, shopCode, orderNO, checkType, examType);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 通过shopCode查询shop
    [WebMethod]
    public DataSet SearchShopByShopCode(string shopCode)
    {
        string sql = string.Format("SELECT ShopCode,ShopName FROM Shop WHERE ShopCode = '{0}' AND UseChk = 1 ",
                                        shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 问卷开始的时候查询上次没答完的那个问卷开始回答，如果没答过问卷就查询第一个
    [WebMethod]
    public DataSet SearchStartSubject(string projectCode, string shopCode, char checkType, string examType)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectStart_R] @ProjectCode= '{0}',@ShopCode = '{1}',@CheckType = '{2}',@SubjectTypeCodeExam='{3}' ",
                                            projectCode,
                                            shopCode, checkType, examType);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 查询PassReCheckBySubjectCode
    [WebMethod]
    public DataSet SearchPassReCheckBySubjectCode(string projectCode, string subjectCode, string shopCode)
    {
        string sqlCheckType = string.Format("EXEC DSAT_SearchPassReCheckBySubjectCodeAndShopCode_R '{0}','{1}','{2}'"
                    , projectCode, subjectCode, shopCode);
        DataSet ds = CommonHandler.query(sqlCheckType);
        return ds;
    }
    #endregion

    #region 查询PassReCheckBySubjectCode
    [WebMethod]
    public DataSet SearchAnswerDtl2(string projectCode, string subjectCode, string shopCode)
    {

        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl2_R] @ProjectCode= '{0}',@SubjectCode = '{1}',@ShopCode = '{2}' ",
                            projectCode, subjectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="picName">图片或者文件的名字</param>
    /// <param name="shopName">项目名+经销商名</param>
    /// <param name="type"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] SearchAnswerDtl2Pic(string picName, string shopName, string subjectCode, string type, string code)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = "";
        if (type == "SpecialCase")
        {
            filePath = appDomainPath + @"UploadImage\" + @"SpecialCasePictures\" + code + @"\" + picName;
        }
        else if (type == "Notice")
        {
            filePath = appDomainPath + @"UploadImage\" + @"NoticeAttachment\" + code + @"\" + picName;
        }
        else
        {
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName + ".jpg"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName + ".jpg";
            }
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg";
            }
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc";
            }
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx";
            }
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls";
            }
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx";
            }
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt";
            }
            if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx"))
            {
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx";
            }
        }
        if (File.Exists(filePath))
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                byte[] b = new byte[fs.Length];
                fs.Read(b, 0, b.Length);
                fs.Close();
                return b;
            }
        }
        else
        {
            return null;
        }

    }
    [Serializable]
    public class PictureDto
    {
        public Image Picture { get; set; }
        public string PictureName { get; set; }
    }
    [WebMethod]
    public List<PictureDto> SearchAllPicture(string[] picName, string shopName)
    {
        List<PictureDto> picList = new List<PictureDto>();
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        DataSet ds = new DataSet();
        for (int i = 0; i < picName.Length; i++)
        {
            string filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName[i] + ".jpg";
            if (File.Exists(filePath))
            {
                PictureDto pic = new PictureDto();
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    Image image = Image.FromStream(fs);
                    pic.Picture = image;
                    pic.PictureName = picName[i];

                }
                picList.Add(pic);
            }
        }
        return picList;

    }
    #endregion

    #region 更新复核内容
    [WebMethod]
    public void UpdateRecheckContent(string projectCode, string subjectCode, string shopCode, int? score, string recheckContent, char checkType)
    {
        string sql = "";
        if (score == null)
        {
            sql = string.Format("Exec DSAT_UpdateforModify '{0}','{1}','{2}','Null','{4}','{5}'",
                                   projectCode,
                                   subjectCode, shopCode,
                                   score, recheckContent, checkType);
        }
        else
        {
            sql = string.Format("Exec DSAT_UpdateforModify '{0}','{1}','{2}','{3}','{4}','{5}'",
                                   projectCode,
                                   subjectCode, shopCode,
                                   score, recheckContent, checkType);
        }
        CommonHandler.query(sql);
    }
    #endregion


    #region 查询所有的CheckOptionType
    [WebMethod]
    public DataSet SearchAllCheckOptions()
    {
        string sql = string.Format("SELECT CheckOptionCode,CheckOptionName FROM CheckOptions");//cboArea.SelectedItem
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    #endregion

    #region 查询检查标准
    [WebMethod]
    public DataSet SearchInspectionStandard(string projectCode, string subjectCode)
    {
        string sql = string.Format("EXEC up_DSAT_InspectionStandard_R '{0}','{1}'", projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    #endregion

    #region 保存检查标准
    [WebMethod]
    public void SaveInspectionStandard(string projectCode, string subjectCode,
                                                int seqNO, string InspectionStandardName, string userID, char statusType)
    {
        string sql = string.Empty;
        if (statusType == 'I' || statusType == 'U')
        {
            sql = string.Format("EXEC up_DSAT_InspectionStandard_S '{0}','{1}','{2}','{3}','{4}'"
               , projectCode, subjectCode, seqNO, InspectionStandardName, userID);
        }
        else if (statusType == 'D')
        {
            sql = string.Format("EXEC  up_DSAT_InspectionStandard_D '{0}','{1}','{2}'",
                projectCode, subjectCode, seqNO);
        }
        CommonHandler.query(sql);
    }
    #endregion

    #region 查询复核结果
    [WebMethod]
    public DataSet SearchRecheckResult(string projectCode, string areaCode, string shopCode)
    {
        string sql = string.Format("EXEC [up_DSAT_ReCheck_R] @ProjectCode = '{0}', @AreaCode = '{1}', @ShopCode = '{2}' ",
                projectCode, areaCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion


    #region 查询复核Log
    [WebMethod]
    public DataSet SearchRecheckLog(string projectCode, string shopCode, string subjectCode)
    {
        string sql = string.Format("EXEC [DSAT_ReCheckLog_R] '{0}','{1}','{2}' ", projectCode, shopCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 数据导入导出
    [WebMethod]
    public DataSet ShopAndSubjectOut(string projectCode)
    {
        string sql = string.Format("Exec up_DSAT_DataTransfer_ShopAndSubject_OUT '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    [WebMethod]
    public void ShopAndSubjectIn(string doc)
    {
        string sql = string.Format("exec up_DSAT_DataTransfer_ShopAndSubject_IN '{0}'", doc);
        DataSet ds = CommonHandler.query(sql);

    }

    [WebMethod]
    public DataSet AnswerOut(string projectcode, string shopCode)
    {
        string sql = string.Format("Exec up_DSAT_DataTransfer_Answer_OUT '{0}','{1}'", projectcode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;

    }

    [WebMethod]
    public void AnswerIn(string doc, string projectCode, string shopcode)
    {
        string sql = string.Format("up_DSAT_DataTransfer_Answer_IN '{0}','{1}','{2}'", doc, projectCode, shopcode);
        DataSet ds = CommonHandler.query(sql);

    }
    [WebMethod]
    public void DeleteData(string projectCode)
    {
        string sql = string.Format("Exec DSAT_DeleteData '{0}' ", projectCode);
        DataSet ds = CommonHandler.query(sql);
    }
    #endregion


    #endregion

    #region DSAT 2.0
    #region 失分说明(LossResultReg)

    #region 查询失分说明
    [WebMethod]
    public DataSet SearchLoss(string projectCode, string subjectCode)
    {
        string sql = string.Format("EXEC [up_DSAT_LossResult_R] '{0}','{1}' ", projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 保存失分说明
    [WebMethod]
    public void SaveLoss(string lossCode, string lossName, string inUserID)
    {
        string sql = string.Format("EXEC [up_DSAT_LossResult_S] '{0}','{1}','{2}' ", lossCode, lossName, inUserID);
        DataSet ds = CommonHandler.query(sql);
    }

    [WebMethod]
    public void SaveLossForm(string projectCode, string subjectCode, string lossCode, string lossName, string inUserID, char statusType, string lossType)
    {
        string sql = "";
        if (statusType == 'I' || statusType == 'U')
        {
            sql = string.Format("EXEC [up_DSAT_LossResult_S] '{0}','{1}','{2}', '{3}','{4}','{5}'",
                projectCode, subjectCode, lossCode, lossName, inUserID, lossType);
        }
        else
        {
            sql = string.Format("EXEC [up_DSAT_LossResult_D] '{0}','{1}','{2}'", projectCode, subjectCode, lossCode);
        }
        DataSet ds = CommonHandler.query(sql);
    }
    #endregion

    #endregion
    #region 章节
    #region 查询章节信息
    [WebMethod]
    public DataSet SearchChapter(string projectCode, string chapterCode)
    {
        string sql = string.Format("EXEC [up_DSAT_Charter_R] '{0}','{1}' ", projectCode, chapterCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion

    #region 保存章节信息
    [WebMethod]
    public void SaveChapter(string projectCode, string chapterCode, string chapterName, string chapterContent,
                            int orderNo, string InUserID, decimal weight)
    {
        string sql = string.Format("EXEC [up_DSAT_Charter_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ",
                                        projectCode, chapterCode, chapterName, chapterContent, orderNo, InUserID, weight);
        DataSet ds = CommonHandler.query(sql);
        //return ds;
    }



    #endregion
    #endregion

    #region 环节
    #region 查询环节信息
    [WebMethod]
    public DataSet SearchLink(string projectCode, string chapterCode)
    {
        string sql = string.Format("EXEC [up_DSAT_Link_R] '{0}','{1}' ", projectCode, chapterCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion

    #region 保存环节信息
    [WebMethod]
    public void SaveLink(string projectCode, string chapterCode, string linkCode, string linkName, string linkContent,
                           string InUserID, decimal? weight)
    {
        string sql = string.Empty;
        if (weight == null)
        {
            sql = string.Format("EXEC [up_DSAT_Link_S] '{0}','{1}','{2}','{3}','{4}','{5}' ",
                                           projectCode, chapterCode, linkCode, linkName, linkContent, InUserID);
        }
        else
        {
            sql = string.Format("EXEC [up_DSAT_Link_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ",
                                              projectCode, chapterCode, linkCode, linkName, linkContent, InUserID, weight);
        }
        DataSet ds = CommonHandler.query(sql);
        //return ds;
    }



    #endregion
    #endregion
    #region 得分登记
    [WebMethod]
    public void DeletePicture(string shopName, string fileName, string subjectCode)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string uploadImagePath = appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\";
        if (File.Exists(uploadImagePath + fileName + ".jpg"))
        {
            File.Delete(uploadImagePath + fileName + ".jpg");
        }
    }
    [WebMethod]
    public DataSet SearchSubjectOrder(int orderNO)
    {
        string sql = string.Format("EXEC up_DSAT_SearchSubjectOrderNO {0}", orderNO);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region 加权得分率
    [WebMethod]
    public DataSet SearchHearForWeightRate(string projectCode, string shopCode)
    {
        string[] spiltShopCode = shopCode.Split(',');
        string sqlInit = string.Format("EXEC [DSAT_ChapterRatio_Head_R] '{0}','{1}' ", projectCode, spiltShopCode[0]);
        DataSet dsInit = CommonHandler.query(sqlInit);
        for (int i = 0; i < spiltShopCode.Length; i++)
        {
            string sql = string.Format("EXEC [DSAT_ChapterRatio_Head_R] '{0}','{1}' ", projectCode, spiltShopCode[i]);
            DataSet ds = CommonHandler.query(sql);
            dsInit.Merge(ds);
        }
        DataView dv = new DataView(dsInit.Tables[0]);
        System.Data.DataTable dt = dv.ToTable(true, "Column1", "Caption1", "Column2", "Caption2", "Order");
        dsInit.Tables.Clear();
        dsInit.Tables.Add(dt);
        return dsInit;
    }
    [WebMethod]
    public DataSet SearchLeftForWeightRate(string projectCode, string chapterCode, bool check)
    {
        string[] spiltChapterCode = chapterCode.Split(',');
        string sqlInit = string.Format("EXEC [DSAT_ChapterWeight_Left_R] '{0}','{1}' ", projectCode, spiltChapterCode[0]);
        DataSet dsInit = CommonHandler.query(sqlInit);


        for (int i = 0; i < spiltChapterCode.Length; i++)
        {
            string sql = string.Format("EXEC [DSAT_ChapterWeight_Left_R] '{0}','{1}' ", projectCode, spiltChapterCode[i]);
            DataSet ds = CommonHandler.query(sql);
            dsInit.Merge(ds);
        }
        if (check)
        {
            string sql = string.Format("EXEC [DSAT_FFVWeight_Left_R] '{0}' ", projectCode);
            DataSet ds = CommonHandler.query(sql);
            dsInit.Merge(ds);
        }
        DataView dv = new DataView(dsInit.Tables[0]);
        System.Data.DataTable dt = dv.ToTable(true, "CharterCode", "CharterName", "Weight");
        dsInit.Tables.Clear();
        dsInit.Tables.Add(dt);
        return dsInit;
    }
    [WebMethod]
    public DataSet SearchBodayForWeightRate(string projectCode, string chaterCode, string shopCode, bool fCheck, bool check)
    {
        string[] spiltChapterCode = chaterCode.Split(',');
        string[] spiltShopCode = shopCode.Split(',');
        string sqlInit = string.Format("EXEC [DSAT_ChapterWeight_Data_R] '{0}','{1}' ,'{2}',{3} ", projectCode, spiltChapterCode[0], spiltShopCode[0], fCheck == true ? 1 : 0);
        DataSet dsInit = CommonHandler.query(sqlInit);


        for (int i = 0; i < spiltChapterCode.Length; i++)
        {
            for (int j = 0; j < spiltShopCode.Length; j++)
            {
                string sql = string.Format("EXEC [DSAT_ChapterWeight_Data_R] '{0}','{1}' ,'{2}',{3} ", projectCode, spiltChapterCode[i], spiltShopCode[j], fCheck == true ? 1 : 0);
                DataSet ds = CommonHandler.query(sql);
                dsInit.Merge(ds);
            }
        }
        if (check)
        {
            for (int j = 0; j < spiltShopCode.Length; j++)
            {
                string sql = string.Format("EXEC [DSAT_FFVWeight_Data_R] '{0}','{1}' ", projectCode, spiltShopCode[j]);
                DataSet ds = CommonHandler.query(sql);
                dsInit.Merge(ds);
            }
        }
        DataView dv = new DataView(dsInit.Tables[0]);
        System.Data.DataTable dt = dv.ToTable(true, "Column1", "Column2", "CharterCode", "Value");
        dsInit.Tables.Clear();
        dsInit.Tables.Add(dt);
        return dsInit;
    }
    [WebMethod]
    public void ImportFFV(byte[] b, string projectCode, string inUserID)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string uploadImagePath = appDomainPath + @"UploadImage\";

        if (!Directory.Exists(appDomainPath + @"UploadImage\"))
        {
            Directory.CreateDirectory(uploadImagePath);
        }
        string path = uploadImagePath + "ffv.xls";
        if (b != null)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                fs.Write(b, 0, b.Length);
            }
        }
        string sql = string.Format("EXEC [DSAT_ImportExcel] '{0}','{1}','{2}' ", path, projectCode, inUserID);
        DataSet ds = CommonHandler.query(sql);
    }
    [WebMethod]
    public void SaveFFVRate(string projectCode, string allRate, string eastRate, string southRate,
                            string westRate, string northRate, string weight, string userID)
    {
        decimal? dallRate = null;
        decimal? deastRate = null;
        decimal? dsouthRate = null;
        decimal? dwestRate = null;
        decimal? dnorthRate = null;
        decimal? dweight = null;
        if (!string.IsNullOrEmpty(allRate) && allRate != null)
        {
            dallRate = Convert.ToDecimal(allRate);
        }
        if (!string.IsNullOrEmpty(eastRate) && eastRate != null)
        {
            deastRate = Convert.ToDecimal(eastRate);
        }
        if (!string.IsNullOrEmpty(southRate) && southRate != null)
        {
            dsouthRate = Convert.ToDecimal(southRate);
        }
        if (!string.IsNullOrEmpty(northRate) && northRate != null)
        {
            dnorthRate = Convert.ToDecimal(northRate);
        }
        if (!string.IsNullOrEmpty(westRate) && westRate != null)
        {
            dwestRate = Convert.ToDecimal(westRate);
        }

        if (!string.IsNullOrEmpty(weight) && weight != null)
        {
            dweight = Convert.ToDecimal(weight);
        }
        string sql = string.Format("exec [up_DSAT_SaveFFVRate] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
            projectCode, allRate, eastRate, southRate, westRate, northRate, weight, userID);
        CommonHandler.query(sql);
    }

    [WebMethod]
    public void SaveFFVShopRate(string projectCode, string shopCode, string weight, string userID)
    {
        string sql = string.Format("exec [up_DSAT_SaveFFVShopRate] '{0}','{1}','{2}','{3}'",
                                 projectCode, shopCode, weight, userID);
        CommonHandler.query(sql);
    }
    #endregion
    #region 飞检数据导入
    [WebMethod]
    public void SaveFeiJianSubject(string projectCode, string subjectCode, string checkPoint, string userId, string linkCode)
    {
        string sql = string.Format("exec [up_DSAT_SaveFeiJianSubject] '{0}','{1}','{2}','{3}','{4}'", projectCode, subjectCode, checkPoint, userId, linkCode);
        CommonHandler.query(sql);
    }

    [WebMethod]
    public void SaveFeiJianScore(string projectCode, string subjectCode, string shopCode, decimal? score, string userId)
    {
        string sql = string.Format("exec [up_DSAT_SaveFeiJianScore] '{0}','{1}','{2}','{3}','{4}'", projectCode, subjectCode, shopCode, score, userId);
        CommonHandler.query(sql);
    }


    #endregion

    #region 用户查询

    [WebMethod]
    public DataSet SearchUserInfo(string userID)
    {
        string sql = string.Format("Exec [up_DSAT_SearchUserInfo] '{0}'", userID);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #endregion

    #region DSAT 3.0
    #region 查询分数设置
    [WebMethod]
    public DataSet SearchScoreSet(string projectCode, string subjectCode)
    {
        string sql = string.Format("EXEC [up_DSAT_ScoreSet_R] '{0}','{1}' ", projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #region 删除分数设置
    [WebMethod]
    public void DeleteScoreSet(string projectCode, string subjectCode, int? seqNO)
    {
        string sql = string.Format("EXEC [up_DSAT_ScoreSet_D] '{0}','{1}','{2}' ", projectCode, subjectCode, seqNO);
        CommonHandler.query(sql);

    }

    #endregion
    #region 添加分数设置
    [WebMethod]
    public void InsertScoreSet(string projectCode, string subjectCode, int? seqNO, Decimal? score, bool? notInvolved, string inUserID, DateTime? inDateTime)
    {
        string sql = string.Format("EXEC [up_DSAT_ScoreSet_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", projectCode, subjectCode, seqNO, score, notInvolved, inUserID, inDateTime);
        CommonHandler.query(sql);

    }

    #endregion
    #endregion
    #region 得分登记页面
    #region 查询失分说明

    [WebMethod]
    public DataSet SearchLossDesc(string projectCode, string shopCode, string subjectCode)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl3_R] '{0}','{1}','{2}' ", projectCode, shopCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region 保存失分说明
    [WebMethod]
    public void SaveLossDesc(string projectCode, string shopCode, string subjectCode, string lossDesc, string picName, int? SeqNO, char statusType)
    {
        string sql = "";
        if (statusType != 'D')
        {
            sql = string.Format("EXEC [up_DSAT_AnswerDtl3_S] '{0}','{1}','{2}','{3}','{4}','{5}' ", projectCode, subjectCode, shopCode, SeqNO, lossDesc, picName);

        }
        else
        {
            sql = string.Format("EXEC [up_DSAT_AnswerDtl3_D] '{0}','{1}','{2}','{3}' ", projectCode, subjectCode, shopCode, SeqNO);
        }
        CommonHandler.query(sql);

    }
    #endregion
    #region 保存得分Log
    [WebMethod]
    public void SaveAnswerLog(string projectCode, string shopCode, string subjectCode, string statusCode, decimal? score, string desc, string userID)
    {
        string sql = "";
        if (score == null)
        {
            sql = string.Format(@"EXEC [up_DSAT_AnswerLog_S] @ProjectCode = '{0}',@SubjectCode = '{1}',@ShopCode='{2}',@StatusCode='{3}',
                                            @Score = null,@Desc = '{5}',@UserID='{6}' ",
                                projectCode, subjectCode, shopCode, statusCode, score, desc, userID);
        }
        else
        {
            sql = string.Format("EXEC [up_DSAT_AnswerLog_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ", projectCode, subjectCode, shopCode, statusCode, score, desc, userID);
        }
        CommonHandler.query(sql);

    }
    #endregion
    #region 申请复审

    [WebMethod]
    public void SaveRecheckStatus(string projectCode, string shopCode, string statusCode, string userID)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerRecheckStatus_S] '{0}','{1}','{2}','{3}' ", projectCode, shopCode, statusCode, userID);
        CommonHandler.query(sql);

    }
    #endregion
    #region 查询当前复审进度

    [WebMethod]
    public DataSet SearchRecheckStatus(string projectCode, string shopCode)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerRecheckStatus_R] '{0}','{1}' ", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;

    }
    #endregion
    #endregion
    #region 得分复审
    #region 复审完毕
    [WebMethod]
    public void RechekComplete(string projectCode, string shopCode, string statusTypeCode, string userID)
    {
        string sql = string.Format(@"EXEC [up_DSAT_AnswerRecheckFinish_S] @ProjectCode = '{0}',@ShopCode='{1}'
                                        ,@StatusCode='{2}' ,@UserID='{3}'", projectCode, shopCode, statusTypeCode, userID);
        CommonHandler.query(sql);
    }
    [WebMethod]
    public DataSet CheckShopAllPassRechk(string projectCode, string shopCode, string statusTypeCode)
    {
        string sql = string.Format(@"EXEC [up_DSAT_CheckShopAllPassRechk_R] @ProjectCode = '{0}',@ShopCode='{1}'
                                        ,@StatusCode='{2}'", projectCode, shopCode, statusTypeCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion
    #endregion
    #region 复审记录查询
    [WebMethod]
    public DataSet SearchAnswerLog(string projectCode, string shopCode)
    {
        string sql = string.Format("EXEC up_DSAT_AnswerLog_R '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region 复审进度查询
    [WebMethod]
    public DataSet SearchReCheckProcess(string projectCode, string shopCode)
    {
        string sql = string.Format("EXEC up_DSAT_AnswerRecheckStatus_R '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region 执行文件Popup
    [WebMethod]
    public DataSet SearchSubjectBySubjectCodeAndProjectCode(string projectCode, string subjectCode)
    {
        string sql = string.Format("EXEC up_DSAT_GetSubjectBySubjectCode_R '{0}','{1}'", projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region 同步修改的SP
    [WebMethod]
    public DataSet SyncSp()
    {
        string sql = string.Format("Exec up_DSAT_SyncSP");
        DataSet ds = CommonHandler.query(sql);
        return ds;



    }
    #endregion
    #region 得到图片路径
    [WebMethod]
    public string getImagePath(string projectCode, string userID)
    {
        string path = string.Empty;
        string sql = string.Format("exec up_DSAT_GetUserImageFilePath_R '{0}','{1}'", projectCode, userID);
        DataSet ds = CommonHandler.query(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            path = ds.Tables[0].Rows[0]["FilePath"].ToString();
        }
        return path;
    }
    #endregion
    #region 查询卖场复审状态
    [WebMethod]
    public DataSet GetShopRecheckStatus(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [up_DSAT_ReCheckStatus_R] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    #endregion
    #endregion

    #region DSAT 4.0
    #region 查询所有的ShopSubjectExam
    [WebMethod]
    public DataSet GetShopSubjectExamTypeList(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [up_DSAT_ShopProjects_R2] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion
    #region 保存卖场A/B卷信息
    [WebMethod]
    public void SaveShopExamType(string projectCode, string shopcode, string type, string checkUserId, DateTime? checkDate)
    {
        string sql = string.Format("exec up_DSAT_ShopProjects_IU '{0}','{1}','{2}','{3}','{4}'", projectCode, shopcode, type, checkUserId, checkDate);
        CommonHandler.query(sql);
    }
    #endregion
    #region 查询Shop试卷类型by shopcode and projectcode
    [WebMethod]
    public DataSet SearchShopExamTypeByProjectCodeAndShopCode(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [up_DSAT_ShopProjects_R] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion
    #region 判断哪些是没有打分的
    [WebMethod]
    public DataSet GetNotAnswerSubject(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [up_DSAT_SubjectCheck] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region 改变试卷类型的时候删除已经打分的题目
    [WebMethod]
    public void DeleteAnserForError(string projectCode, string shopCode, string subjectCode)
    {
        string sql = string.Format("exec [up_DSAT_DelErrorAnswer_D] '{0}','{1}','{2}'", projectCode, shopCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);

    }
    #endregion
    #region 保存RecheckDtl
    [WebMethod]
    public void SaveRecheckDtl(string projectCode, string subjectCode, string shopCode, string recheckTypeCode, string recheckUserId, string errorCode)
    {
        string sql = string.Format("exec [up_DSAT_ReCheckDtl_S] '{0}','{1}','{2}','{3}','{4}','{5}'"
                                , projectCode, subjectCode, shopCode, recheckUserId, recheckTypeCode, errorCode);
        CommonHandler.query(sql);
    }
    #endregion
    #region 删除RecheckDtl
    [WebMethod]
    public void DeleteRecheckDtl(string projectCode, string subjectCode, string shopCode, string recheckTypeCode, string errorCode)
    {
        string sql = string.Format("exec [up_DSAT_ReCheckDtl_D] '{0}','{1}','{2}','{3}','{4}'"
                                , projectCode, subjectCode, shopCode, recheckTypeCode, errorCode);
        CommonHandler.query(sql);
    }
    #endregion
    #region 用户自定义查询
    [WebMethod]
    public DataSet UserDinfineSearch(string xml_subject, string xml_shop, string xml_user, string projectCode, string xml_columns)
    {
        string sql = string.Format("exec up_DSAT_UserDefinedReport_R '{0}','{1}','{2}','{3}','{4}'",
                                    xml_columns, projectCode, xml_subject, xml_shop, xml_user);
        return CommonHandler.query(sql);
    }
    #endregion
    #region 查询检查标准对应的图片List
    [WebMethod]
    public DataSet GetPicListByInstandard(string projectCode, string shopCode, string subjectCode, int seqNo)
    {
        string sql = string.Format("SELECT PicNameList FROM Answerdtl WHERE projectCode = '{0}' AND shopCode = '{1}' AND SubjectCode = '{2}' AND seqNO = {3}",
                                    projectCode, shopCode, subjectCode, seqNo);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region
    [WebMethod]
    public DataSet SearchFullScoreByProjectCodeAndSubjectCode(string projectCode, string subjectCode)
    {
        string sql = string.Format("exec [up_DSAT_SubjectFullScore_R] '{0}','{1}'", projectCode, subjectCode);

        DataSet ds = CommonHandler.query(sql);
        return ds;

    }
    #endregion
    #endregion
    #region 复制上期数据
    [WebMethod]
    public DataSet CopyLastData(string oldProjectCode, string projectCode)
    {
        string sql = string.Format("exec [up_AddNewSeasonData] '{0}','{1}'",
                                    oldProjectCode, projectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region AnswerStartInfo
    [WebMethod]
    public DataSet AnswerStartInfoSearch(string projectCode, string shopCode)
    {
        string sql = string.Format("EXEC up_DSAT_AnswerStartInfo_R '{0}','{1}'", projectCode, shopCode);
        return CommonHandler.query(sql);
    }
    [WebMethod]
    public void AnswerStartInfoSave(string projectCode, string shopCode, string leaderName, string userID, string startDate, string sellStart, string sellEnd, string afterStart, string afterEnd, string sellCount, string afterCount, string sellLocalCount, string afterLocalCount)
    {
        string sql = string.Format("EXEC up_DSAT_AnswerStartInfo_S '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'",
            projectCode, shopCode, leaderName, startDate, userID, sellStart, sellEnd, afterStart, afterEnd, sellCount, afterCount, sellLocalCount, afterLocalCount);

        CommonHandler.query(sql);
    }
    #endregion
    #region 复审进度详细查询
    [WebMethod]
    public DataSet SearchReCheckProcessdtl(string projectCode, string shopCode)
    {
        string sql = string.Format("EXEC up_DSAT_AnswerRecheckStatusDtl_R '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion
    #region StandardSearch

    [WebMethod]
    public DataSet SearchCheckOptionByProjectCodeAndShopCode(string projectCode, string shopCode)
    {
        string sql = string.Format("EXEC up_DSAT_Standard_R '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchHead_InspectionStandard(string projectCode)
    {
        string sql = string.Format("exec up_DSAT_InspectionStandard_Head_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchBodyData_InspectionStandard(string projectCode, string shopCode)
    {
        string sql = string.Format("exec up_DSAT_InspectionStandard_DATA_R '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    [WebMethod]
    public DataSet SearchLeft_InspectionStandard(string projectCode, string shopCode)
    {
        string sql = string.Format("exec up_DSAT_InspectionStandard_Left_R '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion
    #region 报告使用
    [WebMethod]
    public DataSet SearchUserInfoByReport(string userId, string password)
    {
        string sql = string.Format("EXEC [up_XD2_UserInfo_R] '{0}','{1}'"
                                    , userId, password);
        return CommonHandler.query(sql);
    }
    [WebMethod]
    public DataSet SearchShopByRole(string userId, string roleType, string areaCode, string shopCode, string contantName)
    {
        string sql = string.Format("EXEC [up_XD2_RoleShop_R] '{0}','{1}','{2}','{3}','{4}'"
                                    , userId, roleType, areaCode, shopCode, contantName);
        return CommonHandler.query(sql);
    }
    [WebMethod]
    public DataSet UpdateDownLoadInfo(string shopCode, DateTime downloadDate, string downUser)
    {
        string sql = string.Format("EXEC [up_XD2_ShopUpdate_R] '{0}','{1}','{2}'"
                                       , shopCode, downloadDate, downUser);
        return CommonHandler.query(sql);
    }
    [WebMethod]
    public DataSet UpdateViewInfo(string shopCode, DateTime viewDate, string viewUser)
    {
        string sql = string.Format("EXEC [up_XD2_ShopUpdate_R1] '{0}','{1}','{2}'"
                                       , shopCode, viewDate, viewUser);
        return CommonHandler.query(sql);
    }
    #endregion
    #region RecheckError
    [WebMethod]
    public DataSet SearchRecheckError(string recheckErrorCode, string recheckErrorName)
    {
        string sql = string.Format("EXEC [up_DSAT_RecheckError_R] '{0}','{1}'"
                                    , recheckErrorCode, recheckErrorName);
        return CommonHandler.query(sql);
    }
    [WebMethod]
    public void SaveRecheckError(string recheckErrorCode, string recheckErrorName, string userId)
    {
        string sql = string.Format("EXEC [up_DSAT_RecheckError_S] '{0}','{1}','{2}'"
                                    , recheckErrorCode, recheckErrorName, userId);
        CommonHandler.query(sql);
    }
    #endregion
    #region 特殊案例更新分数
    [WebMethod]
    public void UpDateAnswer(string projectCode, string shopCode, string subjectCode, string score)
    {
        string sql = string.Format("EXEC [up_DSAT_Answer_U] '{0}','{1}','{2}','{3}'"
                                    , projectCode, shopCode, subjectCode, score);
        CommonHandler.query(sql);
    }
    [WebMethod]
    public DataSet SearchAnswer(string projectCode, string shopCode, string subjectCode)
    {
        string sql = string.Format("EXEC [up_DSAT_Answer_R] '{0}','{1}','{2}'"
                                    , projectCode, shopCode, subjectCode);
        return CommonHandler.query(sql);
    }
    #endregion
    #region 终审
    [WebMethod]
    public DataSet AnswerRecheckLastMst_R(string projectCode, string shopCode)
    {
        string sql = string.Format("EXEC [up_DSAT_AnswerRechecklastMst_R] '{0}','{1}'"
                                    , projectCode, shopCode);
        return CommonHandler.query(sql);
    }
    #endregion
    #region Return DateTime
    [WebMethod]
    public DateTime ReturnDateTimeNow()
    {
        return DateTime.Now;
    }
    #endregion
    #region Infiniti
    [WebMethod]
    public void ShopVinListSave(string projectCode, string vinCode,string shopCode,string type)
    {
        string sql = string.Format("EXEC [up_Infiniti_Answer_S] '{0}','{1}','{2}','{3}'"
                                    , projectCode, vinCode, shopCode, type);
         CommonHandler.query(sql);
    }
    [WebMethod]
    public DataSet ShopVinListSearch(string projectCode, string vinCode,string shopCode)
    {
        string sql = string.Format("EXEC [up_Infiniti_Answer_R] '{0}','{1}','{2}'"
                                    , projectCode, vinCode, shopCode);
        return CommonHandler.query(sql);
    }
    #endregion
    #endregion

    #region zhang.xichun

    #region Shop/Shop_Popup

    [WebMethod]
    public DataSet SearchShop(string shopCode, string shopName)
    {
        string sql = string.Format("EXEC [up_Infiniti_Shop_R] @ShopCode= '{0}',@ShopName = '{1}' ", shopCode, shopName);//cboArea.SelectedItem
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public void SaveShop(string shopCode, string shopName, bool useChk, string password, string areaCode, string userId)
    {
        string sql = string.Format("EXEC up_Infiniti_Shop_S '{0}','{1}','{2}','{3}','{4}'",
                                   shopCode, shopName, password, areaCode, userId);
        CommonHandler.query(sql);
    }

    #endregion

    #region ShopScoreSearch

    [WebMethod]
    public DataSet SearchHead(string projectCode, string shopCode, string userName, DateTime startDate, DateTime endDate)
    {
        string sql = string.Format("exec ReportScore_Head_R '{0}','{1}','{2}','{3}','{4}'", projectCode, shopCode, userName, startDate, endDate);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchBodyData(string projectCode, string shopCode, bool lossCheck, bool recheck, bool all, string userName, DateTime startDate, DateTime endDate)
    {
        string sql = string.Format("exec ReportScore_DATA_R '{0}','{1}',{2},{3},{4},'{5}','{6}','{7}'", projectCode, shopCode, lossCheck, recheck, all, userName, startDate, endDate);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    [WebMethod]
    public DataSet SearchLeft(string projectCode)
    {
        string sql = string.Format("exec ReportSocre_Left_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    [WebMethod]
    public DataSet SearchSubjectBySubjectCode(string subjectCode)
    {
        string sql = string.Format("select OrderNO from Subjects where SubjectCode = '{0}'", subjectCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    [WebMethod]
    public DataSet SearchPassReCheckBySubjectCodeAndShopCode(string projectCode, string subjectCode, string shopCode)
    {
        string sql = string.Format("EXEC DSAT_SearchPassReCheckBySubjectCodeAndShopCode_R '{0}','{1}','{2}'"
                   , projectCode, subjectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion

    #region Cbo_DataSource

    [WebMethod]
    public DataSet GetAllArea()
    {
        string sql = string.Format("SELECT AreaCode,AreaName FROM Area ");
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet GetAllProject()
    {
        string sql = string.Format("SELECT ProjectCode,ProjectName FROM Projects ORDER BY ORDERNO desc");
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    #endregion

    #region StandardRate

    [WebMethod]
    public DataSet SearchRateAllByProjectCode(string projectCode)
    {
        string sql = string.Format("EXEC up_DSAT_StandardRate_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchRateAllByArea(string projectCode)
    {
        string sql = string.Format("EXEC up_DSAT_StandardRateByArea_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    #endregion

    #region SubjectFile

    [WebMethod]
    public DataSet SearchSubjectFile(string projectCode, string subjectCode)
    {
        string sql = string.Format("EXEC up_DSAT_FileAndPicture_R '{0}','{1}'", projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public void SaveFileAndPicture(char statusType, string projectCode, string subjectCode, int seqNO,
                                   string fileName, string fileType)
    {
        string sql = string.Empty;
        if (statusType == 'I' || statusType == 'U')
        {
            sql = string.Format("EXEC up_DSAT_FileAndPicture_S '{0}','{1}','{2}','{3}','{4}','{5}'"
               , projectCode, subjectCode, seqNO, fileName, fileType, "Sysadmin");
        }
        else if (statusType == 'D')
        {
            sql = string.Format("EXEC  up_DSAT_FileAndPicture_D '{0}','{1}','{2}'",
                projectCode, subjectCode, seqNO);
        }
        CommonHandler.query(sql);
    }

    #endregion

    #region Subjects

    [WebMethod]
    public DataSet SearchProject()
    {
        string sql = string.Format("EXEC up_DSAT_Projects_R");
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchSubject(string projectCode, string chapterCode, string linkCode, string examTypeCode)
    {
        string sql = string.Format("EXEC up_DSAT_Subjects_R '{0}','{1}','{2}','{3}'", projectCode, chapterCode, linkCode, examTypeCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public void SaveProject(char statusType, string projectCode, string year, string quarter)
    {
        string sql = String.Empty;
        if (statusType == 'I')
        {
            sql = string.Format("EXEC up_DSAT_Projects_S '{0}','{1}','{2}','{3}','{4}'", year + quarter,
                year + quarter, "sysadmin", year, quarter);
        }
        else if (statusType == 'U')
        {
            sql = string.Format("EXEC up_DSAT_Projects_S '{0}','{1}','{2}','{3}','{4}'", projectCode,
                projectCode, "sysadmin", year, quarter);
        }
        CommonHandler.query(sql);
    }

    [WebMethod]
    public void SaveSubject(char statusType, string projectCode, string subjectCode, string implementation, string checkPoint,
                            string desc, string additionalDesc, string inspectionDesc, string inspectionNeedFile,
                            string remark, int orderNO, string linkCode, decimal fullScore, bool? scoreCheck, string subjectTypeCode, string subjectTypeCodeExam,
                            bool subjectDelChk, bool addErrorChk, decimal? weight, string fileNO)
    {
        string sql = String.Empty;
        if (statusType == 'I' || statusType == 'U')
        {
            if (weight == null)
            {
                sql = string.Format("EXEC up_DSAT_Subjects_S '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},'{14}','{15}',{16},{17}"
                                        , projectCode, subjectCode, implementation, checkPoint, desc, additionalDesc,
                                          inspectionDesc, inspectionNeedFile, remark, orderNO, "sysadmin", linkCode,
                                          fullScore, scoreCheck, subjectTypeCode, subjectTypeCodeExam, subjectDelChk, addErrorChk);
            }
            else
            {
                sql = string.Format("EXEC up_DSAT_Subjects_S '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},'{14}','{15}',{16},{17},{18}"
                                    , projectCode, subjectCode, implementation, checkPoint, desc, additionalDesc,
                                      inspectionDesc, inspectionNeedFile, remark, orderNO, "sysadmin", linkCode,
                                      fullScore, scoreCheck, subjectTypeCode, subjectTypeCodeExam, subjectDelChk, addErrorChk, weight);
            }
        }
        else if (statusType == 'D')
        {
            sql = string.Format("EXEC up_DSAT_Subjects_D '{0}','{1}'", projectCode, subjectCode);
        }
        CommonHandler.query(sql);
    }


    [WebMethod]
    public DataSet CheckSubjectExists(string projectCode, string subjectCode)
    {
        string sql = string.Format("EXEC up_DSAT_CheckSubjectExists_R '{0}','{1}'", projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchInspectionStandardByProjectCodeAndSubjectCode(string projectCode, string subjectCode)
    {
        string sql = string.Format("SELECT InspectionStandardName FROM InspectionStandard WHERE ProjectCode = '{0}' AND SubjectCode = '{1}'",
                                projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchFileAndPictureByProjectCodeAndSubjectCode(string projectCode, string subjectCode)
    {
        string sql = string.Format("SELECT [FileName] FROM FileAndPicture  WHERE ProjectCode = '{0}' AND SubjectCode = '{1}'",
                                    projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    #endregion

    #region LoginForm

    [WebMethod]
    public DataSet SearchUserByUserID(string userID)
    {
        string sql = string.Format("SELECT UserID,PSW,RoleType,MacAddress FROM dbo.UserInfo WHERE UserID = '{0}'",
                                    userID);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    #endregion

    #region RateSearch

    [WebMethod]
    public List<DataSet[]> SearchScoreRate(string projectCode, string[] chapterCodes, string[] linkCode, string[] subjectCodes, string[] shopCodes, bool fCheck)
    {
        List<DataSet[]> result = new List<DataSet[]>();
        result.Add(SearchScoreRateForChapter(projectCode, chapterCodes, shopCodes, fCheck));
        result.Add(SearchScoreRateForLink(projectCode, linkCode, shopCodes, fCheck));
        result.Add(SearchScoreRateForSubject(projectCode, subjectCodes, fCheck));

        return result;
    }

    #region ChapterRatio

    [WebMethod]
    public DataSet[] SearchScoreRateForChapter(string projectCode, string[] chapterCodes, string[] shopCodes, bool fCheck)
    {
        DataSet[] result = new DataSet[3];
        result[0] = SearchScoreRateForChapterHead(projectCode, shopCodes);
        result[1] = SearchScoreRateForChapterBodyData(projectCode, chapterCodes, shopCodes, fCheck);
        result[2] = SearchScoreRateForChapterLeft(projectCode, chapterCodes);

        return result;
    }

    [WebMethod]
    public DataSet SearchScoreRateForChapterHead(string projectCode, string[] shopCodes)
    {
        string sql = string.Format("exec DSAT_ChapterRatio_Head_R '{0}','{1}'", projectCode, shopCodes[0]);
        DataSet ds = CommonHandler.query(sql);
        for (int i = 1; i < shopCodes.Length; i++)
        {
            sql = string.Format("exec DSAT_ChapterRatio_Head_R '{0}','{1}'", projectCode, shopCodes[i]);
            DataSet TempDS = CommonHandler.query(sql);
            ds.Merge(TempDS);
        }

        DataView myDataView = new DataView(ds.Tables[0]);
        System.Data.DataTable tempDT = myDataView.ToTable(true, "Column1", "Caption1", "Column2", "Caption2", "Order");
        ds.Tables.Clear();
        ds.Tables.Add(tempDT);

        return ds;
    }

    [WebMethod]
    public DataSet SearchScoreRateForChapterBodyData(string projectCode, string[] chapterCodes, string[] shopCodes, bool fCheck)
    {
        string sql = string.Format("exec DSAT_ChapterRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, chapterCodes[0], shopCodes[0], fCheck == true ? "1" : "0");
        DataSet ds = CommonHandler.query(sql);
        for (int i = 0; i < chapterCodes.Length; i++)
        {
            for (int j = 1; j < shopCodes.Length; j++)
            {
                sql = string.Format("exec DSAT_ChapterRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, chapterCodes[i], shopCodes[j], fCheck == true ? "1" : "0");
                DataSet TempDS = CommonHandler.query(sql);
                ds.Merge(TempDS);
            }
        }

        DataView myDataView = new DataView(ds.Tables[0]);
        System.Data.DataTable tempDT = myDataView.ToTable(true, "Column1", "Column2", "CharterCode", "Value");
        ds.Tables.Clear();
        ds.Tables.Add(tempDT);

        return ds;
    }

    [WebMethod]
    public DataSet SearchScoreRateForChapterLeft(string projectCode, string[] chapterCodes)
    {
        string sql = string.Format("exec DSAT_ChapterRatio_Left_R '{0}','{1}'", projectCode, chapterCodes[0]);
        DataSet ds = CommonHandler.query(sql);
        for (int i = 1; i < chapterCodes.Length; i++)
        {
            sql = string.Format("exec DSAT_ChapterRatio_Left_R '{0}','{1}'", projectCode, chapterCodes[i]);
            DataSet TempDS = CommonHandler.query(sql);
            ds.Merge(TempDS);
        }

        DataView myDataView = new DataView(ds.Tables[0]);
        System.Data.DataTable tempDT = myDataView.ToTable(true, "CharterCode", "CharterName");
        ds.Tables.Clear();
        ds.Tables.Add(tempDT);

        return ds;
    }

    #endregion

    #region LinkRatio

    [WebMethod]
    public DataSet[] SearchScoreRateForLink(string projectCode, string[] linkCode, string[] shopCodes, bool fCheck)
    {
        DataSet[] result = new DataSet[3];
        result[0] = SearchScoreRateForLinkHead(projectCode, shopCodes);
        result[1] = SearchScoreRateForLinkBodyData(projectCode, linkCode, shopCodes, fCheck);
        result[2] = SearchScoreRateForLinkLeft(projectCode, linkCode);

        return result;
    }

    [WebMethod]
    public DataSet SearchScoreRateForLinkHead(string projectCode, string[] shopCodes)
    {
        string sql = string.Format("exec DSAT_LinkRatio_Head_R '{0}','{1}'", projectCode, shopCodes[0]);
        DataSet ds = CommonHandler.query(sql);
        for (int i = 1; i < shopCodes.Length; i++)
        {
            sql = string.Format("exec DSAT_LinkRatio_Head_R '{0}','{1}'", projectCode, shopCodes[i]);
            DataSet TempDS = CommonHandler.query(sql);
            ds.Merge(TempDS);
        }

        DataView myDataView = new DataView(ds.Tables[0]);
        System.Data.DataTable tempDT = myDataView.ToTable(true, "Column1", "Caption1", "Column2", "Caption2", "Order");
        ds.Tables.Clear();
        ds.Tables.Add(tempDT);

        return ds;
    }

    [WebMethod]
    public DataSet SearchScoreRateForLinkBodyData(string projectCode, string[] linkCode, string[] shopCodes, bool fCheck)
    {
        string sql = string.Format("exec DSAT_LinkRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, linkCode[0], shopCodes[0], fCheck == true ? "1" : "0");
        DataSet ds = CommonHandler.query(sql);
        for (int i = 0; i < linkCode.Length; i++)
        {
            for (int j = 1; j < shopCodes.Length; j++)
            {
                sql = string.Format("exec DSAT_LinkRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, linkCode[i], shopCodes[j], fCheck == true ? "1" : "0");
                DataSet TempDS = CommonHandler.query(sql);
                ds.Merge(TempDS);
            }
        }

        DataView myDataView = new DataView(ds.Tables[0]);
        System.Data.DataTable tempDT = myDataView.ToTable(true, "Column1", "Column2", "LinkCode", "Value");
        ds.Tables.Clear();
        ds.Tables.Add(tempDT);

        return ds;
    }

    [WebMethod]
    public DataSet SearchScoreRateForLinkLeft(string projectCode, string[] linkCode)
    {
        string sql = string.Format("exec DSAT_LinkRatio_Left_R '{0}','{1}'", projectCode, linkCode[0]);
        DataSet ds = CommonHandler.query(sql);
        for (int i = 1; i < linkCode.Length; i++)
        {
            sql = string.Format("exec DSAT_LinkRatio_Left_R '{0}','{1}'", projectCode, linkCode[i]);
            DataSet TempDS = CommonHandler.query(sql);
            ds.Merge(TempDS);
        }

        DataView myDataView = new DataView(ds.Tables[0]);
        System.Data.DataTable tempDT = myDataView.ToTable(true, "LinkCode", "LinkName");
        ds.Tables.Clear();
        ds.Tables.Add(tempDT);

        return ds;
    }

    #endregion

    #region SubjectRatio

    [WebMethod]
    public DataSet[] SearchScoreRateForSubject(string projectCode, string[] subjectCode, bool fCheck)
    {
        string sql = string.Format("exec DSAT_SubjectRatio_Data_R2 '{0}','{1}',{2}", projectCode, subjectCode[0], fCheck == true ? "1" : "0");
        DataSet ds = CommonHandler.query(sql);
        for (int i = 0; i < subjectCode.Length; i++)
        {
            sql = string.Format("exec DSAT_SubjectRatio_Data_R2 '{0}','{1}',{2}", projectCode, subjectCode[i], fCheck == true ? "1" : "0");
            DataSet TempDS = CommonHandler.query(sql);
            ds.Merge(TempDS);
        }

        DataView myDataView = new DataView(ds.Tables[0]);
        System.Data.DataTable tempDT = myDataView.ToTable(true, "SubjectCode", "CheckPoint", "全国", "东区", "南区", "西区", "北区");
        ds.Tables.Clear();
        ds.Tables.Add(tempDT);

        return new DataSet[] { ds };
    }

    #endregion

    #endregion

    #region FinallyScoreRateSearch

    [WebMethod]
    public List<DataSet[]> SearchFinallyScoreRate(string projectCode)
    {
        List<DataSet[]> result = new List<DataSet[]>();
        result.Add(SearchFinallyScoreRateForWeight(projectCode));
        result.Add(SearchFinallyScoreRateForOrder(projectCode));

        return result;
    }

    [WebMethod]
    public DataSet[] SearchFinallyScoreRateForWeight(string projectCode)
    {
        DataSet[] result = new DataSet[3];
        result[0] = SearchFinallyScoreRateForWeightHead(projectCode);
        result[1] = SearchFinallyScoreRateForWeightBodyData(projectCode);
        result[2] = SearchFinallyScoreRateForWeightLeft(projectCode);

        return result;
    }

    [WebMethod]
    public DataSet SearchFinallyScoreRateForWeightHead(string projectCode)
    {
        string sql = string.Format("exec DSAT_ChapterRatio_Head_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchFinallyScoreRateForWeightBodyData(string projectCode)
    {
        string sql = string.Format("exec DSAT_AllWeight_Data_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchFinallyScoreRateForWeightLeft(string projectCode)
    {
        //string sql = string.Format("exec DSAT_AllWeight_Left_R '{0}'", projectCode);
        string sql = string.Format("SELECT 'All' as CharterCode, '所有' as CharterName");
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet[] SearchFinallyScoreRateForOrder(string projectCode)
    {
        DataSet[] result = new DataSet[1];

        string sql = string.Format("exec DSAT_ShopRateRank_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);

        result[0] = ds;
        return result;
    }

    #endregion
    #region RoleTypeProgram

    [WebMethod]//查询RoleTypeProgram
    public DataSet SearchRoleTypeProgramByRoleTypeCode(string roleTypeCode)
    {
        string sql = string.Format("SELECT RoleTypeProgramID,RoleTypeCode,ProgramCode FROM dbo.RoleTypeProgram WHERE RoleTypeCode = '{0}'", roleTypeCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]//插入RoleTypeProgram
    public void InsertRoleTypeProgram(string roleTypeCode, string programCode, string inUserID, DateTime inDateTime)
    {
        string sql = string.Format("INSERT INTO dbo.RoleTypeProgram VALUES('{0}','{1}','{2}','{3}')", roleTypeCode, programCode, inUserID, inDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        CommonHandler.query(sql);
    }

    [WebMethod]//删除RoleTypeProgram
    public void DeleteRoleTypeProgram(int roleTypeProgramID)
    {
        string sql = string.Format("DELETE dbo.RoleTypeProgram WHERE RoleTypeProgramID = {0}", roleTypeProgramID);
        CommonHandler.query(sql);
    }

    [WebMethod]//查询全部RoleType
    public DataSet SearchAllRoleType()
    {
        string sql = string.Format("SELECT RoleTypeID,RoleTypeCode,RoleTypeName FROM dbo.RoleType");
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]//查询全部Program
    public DataSet SearchAllProgram()
    {
        string sql = string.Format("SELECT ProgramID,ProgramCode,ProgramName FROM dbo.Program");
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]//查询当前用户的菜单
    public DataSet SearchCurrentUserProgram(string roleTypeCode)
    {
        string sql = string.Format("SELECT p.ProgramCode,p.ProgramName FROM dbo.Program AS p INNER JOIN dbo.RoleTypeProgram AS r ON r.ProgramCode = p.ProgramCode WHERE r.RoleTypeCode = '{0}'", roleTypeCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    #endregion

    #region UserInfo

    [WebMethod]//插入UserInfoDto
    public void InsertUserInfo(string userID, string psw, string roleType, string inUserID, string userName, string macAddress)
    {
        string sql = string.Format("INSERT INTO dbo.UserInfo VALUES('{0}','{1}','{2}','{3}',GETDATE(),'{4}','{5}')", userID, psw, roleType, inUserID, userName, macAddress);
        CommonHandler.query(sql);
    }

    [WebMethod]//删除UserInfoDto
    public void DeleteUserInfoDto(string userID)
    {
        string sql = string.Format("DELETE dbo.UserInfo WHERE UserID = '{0}'", userID);
        CommonHandler.query(sql);
    }

    [WebMethod]//修改UserInfoDto
    public void UpdateUserInfoDto(string userID, string psw, string roleType, string inUserID, string userName, string macAddress)
    {
        string sql = string.Format("UPDATE dbo.UserInfo SET PSW = '{1}', RoleType = '{2}',InUserID='{3}',InDateTime=GETDATE(),UserName = '{4}',MacAddress = '{5}' WHERE UserID = '{0}'", userID, psw, roleType, inUserID, userName, macAddress);
        CommonHandler.query(sql);
    }



    [WebMethod]//查询UserInfoDto
    public DataSet SearchUserInfoDto(string userID, string roleType)
    {
        if (string.IsNullOrEmpty(userID) && string.IsNullOrEmpty(roleType))
        {
            string sql = string.Format("SELECT UserID,RoleType,PSW,UserName,MacAddress FROM dbo.UserInfo");
            return CommonHandler.query(sql);
        }
        else if (string.IsNullOrEmpty(userID))
        {
            string sql = string.Format("SELECT UserID,RoleType,PSW,UserName,MacAddress FROM dbo.UserInfo WHERE RoleType = '{0}'", roleType);
            return CommonHandler.query(sql);
        }
        else if (string.IsNullOrEmpty(roleType))
        {
            string sql = string.Format("SELECT UserID,RoleType,PSW,UserName,MacAddress FROM dbo.UserInfo WHERE UserID LIKE '%{0}%'", userID);
            return CommonHandler.query(sql);
        }
        else
        {
            string sql = string.Format("SELECT UserID,RoleType,PSW,UserName,MacAddress FROM dbo.UserInfo WHERE UserID LIKE '%{0}%' AND RoleType = '{1}'", userID, roleType);
            return CommonHandler.query(sql);
        }
    }

    #endregion
    #region MainForm

    [WebMethod]
    public DataSet SearchSpecialCaseByNeedVICoConfirm()
    {
        string sql = @"SELECT A.[ProjectCode]
                                 ,A.[ShopCode]
                                 ,B.[ShopName]
                                 ,A.[SubjectCode]
                                 ,A.SpecialCaseCode
                             FROM [dbo].[SpecialCase] AS A
                       INNER JOIN [dbo].[Shop] AS B
                               ON A.ShopCode = B.ShopCode
                            WHERE A.NeedVICoConfirmChk = 1
                              AND (A.VICoAdvice is null OR A.VICoAdvice = '')";
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion
    #region 数据导入导出
    [WebMethod]
    public void SaveUserImageFilePath(string projectCode, string userID, string folderPath)
    {
        string sql = string.Format("EXEC [up_DSAT_SaveUserImageFilePath_CU] @ProjectCode='{0}',@UserID='{1}',@FilePath='{2}'", projectCode, userID, folderPath);
        CommonHandler.query(sql);
    }

    [WebMethod]
    public DataSet SearchUserImageFilePath(string projectCode, string userID)
    {
        string sql = string.Format("EXEC [up_DSAT_GetUserImageFilePath_R] @ProjectCode='{0}',@UserID='{1}'", projectCode, userID);
        return CommonHandler.query(sql);
    }
    #endregion

    #region Area
    [WebMethod]
    public DataSet GetAllAreaType()
    {
        string sql = string.Format("SELECT A.Code AS AreaTypeCode,A.CNDesc AS AreaTypeName FROM HiddenCode as A WHERE A.GroupCode = 'AreaType'");//cboArea.SelectedItem
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    [WebMethod]
    public DataSet SearchArea(string areaTypeCode)
    {
        string sql = string.Format("EXEC [up_DSAT_Area_R] '{0}'", areaTypeCode);//cboArea.SelectedItem
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public void SaveArea(string areaCode, string areaName, string upperAreaCode, string areaTypeCode, string userID, string areaMng, string password, string areaNameEn, string email)//CodeList<XHX.DTO.AreaDto> areaList,string userID
    {
        string sql = string.Format("EXEC [up_DSAT_Area_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'",
                         areaCode, areaName, upperAreaCode, areaTypeCode, userID, areaMng, password, areaNameEn, email);
        CommonHandler.query(sql);
    }
    [WebMethod]
    public void DeleteArea(string areaCode)
    {
        string sql = string.Format("EXEC [up_DSAT_Area_D] '{0}'",
                         areaCode);
        CommonHandler.query(sql);
    }
    #endregion
    #region 获取项目当前版本
    [WebMethod]
    public DataSet getCurrentVersion()
    {
        string sql = string.Format("select CurrentVersion from ProjectVersion");
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region PadToDB

    [WebMethod]
    public void SaveAnswerList(List<String> dataList)
    {
        foreach (String data in dataList)
        {
            try
            {
                string[] properties = data.Split('$');
                string projectCode = properties[0];
                string subjectCode = properties[1];
                string shopCode = properties[2];
                decimal? score = Convert.ToDecimal(properties[3]);
                string remark = properties[4];
                string imageName = properties[5];
                string userid = properties[6];
                char checkType = Convert.ToChar(properties[7]);
                string passReCheck = properties[8];
                string date = properties[9];
                string indate = properties[10];
                string spCode = properties[11];
                string spType = properties[12];
                SaveAnswer(projectCode, subjectCode, shopCode,
                           score, remark, imageName, userid,
                           checkType, passReCheck, date, indate, spCode, spType);
            }
            catch (Exception)
            {

            }

        }
    }


    [WebMethod]
    public void SaveAnswerLogList(List<String> dataList)
    {
        foreach (String data in dataList)
        {
            try
            {
                string[] properties = data.Split('$');
                string projectCode = properties[0];
                string subjectCode = properties[1];
                string shopCode = properties[2];
                decimal? score = 0;
                try { score = Convert.ToDecimal(properties[3]); }
                catch { score = null; }
                string desc = properties[4];
                string userID = properties[5];
                string statusCode = properties[6];
                SaveAnswerLog(projectCode, shopCode, subjectCode,
                           statusCode, score, desc, userID);
            }
            catch (Exception)
            {

            }

        }
    }


    [WebMethod]
    public void SaveAnswerDtlList(List<String> dataList)
    {
        foreach (String data in dataList)
        {
            try
            {
                string[] properties = data.Split('$');
                string projectCode = properties[0];
                string subjectCode = properties[1];
                string shopCode = properties[2];
                int SeqNO = Convert.ToInt32(properties[3]);
                string userid = properties[4];
                string checkOptionCode = String.IsNullOrEmpty(properties[5]) ? "01" : properties[5];
                string picNameList = properties[6];
                SaveAnswerDtl(projectCode, subjectCode, shopCode,
                           SeqNO, userid, checkOptionCode, picNameList);
            }
            catch (Exception)
            {

            }

        }
    }


    [WebMethod]
    public void SaveAnswerDtl2StreamList(List<String> dataList)
    {
        foreach (String data in dataList)
        {
            try
            {
                string[] properties = data.Split('$');
                string projectCode = properties[0];
                string subjectCode = properties[1];
                string shopCode = properties[2];
                int seqNO = Convert.ToInt32(properties[3]);
                string userid = properties[4];
                string checkOptionCode = properties[5];
                SaveAnswerDtl2Stream(projectCode, subjectCode, shopCode,
                           seqNO, userid, checkOptionCode, null, "", "");
            }
            catch (Exception)
            {

            }

        }
    }


    [WebMethod]
    public void SaveLossDescList(List<String> dataList)
    {
        foreach (String data in dataList)
        {
            try
            {
                string[] properties = data.Split('$');
                string projectCode = properties[0];
                string subjectCode = properties[1];
                string shopCode = properties[2];
                int seqNO = Convert.ToInt32(properties[3]);
                string picName = properties[4];
                SaveLossDesc(projectCode, shopCode, subjectCode,
                           "", picName, seqNO, 'I');
            }
            catch (Exception)
            {

            }

        }
    }


    //[WebMethod]
    //public void UploadImgZipFile(byte[] zipFile)
    //{
    //    string uploadZipFilePath = AppDomain.CurrentDomain.BaseDirectory + @"UploadZip\";
    //    DirectoryInfo dir = new DirectoryInfo(uploadZipFilePath);
    //    if (!dir.Exists)
    //    {
    //        dir.Create();
    //    }
    //    string uploadZipFileName = Path.Combine(uploadZipFilePath, Guid.NewGuid().ToString() + ".zip");
    //    FileStream fs = new FileStream(uploadZipFileName, FileMode.Create);
    //    fs.Write(zipFile, 0, zipFile.Length);
    //    fs.Close();

    //    string uploadImgPath = AppDomain.CurrentDomain.BaseDirectory + @"UploadImage\";
    //    CommonHandler.UnZip(uploadZipFileName, uploadImgPath, "");

    //    File.Delete(uploadZipFileName);
    //}
    [WebMethod]
    public void UploadImgZipFile(string parentDirName, byte[] zipFile)
    {
        string uploadZipFilePath = AppDomain.CurrentDomain.BaseDirectory + @"UploadZip\";
        DirectoryInfo dir = new DirectoryInfo(uploadZipFilePath);
        if (!dir.Exists)
        {
            dir.Create();
        }
        string uploadZipFileName = Path.Combine(uploadZipFilePath, Guid.NewGuid().ToString() + ".zip");
        FileStream fs = new FileStream(uploadZipFileName, FileMode.Create);
        fs.Write(zipFile, 0, zipFile.Length);
        fs.Close();

        string uploadImgPath = AppDomain.CurrentDomain.BaseDirectory + @"UploadImage\" + parentDirName + @"\";
        CommonHandler.UnZip(uploadZipFileName, uploadImgPath, "");

        File.Delete(uploadZipFileName);
    }

    [WebMethod]
    public DataSet SearchAllProjectsForPad()
    {
        string sql = "SELECT * FROM Projects";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllChaptersForPad()
    {
        string sql = "SELECT * FROM Chapters";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllCharterLinkForPad()
    {
        string sql = "SELECT * FROM Charter_Link";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllSubjectsForPad()
    {
        string sql = "SELECT * FROM Subjects";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllInspectionStandardForPad()
    {
        string sql = "SELECT * FROM InspectionStandard";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllLossResultForPad()
    {
        string sql = "SELECT * FROM LossResult";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllFileAndPictureForPad()
    {
        string sql = "SELECT * FROM FileAndPicture";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllScoreSetForPad()
    {
        string sql = "SELECT * FROM ScoreSet";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllShopProjectsForPad()
    {
        string sql = "SELECT * FROM ShopProjects";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllUserInfoForPad()
    {
        string sql = "SELECT * FROM UserInfo";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllSpecialCaseForPad()
    {
        string sql = "SELECT * FROM SpecialCase";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllAreaForPad()
    {
        string sql = "SELECT * FROM Area";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllAreaShopForPad()
    {
        string sql = "SELECT * FROM AreaShop";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchAllShopForPad()
    {
        string sql = "SELECT * FROM Shop";
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    #region SaveAnswerDtl3

    [WebMethod]
    public void SaveAnswerDtl3StringList(List<String> dataList)
    {
        foreach (String data in dataList)
        {
            try
            {
                string[] properties = data.Split('$');
                string projectCode = properties[0];
                string subjectCode = properties[1];
                string shopCode = properties[2];
                int seqNO = Convert.ToInt32(properties[3]);
                string lossDesc = properties[4];
                string picName = properties[5];
                if (!string.IsNullOrEmpty(lossDesc))
                {
                    SaveAnswerDtl3(projectCode, subjectCode, shopCode,
                               seqNO, lossDesc, picName);
                }
            }
            catch (Exception)
            {

            }

        }

    }


    [WebMethod]
    public void SaveAnswerDtl3(string projectCode, string subjectCode, string shopCode, int seqNo, string lossDesc, string picName)
    {
        string sql = string.Format(@"EXEC up_DSAT_AnswerDtl3_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@SeqNO = {3}
                                                 ,@LossDesc = '{4}'
                                                 ,@PicName = '{5}'",
                                                 projectCode, subjectCode, shopCode,
                                                 seqNo, lossDesc, picName);
        CommonHandler.query(sql);

        //        string sql1 = string.Format(@"EXEC up_DSAT_AnswerDtl3_U
        //                                    @ProjectCode = '{0}'
        //                                     ,@SubjectCode = '{1}'
        //                                     ,@ShopCode = '{2}'", projectCode, subjectCode, shopCode);
        //CommonHandler.query(sql1);
    }

    #endregion


    #endregion
    #region SingleShopReport
    [WebMethod]
    public DataSet[] GetShopReportDto(string projectCode, string shopCode)
    {

        string sql = string.Format("exec [dbo].[up_DSAT_RPT_FengMian_R] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);
        DataSet[] dataSetList = new DataSet[] { ds, SearchChaptersScore(projectCode, shopCode), SearchLinkScore(projectCode, shopCode), SearchSubjectsScore(projectCode, shopCode), SearchAllScore(projectCode, shopCode) };

        return dataSetList;
    }
    private DataSet SearchAllScore(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [dbo].[up_DSAT_RPT_AllScore_R2_Weight] '{0}','{1}'", projectCode, shopCode);
        return CommonHandler.query(sql);
    }
    private DataSet SearchChaptersScore(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [dbo].[up_DSAT_RPT_ChaptersScore_R2_Weight] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    private DataSet SearchLinkScore(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [dbo].[up_DSAT_RPT_LinkScore_R2_Weight] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    private DataSet SearchSubjectsScore(string projectCode, string shopCode)
    {
        string sql = string.Format("exec [dbo].[up_DSAT_RPT_SubjectsScore_R2_Weight] '{0}','{1}'", projectCode, shopCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchShopByProjectCode(string projectCode)
    {
        string sql = string.Format(@"SELECT ShopCode,ShopName FROM Shop where ShopCode in(
                                select ShopCode from Answer where ProjectCode = '{0}' group by ShopCode) ", projectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public DataSet SearchShopNotScore(string projectCode)
    {
        string sql = string.Format("EXEC up_DSAT_ShopCodeNotScore_R '{0}'", projectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public void InsertShopNotScore(string projectCode, string shopCode, bool notScoreChk)
    {
        string sql = string.Empty;
        if (notScoreChk)
        {
            sql = string.Format("EXEC up_DSAT_ShopCodeNotScore_S '{0}','{1}'", projectCode, shopCode);
        }
        else
        {
            sql = string.Format("EXEC up_DSAT_ShopCodeNotScore_D '{0}','{1}'", projectCode, shopCode);
        }
        DataSet ds = CommonHandler.query(sql);
    }


    //private ShopReportDto GetShopReportDtoClient(string projectCode, string shopCode)
    //{
    //    DataSet[] dataSetList = GetShopReportDto(projectCode, shopCode);
    //    ShopReportDto shopReportDto = new ShopReportDto();
    //    List<ChaptersScoreDto> chaptersScoreDtoList = new List<ChaptersScoreDto>();
    //    List<LinkScoreDto> linkScoreDtoList = new List<LinkScoreDto>();
    //    List<SubjectsScoreDto> subjectsScoreDtoList = new List<SubjectsScoreDto>();
    //    List<AllScoreDto> allScoreDtoList = new List<AllScoreDto>();
    //    shopReportDto.ChaptersScoreDtoList = chaptersScoreDtoList;
    //    shopReportDto.LinkScoreDtoList = linkScoreDtoList;
    //    shopReportDto.SubjectsScoreDtoList = subjectsScoreDtoList;
    //    shopReportDto.AllScoreDtoList = allScoreDtoList;

    //    DataSet ds = dataSetList[0];
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            shopReportDto.ProjectCode = Convert.ToString(ds.Tables[0].Rows[i]["ProjectCode"]);
    //            shopReportDto.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
    //            shopReportDto.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
    //            shopReportDto.AreaName = Convert.ToString(ds.Tables[0].Rows[i]["AreaName"]);
    //            shopReportDto.Province = Convert.ToString(ds.Tables[0].Rows[i]["Province"]);
    //            shopReportDto.City = Convert.ToString(ds.Tables[0].Rows[i]["City"]);
    //            shopReportDto.ShopNamePY = Convert.ToString(ds.Tables[0].Rows[i]["ShopNamePY"]);
    //            shopReportDto.ProvincePY = Convert.ToString(ds.Tables[0].Rows[i]["ProvincePY"]);
    //            shopReportDto.CityPY = Convert.ToString(ds.Tables[0].Rows[i]["CityPY"]);
    //            shopReportDto.AreaNameEn = Convert.ToString(ds.Tables[0].Rows[i]["AreaNameEn"]);

    //        }
    //    }
    //    ds = dataSetList[4];
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            AllScoreDto allScoreDto = new AllScoreDto();
    //            allScoreDto.Weight = Convert.ToDecimal(ds.Tables[0].Rows[i]["Weight"]);
    //            allScoreDto.ScoreShop = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreShop"]);
    //            allScoreDto.ScoreArea_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_AVG"]);
    //            allScoreDto.ScoreArea_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_MAX"]);
    //            allScoreDto.ScoreAll_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_AVG"]);
    //            allScoreDto.ScoreAll_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_MAX"]);
    //            allScoreDtoList.Add(allScoreDto);
    //        }
    //    }
    //    ds = dataSetList[1];
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            ChaptersScoreDto chaptersScoreDto = new ChaptersScoreDto();
    //            chaptersScoreDto.CharterCode = Convert.ToInt32(ds.Tables[0].Rows[i]["CharterCode"]);
    //            chaptersScoreDto.Weight = Convert.ToDecimal(ds.Tables[0].Rows[i]["Weight"]);
    //            chaptersScoreDto.ScoreShop = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreShop"]);
    //            chaptersScoreDto.ScoreArea_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_AVG"]);
    //            chaptersScoreDto.ScoreArea_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_MAX"]);
    //            chaptersScoreDto.ScoreAll_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_AVG"]);
    //            chaptersScoreDto.ScoreAll_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_MAX"]);
    //            chaptersScoreDtoList.Add(chaptersScoreDto);
    //        }
    //    }

    //    ds = dataSetList[2];
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            LinkScoreDto linkScoreDto = new LinkScoreDto();
    //            linkScoreDto.CharterCode = Convert.ToInt32(ds.Tables[0].Rows[i]["CharterCode"]);
    //            linkScoreDto.CharterName = Convert.ToString(ds.Tables[0].Rows[i]["CharterName"]);
    //            linkScoreDto.LinkCode = Convert.ToString(ds.Tables[0].Rows[i]["LinkCode"]);
    //            linkScoreDto.LinkName = Convert.ToString(ds.Tables[0].Rows[i]["LinkName"]);
    //            linkScoreDto.ScoreShop = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreShop"]);
    //            linkScoreDto.ScoreArea_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_AVG"]);
    //            linkScoreDto.ScoreArea_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreArea_MAX"]);
    //            linkScoreDto.ScoreAll_AVG = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_AVG"]);
    //            linkScoreDto.ScoreAll_MAX = Convert.ToDecimal(ds.Tables[0].Rows[i]["ScoreAll_MAX"]);
    //            linkScoreDtoList.Add(linkScoreDto);
    //        }
    //    }

    //    ds = dataSetList[3];
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            SubjectsScoreDto subjectsScoreDto = new SubjectsScoreDto();
    //            subjectsScoreDto.SubjectCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectCode"]);
    //            subjectsScoreDto.FullScore = Convert.ToDecimal(ds.Tables[0].Rows[i]["FullScore"]);
    //            subjectsScoreDto.Score = Convert.ToDecimal(ds.Tables[0].Rows[i]["Score"]);
    //            subjectsScoreDto.LostDesc = Convert.ToString(ds.Tables[0].Rows[i]["LossDesc"]);
    //            subjectsScoreDto.PicName = Convert.ToString(ds.Tables[0].Rows[i]["PicName"]);
    //            subjectsScoreDtoList.Add(subjectsScoreDto);
    //        }
    //    }

    //    return shopReportDto;
    //}
    //private void WriteDataToExcel(string projectCode, string shopCode)
    //{
    //    ShopReportDto shopReportDto = GetShopReportDtoClient(projectCode, shopCode);
    //    MSExcelUtil msExcelUtil = new MSExcelUtil();
    //    //  Workbook workbook = msExcelUtil.OpenExcelByMSExcel(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\Template\SingleShopReportTemplate_20130812.xlsx");
    //    string path = AppDomain.CurrentDomain.BaseDirectory + @"Single\" + "BMWSingleReport.xlsx";
    //    Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path.Trim());

    //    #region 封面
    //    {
    //        Worksheet worksheet_FengMian = workbook.Worksheets["封面"] as Worksheet;
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "E9", shopReportDto.ShopCode);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "I9", shopReportDto.ShopName);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "E11", shopReportDto.AreaName);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "I11", shopReportDto.Province);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "E13", shopReportDto.ProjectCode);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "I13", shopReportDto.City);

    //        msExcelUtil.SetCellValue(worksheet_FengMian, "E10", shopReportDto.ShopCode);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "I10", shopReportDto.ShopNamePY);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "E12", shopReportDto.AreaNameEn);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "I12", shopReportDto.ProvincePY);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "E14", shopReportDto.ProjectCode);
    //        msExcelUtil.SetCellValue(worksheet_FengMian, "I14", shopReportDto.CityPY);
    //        //设置单元格的格式
    //        /*虽然模板设置了格式，但是在赋值之后格式消失了，所以需要重新设置*/

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 9, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 9, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 9, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 9, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 11, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 11, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 11, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 11, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 13, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 13, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 13, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 13, "Arial");



    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 10, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 10, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 10, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 10, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 12, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 12, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 12, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 12, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 14, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "E", 14, "Arial");

    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 14, "微软雅黑");
    //        msExcelUtil.SetCellFont(worksheet_FengMian, "I", 14, "Arial");
    //    }
    //    #endregion

    //    #region 经销商得分概况
    //    {
    //        Worksheet worksheet_ShopScore = workbook.Worksheets["经销商得分概况"] as Worksheet;
    //        List<ChaptersScoreDto> chaptersScoreDtoList = shopReportDto.ChaptersScoreDtoList;
    //        /*Modify by 20130831*/
    //        int rowIndex = 3;
    //        foreach (AllScoreDto allscoreDto in shopReportDto.AllScoreDtoList)
    //        {
    //            //msExcelUtil.SetCellValue(worksheet_ShopScore, "C", rowIndex, allscoreDto.Weight);
    //            msExcelUtil.SetCellValue(worksheet_ShopScore, "D", rowIndex, allscoreDto.ScoreShop);
    //            //如果勾选的话只查询经销商的得分，
    //            //如果没有勾选的话则区域和全国都查询
    //            //if (!checkBox1.Checked)
    //            //{
    //            //    msExcelUtil.SetCellValue(worksheet_ShopScore, "E", rowIndex, allscoreDto.ScoreArea_AVG);
    //                msExcelUtil.SetCellValue(worksheet_ShopScore, "F", rowIndex, allscoreDto.ScoreArea_MAX);
    //                msExcelUtil.SetCellValue(worksheet_ShopScore, "G", rowIndex, allscoreDto.ScoreAll_AVG);
    //                msExcelUtil.SetCellValue(worksheet_ShopScore, "H", rowIndex, allscoreDto.ScoreAll_MAX);
    //            //}

    //            rowIndex++;
    //        }

    //        foreach (ChaptersScoreDto chaptersScoreDto in chaptersScoreDtoList)
    //        {
    //            // msExcelUtil.SetCellValue(worksheet_ShopScore, "C", rowIndex, chaptersScoreDto.Weight);
    //            msExcelUtil.SetCellValue(worksheet_ShopScore, "D", rowIndex, chaptersScoreDto.ScoreShop);

    //            //如果勾选的话只查询经销商的得分，
    //            //如果没有勾选的话则区域和全国都查询
    //            //if (!checkBox1.Checked)
    //            //{
    //                msExcelUtil.SetCellValue(worksheet_ShopScore, "E", rowIndex, chaptersScoreDto.ScoreArea_AVG);
    //                msExcelUtil.SetCellValue(worksheet_ShopScore, "F", rowIndex, chaptersScoreDto.ScoreArea_MAX);
    //                msExcelUtil.SetCellValue(worksheet_ShopScore, "G", rowIndex, chaptersScoreDto.ScoreAll_AVG);
    //                msExcelUtil.SetCellValue(worksheet_ShopScore, "H", rowIndex, chaptersScoreDto.ScoreAll_MAX);
    //            //}

    //            rowIndex++;
    //        }

    //        List<LinkScoreDto> linkScoreDtoList = shopReportDto.LinkScoreDtoList;
    //        foreach (LinkScoreDto linkScoreDto in linkScoreDtoList)
    //        {
    //            int aa = linkScoreDtoList.IndexOf(linkScoreDto);
    //            for (int i = 25; i < 80; i++)
    //            {
    //                string cellValue = msExcelUtil.GetCellValue(worksheet_ShopScore, "B", i);
    //                if (cellValue != "" && cellValue.Trim().Contains((linkScoreDto.LinkCode + linkScoreDto.LinkName).Trim()))
    //                {
    //                    msExcelUtil.SetCellValue(worksheet_ShopScore, "D", i, linkScoreDto.ScoreShop);

    //                    //如果勾选的话只查询经销商的得分，
    //                    //如果没有勾选的话则区域和全国都查询
    //                    //if (!checkBox1.Checked)
    //                    //{
    //                        msExcelUtil.SetCellValue(worksheet_ShopScore, "E", i, linkScoreDto.ScoreArea_AVG);
    //                        msExcelUtil.SetCellValue(worksheet_ShopScore, "F", i, linkScoreDto.ScoreArea_MAX);
    //                        msExcelUtil.SetCellValue(worksheet_ShopScore, "G", i, linkScoreDto.ScoreAll_AVG);
    //                        msExcelUtil.SetCellValue(worksheet_ShopScore, "H", i, linkScoreDto.ScoreAll_MAX);
    //                    //}

    //                }
    //            }
    //        }
    //    }
    //    #endregion

    //    #region 指标点得分详情
    //    List<SubjectsScoreDto> subjectsScoreDtoListDetail = shopReportDto.SubjectsScoreDtoList;
    //    {
    //        Worksheet worksheet_ShopScoreDetail = workbook.Worksheets["指标点得分详情"] as Worksheet;
    //        int rowIndex1 = 3;
    //        foreach (SubjectsScoreDto subjectsScoreDto in subjectsScoreDtoListDetail)
    //        {

    //            msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "E", rowIndex1, subjectsScoreDto.FullScore);
    //            if (subjectsScoreDto.Score == 9999 || subjectsScoreDto.Score == Convert.ToDecimal(9999.00))
    //            {
    //                msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "F", rowIndex1, "N/A");
    //            }
    //            else
    //            {
    //                msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "F", rowIndex1, subjectsScoreDto.Score);
    //            }
    //            msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "G", rowIndex1, subjectsScoreDto.LostDesc);
    //            //设置单元格的格式
    //            /*虽然模板设置了格式，但是在赋值之后格式消失了，所以需要重新设置*/
    //            msExcelUtil.SetCellFont(worksheet_ShopScoreDetail, "G", rowIndex1, "微软雅黑");
    //            msExcelUtil.SetCellFont(worksheet_ShopScoreDetail, "G", rowIndex1, "Arial");
    //            rowIndex1++;
    //        }

    //        //foreach (SubjectsScoreDto subjectsScoreDto in subjectsScoreDtoListDetail)
    //        //{

    //        //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "H", rowIndex1, subjectsScoreDto.FullScore);
    //        //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "I", rowIndex1, subjectsScoreDto.Score);
    //        //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail, "J", rowIndex1, subjectsScoreDto.LostDesc);
    //        //    rowIndex1++;
    //        //}
    //    }
    //    #endregion
    //    #region 失分照片
    //    {
    //        List<SubjectsScoreDto> subjectsScoreDtoList = shopReportDto.SubjectsScoreDtoList;
    //        Worksheet worksheet_ShopScoreDetail2 = workbook.Worksheets["失分照片"] as Worksheet;
    //        int rowIndex = 3;
    //        foreach (SubjectsScoreDto subjectsScoreDto in subjectsScoreDtoList)
    //        {
    //            if (String.IsNullOrEmpty(subjectsScoreDto.PicName))
    //            {
    //                msExcelUtil.DeleteRow(worksheet_ShopScoreDetail2, rowIndex);
    //                continue;
    //            }
    //            else
    //            {
    //                //msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "F", rowIndex, subjectsScoreDto.LostDesc);
    //                string[] picNameArray = subjectsScoreDto.PicName.Split(';');
    //                int picIndex = 0;
    //                foreach (string picName in picNameArray)
    //                {
    //                    if (picIndex != 0 && picIndex % 3 == 0)
    //                    {
    //                        msExcelUtil.AddRow(worksheet_ShopScoreDetail2, ++rowIndex);
    //                    }
    //                    if (string.IsNullOrEmpty(picName)) continue;
    //                    byte[] bytes = SearchAnswerDtl2Pic(picName.Replace(".jpg", ""), shopReportDto.ProjectCode + shopReportDto.ShopName, subjectsScoreDto.SubjectCode, "", "");
    //                    if (bytes == null || bytes.Length == 0) continue;
    //                    Image.FromStream(new MemoryStream(bytes)).Save(Path.Combine(Path.GetTempPath(), picName + ".jpg"));
    //                    int colIndex = 3 + picIndex % 3;

    //                    msExcelUtil.InsertPicture(worksheet_ShopScoreDetail2, worksheet_ShopScoreDetail2.Cells[rowIndex, colIndex] as Microsoft.Office.Interop.Excel.Range, Path.Combine(Path.GetTempPath(), picName + ".jpg"), rowIndex);
    //                    picIndex++;
    //                }
    //            }

    //            rowIndex++;
    //        }
    //    }
    //    #endregion
    //    //workbook.Save(Path.Combine(tbnFilePath.Text,shopReportDto.ProjectCode+"_"+shopReportDto.ShopName+".xls"));
    //    string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"Single\", shopReportDto.AreaName + "_" + shopReportDto.ShopCode + "_" + shopReportDto.ShopName + ".xlsx");
    //    if (File.Exists(savePath))
    //    {
    //        File.Delete(savePath);
    //    }
    //    workbook.Close(true, Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"Single\", shopReportDto.AreaName + "_" + shopReportDto.ShopCode + "_" + shopReportDto.ShopName + ".xlsx"), Type.Missing);
    //}
    #endregion
    #region SingleShopReport_DMS
    [WebMethod]
    public DataSet[] GetShopReportDtoDMS(string projectCode, string shopCode, string type)
    {
        DataSet[] dataSetList = new DataSet[] { SearchShopInfoDMS(projectCode, shopCode, type), 
            SearchAnswerCount(projectCode, shopCode,type), SearchFailCount(projectCode, shopCode,type),
            SearchAnswerDetail(projectCode, shopCode,type), };

        return dataSetList;
    }
    private DataSet SearchShopInfoDMS(string projectCode, string shopCode, string type)
    {
        string sql = "";
        if (type == "A")
        {
            sql = string.Format("exec [dbo].[up_DMS_RPT_ShopInfo_Sell_R] '{0}','{1}'", projectCode, shopCode);
        }
        else
        {
            sql = string.Format("exec [dbo].[up_DMS_RPT_ShopInfo_After_R] '{0}','{1}'", projectCode, shopCode);
        }
        return CommonHandler.query(sql);
    }
    private DataSet SearchAnswerCount(string projectCode, string shopCode, string type)
    {
        string sql = "";
        if (type == "A")
        {
            sql =
                string.Format("exec [dbo].[up_DMS_RPT_Answer_Sell_R] '{0}','{1}'", projectCode, shopCode);
        }
        else
        {
            sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_After_R] '{0}','{1}'", projectCode, shopCode);
        }
        return CommonHandler.query(sql);
    }
    private DataSet SearchFailCount(string projectCode, string shopCode, string type)
    {
        string sql = "";
        if (type == "A")
        {
            sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_SellFailCount_R] '{0}','{1}'", projectCode, shopCode);
        }
        else
        {
            sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_AfterFailCount_R] '{0}','{1}'", projectCode, shopCode);
        }
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    private DataSet SearchAnswerDetail(string projectCode, string shopCode, string type)
    {
        string sql = "";
        if (type == "A")
        {
            sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_SellDetail_R] '{0}','{1}'", projectCode, shopCode);
        }
        else
        {
            sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_AfterDetail_R] '{0}','{1}'", projectCode, shopCode);
        }
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public List<string> SearchPicNameList(string projectCode, string shopName, string subjectCode)
    {
        List<string> strList = new List<string>();
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory + @"UploadImage\" + projectCode + shopName + @"\" + subjectCode + @"\";
        if (Directory.Exists(appDomainPath))
        {
            DirectoryInfo dir = new DirectoryInfo(appDomainPath);

            foreach (FileInfo str in dir.GetFiles())
            {
                if (str.Extension == ".jpg" || str.Extension == ".JPG")
                {
                    strList.Add(str.Name);
                }
            }
        }
        return strList;
    }

    [WebMethod]
    public void UpdateSellTool(string projectCode, string shopCode, string subjectCode, string sellCustomerName
       , string sellVinCode, string sellInvoiceDate, string sellInvoiceDMSDate)
    {
        string sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_SellTool_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'",
            projectCode, shopCode, subjectCode, sellCustomerName, sellVinCode, sellInvoiceDate, sellInvoiceDMSDate);
        CommonHandler.query(sql);
    }
    [WebMethod]
    public void UpdateAfterTool(string projectCode, string shopCode, string subjectCode, string afterInvoiceDate
       , string afterInvoiceDMSDate, string afterInvoiceMony, string afterDMSmony)
    {
        string sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_AfterTool_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'",
            projectCode, shopCode, subjectCode, afterInvoiceDate, afterInvoiceDMSDate, afterInvoiceMony, afterDMSmony);
        CommonHandler.query(sql);
    }
    [WebMethod]
    public void AnswerScoreUpdate(string projectCode, string shopCode, string subjectCode, string score)
    {
        string sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_ScoreUpdate_S] '{0}','{1}','{2}','{3}'",
            projectCode, shopCode, subjectCode, score);
        CommonHandler.query(sql);
    }
    [WebMethod]
    public void AnswerRemarkUpdate(string projectCode, string shopCode, string subjectCode, string remark)
    {
        string sql = string.Format("exec [dbo].[up_DMS_RPT_Answer_RemarkUpdate_S] '{0}','{1}','{2}','{3}'",
            projectCode, shopCode, subjectCode, remark);
        CommonHandler.query(sql);
    }
    [WebMethod]
    public void ShopAreaUpdate(string shopCode, string areaCode)
    {
        string sql = string.Format("exec [dbo].[up_DMS_RPT_AreaShop_Update_S] '{0}','{1}'",
            shopCode, areaCode);
        CommonHandler.query(sql);
    }
    #endregion
    #endregion

    #region Chai.YunChun

    #region 公告PopUp
    //按照NoticeID查询公告
    [WebMethod]
    public DataSet GetNoticeByNoticeID(string noticeID)
    {
        string sql = string.Format(@"EXEC [up_DSAT_Notice_R] @NoticeID = '{0}'", noticeID);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    //按照NoticeID查询公告附件
    [WebMethod]
    public DataSet GetAllNoticeAttachment(string noticeID)
    {
        string sql = string.Format(@"EXEC [up_DSAT_NoticeAttachment_R] @NoticeID = '{0}'", noticeID);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    //保存公告并查询
    [WebMethod]
    public DataSet SaveNoticeAndSearch(string noticeID, string noticeTitle, string noticeContent, string userID)
    {
        string sql = string.Format(@"EXEC [up_DSAT_Notice_S] @NoticeID = '{0}'
                                        ,@NoticeTitle = '{1}'
                                        ,@NoticeContent = '{2}'
                                        ,@UserID = '{3}'", noticeID, noticeTitle, noticeContent, userID);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    //保存公告附件
    [WebMethod]
    public void InsertNoticeAttachment(string noticeID, string attachName, byte[] file)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string uploadImagePath = appDomainPath + @"UploadImage\NoticeAttachment\" + noticeID + "\\";
        if (!Directory.Exists(uploadImagePath))
        {
            Directory.CreateDirectory(uploadImagePath);
        }
        if (file != null)
        {
            MemoryStream buf = new MemoryStream(file);

            FileStream fs = new FileStream(uploadImagePath + attachName, FileMode.OpenOrCreate);
            buf.WriteTo(fs);
            buf.Close();
            fs.Close();
            buf = null;
            fs = null;
        }

        string sql = string.Format(@"EXEC [up_DSAT_NoticeAttachment_S] @NoticeID = '{0}'
                                        ,@AttachName = '{1}'", noticeID, attachName);
        CommonHandler.query(sql);
    }
    //删除公告附件
    [WebMethod]
    public void DeleteNoticeAttachment(string noticeID, string seqNO, string attachName)
    {

        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string uploadImagePath = appDomainPath + @"UploadImage\NoticeAttachment\";

        if (File.Exists(uploadImagePath + noticeID + "\\" + attachName))
        {
            try
            {
                File.Delete(uploadImagePath + noticeID + "\\" + attachName);
            }
            catch
            {

            }
        }

        string sql = string.Format(@"EXEC [up_DSAT_NoticeAttachment_D] @NoticeID = '{0}'
                                        ,@SeqNO = '{1}'", noticeID, seqNO);
        CommonHandler.query(sql);


    }

    //下载公告的附件，用流
    [WebMethod]
    public byte[] DownNoticeAttachment(string noticeID, string attachName)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = appDomainPath + @"UploadImage\NoticeAttachment\" + noticeID + "\\" + attachName;
        if (File.Exists(filePath))
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);

            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, b.Length);
            fs.Close();
            return b;
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region 公告查询
    [WebMethod]
    public DataSet GetAllNotice(DateTime startDate, DateTime endDate)
    {
        string sql = string.Format(@"EXEC [up_DSAT_NoticeSelectAll_R] 
                                            @StartDate = '{0}'
                                            ,@EndDate = '{1}'", startDate, endDate);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public void DeleteNotice(string noticeID)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string uploadImagePath = appDomainPath + @"UploadImage\NoticeAttachment\";

        if (Directory.Exists(uploadImagePath + noticeID))
        {
            try
            {
                Directory.Delete(uploadImagePath + noticeID, true);
            }
            catch
            { }
        }

        string sql = string.Format(@"EXEC [up_DSAT_Notice_D]
                                            @NoticeID = '{0}'", noticeID);
        CommonHandler.query(sql);

    }
    #endregion

    #region 特殊案例登记
    //登记，确认特殊案例
    [WebMethod]
    public DataSet InsertSpecialCase(string specialCaseCode, string projectCode, string shopCode
            , string subjectCode, string title, string applyDesc, string finalAdvice
            , string RegType, string userID, string imageName, bool needVICoConfirmChk, string vICoAdvice)
    {
        string sql = string.Format(@"EXEC [dbo].[up_DSAT_SpecialCase_CU]
		@SpecialCaseCode = '{0}',
		@ProjectCode = '{1}',
		@ShopCode = '{2}',
		@SubjectCode = '{3}',
		@Title = '{4}',
		@ApplyDesc = '{5}',
		@FinalAdvice = '{6}',
		@RegType = '{7}',
		@UserID = '{8}',
        @ImageName = '{9}',
        @NeedVICoConfirmChk = {10},
        @VICoAdvice = '{11}'", specialCaseCode, projectCode, shopCode, subjectCode, title, applyDesc, finalAdvice, RegType, userID, imageName, needVICoConfirmChk ? 1 : 0, vICoAdvice);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }

    //保存特殊安全的图片信息
    [WebMethod]
    public void InsertSpecialCasePic(string specialCaseCode, string picName, byte[] pic)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string uploadImagePath = appDomainPath + @"UploadImage\SpecialCasePictures\" + specialCaseCode + "\\";
        if (!Directory.Exists(uploadImagePath))
        {
            Directory.CreateDirectory(uploadImagePath);
        }
        if (pic != null)
        {
            MemoryStream buf = new MemoryStream(pic);

            Image picimage = Image.FromStream(buf, true);
            picimage.Save(uploadImagePath + picName);
        }

        string sql = string.Format(@"EXEC [up_DSAT_SpecialCasePic_U] @SpecialCaseCode = '{0}'
                                        ,@ImageName = '{1}'", specialCaseCode, picName);
        CommonHandler.query(sql);
    }

    [WebMethod]
    public DataSet GetSpecialCase(string specialCaseCode)
    {
        string sql = string.Format(@"EXEC [dbo].[up_DSAT_SpecialCase_R]
  @SpecialCaseCode = '{0}'", specialCaseCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }


    [WebMethod]
    public DataSet GetSpecialCaseSubject(string projectCode, string subjectCode)
    {
        string sql = string.Format(@"EXEC [dbo].[up_DSAT_GetSpecialCaseSubject_R]
		@ProjectCode = '{0}',
		@SubjectCode = '{1}'", projectCode, subjectCode);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    [WebMethod]
    public byte[] GetSpecialCasePic(string specialCaseCode, string picName)
    {
        string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = appDomainPath + @"UploadImage\SpecialCasePictures\" + specialCaseCode + "\\" + picName;
        if (File.Exists(filePath))
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);

            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, b.Length);
            fs.Close();
            return b;
        }
        else
        {
            return null;
        }
    }
    [WebMethod]
    public void DeleteSpecialCase(string specialCaseCode)
    {
        string sql = string.Format(@"[up_DSAT_SpecialCase_D] @SpecialCaseCode = '{0}'", specialCaseCode);
        CommonHandler.query(sql);
    }

    #endregion

    #region 特殊案例查询

    [WebMethod]
    public DataSet GetAllSpecialCase(string projectCode, string shopCode, string subjectCode, DateTime startDate, DateTime endDate)
    {
        string sql = string.Format(@"EXEC [up_DSAT_SpecialCaseSearchAll_R]
		@ProjectCode = '{0}',
		@ShopCode = '{1}',
		@SubjectCode = '{2}',
        @StartDate = '{3}',
        @EndDate = '{4}'", projectCode, shopCode, subjectCode, startDate, endDate);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    #endregion

    #region 执行组修改
    //查询一审结果及修改前后的分数
    [WebMethod]
    public DataSet GetAllExecuteTeamAlter(string projectCode, string reCheckTypeCode, string shopCode, string subjectCode, DateTime startDate, DateTime endDate, bool passRecheck)
    {
        string sql = string.Format(@"EXEC [up_DSAT_ExecuteTeamAlterSearchAll_R]
        @ProjectCode = '{0}',   
        @ShopCode = '{1}',
        @SubjectCode = '{2}',
        @StartDate = '{3}',
        @EndDate = '{4}',
        @ReCheckTypeCode = '{5}' ,
        @PassRecheck = '{6}'", projectCode, shopCode, subjectCode, startDate, endDate, reCheckTypeCode, passRecheck);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    //保存修改后的分数
    [WebMethod]
    public void SaveExecuteTeamAlter(string projectCode, string shopCode, string subjectCode, string reCheckTypeCode, bool AgreeCheck, string AgreeReason, decimal? newScore, string userID)
    {
        if (newScore == null)
        {
            string sql1 = string.Format(@"EXEC [up_DSAT_ExecuteTeamAlterSave_CU]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@SubjectCode= '{2}'
                                    ,@ReCheckTypeCode= '{3}'
                                    ,@AgreeCheck = {4}
                                    ,@AgreeReason = '{5}'
                                    ,@NewScore = null
                                    ,@UserID = '{7}'", projectCode, shopCode, subjectCode, reCheckTypeCode, AgreeCheck ? 1 : 0, AgreeReason, newScore, userID);
            DataSet ds1 = CommonHandler.query(sql1);
        }
        else
        {
            string sql = string.Format(@"EXEC [up_DSAT_ExecuteTeamAlterSave_CU]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@SubjectCode= '{2}'
                                    ,@ReCheckTypeCode= '{3}'
                                    ,@AgreeCheck = {4}
                                    ,@AgreeReason = '{5}'
                                    ,@NewScore = '{6}'
                                    ,@UserID = '{7}'", projectCode, shopCode, subjectCode, reCheckTypeCode, AgreeCheck ? 1 : 0, AgreeReason, newScore, userID);
            DataSet ds = CommonHandler.query(sql);
        }

    }
    //修改状态为一审修改完毕
    [WebMethod]
    public void SaveReCheckStatus(string projectCode, string shopCode, string statusCode, string userID)
    {
        string sql = string.Format(@"EXEC [up_DSAT_ReCheckStatus_S]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@StatusCode = '{2}'
                                    ,@UserID = '{3}'", projectCode, shopCode, statusCode, userID);
        DataSet ds = CommonHandler.query(sql);
    }
    [WebMethod]
    public DataSet SearchExecuteTeamUnAgreeCount(string projectCode, string shopCode, string reCheckTypeCode)
    {
        string sql = string.Format(@"EXEC [up_DSAT_ExecuteTeamUnAgreeCount_R]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@ReCheckTypeCode = '{2}'", projectCode, shopCode, reCheckTypeCode);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }
    #endregion

    #region 仲裁组修改
    //仲裁组查询
    [WebMethod]
    public DataSet GetAllArbitrationTeamAlter(string projectCode, string shopCode, string subjectCode, DateTime startDate, DateTime endDate)
    {
        string sql = string.Format(@"EXEC [up_DSAT_ArbitrationTeamSearchAll_R]
		@ProjectCode = '{0}',
		@ShopCode = '{1}',
		@SubjectCode = '{2}',
        @StartDate = '{3}',
        @EndDate = '{4}'", projectCode, shopCode, subjectCode, startDate, endDate);
        DataSet ds = CommonHandler.query(sql);

        return ds;
    }
    //仲裁组修改
    [WebMethod]
    public void SaveArbitrationTeamAlter(string projectCode, string shopCode, string subjectCode, string reCheckTypeCode, string lastConfirm, string confirmReason, string userID)
    {
        string sql = string.Format(@"EXEC [up_DSAT_ArbitrationTeamSave_U]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@SubjectCode= '{2}'
                                    ,@ReCheckTypeCode= '{3}'
                                    ,@LastConfirm = '{4}'
                                    ,@ConfirmReason = '{5}'
                                    ,@UserID = '{6}'", projectCode, shopCode, subjectCode, reCheckTypeCode, lastConfirm, confirmReason, userID);
        DataSet ds = CommonHandler.query(sql);
    }


    #endregion

    #endregion

    #region liu.yang

    #region Subjects

    [WebMethod]
    public DataSet GetSubjectTypeForCbo()
    {
        string sql = string.Format("SELECT Code AS SubjectTypeCode,CNDesc AS SubjectTypeName FROM HiddenCode WHERE GroupCode = 'SubjectType'");
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion

    #region ReCheckDtl
    [WebMethod]
    public DataSet SearchReCheckDtl(string projectCode, string subjectCode, string shopCode, string recheckType)
    {
        string sql = string.Format("[up_DSAT_RecheckDtl_R] '{0}','{1}','{2}','{3}'", projectCode, subjectCode, shopCode, recheckType);
        DataSet ds = CommonHandler.query(sql);
        return ds;
    }

    #endregion
    #endregion

    //    #region 本地生成单店报告使用
    //    public byte[] SearchAnswerDtl2Pic(string picName, string shopName, string subjectCode, string filePath)
    //    {
    //        string appDomainPath = filePath;
    //        if (File.Exists(appDomainPath + shopName + @"\" + subjectCode + @"\" + picName + ".jpg"))
    //        {
    //            filePath = appDomainPath + shopName + @"\" + subjectCode + @"\" + picName + ".jpg";
    //        }
    //        if (File.Exists(filePath))
    //        {
    //            using (FileStream fs = new FileStream(filePath, FileMode.Open))
    //            {
    //                byte[] b = new byte[fs.Length];
    //                fs.Read(b, 0, b.Length);
    //                fs.Close();
    //                return b;
    //            }
    //        }
    //        else
    //        {
    //            return null;
    //        }

    //    }

    //    public List<string> SearchShopByProjectCode(string projectCode)
    //    {
    //        List<String> dataList = SqliteHelper.Search(@"SELECT ShopCode,ShopName FROM Shop");

    //        return dataList;
    //    }
    //    /// <summary>
    //    /// 查询经销商发票的数量和不合格数量
    //    /// </summary>
    //    /// <param name="projectCode"></param>
    //    /// <returns></returns>
    //    public List<String> Report_SearchShopInvoice(string projectCode, string shopCode, string type)
    //    {
    //        List<String> dataList = new List<string>();
    //        //            if (type == "A")
    //        //            {
    //        //                dataList = SqliteHelper.Search(@"SELECT IFNULL(SUM(CASE WHEN A.Score <> 9999 THEN 1 END),0) AS SumCount, IFNULL(SUM(CASE WHEN A.Score = 0 THEN 1 END),0) AS NotCount
    //        // FROM  Answer A 
    //        //WHERE ProjectCode = '" + projectCode + "' AND ShopCode = '" + shopCode + "'AND A.SubjectCode LIKE 'A%'");
    //        //            }
    //        //            else
    //        //            {
    //        //                dataList = SqliteHelper.Search(@"SELECT IFNULL(SUM(CASE WHEN A.Score <> 9999 THEN 1 END),0) AS SumCount, IFNULL(SUM(CASE WHEN A.Score = 0 THEN 1 END),0) AS NotCount
    //        // FROM  Answer A 
    //        //WHERE ProjectCode = '" + projectCode + "' AND ShopCode = '" + shopCode + "'AND A.SubjectCode LIKE 'B%'");

    //        //            }
    //        return dataList;
    //    }
    //    /// <summary>
    //    /// 查询经销商的基本信息
    //    /// </summary>
    //    /// <param name="projectCode"></param>
    //    /// <returns></returns>
    //    public List<String> Report_SearchShopInfoByShopCode(string projectCode, string shopCode)
    //    {
    //        List<String> dataList = SqliteHelper.Search(@"SELECT  A.ShopCode,A.ShopName,
    //IFNULL((SELECT StartDate FROM Answerstartinfo where projectCode = '" + projectCode + "' AND shopcode = a.shopcode),'1900-01-01 00:00:00') AS StartDate FROM Shop A  WHERE  A.ShopCode = '" + shopCode + "'");
    //        return dataList;
    //    }
    //    /// <summary>
    //    /// 每个类型错误数量
    //    /// </summary>
    //    /// <param name="projectCode"></param>
    //    /// <param name="ShopCode"></param>
    //    /// <returns></returns>
    //    public List<string> Report_SearchPerTypeFailCount(string projectCode, string shopCode)
    //    {
    //        List<String> dataList = new List<string>();
    //        //            List<String> dataList = SqliteHelper.Search(@"SELECT 
    //        //IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a1' AND A.CheckOptionCode='02' THEN 1 END),0) AS A1Count,
    //        //IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a2' AND A.CheckOptionCode='02' THEN 1 END),0) AS A2Count,
    //        //IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a3' AND A.CheckOptionCode='02' THEN 1 END),0) AS A3Count,
    //        //IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a4' AND A.CheckOptionCode='02'  THEN 1 END),0) AS A4Count,
    //        //IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'b1' AND A.CheckOptionCode='02' THEN 1 END) ,0)AS B1Count,
    //        //IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'b2' AND A.CheckOptionCode='02' THEN 1 END),0) AS B2Count,
    //        //IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'b3' AND A.CheckOptionCode='02' THEN 1 END),0) AS B3Count
    //        // FROM AnswerDtl A INNER JOIN 
    //        //InspectionStandard B ON A.ProjectCode = B.ProjectCode AND A.SubjectCode = B.SubjectCode AND A.SeqNO =B.SeqNO
    //        //WHERE  A.ProjectCode='" + projectCode + "' AND A.ShopCode = '" + shopCode + "'");

    //        return dataList;
    //    }
    //    /// <summary>
    //    /// 每个发票的信息
    //    /// </summary>
    //    /// <param name="projectCode"></param>
    //    /// <param name="ShopCode"></param>
    //    /// <returns></returns>
    //    public List<string> Report_PerInvoiceInfo(string projectCode, string shopCode, string type)
    //    {
    //        List<String> dataList = new List<string>();
    //        if (type == "A")
    //        {
    //            dataList = SqliteHelper.Search(@"SELECT B.SubjectCode,C.SellInvoiceCode,
    //CASE WHEN LossDesc LIKE '%a1%' THEN 'Y' END AS A1,
    //CASE WHEN LossDesc LIKE '%a2%' THEN 'Y' END AS A2,
    //CASE WHEN LossDesc LIKE '%a3%' THEN 'Y' END AS A3,
    //CASE WHEN LossDesc LIKE '%a4%' THEN 'Y' END AS A4 ,
    //B.Remark,B.SpCode,LossDesc,PicName
    //FROM AnswerDtl3 A INNER JOIN Answer B ON A.ProjectCode = B.ProjectCode
    //AND A.ShopCode = B.ShopCode AND A.SubjectCode = B.SubjectCode
    //INNER JOIN AnswerStartInfo C ON B.ProjectCode = C.ProjectCode AND B.ShopCode = C.ShopCode
    // WHERE A.ProjectCode='" + projectCode + "' AND A.ShopCode = '" + shopCode + "'AND B.SubjectCode LIKE 'A%'");
    //        }
    //        else
    //        {
    //            dataList = SqliteHelper.Search(@"SELECT B.SubjectCode,C.AfterInvoiceCode,
    //CASE WHEN LossDesc LIKE '%b1%' THEN 'Y' END AS B1,
    //CASE WHEN LossDesc LIKE '%b2%' THEN 'Y' END AS B2,
    //CASE WHEN LossDesc LIKE '%b3%' THEN 'Y' END AS B3,
    //B.Remark,B.SpCode,LossDesc,PicName
    //FROM AnswerDtl3 A INNER JOIN Answer B ON A.ProjectCode = B.ProjectCode
    //AND A.ShopCode = B.ShopCode AND A.SubjectCode = B.SubjectCode
    //INNER JOIN AnswerStartInfo C ON B.ProjectCode = C.ProjectCode AND B.ShopCode = C.ShopCode
    // WHERE A.ProjectCode='" + projectCode + "' AND A.ShopCode = '" + shopCode + "'AND B.SubjectCode LIKE 'B%'");
    //        }

    //        return dataList;
    //    }
    //    #endregion

}
