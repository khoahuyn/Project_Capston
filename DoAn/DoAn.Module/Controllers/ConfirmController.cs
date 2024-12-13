using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers
{
    public class ConfirmController: ViewController
    {
        public ConfirmController() 
        {
            TargetViewId = "ClsImport_DetailView";

        }

        protected override void OnActivated() 
        {
            base.OnActivated();
            if(View.Id == "ClsImport_DetailView") 
            { 
                var confirmation =Frame.GetController<ConfirmationDetailViewController>();
                if (confirmation != null) 
                {
                    confirmation.Active["DeactivateInCode"]=false;
                }
            }
        }
    }
}
