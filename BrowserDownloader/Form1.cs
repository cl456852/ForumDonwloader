using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            pageProcessor = new PageProcessor();
            start();
        }

        private void start()
        {
            downloadHandler.Path = textBox2.Text;
            for (int i = Convert.ToInt32(textBox3.Text); i <= Convert.ToInt32(textBox4.Text); i++)
            {
                AsynObj asynObj = new AsynObj();
                asynObj.Url = string.Format(textBox1.Text, i);
                Config1.BlockingQueue.Enqueue(asynObj);
            }
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            chromeBrowser.Load(asynObj1.Url);
        }

    }
}
