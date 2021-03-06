﻿namespace Downloads.Tests.Api
{
    using System;

    using Downloads.Models;

    internal static class Data
    {
        internal static App[] Apps =
        {
            new App
            {
                Name = "Some App",
                LastUpdated = DateTime.Today,
                DownloadUrl = "https://github.com/releases/some-app",
                LatestVersion = "v3.3.3"
            },
            new App
            {
                Name = "Some App2",
                LastUpdated = DateTime.Today,
                DownloadUrl = "https://github.com/releases/some-app2",
                LatestVersion = "v1.2.0"
            }
        };
    }
}