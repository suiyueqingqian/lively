﻿using Lively.Common;
using Lively.Common.Services.Downloader;
using Lively.Grpc.Client;
using Lively.UI.WinUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lively.UI.WinUI.Helpers
{
    internal static class WebViewUtil
    {
        public static string DownloadUrl => "https://go.microsoft.com/fwlink/p/?LinkId=2124703";

        public static bool IsWebView2Available()
        {
            try
            {
                return !string.IsNullOrEmpty(CoreWebView2Environment.GetAvailableBrowserVersionString());
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> InstallWebView2()
        {
            if (Constants.ApplicationType.IsMSIX)
                return false;

            try
            {
                var filePath = Path.Combine(Constants.CommonPaths.TempDir, "MicrosoftEdgeWebview2Setup.exe");
                var downloader = App.Services.GetRequiredService<IDownloadService>();
                await downloader.DownloadFile(new Uri(DownloadUrl), filePath);
                await Process.Start(filePath, "/silent /install").WaitForExitAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}