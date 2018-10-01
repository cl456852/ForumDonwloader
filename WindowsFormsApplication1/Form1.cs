using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Framework.tool;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        IPageProcessor pageProcessor;
        public Form1()
        {
            InitializeComponent();
        }

        int i = 1;

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            pageProcessor.NavigateHandle(webBrowser1, e, textBox1.Text);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int i = 1; i <= 5; i++)
            {
                webBrowser1.Navigate("https://rarbg.to/torrents.php?category=1%3B4&page=" + i);


                if (webBrowser1.Document != null)
                    Console.WriteLine(webBrowser1.Document.Cookie);
                else
                    Console.WriteLine("null");
                Thread.Sleep(1000);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            pageProcessor = new PageProcessor();
            start();
        }

        private void start()
        {
            for (int i = Convert.ToInt32(textBox2.Text); i <= Convert.ToInt32(textBox3.Text); i++)
            {
                AsynObj asynObj = new AsynObj();
                asynObj.Url = string.Format(textBox4.Text, i);
                Config1.BlockingQueue.Enqueue(asynObj);
            }
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            webBrowser1.Navigate(asynObj1.Url);
        }


        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //if (i <= 100)
            //{
            //    string content = webBrowser1.DocumentText;
            //    Console.WriteLine(content);
            //    if (content.Contains("Please wait while we try to verify your browser"))
            //    {
            //        webBrowser1.Navigate(url + i);
            //        return;
            //    }
            //    DlTool.SaveFile(content, Path.Combine(textBox1.Text, DlTool.ReplaceUrl(url + i) + ".htm"));
            //    webBrowser1.Navigate(url + i);
            //    i++;
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://rarbg.to/download.php?id=xrecd5z&f=KarupsPC.16.11.17.Coco.De.Mal.Hardcore.XXX.720p.MP4-KTR-[rarbg.to].torrent");
            //Console.WriteLine(webBrowser1.DocumentText);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            pageProcessor = new SIS001PageProcessor();
            start();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Config();
            pageProcessor = new ThzPageProcessor();
            start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pageProcessor = new JavtorrentProcessor();
            start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pageProcessor = new YouivProcessor();
            start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Config();
            pageProcessor = new _168xProcessor();
            start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Config();
            pageProcessor = new BailuProcessor();
            start();
        }

        void Config()
        {
            Util.domain = new Regex("http:\\/\\/.*\\/").Match(textBox4.Text).Value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
