using Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Controllers
{
    public class JSSDKController : Controller
    {
        public string AppID = "wxbc35969281313e06";
        public string AppSecret = "4d77df3a08616c040b0deb2030fc1bc6";
        // GET: JSSDK
        public ActionResult Index()
        {
            JsApi jsApi = new JsApi();
            string JsApiirray= jsApi.GetJsApiInfo(AppID, AppSecret);
            //ViewBag.JsApiirray = JsApiirray;
            ViewData["JsApiirray"] = JsApiirray;
            return View();
        }
    }
}