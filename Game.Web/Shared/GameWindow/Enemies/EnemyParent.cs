using Microsoft.AspNetCore.Components;
using System;
using Game.Web.Shared.GameWindow.Enemies.Components;


namespace Game.Web.Shared.GameWindow.Enemies
{
    public class EnemyParent
    {
        public int positionX { get; set; }
        public int positionY { get; set; }
        public string name;
        public string imagePath;
        public int level;
        public int hp;
        public int dmg;

        public EnemyParent(int positionX,int positionY,int level)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.level = level;
        }
    }
}
