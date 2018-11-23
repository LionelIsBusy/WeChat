using Api;
using Demo.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Demo.Controllers
{
    public class UserManageController : Controller
    {
        // GET: UserManage
        public ActionResult Index()
        {
            return View();
        }

        public string AppID = "wxbc35969281313e06";
        public string AppSecret = "4d77df3a08616c040b0deb2030fc1bc6";
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        public ActionResult GetAllUserID()
        {
            string setUrl = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}";
            setUrl = string.Format(setUrl, BasicApi.GetTokenSession(AppID, AppSecret));//获取token、拼凑url
            string respText = Common.CommonMethod.WebRequestPostOrGet(setUrl, "", "get");

            //微信服务器返回的json数据
            //{
            //"total":2,
            //"count":2,
            //"data":{
            //       "openid":["OPENID1","OPENID2"]
            //        },
            //"next_openid":"NEXT_OPENID"
            //}
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            Dictionary<string, object> jsons = (Dictionary<string, object>)respDic["data"];//重点在这里（看看json字符串的结构就明白了吧，键值对data的值又是一个键值对所以需要再次转一次）
            object[] datainfos = (object[])jsons["openid"];//这样才能取到最小单元的键值对中的值openid
            //string s = datainfos[0].ToString();
            List<string> list = new List<string>();
            foreach (var item in datainfos)
            {
                list.Add(item.ToString());
             }

            #region json解析2
            //JObject ja = (JObject)JsonConvert.DeserializeObject(respText);
            //JObject jas = (JObject)JsonConvert.DeserializeObject(ja["data"].ToString());
            //string ss= jas["openid"].ToString();
            #endregion

            return Json(list);
        }

        public ActionResult GetUserInfo(string openidI)
        {
            //if (openid==null)
            //{
            //    return Content("no");
            //}
            string setUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid=OPENID&lang=zh_CN";
            setUrl = setUrl.Replace("OPENID", openidI);
            setUrl = string.Format(setUrl, BasicApi.GetTokenSession(AppID, AppSecret));//获取token、拼凑url
            string respText = Common.CommonMethod.WebRequestPostOrGet(setUrl, "", "get");
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            UserInfo userInfo = new UserInfo();
            userInfo.Nickname = respDic["nickname"].ToString();
            userInfo.Headimgurl = respDic["headimgurl"].ToString();
            return Json(userInfo);
        }

        //public ActionResult SetRemarkName()
        //{

        //}
    }
}