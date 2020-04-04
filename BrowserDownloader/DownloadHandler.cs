// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Framework.tool;
using System;
using System.IO;

namespace CefSharp.Example.Handlers
{
    public class DownloadHandler : IDownloadHandler
    {

        public event EventHandler<DownloadItem> OnBeforeDownloadFired;

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        //string path;
        string downloadPath = @"C:\File\";
        public static AsynObj asynObj;
        //public string Path { get => path; set => path = value; }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            OnBeforeDownloadFired?.Invoke(this, downloadItem);
            
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue(downloadPath + downloadItem.SuggestedFileName, showDialog: false);
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            OnDownloadUpdatedFired?.Invoke(this, downloadItem);

            if (downloadItem.IsComplete)
            {
                try
                {
                    DirectoryInfo TheFolder = new DirectoryInfo(downloadPath);
                    FileInfo[] fileInfos = TheFolder.GetFiles("*", SearchOption.AllDirectories);
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        if (File.Exists(asynObj.Path))
                        {
                            asynObj.Path = Path.Combine(Path.GetDirectoryName(asynObj.Path), Path.GetFileNameWithoutExtension(asynObj.Path) + Guid.NewGuid() + Path.GetExtension(asynObj.Path));
                            Console.WriteLine("duplicateFileName  " + asynObj.Path);
                        }
                        try
                        {
                            File.Move(fileInfo.FullName, asynObj.Path);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("移动文件异常", e);
                        }

                    }
                    Config1.BlockingQueue.Dequeue();
                    AsynObj asynObj1 = Config1.BlockingQueue.Peek();
                    DownloadHandler.asynObj = asynObj1;
                    Console.WriteLine(asynObj.Url);
                    Console.WriteLine(Config1.BlockingQueue.Count);
                    chromiumWebBrowser.Load(asynObj1.Url);
                }
                catch(Exception e)
                {
                    Console.WriteLine("OnDownloadUpdated error "+e.Message);
                }
            }   
        }



    }
}
