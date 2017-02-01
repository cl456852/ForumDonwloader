using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Framework.tool;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int i = 1;

        private const string listUrl = "https://rarbg.to/torrents.php?category=1%3B4&page=";
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
       
            
                
                string content = webBrowser1.DocumentText;
               // Console.WriteLine(content);
                if (content.Contains("Please wait while we try to verify your browser"))
                {
                    Console.WriteLine("Please wait while we try to verify your browser");
                    webBrowser1.Navigate(url + i);
                    return;
                }
                if (content.Contains("We have too many requests from your ip in the past 24h"))
                {
                    Console.WriteLine("We have too many requests from your ip in the past 24h");
                    Config1.Flooding();
                    webBrowser1.Navigate(url + i);
                    return;
                }
                
               // DlTool.SaveFile(content, Path.Combine(textBox1.Text, DlTool.ReplaceUrl(url +( i-1))+".htm"));
               // webBrowser1.Navigate(url + i);
          

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 5; i++) { 
            webBrowser1.Navigate("https://rarbg.to/torrents.php?category=1%3B4&page="+i);
            
 
            if (webBrowser1.Document != null)
                Console.WriteLine(webBrowser1.Document.Cookie);
            else
                Console.WriteLine("null");
            Thread.Sleep(1000);
        }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = Convert.ToInt32(textBox2.Text); i <= Convert.ToInt32(textBox3.Text); i++)
            {
                AsynObj asynObj=new AsynObj();
                asynObj.Url = listUrl + i;
                Config1.BlockingQueue.Enqueue(asynObj);
            }
        }


     

        private string url = "https://rarbg.to/torrents.php?category=1%3B4&page=";

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
    }
}
