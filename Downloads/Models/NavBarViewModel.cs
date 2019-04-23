namespace Downloads.Models
{
    using System.Collections.Generic;

    public class NavBarViewModel
    {
        public NavBarViewModel(bool isHome, IEnumerable<AppNavViewModel> appNavs)
        {
            IsHome = isHome;
            AppNavs = appNavs;
        }

        public bool IsHome { get; }

        public IEnumerable<AppNavViewModel> AppNavs { get; }

        public class AppNavViewModel
        {
            public AppNavViewModel(bool isActive, string name)
            {
                IsActive = isActive;
                Name = name;
            }

            public bool IsActive { get; }

            public string Name { get; }
        }
    }
}