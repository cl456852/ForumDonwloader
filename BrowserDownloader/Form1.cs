using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Example.Handlers;
using CefSharp.WinForms;
using Framework.tool;

namespace BrowserDownloader
{
    public partial class Form1 : Form
    {
        IPageProcessor pageProcessor;
        public Form1()
        {
            InitializeComponent();
            InitializeChromium();
        }

        string CurrentAddress = "";

        public ChromiumWebBrowser chromeBrowser;

        DownloadHandler downloadHandler = new DownloadHandler();

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser();
            chromeBrowser.RequestHandler = new RequestHandler();


            chromeBrowser.DownloadHandler = downloadHandler;
     
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.FrameLoadEnd += WebBrowserFrameLoadEnded;
            chromeBrowser.AddressChanged += Browser_AddressChanged;
        }
        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.CurrentAddress = e.Address;
        }

        private void WebBrowserFrameLoadEnded(object sender, FrameLoadEndEventArgs e)
        {

            if (e.Frame.IsMain)
            {
                //chromeBrowser.ViewSource();
                chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    var html = taskHtml.Result;
                    pageProcessor.NavigateHandle(chromeBrowser, CurrentAddress, textBox2.Text, html);
                });
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //chromeBrowser.Load("https://rarbgprx.org/download.php?id=n2yfuli&h=693&f=Nubiles.19.04.11.Rachel.Adjani.Deeper.XXX.1080p.MP4-KTR-[rarbg.to].torrent");
            pageProcessor = new PageProcessor();
            start();
            Thread th = new Thread(CheckHang);
            th.Start();

        }

        private void start()
        {
           // downloadHandler.Path = textBox2.Text;
            for (int i = Convert.ToInt32(textBox3.Text); i <= Convert.ToInt32(textBox4.Text); i++)
            {
                AsynObj asynObj = new AsynObj();
                asynObj.Url = string.Format(textBox1.Text, i);
                Config1.BlockingQueue.Enqueue(asynObj);
            }
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            chromeBrowser.Load(asynObj1.Url);
        }

        private void LoadPic_Click(object sender, EventArgs e)
        {
            RequestHandler.ifLoadPic = !RequestHandler.ifLoadPic;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pageProcessor = new JavTorrents();
            start();
            //string[] urls = new string[] { "http://javtorrent.re/censored/169803/", "http://javtorrent.re/censored/169802/", "http://javtorrent.re/censored/169801/", "http://javtorrent.re/censored/169800/", "http://javtorrent.re/censored/169799/", "http://javtorrent.re/censored/169798/", "http://javtorrent.re/censored/169797/", "http://javtorrent.re/censored/169796/", "http://javtorrent.re/censored/169795/", "http://javtorrent.re/censored/169794/", "http://javtorrent.re/censored/169793/", "http://javtorrent.re/censored/169792/", "http://javtorrent.re/censored/169791/", "http://javtorrent.re/censored/169790/", "http://javtorrent.re/censored/169789/", "http://javtorrent.re/censored/169788/", "http://javtorrent.re/censored/169787/", "http://javtorrent.re/censored/169786/", "http://javtorrent.re/censored/169785/", "http://javtorrent.re/censored/169784/", "http://javtorrent.re/censored/169783/", "http://javtorrent.re/censored/169782/", "http://javtorrent.re/censored/169781/", "http://javtorrent.re/censored/169780/", "http://javtorrent.re/censored/169779/", "http://javtorrent.re/censored/169778/", "http://javtorrent.re/censored/169777/", "http://javtorrent.re/censored/169776/", "http://javtorrent.re/censored/169775/", "http://javtorrent.re/censored/169774/", "http://javtorrent.re/censored/169773/", "http://javtorrent.re/censored/169772/", "http://javtorrent.re/censored/169771/", "http://javtorrent.re/censored/169770/", "http://javtorrent.re/censored/165322/", "http://javtorrent.re/censored/165321/", "http://javtorrent.re/censored/169755/", "http://javtorrent.re/censored/169754/", "http://javtorrent.re/censored/169753/", "http://javtorrent.re/censored/169752/", "http://javtorrent.re/censored/169751/", "http://javtorrent.re/censored/169750/", "http://javtorrent.re/censored/169749/", "http://javtorrent.re/censored/169748/", "http://javtorrent.re/censored/169747/", "http://javtorrent.re/censored/169746/", "http://javtorrent.re/censored/169745/", "http://javtorrent.re/censored/169744/", "http://javtorrent.re/censored/169743/", "http://javtorrent.re/censored/169742/", "http://javtorrent.re/censored/164455/", "http://javtorrent.re/censored/167904/", "http://javtorrent.re/censored/169803/", "http://javtorrent.re/censored/169802/", "http://javtorrent.re/censored/169801/", "http://javtorrent.re/censored/169800/", "http://javtorrent.re/censored/169799/", "http://javtorrent.re/censored/169798/", "http://javtorrent.re/censored/169797/", "http://javtorrent.re/censored/169796/", "http://javtorrent.re/censored/169795/", "http://javtorrent.re/censored/169794/", "http://javtorrent.re/censored/169793/", "http://javtorrent.re/censored/169792/", "http://javtorrent.re/censored/169791/", "http://javtorrent.re/censored/169790/", "http://javtorrent.re/censored/169789/", "http://javtorrent.re/censored/169788/", "http://javtorrent.re/censored/169787/", "http://javtorrent.re/censored/169786/", "http://javtorrent.re/censored/169785/", "http://javtorrent.re/censored/169784/", "http://javtorrent.re/censored/169783/", "http://javtorrent.re/censored/169782/", "http://javtorrent.re/censored/169781/", "http://javtorrent.re/censored/169780/", "http://javtorrent.re/censored/169779/", "http://javtorrent.re/censored/169778/", "http://javtorrent.re/censored/169777/", "http://javtorrent.re/censored/169776/", "http://javtorrent.re/censored/169775/", "http://javtorrent.re/censored/169774/", "http://javtorrent.re/censored/169773/", "http://javtorrent.re/censored/169772/", "http://javtorrent.re/censored/169771/", "http://javtorrent.re/censored/169770/", "http://javtorrent.re/censored/165322/", "http://javtorrent.re/censored/165321/", "http://javtorrent.re/censored/169755/", "http://javtorrent.re/censored/169754/", "http://javtorrent.re/censored/169753/", "http://javtorrent.re/censored/169752/", "http://javtorrent.re/censored/169751/", "http://javtorrent.re/censored/169750/", "http://javtorrent.re/censored/169749/", "http://javtorrent.re/censored/169748/", "http://javtorrent.re/censored/169747/", "http://javtorrent.re/censored/169746/", "http://javtorrent.re/censored/169745/", "http://javtorrent.re/censored/169744/", "http://javtorrent.re/censored/169743/", "http://javtorrent.re/censored/169742/", "http://javtorrent.re/censored/164455/", "http://javtorrent.re/censored/167904/", "http://javtorrent.re/censored/169803/", "http://javtorrent.re/censored/169802/", "http://javtorrent.re/censored/169801/", "http://javtorrent.re/censored/169800/", "http://javtorrent.re/censored/169799/", "http://javtorrent.re/censored/169798/", "http://javtorrent.re/censored/169797/", "http://javtorrent.re/censored/169796/", "http://javtorrent.re/censored/169795/", "http://javtorrent.re/censored/169794/", "http://javtorrent.re/censored/169793/", "http://javtorrent.re/censored/169792/", "http://javtorrent.re/censored/169791/", "http://javtorrent.re/censored/169790/", "http://javtorrent.re/censored/169789/" };
            //foreach(string url in urls)
            //{
            //    AsynObj asynObj = new AsynObj();
            //    asynObj.Url = url;
            //    Config1.BlockingQueue.Enqueue(asynObj);
            //}
            //AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            //chromeBrowser.Load(asynObj1.Url);
        }

        static string url = "";

        void CheckHang()
        {
            while (true)
            {
                AsynObj asynObj = (AsynObj)Config1.BlockingQueue.Peek();
                if (asynObj != null)
                {
                    string queueUrl = asynObj.Url;
                    if (url == queueUrl)
                    {

                        chromeBrowser.Stop();
                        chromeBrowser.Load(url);
                        Console.WriteLine("reloaded " + url);


                    }
                    url = queueUrl;
                    Thread.Sleep(40000);
                }
            }

        }
    }
}
