using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnWeiXin;
using UnCommon.Tool;
using UnCommon.XMMP;

namespace UnTestWin
{
    public partial class WeiXin : Form
    {
        public WeiXin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            UnAttrMsg msg = new UnAttrMsg();
            msg.ToUserName = "to";
            msg.FromUserName = "from";
            msg.CreateTime = Convert.ToInt64(UnDate.ticksSec());
            msg.MsgType = "news";
            //msg.ArticleCount = 2;


            List<UnAttrNew> listNew = new List<UnAttrNew>();
            UnAttrNew news = new UnAttrNew();
            news.Description = "d1";
            news.PicUrl = "http://www.zztzfx.com/api/wap/images/dt4.jpg";
            news.Title = "t1";
            news.Url = "http://heswxweb.hesbbq.com";
            listNew.Add(news);
            listNew.Add(news);

            msg.Articles = listNew;
            //msg.Content = "content";
            string xml = UnWeChat.getNewsXML(msg);
            Console.WriteLine(xml);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UnAttrTemplate tmp = new UnAttrTemplate();
            tmp.touser = "om_dsfasdf";
            tmp.template_id = "adfasdfasdfadf";
            tmp.url = "http://www.baidu.com";
            //tmp.data = new Dictionary<string, UnAttrTmpData>();
            //tmp.data.Add("first", new UnAttrTmpData("恭喜你购买成功！", "#17317"));
            //tmp.data.Add("keynote1", new UnAttrTmpData("恭喜你购买成功1！", "#173171"));
            tmp.primary_industry = new Dictionary<string, string>();
            tmp.primary_industry.Add("first_class", "运输与仓储");
            tmp.primary_industry.Add("second_class", "快递");

            tmp.secondary_industry = new Dictionary<string, string>();
            tmp.secondary_industry.Add("first_class", "IT科技");
            tmp.secondary_industry.Add("second_class", "互联网|电子商务");

            string str = UnXMMPJson.tToJson(typeof(UnAttrTemplate), tmp);
            //string str = UnXMMPXml.tToXml(typeof(UnAttrTemplate), tmp);
            Console.WriteLine(str);



            //UnAttrTemplate tmp1 = new UnAttrTemplate();
            //tmp1 = (UnAttrTemplate)UnXMMPJson.jsonToT(typeof(UnAttrTemplate), str);
            //Console.WriteLine(((UnAttrTmpData)tmp1.data["first"]).value);
            //Console.WriteLine(tmp.primary_industry["first_class"]);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string rtstr = @"{	
 'template_list': [{
      'template_id': 'iPk5sOIt5X_flOVKn5GrTFpncEYTojx6ddbt8WYoV5s',
      'title': '领取奖金提醒',
      'primary_industry': 'IT科技',
      'deputy_industry': '互联网|电子商务',
      'content': '{ {result.DATA} }\n\n领奖金额:{ {withdrawMoney.DATA} }\n领奖  时间:{ {withdrawTime.DATA} }\n银行信息:{ {cardInfo.DATA} }\n到账时间:  { {arrivedTime.DATA} }\n{ {remark.DATA} }',
      'example': '您已提交领奖申请\n\n领奖金额：xxxx元\n领奖时间：2013-10-10 12:22:22\n银行信息：xx银行(尾号xxxx)\n到账时间：预计xxxxxxx\n\n预计将于xxxx到达您的银行卡'
   }]
}";
            UnAttrTemplate tmp = (UnAttrTemplate)UnXMMPJson.jsonToT(typeof(UnAttrTemplate), rtstr);

            if (tmp != null)
            {
                Console.Write(tmp.template_list[0].primary_industry);
            }
        }
    }
}
