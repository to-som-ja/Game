
using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Move : ICommands
    {   
        public MapBase map;
        public Direction direction=Direction.North;
        public int distance=0;

        public Move(MapBase map,Direction direction,int distance)
        {   
            this.map = map;
            this.direction = direction;
            this.distance = distance;
        }
        public async Task execute()
        {
            int dir = 0;
            switch (direction)
            {   
                case Direction.North:
                    dir = 0;
                    break;
                case Direction.East:
                    dir = 1;
                    break;
                case Direction.South:
                    dir = 2;
                    break;
                case Direction.West:
                    dir = 3;
                    break;
            }
            for (int i = 0; i < distance; i++)
            {
                 await map.moveTo(dir);

            }
        }
    }
}
