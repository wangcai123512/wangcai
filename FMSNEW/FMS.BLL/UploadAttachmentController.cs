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
    public class UploadAttachmentController : Controller
    {
      
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadAttachment(string belongid, string tamp, string number)
        {

            ViewData["belongid"] = belongid;
            ViewData["timestamp"] = tamp;
            ViewData["number"] = number;
            return View();
        }

      
    }
}
