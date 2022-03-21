using System;
using System.Collections.Generic;

namespace Game.Web
{
    public class AppState
    {
        public Action Action { get; set; }
        public Action<Dictionary<string,object>> ActionWithParameters { get; set; }
    }
}
