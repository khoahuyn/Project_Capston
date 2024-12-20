using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DoAn.Module.BusinessObjects.Authentication;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace DoAn.Module.BusinessObjects.Class
{
    [DefaultClassOptions]
    [NavigationItem("Danh Mục")]
    [ImageName("dexuat")]
    [DefaultProperty("Name")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    [DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Appearance("ColorDetailViewNhap1", AppearanceItemType = "LayoutItem", TargetItems = "Donvi", Criteria = "1=1", Context = "DetailView",
        FontColor = "Red", FontStyle = DevExpress.Drawing.DXFontStyle.Bold, Priority = 1)]
    [Appearance("ColorDetailViewNhap2", AppearanceItemType = "LayoutItem", TargetItems = "CS", Criteria = "1=1", Context = "DetailView",
        FontColor = "Red", FontStyle = DevExpress.Drawing.DXFontStyle.Bold, Priority = 1)]
    [Appearance("ColorDetailViewNhap3", AppearanceItemType = "LayoutItem", TargetItems = "templateform", Criteria = "1=1", Context = "DetailView",
        FontColor = "Red", FontStyle = DevExpress.Drawing.DXFontStyle.Bold, Priority = 1)]
    public class ProposalForm(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (Session.IsNewObject(this))
            {
                Date = Define.GetServerDateTime();
                ApplicationUser ns = Define.GetCurrentNhanvien();
                if (ns != null)
                {
                    user = Session.GetObjectByKey<ApplicationUser>(ns.Oid);
                    if (ns.CS != null)
                        CS = Session.GetObjectByKey<Branch>(ns.CS.Oid);
                    if (ns.Donvi != null)
                        Donvi = Session.GetObjectByKey<Department>(ns.Donvi.Oid);
                }
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(Name))
            {
                if (templateform != null && user !=null)
                {
                    Name = templateform.FormName;
                }

            }
            //UpdateTrangthai();user != null && 
        }

        private DateTime _Date;
        [XafDisplayName("Ngày Đề Xuất"), ModelDefault("AllowEdit", "false")]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue<DateTime>(nameof(Date), ref _Date, value); }
        }

        private string _Name;
        [XafDisplayName("Tên Đề Xuất"), Size(255)]

        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>(nameof(Name), ref _Name, value); }
        }



        private int _Status;
        [XafDisplayName("Tiến Trình")]
        [EditorAlias("ProgressProperty")]
        [ModelDefault("DisplayFormat", "")]

        public int Status
        {
            get { return _Status; }
            set { SetPropertyValue<int>(nameof(Status), ref _Status, value); }
        }

        private int _State;
        [XafDisplayName("Tình Trạng")]

        public int State
        {
            get { return _State; }
            set { SetPropertyValue<int>(nameof(State), ref _State, value); }
        }

        private string _Content;
        [XafDisplayName("Nội Dung")]    
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Nội dung' không được để trống!")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]

        public string Content
        {
            get { return _Content; }
            set { SetPropertyValue<string>(nameof(Content), ref _Content, value); }
        }

        

        [Association]
        public XPCollection<Sharing> Sharings
        {
            get { return GetCollection<Sharing>(nameof(Sharings)); }

        }

        [Association]
        public XPCollection<ProposalApproval> ProposalApprovals
        {
            get { return GetCollection<ProposalApproval>(nameof(ProposalApprovals)); }
        }

        [Association]
        public XPCollection<Attachment> Files
        {
            get { return GetCollection<Attachment>(nameof(Files)); }
        }


        [Association]
        public XPCollection<Comments> Comments
        {
            get { return GetCollection<Comments>(nameof(Comments)); }
        }

        private ApplicationUser _user;
        [XafDisplayName("Người Lập")]
        [RuleRequiredField("Yeucau Nhanvien", DefaultContexts.Save, "Phải có người lập phiếu")]
        [Association]
        public ApplicationUser user
        {
            get { return _user; }
            set { SetPropertyValue<ApplicationUser>(nameof(user), ref _user, value); }
        }


        private bool _IsCoso = false;
        [ImmediatePostData, Browsable(false)]
        public bool IsCoso
        {
            get { return _IsCoso; }
            set { SetPropertyValue<bool>(nameof(IsCoso), ref _IsCoso, value); }
        }


        private Branch _CS;
        [XafDisplayName("Cơ Sở (*)")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Cơ Sở' không được để trống!")]
        [Appearance("cs", Visibility = ViewItemVisibility.Hide, Criteria = "!IsCoso", Context = "DetailView")]
        [Association]
        public Branch CS
        {
            get { return _CS; }
            set { SetPropertyValue<Branch>(nameof(CS), ref _CS, value); }
        }


        private Department _Donvi;
        [XafDisplayName("Bộ Phận (*)")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Bộ Phận' không được để trống!")]
        //[DataSourceCriteria("[Nhan] = true")]
        [Appearance("bp", Visibility = ViewItemVisibility.Hide, Criteria = "IsCoso", Context = "DetailView")]
        [Association]
        public Department Donvi
        {
            get { return _Donvi; }
            set { SetPropertyValue<Department>(nameof(Donvi), ref _Donvi, value); }
        }

        private bool _Thuchien = false;
        [XafDisplayName("Thực hiện"), Browsable(false)]
        public bool Thuchien
        {
            get { return _Thuchien; }
            set
            {
                bool isModified = SetPropertyValue<bool>(nameof(Thuchien), ref _Thuchien, value);
                if (isModified && !IsLoading && !IsSaving && !IsDeleted)
                    RefreshMau();
            }
        }


        private TemplateForm _templateform;
        [XafDisplayName("Mẫu Đơn")]
        [Association]
        [RuleRequiredField("Yeucau mauphieu", DefaultContexts.Save, "Phải có mẫu đề xuất")]
        //[DataSourceProperty(nameof(DSMau))]
        public TemplateForm templateform
        {
            get { return _templateform; }
            set
            {
                bool IsModified = SetPropertyValue<TemplateForm>(nameof(templateform), ref _templateform, value);
                if (IsModified && !IsLoading && !IsSaving && !IsDeleted && value != null)
                {
                    //IsCoso = value.ApdungCS;
                    //&& Nhanvien != null
                    if (string.IsNullOrEmpty(Name) && user!=null && value != null)
                    {
                        Name = templateform.FormName;
                    }
                }
            }
            //set { SetPropertyValue<TemplateForm>(nameof(templateform), ref _templateform, value); }
        }

        private XPCollection<TemplateForm> ListMau;

        public XPCollection<TemplateForm> DSMau
        {
            get
            {
                if (ListMau == null)
                {
                    ListMau = new XPCollection<TemplateForm>(Session);
                    RefreshMau();
                }
                return ListMau;
            }
        }

        private void RefreshMau()
        {
            ListMau = new XPCollection<TemplateForm>(Session)
            {
                Criteria = CriteriaOperator.Parse("Oid=?", 0)
            };
            if (Thuchien)
            {
                ApplicationUser nv = Define.GetCurrentNhanvien();
                if (nv != null && nv.Donvi != null)
                {
                    XPCollection<TemplateForm> Mauchon = new(Session)
                    {
                        Criteria = CriteriaOperator.Parse("Thuchien=? && Huy=?", true, false)
                    };
                    foreach (TemplateForm p in Mauchon)
                    {
                        ListMau.Add(p);
                    }
                }
            }
            else
            {
                ApplicationUser nv = Define.GetCurrentNhanvien();
                if (nv != null && nv.Donvi != null)
                {
                    XPCollection<DepartmentForm> Mauchon = new(Session)
                    {
                        Criteria = CriteriaOperator.Parse("Donvi=? && Mau.Huy=?", Session.GetObjectByKey<Department>(nv.Donvi.Oid), false)
                    };
                    foreach (DepartmentForm p in Mauchon)
                    {
                        ListMau.Add(p.templateform);
                    }
                }
            }

        }

    }
}