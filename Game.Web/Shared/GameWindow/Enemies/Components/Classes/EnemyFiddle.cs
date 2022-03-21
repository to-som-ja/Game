
using Game.Web.Shared.GameWindow.Enemies.Components;
using System;
using System.Collections.Generic;

namespace Game.Web.Shared.GameWindow.Enemies
{
    public class EnemyFiddle : EnemyParent, IEnemy
    {
        public EnemyFiddle(int positionX, int positionY, int level) : base(positionX, positionY, level)
        {
            name = "Fiddle";
            imagePath = "Images/Enemies/enemy-Fiddle.png";
            hp = 1000;
        }

        public Type ComponentType()
        {
            return typeof(EnemyFiddleComponent);
        }
    }

}
