namespace Downloads.Tests.Api
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
                LastUpdated = DateTime.Today
            }
        };
    }
}