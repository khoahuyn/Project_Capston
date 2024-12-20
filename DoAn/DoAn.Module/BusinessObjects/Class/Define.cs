using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit;
using DoAn.Module.BusinessObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.BusinessObjects.Class
{
    public static class Define
    {
        public static bool bAdmin;

        public enum EStatusDuyet
        {
            [XafDisplayName("Chờ xử lý")]
            choduyet = 0,
            [XafDisplayName("Không đồng ý")]
            huy = 1,//chuyen vb sang readonly
            [XafDisplayName("Đồng ý")]
            daduyet = 2,
        }
        public enum EStatusVB
        {
            [XafDisplayName("Chờ duyệt")]
            choduyet = 0,
            [XafDisplayName("Hủy")]
            dahuy = 1,
            [XafDisplayName("Đang duyệt")]
            dangduyet = 2,
            [XafDisplayName("Đã duyệt")]
            daduyet = 3,
        }
        public static void CustomInfo(string msg, bool bDone = false)
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            if (bDone)
                app.ShowViewStrategy.ShowMessage("Đã hoàn thành tác vụ", InformationType.Success);
            else
                app.ShowViewStrategy.ShowMessage(msg, InformationType.Success);
        }
        public static void CustomError(string msg)
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            app.ShowViewStrategy.ShowMessage(msg, InformationType.Error);
        }
        public static DateTime GetServerDateTime()
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            using IObjectSpace objectSpace = app.CreateObjectSpace(typeof(ApplicationUser));
            using Session session = ((XPObjectSpace)objectSpace).Session;
            string sql = "SELECT GETDATE();";
            DateTime timeCurrent = (DateTime)session.ExecuteScalar(sql);
            return timeCurrent;
        }


        public static ApplicationUser GetCurrentNhanvien()
        {
            ApplicationUser nv = null;
            XafApplication app = ApplicationHelper.Instance.Application;
            IObjectSpace os = app.CreateObjectSpace(typeof(ApplicationUser));
            ApplicationUser CurentUser = os.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);
            if (CurentUser != null)
            {
                if (!string.IsNullOrEmpty(CurentUser.UserName))
                {
                    nv = os.FindObject<ApplicationUser>(CriteriaOperator.Parse("Email=?", CurentUser.UserName));
                }
            }
            //os.Dispose();
            return nv;
        }

        //public static string GetNhanvienMail()
        //{
        //    string mail = "";
        //    XafApplication app = ApplicationHelper.Instance.Application;
        //    using IObjectSpace os = app.CreateObjectSpace(typeof(ApplicationUser));
        //    ApplicationUser CurentUser = os.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);
        //    if (CurentUser != null)
        //    {
        //        mail = CurentUser.UserName;
        //        if (CurentUser.UserName.ToLower() == "admin") bAdmin = true;
        //    }
        //    return mail;
        //}

        public static List<UserEnvironment> UserEnvironments = new();

        public static int GetUserIntValue(string userKey)
        {
            int value = 0;
            foreach (UserEnvironment item in UserEnvironments)
            {
                if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                {
                    value = CommonLib.CInt(item.Uservalue);
                    break;
                }
            }
            return value;
        }

        public static void AddUserValue(string userKey, string value)
        {
            bool Co = false;
            foreach (UserEnvironment item in UserEnvironments)
            {
                if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                {
                    Co = true;
                    item.Uservalue = value;
                    return;
                }
            }
            if (!Co)
            {
                UserEnvironment item = new()
                {
                    UserId = SecuritySystem.CurrentUserId.ToString(),
                    Userkey = userKey,
                    Uservalue = value
                };
                UserEnvironments.Add(item);
            }
        }

        public static bool LapphieuDX(ProposalForm phieu, IObjectSpace objectSpace)
        {
            bool lapOK = true;
            try
            {
                if (phieu != null)
                {
                    if (phieu.templateform == null)
                    {
                        Define.CustomError("Cần chọn mẫu");
                        return false;
                    }
                    if (phieu.IsCoso)
                    {
                        if (phieu.CS == null)
                        {
                            Define.CustomError("Cần chọn cơ sở trực tiếp");
                            return false;
                        }
                    }
                    else
                    {
                        if (phieu.Donvi == null)
                        {
                            Define.CustomError("Cần chọn bộ phận nhận trực tiếp");
                            return false;
                        }
                    }
                    if (phieu.Content != null)
                    {
                        Define.CustomError("Đề xuất đã có nội dung, không tạo lại được");
                        return false;
                    }
                    var richServer = new RichEditDocumentServer();
                    richServer.Document.RtfText = phieu.templateform.Content;
                    //Thong tin nguoi lap
                    if (phieu.user != null)
                    {
                        string hoten = phieu.user.Name;
                        string donvi = "";
                        if (phieu.user.Donvi != null)
                            donvi = phieu.user.Donvi.DepartmentName;
                        string tencs = "";
                        if (phieu.user.CS != null)
                        {
                            tencs = phieu.user.CS.BranchName;
                        }
                        richServer.Document.ReplaceAll("{Hoten}", hoten, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                        richServer.Document.ReplaceAll("{Donvi}", donvi, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                        richServer.Document.ReplaceAll("{CS}", tencs, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                    }
                    Branch coso = phieu.CS;
                    phieu.Save();
                    objectSpace.CommitChanges();
                    richServer.Document.ReplaceAll("{so}", phieu.Oid.ToString(), DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                    phieu.Content = richServer.Document.RtfText;

                }
            }
            catch
            {
                lapOK = false;
            }

            return lapOK;
        }
    }
}
