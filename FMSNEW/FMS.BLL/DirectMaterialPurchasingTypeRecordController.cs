using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Web;
using System.IO;
using Aspose.Cells;
using Common.Models;
using Newtonsoft.Json;

namespace FMS.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectMaterialPurchasingTypeRecordController : UserController
    {
        public DirectMaterialPurchasingTypeRecordController()
            : base("DirectMaterialPurchasingTypeRecord")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult DirectMaterialPurchasingTypeRecord()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}