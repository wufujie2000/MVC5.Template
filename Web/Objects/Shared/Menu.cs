using System;
using System.Collections.Generic;

namespace Template.Objects
{
    public class Menu
    {
        public Boolean IsAnonymous { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsOpen { get; set; }
        public String IconClass { get; set; }
        public String Title { get; set; }

        public String Area { get; set; }
        public String Controller { get; set; }
        public String Action { get; set; }

        public IEnumerable<Menu> Submenus { get; set; }

        public Menu()
        {
            Submenus = new List<Menu>();
        }
    }
}
