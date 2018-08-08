using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Script.Serialization;


namespace FMS.BLL
{
    /// <summary>
    /// 设置商业伙伴
    /// </summary>
    public class CompanyInformationSetController : UserController
    {
        public CompanyInformationSetController()
            : base("CompanyInformationSet")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<T_BankAccount> bankAccount1 = new BankAccountSvc().GetBankAccount(Session["CurrentCompanyGuid"].ToString());
            ViewBag.BankList1 = bankAccount1;
            ViewBag.CompanyInfo = new CompanySvc().GetCompanyInformation(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault();
            ViewData["CompanyName"] = (new CompanySvc().GetCompanyInformation(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).Name;
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            ViewData["LoginName"] = Session["LoginName"];
            return View(new CompanySvc().GetCompanyInformation(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault());
        }

        /// <summary>
        /// 更新商业伙伴信息
        /// </summary>
        /// <param name="Company">商业伙伴对象</param>
        /// <returns></returns>
        public string UpdCompanyInformation(T_Company information, List<T_BankAccount> bankItems)
        {
            information.C_GUID = Session["CurrentCompanyGuid"].ToString();
            bool result = new CompanySvc().UpdCompanyInformation(information);
            string msg = string.Empty;
            if (result)
            {
                Session["Taxpayer"] = information.Taxpayer;
                BankAccountSvc bankAccount = new BankAccountSvc();
                if (bankItems != null)
                {
                    foreach (T_BankAccount account in bankItems)
                    {
                        if (string.IsNullOrEmpty(account.BA_GUID))
                        {
                            account.BA_GUID = Guid.NewGuid().ToString();
                        }
                        account.C_GUID = Session["CurrentCompanyGuid"].ToString();
                        //account.AccountType = string.Empty;
                        bankAccount.UpdBankAccount(account);
                    }
                }

                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 更新银行信息
        /// </summary>
        /// <param name="Company">银行对象</param>
        /// <returns></returns>
        public string UpdBankInformation(List<T_BankAccount> bankItems)
        {
            string msg = string.Empty;


            bool result = false;
         
                BankAccountSvc bankAccount = new BankAccountSvc();
       
                foreach (T_BankAccount account in bankItems)
                    {
                    if (string.IsNullOrEmpty(account.BA_GUID))                     {
                        account.BA_GUID = Guid.NewGuid().ToString();
                    }
                    account.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    account.AccountType = string.Empty;
                    result = bankAccount.UpdBankAccount(account);
                    }
                

                msg = General.Resource.Common.Success;
           
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        public string RemoveBankInfo(string id)
        {
            bool result = new BankAccountSvc().DelBankAccount(id);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
               , result.ToString().ToLower(), msg);
        }

        public string UpdLOGO(string path)
        {
            bool result = new CompanySvc().UpdLOGO(Session["CurrentCompanyGuid"].ToString(),path);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        public string UpdBusinessLicense(string path)
        {
            bool result = new CompanySvc().UpdBusinessLicense(Session["CurrentCompanyGuid"].ToString(), path);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        public ActionResult UploadifyFun(HttpPostedFileBase Filedata)
        {
            if (Filedata == null ||
                String.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }

            string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string fn = Guid.NewGuid().ToString() + "-" + filename;
            string LOGO = "../UploadFile/" + fn;
            UpdLOGO(LOGO);
            string virtualPath = String.Format("~/UploadFile/{0}", fn);

            string path = this.Server.MapPath(virtualPath);
            Filedata.SaveAs(path);
            return this.Json(new object { });
        }

        public ActionResult UploadifyFun1(HttpPostedFileBase Filedata)
        {
            if (Filedata == null ||
                String.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }
            string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string fn = Guid.NewGuid().ToString() + "-" + filename;
            string BusinessLicense = "../UploadFile/" + fn;
            UpdBusinessLicense(BusinessLicense);
            string virtualPath = String.Format("~/UploadFile/{0}", fn);

            string path = this.Server.MapPath(virtualPath);
            Filedata.SaveAs(path);
            return this.Json(new object { });
        }

        /// <summary>
        /// 表示图片的上传结果。
        /// </summary>
        private struct Result
        {
            /// <summary>
            /// 表示图片是否已上传成功。
            /// </summary>
            public bool success;
            /// <summary>
            ///
            /// </summary>
            public string userid;
            /// <summary>
            ///
            /// </summary>
            public string username;
            /// <summary>
            /// 自定义的附加消息。
            /// </summary>
            public string msg;
            /// <summary>
            /// 表示原始图片的保存地址。
            /// </summary>
            public string sourceUrl;
            /// <summary>
            /// 表示所有头像图片的保存地址，该变量为一个数组。
            /// </summary>
            public ArrayList avatarUrls;
        }

        /// <summary>
        /// 生成指定长度的随机码。
        /// </summary>
        private string CreateRandomCode(int length)
        {
            string[] codes = new string[36] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            StringBuilder randomCode = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                randomCode.Append(codes[rand.Next(codes.Length)]);
            }
            return randomCode.ToString();
        }

        public void Upload()
        {
            Result result = new Result();
            result.avatarUrls = new ArrayList();
            result.success = false;
            result.msg = "Failure!";
            // 取服务器时间+8位随机码作为部分文件名，确保文件名无重复。
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmssff") + CreateRandomCode(8);

            #region 处理原始图片

            // 默认的 file 域名称是__source，可在插件配置参数中自定义。参数名：src_field_name
            HttpPostedFileBase file = Request.Files["__source"];
            // 如果在插件中定义可以上传原始图片的话，可在此处理，否则可以忽略。
            if (file != null)
            {
                //原始图片的文件名，如果是本地或网络图片为原始文件名、如果是摄像头拍照则为 *FromWebcam.jpg
                string sourceFileName = file.FileName;
                //原始文件的扩展名
                string sourceExtendName = sourceFileName.Substring(sourceFileName.LastIndexOf('.') + 1);
                //当前头像基于原图的初始化参数（只有上传原图时才会发送该数据，且发送的方式为POST），用于修改头像时保证界面的视图跟保存头像时一致，提升用户体验度。
                //修改头像时设置默认加载的原图url为当前原图url+该参数即可，可直接附加到原图url中储存，不影响图片呈现。
                string initParams = Request.Form["__initParams"];
                result.sourceUrl = string.Format("upload/csharp_source_{0}.{1}", fileName, sourceExtendName);
                file.SaveAs(Server.MapPath(result.sourceUrl));
                result.sourceUrl += initParams;
                /*
                 * 可在此将 result.sourceUrl 储存到数据库，如果有需要的话。
                 * Save to database...
                 */
            }

            #endregion

            #region 处理头像图片

            //默认的 file 域名称：__avatar1,2,3...，可在插件配置参数中自定义，参数名：avatar_field_names
            string[] avatars = new string[3] {"__avatar1", "__avatar2", "__avatar3"};
            int avatar_number = 1;
            int avatars_length = avatars.Length;
            for (int i = 0; i < avatars_length; i++)
            {
                file = Request.Files[avatars[i]];
                string virtualPath = string.Format("upload/csharp_avatar{0}_{1}.jpg", avatar_number, fileName);
                result.avatarUrls.Add(virtualPath);
                file.SaveAs(Server.MapPath(virtualPath));
                /*
                 *	可在此将 virtualPath 储存到数据库，如果有需要的话。
                 *	Save to database...
                 */
                avatar_number++;
            }

            #endregion

            //upload_url中传递的额外的参数，如果定义的method为get请将下面的Request.Form换为Request.QueryString
            result.userid = Request.Form["userid"];
            result.username = Request.Form["username"];

            result.success = true;
            result.msg = "Success!";
            //返回图片的保存结果（返回内容为json字符串，可自行构造，该处使用Newtonsoft.Json构造）
            Response.Write(JsonConvert.SerializeObject(result));
        }

    }
}
