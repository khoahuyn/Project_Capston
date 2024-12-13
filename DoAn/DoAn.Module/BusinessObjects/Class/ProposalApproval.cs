using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
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
    [System.ComponentModel.DisplayName("Đề Xuất Phê Duyệt")]
    [NavigationItem(false)]
    [ImageName("steps")]
    [DefaultProperty("Step")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProposalApproval(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _Step;
        [XafDisplayName("Bước Phê Duyệt")]

        public string Step
        {
            get { return _Step; }
            set { SetPropertyValue<string>(nameof(Step), ref _Step, value); }
        }


        private string _Signature;
        [XafDisplayName("Ảnh Chữ Ký")]

        public string Signature
        {
            get { return _Signature; }
            set { SetPropertyValue<string>(nameof(Signature), ref _Signature, value); }
        }


        private DateTime _DueDate;
        [XafDisplayName("Ngày Đến Hạn")]
        [ModelDefault("DisPlayFormat","{0:dd/MM/yyyy}")]
        [ModelDefault("EditMask", "{0:dd/MM/yyyy}")]

        public DateTime DueDate
        {
            get { return _DueDate; }
            set { SetPropertyValue<DateTime>(nameof(DueDate), ref _DueDate, value); }
        }


        private DateTime _ApprovalDate;
        [XafDisplayName("Ngày Phê Duyệt")]
        [ModelDefault("DisPlayFormat", "{0:dd/MM/yyyy}")]
        [ModelDefault("EditMask", "{0:dd/MM/yyyy}")]
        public DateTime ApprovalDate
        {
            get { return _ApprovalDate; }
            set { SetPropertyValue<DateTime>(nameof(ApprovalDate), ref _ApprovalDate, value); }
        }


        private string _Status;
        [XafDisplayName("Tiến Trình")]

        public string Status
        {
            get { return _Status; }
            set { SetPropertyValue<string>(nameof(Status), ref _Status, value); }
        }


        private string _Note;
        [XafDisplayName("Chú Thích")]

        public string Note
        {
            get { return _Note; }
            set { SetPropertyValue<string>(nameof(Note), ref _Note, value); }
        }

        private ProposalForm _proposalform;
        [XafDisplayName("Phiếu Đề Xuất")]
        [Association]
        public ProposalForm proposalform
        {
            get { return _proposalform; }
            set { SetPropertyValue<ProposalForm>(nameof(proposalform), ref _proposalform, value); }
        }


        private Employee _employee;
        [XafDisplayName("Nhân Sự")]
        [Association]
        public Employee employee
        {
            get { return _employee; }
            set { SetPropertyValue<Employee>(nameof(employee), ref _employee, value); }
        }
    }
}