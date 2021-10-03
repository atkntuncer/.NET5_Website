using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Entities
{
    public class Menu
    {
        public int ParentID { get; set; }
        public string Title { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string IconCss { get; set; }
        public int  Index { get; set; }

    }
}
