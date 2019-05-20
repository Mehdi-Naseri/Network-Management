using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemoteComputerWeb.ViewModel
{
    public class ActiveDirectoryComputerViewModel
    {
        public String SamAccountName { get; set; }
        public String DisplayName { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Boolean? Enabled { get; set; }
        //public DateTime? LastLogon { get; set; }
        public string LastLogon { get; set; }
    }
}