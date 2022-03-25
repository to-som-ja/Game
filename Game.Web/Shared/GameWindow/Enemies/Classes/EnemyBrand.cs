
using Game.Web.Shared.GameWindow.Enemies.Components;
using System;
using System.Collections.Generic;

namespace Game.Web.Shared.GameWindow.Enemies
{
    public class EnemyBrand : EnemyParent, IEnemy
    {
        public EnemyBrand(int positionX, int positionY, int level) : base(positionX, positionY, level)
        {
            name = "Brand";
            imagePath = "Images/Enemies/enemy-Brand.png";
            hp = 100;
        }

        public Type ComponentType()
        {
            return typeof(EnemyBrandComponent);
        }
    }

}
