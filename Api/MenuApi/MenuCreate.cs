
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Api.MenuApi
{
    public class MenuCreate
    {
        //public string access_token { get; set; }
        //public void CreateMenu()
        //{
        //    FileStream fs1 = new FileStream(HttpContext.Current.Server.MapPath(".") + "\\menu.txt", FileMode.Open);
        //    StreamReader sr = new StreamReader(fs1, Encoding.GetEncoding("UTF-8"));
        //    string menu = sr.ReadToEnd();
        //    sr.Close();
        //    fs1.Close();
        //    var str = GetPage("https://api.weixin.qq.com/cgi-bin/token?grant_type = client_credential & appid = wxbc35969281313e06 & secret = 4d77df3a08616c040b0deb2030fc1bc6", "");
        //    JObject jo = JObject.Parse(str);
        //    access_token = jo["access_token"].ToString();
        //    GetPage("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token + "", menu);
        //} 
        
        ////向服务器发送post请求
        //public string GetPage(string posturl, string postData)
        //{
        //    Stream outstream = null;
        //    Stream instream = null;
        //    StreamReader sr = null;
        //    HttpWebResponse response = null;
        //    HttpWebRequest request = null;
        //    Encoding encoding = Encoding.UTF8;
        //    byte[] data = encoding.GetBytes(postData);
        //    // 准备请求...
        //    try
        //    {
        //        // 设置参数
        //        request = WebRequest.Create(posturl) as HttpWebRequest;
        //        CookieContainer cookieContainer = new CookieContainer();
        //        request.CookieContainer = cookieContainer;
        //        request.AllowAutoRedirect = true;
        //        request.Method = "POST";
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = data.Length;
        //        outstream = request.GetRequestStream();
        //        outstream.Write(data, 0, data.Length);
        //        outstream.Close();
        //        //发送请求并获取相应回应数据
        //        response = request.GetResponse() as HttpWebResponse;
        //        //直到request.GetResponse()程序才开始向目标网页发送Post请求
        //        instream = response.GetResponseStream();
        //        sr = new StreamReader(instream, encoding);
        //        //返回结果网页（html）代码
        //        string content = sr.ReadToEnd();
        //        string err = string.Empty;
        //        HttpContext.Current.Response.Write(content);
        //        return content;
        //    }
        //    catch (Exception ex)
        //    {
        //        string err = ex.Message;
        //        return string.Empty;
        //    }
        //}






        #region 发布菜单  sohovan
        public void CreateMenu(string AppID, string AppSecret)
        {
            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(".") + "\\Menu.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8"));
            string menu = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            SubmitMenu(menu, AppID, AppSecret);
            sr.Dispose();
            fs.Dispose();
        }

        JavaScriptSerializer Jss = new JavaScriptSerializer();


        /// <summary>
        /// 发布菜单
        /// </summary>
        /// <param name="MenuJson">配置的菜单json数据</param>
        /// <param name="AppID">AppID</param>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns>返回0成功否则错误码</returns>
        public string SubmitMenu(string MenuJson, string AppID, string AppSecret)
        {
            string setMenuUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            setMenuUrl = string.Format(setMenuUrl, BasicApi.GetTokenSession(AppID, AppSecret));//获取token、拼凑url
            string respText = Common.CommonMethod.WebRequestPostOrGet(setMenuUrl, MenuJson,"post");
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            return respDic["errcode"].ToString();//返回0发布成功
        }
        #endregion
    }
}
