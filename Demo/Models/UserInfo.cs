using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class UserInfo
    {
    // "subscribe": 1, 
    //"openid": "o6_bmjrPTlm6_2sgVt7hMZOPfL2M", 
    //"nickname": "Band", 
    //"sex": 1, 
    //"language": "zh_CN", 
    //"city": "广州", 
    //"province": "广东", 
    //"country": "中国", 
    //"headimgurl":"http://thirdwx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0",
    //"subscribe_time": 1382694957,
    //"unionid": " o6_bmasdasdsad6_2sgVt7hMZOPfL"
    //"remark": "",
    //"groupid": 0,
    //"tagid_list":[128,2],
    //"subscribe_scene": "ADD_SCENE_QR_CODE",
    //"qr_scene": 98765,
    //"qr_scene_str": ""

        public int Subscribe { get; set; }
        public string Npenid { get; set; }
        public string Nickname { get; set; }
        public int Sex { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
        public string Subscribe_time { get; set; }
        public string Unionid { get; set; }
        public string Remark { get; set; }
        public int Groupid { get; set; }
        public List<int> Tagid_list { get; set; }
        public string Subscribe_scene { get; set; }
        public int Qr_scene { get; set; }
        public string Qr_scqr_scene_strene { get; set; }
    }
}