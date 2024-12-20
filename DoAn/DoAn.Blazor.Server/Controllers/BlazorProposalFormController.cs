using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraRichEdit;
using DoAn.Module.BusinessObjects.Class;

namespace DoAn.Blazor.Server.Controllers
{
    public class BlazorProposalFormController: ObjectViewController<ListView, ProposalForm>
    {
        public BlazorProposalFormController()
        {
            TargetViewId = "ProposalForm_ListView";
            SimpleAction newPhieu = new(this, "newPhieu", "View")
            {
                Caption = "Create New Form",
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
            if (Define.LapphieuDX(phieu, objectSpace))
            {
                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(ProposalForm));
                ProposalForm p = objectSpace1.GetObject(phieu);
                Define.AddUserValue("viewdx", p.Oid.ToString());
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

    }
}
