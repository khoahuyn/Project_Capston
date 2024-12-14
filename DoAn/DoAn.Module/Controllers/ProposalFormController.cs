using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraCharts.GLGraphics.Platform;
using DevExpress.XtraRichEdit;
using DoAn.Module.BusinessObjects.Class;
using DoAn.Module.BusinessObjects.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers
{
    public class ProposalFormController: ObjectViewController<ListView, ProposalForm>
    {
        

        public ProposalFormController() 
        {
            TargetViewId = "ProposalForm_ListView";
            SimpleAction newPhieu = new(this, "newPhieu", "View")
            {
                Caption = "Create Proposal Form",
                ImageName = "dxmoi",
                TargetViewId = "ProposalForm_ListView",
                ConfirmationMessage = "Bạn có muốn tạo phiếu đề xuất ?",
            };
            newPhieu.Execute += NewPhieu_Execute;
        }

        private void NewPhieu_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalForm));
            ProposalForm pmoi = objectSpace.CreateObject<ProposalForm>();
            DetailView dView = Application.CreateDetailView(objectSpace, "Phieumoi", true, pmoi);
            Application.ShowViewStrategy.ShowViewInPopupWindow(dView, () => Lapphieu(pmoi, objectSpace));
        }

        void Lapphieu(ProposalForm phieu, IObjectSpace objectSpace)
        {
            if (LapphieuDX(phieu, objectSpace))
            {
                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(ProposalForm));
                ProposalForm p = objectSpace1.GetObject(phieu);
                //TCom.AddUserValue("viewdx", p.Oid.ToString());
                DashboardView dashboardView = Application.CreateDashboardView(objectSpace1, "viewDX", true);

                ShowViewParameters svp = new()
                {
                    CreatedView = dashboardView,
                    TargetWindow = TargetWindow.Default,
                    Context = TemplateContext.View
                };
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
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
                        return false;
                    }
                    if(phieu.employee.IsCoso)
                    {
                        if (phieu.employee.branch==null)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (phieu.employee.department == null)
                        {
                            return false;
                        }
                    }
                    if (phieu.Content != null) 
                    {
                        return false;
                    }
                    var richServer = new RichEditDocumentServer();
                    richServer.Document.RtfText = phieu.templateform.Content;
                    if (phieu.employee != null) 
                    {
                        string hoten = phieu.employee.Name;
                        string donvi = "";
                        if (phieu.employee.department != null) 
                            donvi=phieu.employee.department.DepartmentName;
                        string tencs = "";
                        if(phieu.employee.branch != null)
                        {
                            tencs = phieu.employee.branch.BranchName;
                        }
                        richServer.Document.ReplaceAll("{Hoten}", hoten, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                        richServer.Document.ReplaceAll("{Donvi}", donvi, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                        richServer.Document.ReplaceAll("{CS}", tencs, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                    }
                    Branch coso=phieu.employee.branch;
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
