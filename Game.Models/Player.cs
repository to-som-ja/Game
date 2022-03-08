using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Models
{
    public class Player
    {
        public int positionX { get; set; }
        public int positionY { get; set; }
        public int relativePositionX { get; set; }
        public int relativePositionY { get; set; }
        public string ImagePath { get; set; }

        public Player(int positionX,int positionY,string ImagePath)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.ImagePath = ImagePath; 
        }
    }
}
