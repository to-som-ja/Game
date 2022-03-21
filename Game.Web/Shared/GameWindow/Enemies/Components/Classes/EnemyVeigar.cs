
using Game.Web.Shared.GameWindow.Enemies.Components;
using System;
using System.Collections.Generic;

namespace Game.Web.Shared.GameWindow.Enemies
{
    public class EnemyVeigar : EnemyParent, IEnemy
    {
        public double maxTime;
        public EnemyVeigar(int positionX, int positionY, int level):base(positionX, positionY, level)
        {
            name = "Veigar";
            imagePath = "Images/Enemies/enemy-Veigar.png";
            hp = 100;
            maxTime = 100 / level;
        }

        public Type ComponentType()
        {
            return typeof(EnemyVeigarComponent);
        }
    }

}
