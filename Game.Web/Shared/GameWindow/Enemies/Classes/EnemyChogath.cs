
using Game.Web.Shared.GameWindow.Enemies.Components;
using System;
using System.Collections.Generic;

namespace Game.Web.Shared.GameWindow.Enemies
{
    public class EnemyChogath : EnemyParent, IEnemy
    {
        public EnemyChogath(int positionX, int positionY, int level) : base(positionX, positionY, level)
        {
            name = "Chogath";
            imagePath = "Images/Enemies/enemy-Chogath.png";
            hp = 1000;
        }

        public Type ComponentType()
        {
            return typeof(EnemyChogathComponent);
        }
    }

}
