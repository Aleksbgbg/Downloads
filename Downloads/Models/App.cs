namespace Downloads.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class App
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string LatestVersion { get; set; }

        public string Description { get; set; }

        public DateTime LastUpdated { get; set; }

        public string GitHubUrl { get; set; }
    }
}