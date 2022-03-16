
using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Move : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        int staminaUse = 3;
        public Direction direction = Direction.North;
        public int distance = 0;

        public Move(CodeBlockBase codeBlock, MapBase map, Direction direction, int distance)
        {
            this.map = map;
            this.direction = direction;
            this.distance = distance;
            this.codeBlock = codeBlock;
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
                if (map.canMove(dir))
                {
                    if (codeBlock.stamina >= staminaUse)
                    {
                        codeBlock.stamina -= staminaUse;
                        await map.moveTo(dir);
                    }
                    else
                    {
                        codeBlock.addTextToConsole("No stamina", "red");
                    }
                }
                else
                {
                    codeBlock.addTextToConsole("Cant go there", "red");
                }

            }
        }
    }
}
