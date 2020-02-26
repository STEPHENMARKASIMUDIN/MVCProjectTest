using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DebitCreditMemo.Models;
using DebitCreditMemo.LoginService;
using System.Web.Security;
using System.Configuration;
using Newtonsoft.Json;

namespace DebitCreditMemo.Controllers
{
    [OutputCacheAttribute(Duration = 0, NoStore = true)]
    public class LoginController : Controller
    {
        string service = ConfigurationManager.AppSettings["API"].ToString();
        
        public ActionResult Login()
        {
            if (TempData["Auth"] == null) 
            { TempData["Auth"] = "falsee";}
            else { }
            return View();
        }

        public ActionResult Authorize(LoginViewModel user)
        {
            //ServiceSoapClient ss = new ServiceSoapClient();
            Service ss = new Service();
            MLDirectoryLogInInfo resp = new MLDirectoryLogInInfo();
            resp = ss.MLDirectoryLoginHO(user.Username, user.Password); 
            if(resp.CostCenter.Trim().EndsWith("CAD")) {
                TempData["Auth"] = "true";
                Session["user"] = resp.FullName.Trim();
                Session["usrname"] = resp.ResID.Trim();
                FormsAuthentication.SetAuthCookie(resp.Resource, false);

                CreateResponse res = getSignatoryList();
                Session["Approvers"] = res.SignatoryList;
                
                return RedirectToAction("Memo","Memo");
            }
            else
            {
                TempData["Auth"] = "false";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logoff()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public CreateResponse getSignatoryList()
        {
            CreateResponse res = new CreateResponse();
            var source = new System.Uri(service + "/SignatoryList");
            var reqHandler = new RequestHandler("GET", source, "application/json", null);
            var response = reqHandler.HttpGetRequest();
            res = JsonConvert.DeserializeObject<CreateResponse>(response);

            return res;
        }
    }
}