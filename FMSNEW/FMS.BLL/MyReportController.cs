using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using Utility;
using Common.Models;
using Common.DAL;

namespace FMS.BLL
{
    public class MyReportController : UserController
    {
        public MyReportController()
            : base("MyReport")
        { }
        public ActionResult Index()
        {
            return View();
        }
    }
}
