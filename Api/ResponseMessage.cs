using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api
{
    public class ResponseMessage
    {
        #region 接收的类型
        public static string GetText(string FromUserName, string ToUserName, string Content)
        {
            Common.CommonMethod.WriteTxt(Content);//接收的文本消息
            string XML = "";
            switch (Content) { 
                case "关键字":
                    XML = ReText(FromUserName, ToUserName, "关键词回复测试");
                    break;
                case"单图文":
                    XML = ReArticle(FromUserName, ToUserName, "测试标题", "测试详情——百度搜索链接", "http://pic.cnblogs.com/avatar/743013/20150521120816.png", "http://www.baidu.com");
                    break;
                case "老花":
                    XML = ReText(FromUserName, ToUserName, "花花公子大少爷 租金两万九");
                    break;
                case "菲特":
                    XML = ReText(FromUserName, ToUserName, "曾今的股神菲特 薛定谔的猫");
                    break;
                case "肖医师":
                    XML = ReText(FromUserName, ToUserName, "妙手回春肖医师 文能挥笔武能动刀");
                    break;
                case "刘总":
                    XML = ReText(FromUserName, ToUserName, "上至上流社会游刃有余 下至黄泥工地捡铁致富");
                    break;
                case "祥哥":
                    XML = ReText(FromUserName, ToUserName, "老实人");
                    break;
                case "康老板":
                    XML = ReText(FromUserName, ToUserName, "皇冠哥 官三代 享福享乐大老爷");
                    break;
                default:
                    XML = ReText(FromUserName, ToUserName, "请输入关键字，比如：老花、菲特、肖医师、刘总、祥哥、康老板");
                    break;
            }
            return XML;
        }

        /// <summary>
        /// 自动回复微信发送过来的图片 “图尚往来”
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="ToUserName"></param>
        /// <param name="PicUrl"></param>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        public static string GetImage(string FromUserName, string ToUserName, string PicUrl,string MediaId)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + Common.CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[image]]></MsgType>";//回复类型文本
            XML += "<Image><PicUrl><![CDATA[" + PicUrl + "]]></PicUrl>";   //picUrl可有可无   MediaId要写在Image节点下，与官方文档不一致
            XML += "<MediaId><![CDATA[" + MediaId+ "]]></MediaId></Image><FuncFlag>0</FuncFlag></xml>";//回复内容 FuncFlag设置为1的时候，自动星标刚才接收到的消息，适合活动统计使用
            return XML;
        }

        /// <summary>
        /// 自动回复微信发送过来的语音 
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="ToUserName"></param>
        /// <param name="ForMat"></param>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        public static string GetVoice(string FromUserName, string ToUserName, string ForMat, string MediaId)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + Common.CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[voice]]></MsgType>";//回复类型文本
            XML += "<Voice><ForMat><![CDATA[" + ForMat + "]]></ForMat>";    //MediaId要写在Voice节点下，与官方文档不一致
            XML += "<MediaId><![CDATA[" + MediaId + "]]></MediaId></Voice><FuncFlag>0</FuncFlag></xml>";//回复内容 FuncFlag设置为1的时候，自动星标刚才接收到的消息，适合活动统计使用
            return XML;
        }

        #endregion

        #region 回复方式
        /// <summary>
        /// 回复文本
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="Content">回复类型文本</param>
        /// <returns>拼凑的XML</returns>
        public static string ReText(string FromUserName, string ToUserName,string Content)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + Common.CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[text]]></MsgType>";//回复类型文本
            XML += "<Content><![CDATA["+ Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";//回复内容 FuncFlag设置为1的时候，自动星标刚才接收到的消息，适合活动统计使用
            return XML;
        }

        /// <summary>
        /// 回复单图文
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="Title">标题</param>
        /// <param name="Description">详情</param>
        /// <param name="PicUrl">图片地址</param>
        /// <param name="Url">地址</param>
        /// <returns>拼凑的XML</returns>
        public static string ReArticle(string FromUserName, string ToUserName, string Title, string Description, string PicUrl, string Url)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + Common.CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[news]]></MsgType><Content><![CDATA[]]></Content><ArticleCount>1</ArticleCount><Articles>";
            XML += "<item><Title><![CDATA[" + Title + "]]></Title><Description><![CDATA[" + Description + "]]></Description><PicUrl><![CDATA[" + PicUrl + "]]></PicUrl><Url><![CDATA[" + Url + "]]></Url></item>";
            XML += "</Articles><FuncFlag>0</FuncFlag></xml>";
            return XML;
        }

        /// <summary>
        /// 多图文回复
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="ArticleCount">图文数量</param>
        /// <param name="dtArticle"></param>
        /// <returns></returns>
        public static string ReArticle(string FromUserName, string ToUserName, int ArticleCount, System.Data.DataTable dtArticle)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + Common.CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[news]]></MsgType><Content><![CDATA[]]></Content><ArticleCount>" + ArticleCount + "</ArticleCount><Articles>";
            foreach (System.Data.DataRow Item in dtArticle.Rows)
            {
                XML += "<item><Title><![CDATA[" + Item["Title"] + "]]></Title><Description><![CDATA[" + Item["Description"] + "]]></Description><PicUrl><![CDATA[" + Item["PicUrl"] + "]]></PicUrl><Url><![CDATA[" + Item["Url"] + "]]></Url></item>";
            }
            XML += "</Articles><FuncFlag>0</FuncFlag></xml>";
            return XML;
        }
        #endregion


    }
}
