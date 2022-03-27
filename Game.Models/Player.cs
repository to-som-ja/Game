using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Models
{
    public class Player
    {
        public int positionX { get; set; }
        public int positionY { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public int damage { get; set; }
        public int relativePositionX { get; set; }
        public int relativePositionY { get; set; }
        public string ImagePath { get; set; }
        public List<Item> Items { get; set; }
        public Direction? direction { get; set; }
        public Player(int positionX,int positionY,string ImagePath)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.ImagePath = ImagePath; 
            Items = new List<Item>();
            direction = Direction.South;
            level = 1;
            experience = 0;
            damage = 4;
        }
        public bool levelUp()
        {
            if (experience >= level * 10)
            {
                experience = experience - level * 10;
                level++;
                damage += 2;
                return true;
            }
            return false;
        }
    }
}
