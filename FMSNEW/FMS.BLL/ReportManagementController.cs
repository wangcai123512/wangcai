using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    public class ReportManagementController : UserController
    {
        public ReportManagementController() : base("ReportManagement") { }
        public ActionResult Index()
        {
            return View();
        }
    }
}
