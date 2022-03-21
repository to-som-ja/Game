
using Game.Web.Shared.GameWindow.Enemies.Components;
using System;
using System.Collections.Generic;

namespace Game.Web.Shared.GameWindow.Enemies
{
    public class EnemyMorde : EnemyParent, IEnemy
    {
        public EnemyMorde(int positionX, int positionY, int level) : base(positionX, positionY, level)
        {
            name = "Morde";
            imagePath = "Images/Enemies/enemy-Morde.png";
            hp = 1000;
        }

        public Type ComponentType()
        {
            return typeof(EnemyMordeComponent);
        }
    }

}
