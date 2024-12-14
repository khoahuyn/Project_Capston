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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DoAn.Module.BusinessObjects.Class
{
    [DefaultClassOptions]
    //[System.ComponentModel.DisplayName("Nhân Sự")]
    [NavigationItem("Danh Mục")]
    [ImageName("nhanvien")]
    [DefaultProperty("EmployeeId")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    [DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Employee(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        private string _EmployeeId;
        [XafDisplayName("Số Thẻ"), Size(50)]
        [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Số Thẻ' không được để trống!")]

        public string EmployeeId
        {
            get { return _EmployeeId; }
            set { SetPropertyValue<string>(nameof(EmployeeId), ref _EmployeeId, value); }
        }

        private string _Name;
        [XafDisplayName("Họ Và Tên"), Size(50)]
        [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Họ Và Tên' không được để trống!")]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>(nameof(Name), ref _Name, value); }
        }

        private string _PhoneNumber;
        [XafDisplayName("Số Điện Thoại"), Size(50)]
        [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Số Điện Thoại' không được để trống!")]

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { SetPropertyValue<string>(nameof(PhoneNumber), ref _PhoneNumber, value); }
        }

        private string _Gender;
        [XafDisplayName("Giới Tính")]
        [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Giới Tính' không được để trống!")]

        public string Gender
        {
            get { return _Gender; }
            set { SetPropertyValue<string>(nameof(Gender), ref _Gender, value); }
        }

        private string _Email;
        [XafDisplayName("Email")]
        [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Email' không được để trống!")]

        public string Email
        {
            get { return _Email; }
            set { SetPropertyValue<string>(nameof(Email), ref _Email, value); }
        }


        private string _Avatar;
        [XafDisplayName("Chân Dung")]

        public string Avatar
        {
            get { return _Avatar; }
            set { SetPropertyValue<string>(nameof(Avatar), ref _Avatar, value); }
        }

        private string _Signature;
        [XafDisplayName("Ảnh Chữ Ký")]

        public string Signature
        {
            get { return _Signature; }
            set { SetPropertyValue<string>(nameof(Signature), ref _Signature, value); }
        }

        private bool _IsCoso = false;
        [ImmediatePostData, Browsable(false)]
        public bool IsCoso
        {
            get { return _IsCoso; }
            set { SetPropertyValue<bool>(nameof(IsCoso), ref _IsCoso, value); }
        }


        private Branch _branch;
        [XafDisplayName("Cơ Sở (*)")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Cơ Sở' không được để trống!")]
        [Appearance("cs", Visibility = ViewItemVisibility.Hide, Criteria = "!IsCoso", Context = "DetailView")]

        [Association]
        public Branch branch
        {
            get { return _branch; }
            set { SetPropertyValue(nameof(branch), ref _branch, value); }
        }




        private Department _department;
        [XafDisplayName("Bộ Phận (*)")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Bộ Phận' không được để trống!")]
        [DataSourceCriteria("[Nhan] = true")]
        [Appearance("bp", Visibility = ViewItemVisibility.Hide, Criteria = "IsCoso", Context = "DetailView")]

        [Association]
        public Department department
        {
            get { return _department; }
            set { SetPropertyValue(nameof(department), ref _department, value); }
        }


        private Role _role;
        [XafDisplayName("Vai Trò")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Vai Trò' không được để trống!")]

        [Association]
        public Role role
        {
            get { return _role; }
            set { SetPropertyValue<Role>(nameof(role), ref _role, value); }
        }



        [Association]
        public XPCollection<Sharing> Sharings
        {
            get { return GetCollection<Sharing>(nameof(Sharings)); }
        }

        [Association]
        public XPCollection<Attachment> Files
        {
            get { return GetCollection<Attachment>(nameof(Files)); }
        }


        [Association]
        public XPCollection<ProposalApproval> ProposalApprovals
        {
            get { return GetCollection<ProposalApproval>(nameof(ProposalApprovals)); }
        }


        [Association]
        public XPCollection<ProposalForm> ProposalForms
        {
            get { return GetCollection<ProposalForm>(nameof(ProposalForms)); }
        }



        [Association]
        public XPCollection<TemplateForm> TemplateForms
        {
            get { return GetCollection<TemplateForm>(nameof(TemplateForms)); }
        }


        [Association]
        public XPCollection<Comments> Comments
        {
            get { return GetCollection<Comments>(nameof(Comments)); }
        }


    }
}