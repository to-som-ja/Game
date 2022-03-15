using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Models
{
    public class Item
    {   
        public string name { get; set; }
        public string imgPath { get; set; }

        public Item(string name, string imgPath)
        {
            this.name = name;
            this.imgPath = imgPath;
        }
    }
}
