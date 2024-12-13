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
    //[System.ComponentModel.DisplayName("Vai Trò")]
    [NavigationItem("Danh Mục")]
    [ImageName("chucvu")]
    [DefaultProperty("RoleName")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Role(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _RoleName;
        [XafDisplayName("Tên Chức Danh"), Size(50)]

        public string RoleName
        {
            get { return _RoleName; }
            set { SetPropertyValue<string>(nameof(RoleName), ref _RoleName, value); }
        }


        private string _RoleNumber;
        [XafDisplayName("Mã Chức Danh")]

        public string RoleNumber
        {
            get { return _RoleNumber; }
            set { SetPropertyValue<string>(nameof(RoleNumber), ref _RoleNumber, value); }
        }


        private string _BinaryCode;
        [XafDisplayName("Mã Nhị Phân")]

        public string BinaryCode
        {
            get { return _BinaryCode; }
            set { SetPropertyValue<string>(nameof(BinaryCode), ref _BinaryCode, value); }
        }

        [Association]
        public XPCollection<Employee> Employees
        {
            get { return GetCollection<Employee>(nameof(Employees)); }
        }


        [Association]
        public XPCollection<ApprovalProcess> ApprovalProcesses
        {
            get { return GetCollection<ApprovalProcess>(nameof(ApprovalProcesses)); }
        }
    }
}