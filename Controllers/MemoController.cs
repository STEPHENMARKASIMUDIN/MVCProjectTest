using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;
using DebitCreditMemo.Models;
using log4net.Config;
using DebitCreditMemo.Controllers;

namespace DebitCreditMemo.Controllers
{
    [OutputCacheAttribute(Duration = 0, NoStore = true)]
    public class MemoController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MemoController));
        string service = ConfigurationManager.AppSettings["API"].ToString();

        [Route("create-memo")]
        public ActionResult Memo()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login","Login");
            }
            else
            {
                return View();
            }
        }

        [Route("CreateMemo")]
        [HttpPost]
        public JsonResult CreateMemo(HomeViewModel mod, string fileBase64)
        {
            XmlConfigurator.Configure();

            string bts; byte[] file;
            string fileType;
            if (fileBase64 == null || fileBase64 == "")
            {
                bts = null;
                fileType = null;
            }
            else
            {
                if (fileBase64.Contains("application/msword")) { bts = fileBase64.Remove(0, 31); fileType = fileBase64.Substring(0,31);  }
                else if (fileBase64.Contains("application/vnd")) { bts = fileBase64.Remove(0, 84); fileType = fileBase64.Substring(0, 84); }
                else { bts = fileBase64.Remove(0, 28); fileType = fileBase64.Substring(0, 28); }
                file = System.Convert.FromBase64String(bts);
            }

            CreateMemoModel mem = new CreateMemoModel();
            CreateResponse resp = new CreateResponse();
            mem.IRNumber =  mod.IRNo;
            mem.TransactionType = mod.DebitCredit;
            mem.TransactionDate1 = mod.TransactionDate1;
            mem.TransactionDate2 = mod.TransactionDate2;
            mem.TransactionDate2 = mod.TransactionDate2;
            mem.Reason1 = (mod.WrongAmount).Equals(true) ? "WrongAmount" : null;
            mem.Reason2 = (mod.Erroneous).Equals(true) ? "Erroneous" : null;
            mem.Reason3 = (mod.SystemError).Equals(true) ? "SystemError" : null;
            mem.ReferenceNo1 = !String.IsNullOrEmpty(mod.refno1) ? mod.refno1 : null;
            mem.ReferenceNo2 = !String.IsNullOrEmpty(mod.refno2) ? mod.refno2 : null;
            mem.ReferenceNo3 = !String.IsNullOrEmpty(mod.refno3) ? mod.refno3 : null;
            mem.WalletUser = mod.WalletUser;
            mem.WalletID = mod.WalletID;
            mem.WrongAmount = mod.IncorrectAmount;
            mem.CorrectAmount = mod.CorrectAmount;
            mem.AdjustedAmount = mod.AdjustAmount;
            mem.OperatorName = Session["user"].ToString();
            mem.File = bts;
            mem.FileType = fileType;
            
            resp.data = mem;

            byte[] data = UTF8Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resp));
            var source = new System.Uri(service + "/CreateMemo");
            var reqHandler = new RequestHandler("POST", source, "application/json", data);
            var response = reqHandler.HttpPostRequest();
            
            if(!response.Contains("200")){
                log.Info(response.ToString());
                return Json(new { status = "5000", msg = "System encountered an error. Please retry or contact admin." });
            }
            else {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateResponse>(response);
                log.Info(JsonConvert.SerializeObject(resp, Formatting.None).ToString());
                return Json( new { status = resp.ResponseCode, msg = resp.ResponsMessage });
            }
        }

        [Route("memo-details-approver")]
        [HttpGet]
        public ActionResult MemoDetails(string memoseries, string status) 
        {
            XmlConfigurator.Configure();

            LoginController ctr = new LoginController();
            CreateResponse resp = new CreateResponse();
            CreateResponse details = new CreateResponse(); 
            HomeViewModel sss = new HomeViewModel(); 
            List<Signatories> sign;

            //get signatories
            CreateResponse signatories = ctr.getSignatoryList();
            sign = signatories.SignatoryList;

            var source = new System.Uri(service + "/MemoRequestDetails/?memoSeries="+ memoseries +"&status="+ status);
            var reqHandler = new RequestHandler("GET", source, "application/json", null);
            var response = reqHandler.HttpGetRequest();
            resp = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateResponse>(response);
            log.Info(JsonConvert.SerializeObject(resp, Formatting.None).ToString());
            
            if (resp.ResponseCode == 200)
            {
                details.status = Convert.ToInt32(status);
                sss.OperatorName = resp.MemoRequestDetails.OperatorName;
                sss.DebtCredNo = resp.MemoRequestDetails.MemoSeries;
                sss.IRNo = resp.MemoRequestDetails.IRNumber;
                sss.DebitCredit = resp.MemoRequestDetails.TransactionType;
                sss.WrongAmount = String.IsNullOrEmpty(resp.MemoRequestDetails.Reason1) ? false : true;
                sss.Erroneous = String.IsNullOrEmpty(resp.MemoRequestDetails.Reason2) ? false : true;
                sss.SystemError = String.IsNullOrEmpty(resp.MemoRequestDetails.Reason3) ? false : true;
                sss.TransactionDate1 = resp.MemoRequestDetails.TransactionDate1;
                sss.TransactionDate2 = resp.MemoRequestDetails.TransactionDate2;
                sss.TransactionDate3 = resp.MemoRequestDetails.TransactionDate3;
                sss.refno1 = resp.MemoRequestDetails.ReferenceNo1;
                sss.refno2 = resp.MemoRequestDetails.ReferenceNo2;
                sss.refno3 = resp.MemoRequestDetails.ReferenceNo3;
                sss.WalletUser = resp.MemoRequestDetails.WalletUser;
                sss.WalletID = resp.MemoRequestDetails.WalletID;
                sss.AdjustAmount = Convert.ToDecimal(resp.MemoRequestDetails.AdjustedAmount);
                sss.CorrectAmount = Convert.ToDecimal(resp.MemoRequestDetails.CorrectAmount);
                sss.IncorrectAmount = Convert.ToDecimal(resp.MemoRequestDetails.WrongAmount);
                sss.File = !String.IsNullOrEmpty(resp.MemoRequestDetails.File) ? resp.MemoRequestDetails.MemoSeries+ "_Attachment" : "No file attached.";
                sss.FileValue = !String.IsNullOrEmpty(resp.MemoRequestDetails.File) ? resp.MemoRequestDetails.File : "";
                sss.FileType = resp.MemoRequestDetails.FileType;
                sss.Created_at = resp.MemoRequestDetails.Created_at;
                sss.Noted_at = resp.MemoRequestDetails.Noted_at;
                sss.Received_at = resp.MemoRequestDetails.Received_at;
                sss.Audited_at = resp.MemoRequestDetails.Audited_at;
                
                details.MemoRequestDetails = resp.MemoRequestDetails;
                details.SignatoryList = sign;
                details.details = sss;
                Session["RequestDetails"] = details;
                log.Info(JsonConvert.SerializeObject(details, Formatting.None).ToString());
                return View();
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
             
        }

        [Route("memo-update")]
        [HttpPost]
        public ActionResult UpdateMemo()
        {
            CreateResponse svcresp = new CreateResponse();
            CreateResponse details = new CreateResponse();
            CreateMemoModel up = new CreateMemoModel();
            details = (CreateResponse)Session["RequestDetails"];

            up.Fullname = details.status == 1 ? details.SignatoryList[0].Name : details.status == 2 ? details.SignatoryList[1].Name : details.status == 3 ? details.SignatoryList[2].Name : string.Empty;
            up.MemoSeries = details.details.DebtCredNo;
            up.Status = Convert.ToInt32(details.status);
            up.OperatorName = details.details.OperatorName;
            up.IRNumber = details.details.IRNo;
            svcresp.data = up;
            log.Info(JsonConvert.SerializeObject(svcresp.data, Formatting.None).ToString());

            byte[] data = UTF8Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(svcresp));
            var source = new System.Uri(service + "/UpdateMemo");
            var reqHandler = new RequestHandler("POST", source, "application/json", data);
            var response = reqHandler.HttpPostRequest();
            svcresp = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateResponse>(response);
            if (!response.Contains("200"))
            {
                log.Info(response.ToString());
                if (svcresp.ResponseCode == 400)
                {
                    return Json(new { status = svcresp.ResponseCode, msg = svcresp.ResponsMessage });
                }
                else
                {
                    return Json(new { status = "5000", msg = "System encountered an error. Please retry or contact admin." });
                }
            }
            else
            {
                log.Info(JsonConvert.SerializeObject(svcresp, Formatting.None).ToString());
                return Json(new { status = svcresp.ResponseCode, msg = svcresp.ResponsMessage });              
            }
        }

        [Route("invalid-url")]
        public ActionResult ErrorPage()
        {
            return View();
        }

        [Route("view-memo")]
        [HttpGet]
        public ActionResult ViewRequests()
        {
            XmlConfigurator.Configure();

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                CreateResponse respe = new CreateResponse();

                var source = new System.Uri(service + "/GetAllRequest");
                var reqHandler = new RequestHandler("GET", source, "application/json", null);
                var response = reqHandler.HttpGetRequest();
                if (!response.Contains("200"))
                {
                    RedirectToAction("ErrorPage");
                }
                else
                {
                    respe = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateResponse>(response);
                    Session["RequestList"] = respe.RequestList;
                    log.Info(respe.RequestList.Count());
                }

                return View(respe);
            }
        }

        public string getStatus(string app1, string app2, string app3) {
            XmlConfigurator.Configure();
            string currentStatus = "";

            if (string.IsNullOrEmpty(app1) && string.IsNullOrEmpty(app2) && string.IsNullOrEmpty(app3)) {
                currentStatus = "1"; //for noted
            }
            if (!string.IsNullOrEmpty(app1) && string.IsNullOrEmpty(app2) && string.IsNullOrEmpty(app3)) {
                currentStatus = "2"; //for audit
            }
            else if (!string.IsNullOrEmpty(app1) && !string.IsNullOrEmpty(app2) && string.IsNullOrEmpty(app3)) {
                currentStatus = "3"; //for receive
            }
            else if (!string.IsNullOrEmpty(app1) && !string.IsNullOrEmpty(app2) && !string.IsNullOrEmpty(app3)) {
                currentStatus = "4"; //approved
            }
            else
            {
                currentStatus = "99";
            }
            log.Info(currentStatus);
            return currentStatus;
        }

        [Route("memo-details")]
        [HttpGet]
        public ActionResult ViewMemoDetails(string memoseries, int status)
        {
            XmlConfigurator.Configure();

            LoginController ctr = new LoginController();
            CreateResponse resp = new CreateResponse();
            CreateResponse details = new CreateResponse();
            HomeViewModel sss = new HomeViewModel();
            List<Signatories> sign;

            //get signatories
            CreateResponse signatories = ctr.getSignatoryList();
            sign = signatories.SignatoryList;

            var source = new System.Uri(service + "/CurrentStatusMemoDetails/?memoSeries=" + memoseries + "&status=" + status);
            var reqHandler = new RequestHandler("GET", source, "application/json", null);
            var response = reqHandler.HttpGetRequest();
            resp = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateResponse>(response);
            log.Info(JsonConvert.SerializeObject(resp, Formatting.None).ToString());

            if (resp.ResponseCode == 200)
            {
                details.status = Convert.ToInt32(status);
                sss.OperatorName = resp.MemoRequestDetails.OperatorName;
                sss.DebtCredNo = resp.MemoRequestDetails.MemoSeries;
                sss.IRNo = resp.MemoRequestDetails.IRNumber;
                sss.DebitCredit = resp.MemoRequestDetails.TransactionType;
                sss.WrongAmount = String.IsNullOrEmpty(resp.MemoRequestDetails.Reason1) ? false : true;
                sss.Erroneous = String.IsNullOrEmpty(resp.MemoRequestDetails.Reason2) ? false : true;
                sss.SystemError = String.IsNullOrEmpty(resp.MemoRequestDetails.Reason3) ? false : true;
                sss.TransactionDate1 = resp.MemoRequestDetails.TransactionDate1;
                sss.TransactionDate2 = resp.MemoRequestDetails.TransactionDate2;
                sss.TransactionDate3 = resp.MemoRequestDetails.TransactionDate3;
                sss.refno1 = resp.MemoRequestDetails.ReferenceNo1;
                sss.refno2 = resp.MemoRequestDetails.ReferenceNo2;
                sss.refno3 = resp.MemoRequestDetails.ReferenceNo3;
                sss.WalletUser = resp.MemoRequestDetails.WalletUser;
                sss.WalletID = resp.MemoRequestDetails.WalletID;
                sss.AdjustAmount = Convert.ToDecimal(resp.MemoRequestDetails.AdjustedAmount);
                sss.CorrectAmount = Convert.ToDecimal(resp.MemoRequestDetails.CorrectAmount);
                sss.IncorrectAmount = Convert.ToDecimal(resp.MemoRequestDetails.WrongAmount);
                sss.File = !String.IsNullOrEmpty(resp.MemoRequestDetails.File) ? resp.MemoRequestDetails.MemoSeries + "_Attachment" : "No file attached.";
                sss.FileValue = !String.IsNullOrEmpty(resp.MemoRequestDetails.File) ? resp.MemoRequestDetails.File : "";
                sss.FileType = resp.MemoRequestDetails.FileType;
                sss.Created_at = resp.MemoRequestDetails.Created_at;
                sss.Noted_at = resp.MemoRequestDetails.Noted_at;
                sss.Received_at = resp.MemoRequestDetails.Received_at;
                sss.Audited_at = resp.MemoRequestDetails.Audited_at;

                details.MemoRequestDetails = resp.MemoRequestDetails;
                details.SignatoryList = sign;
                details.details = sss;
                Session["RequestDetailsX"] = details;
                log.Info(JsonConvert.SerializeObject(details, Formatting.None).ToString());
                return View();
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
            
        }
    }
}