namespace Downloads.Models
{
    using System;

    public class App
    {
        public App(string name, string latestVersion, string description, DateTime lastUpdated, string gitHubUrl)
        {
            Name = name;
            LatestVersion = latestVersion;
            Description = description;
            LastUpdated = lastUpdated;
            GitHubUrl = gitHubUrl;
        }

        public string Name { get; }

        public string LatestVersion { get; }

        public string Description { get; }

        public DateTime LastUpdated { get; }

        public string GitHubUrl { get; }
    }
}