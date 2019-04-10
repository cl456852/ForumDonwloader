// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Example.Handlers
{
    public class DownloadHandler : IDownloadHandler
    {
        public event EventHandler<DownloadItem> OnBeforeDownloadFired;

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        string path;

        public string Path { get => path; set => path = value; }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            OnBeforeDownloadFired?.Invoke(this, downloadItem);
            string downloadPath = @"C:\File\cefsharp";
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
        }



    }
}
