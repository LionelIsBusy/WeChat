using Api;
using Api.MenuApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;

namespace Demo
{
    /// <summary>
    /// API 的摘要说明
    /// </summary>
    public class API : IHttpHandler, IRequiresSessionState
    {
        public string AppID = "wxbc35969281313e06";
        public string AppSecret = "4d77df3a08616c040b0deb2030fc1bc6";
        //加载菜单
        protected void Page_Load(object sender, EventArgs e)
        {
            MenuCreate menuCreate = new MenuCreate();
            menuCreate.CreateMenu(AppID, AppSecret);
        }

        public void ProcessRequest(HttpContext context)   //此方法以及最后面的IsReusable方法实现IHttpHandler接口
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.HttpMethod.ToLower() == "post")
            {
                //回复消息的时候也需要验证消息，这个很多开发者没有注意这个，存在安全隐患  
                //微信中 谁都可以获取信息 所以 关系不大 对于普通用户 但是对于某些涉及到验证信息的开发非常有必要
                if (CheckSignature())
                {
                    //接收消息
                    ReceiveXml();
                }
                else
                {
                    HttpContext.Current.Response.Write("消息并非来自微信");
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                CheckWechat();
            }
        }

        #region 验证微信签名
        /// <summary>
        /// 返回随机数表示验证成功
        /// </summary>
        private void CheckWechat()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["echoStr"]))
            {
                HttpContext.Current.Response.Write("消息并非来自微信");
                HttpContext.Current.Response.End();
            }
            string echoStr = HttpContext.Current.Request.QueryString["echoStr"];
            if (CheckSignature())
            {
                HttpContext.Current.Response.Write(echoStr);
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <returns></returns>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        private bool CheckSignature()
        {
            string token = "sohovan";  //不是access_token

            string signature = HttpContext.Current.Request.QueryString["signature"].ToString();
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"].ToString();
            string nonce = HttpContext.Current.Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);

            //tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1"); //此方法提示已过时  用以下代码代替
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }


            if (sh1.ToLower() == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 导出文件
        private void WriteText(string fileName, string text)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, true))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //调用这个方法来打印数据，打印一条追加一条。替换原来的Console.WriteLine(str);
        private void PrintData(string str)
        {
            Console.WriteLine(str);
            WriteText(@"C:/test.txt", str);
        }
        #endregion



        #region 接收消息
        /// <summary>
        /// 接收微信发送的XML消息并且解析
        /// </summary>
        private void ReceiveXml()
        {
            Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
            byte[] requestByte = new byte[requestStream.Length];
            requestStream.Read(requestByte, 0, (int)requestStream.Length);
            string requestStr = Encoding.UTF8.GetString(requestByte);

            if (!string.IsNullOrEmpty(requestStr))
            {
                //封装请求类
                XmlDocument requestDocXml = new XmlDocument();
                requestDocXml.LoadXml(requestStr);
                XmlElement rootElement = requestDocXml.DocumentElement;
                WxXmlModel WxXmlModel = new WxXmlModel();
                WxXmlModel.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                WxXmlModel.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                WxXmlModel.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                WxXmlModel.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;

                switch (WxXmlModel.MsgType)
                {
                    case "text"://文本
                        WxXmlModel.Content = rootElement.SelectSingleNode("Content").InnerText;
                        break;
                    case "image"://图片
                        WxXmlModel.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                        WxXmlModel.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                        break;
                    case "voice":
                        WxXmlModel.Format= rootElement.SelectSingleNode("Format").InnerText;
                        WxXmlModel.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                        break;
                    case "event"://事件
                        WxXmlModel.Event = rootElement.SelectSingleNode("Event").InnerText;
                        if (WxXmlModel.Event == "subscribe")//关注类型
                        {
                            WxXmlModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                        }
                        break;
                    default:
                        break;
                }

                ResponseXML(WxXmlModel);//回复消息
            }
        }
        #endregion

        #region 回复消息
        private void ResponseXML(WxXmlModel WxXmlModel)
        {
            string XML = "";
            switch (WxXmlModel.MsgType)
            {
                case "text"://文本回复
                    XML = Api.ResponseMessage.GetText(WxXmlModel.FromUserName, WxXmlModel.ToUserName, WxXmlModel.Content);
                    break;
                case "image":
                    XML = Api.ResponseMessage.GetImage(WxXmlModel.FromUserName, WxXmlModel.ToUserName, WxXmlModel.PicUrl, WxXmlModel.MediaId);
                    break;
                case "voice":
                    XML = Api.ResponseMessage.GetVoice(WxXmlModel.FromUserName, WxXmlModel.ToUserName, WxXmlModel.Format, WxXmlModel.MediaId);
                    break;
                case "event":
                    if (WxXmlModel.Event == "subscribe")
                    {
                        XML = Api.ResponseMessage.ReText(WxXmlModel.FromUserName, WxXmlModel.ToUserName, "谢谢你这么可爱还关注我");
                    }
                    break;
                default://默认回复
                    break;
            }
            HttpContext.Current.Response.Write(XML);
            HttpContext.Current.Response.End();
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}