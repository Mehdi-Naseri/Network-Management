﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RemoteComputerWeb.ViewModel;
using System.DirectoryServices.AccountManagement;

namespace RemoteComputerWeb.Controllers
{
    public class ActiveDirectoryComputersController : Controller
    {
        //
        // GET: /ActiveDirectoryComputers/
        public ActionResult Index()
        {
            string DomainName = null, ErrorMessage = null;
            try
            {
                DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            ErrorMessage += (DomainName == null || DomainName == "") ? "دسترسی به سرور اکتیو دایرکتوری امکان پذیر نشد." : null;
            ViewBag.ErrorMessage = ErrorMessage;

            ViewBag.DomainName = DomainName;
            ViewBag.Container = "dc=pseez,dc=local";
            return View();
        }


        [HttpGet]
        public ActionResult Read(string DomainName, string Container)
        {
            string error;
            IEnumerable<ActiveDirectoryComputerViewModel> a = ShowComputers(DomainName, Container, out error);
            return Json(a, JsonRequestBehavior.AllowGet);
        }


        private IEnumerable<ActiveDirectoryComputerViewModel> ShowComputers(string DomainName, string Container, out string error)
        {
            IList<ActiveDirectoryComputerViewModel> computers = new List<ActiveDirectoryComputerViewModel>();
            try
            {
                //string stringDomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                int intCounter = 0;
                PrincipalContext PrincipalContext1 = new PrincipalContext(ContextType.Domain, DomainName, Container);
                ComputerPrincipal ComputerPrincipal1 = new ComputerPrincipal(PrincipalContext1);
                PrincipalSearcher search = new PrincipalSearcher(ComputerPrincipal1);
                foreach (ComputerPrincipal result in search.FindAll())
                {
                    ActiveDirectoryComputerViewModel Computer1 = new ActiveDirectoryComputerViewModel();
                    Computer1.SamAccountName = result.SamAccountName;
                    Computer1.DisplayName = result.DisplayName;
                    Computer1.Name = result.Name;
                    Computer1.Description = result.Description;
                    Computer1.Enabled = result.Enabled;
                    Computer1.LastLogon = result.LastLogon.HasValue ? ((DateTime)result.LastLogon).ToString("yyyy/MM/dd HH:mm:ss") : null;

                    computers.Add(Computer1);
                    intCounter++;
                }
                search.Dispose();
                error = null;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return computers;
        }
	}
}