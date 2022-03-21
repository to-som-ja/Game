
using System;
using System.Collections.Generic;

namespace Game.Web.Shared.GameWindow.Enemies
{
    public class EnemyVeigar : EnemyParent, IEnemy
    {

        public EnemyVeigar(int positionX, int positionY, int level):base(positionX, positionY, level)
        {
            name = "Veigar";
            imagePath = "Images/Enemies/enemy-Veigar.png";
        }

        public Type ComponentType()
        {
            return typeof(EnemyVeigarComponent);
        }
    }

}
