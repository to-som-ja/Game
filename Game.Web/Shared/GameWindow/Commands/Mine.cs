using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Mine : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        int staminaUse = 20;
        public Mine(CodeBlockBase codeBlock, MapBase map)
        {
            this.map = map;
            this.codeBlock = codeBlock;
        }
        public async Task execute()
        {
            Block block;
            switch (map.player.direction)
            {
                case Direction.North:
                    block = map.mapGrid[map.mapFunction(map.player.positionX, map.player.positionY - 1)];
                    break;
                case Direction.South:
                    block = map.mapGrid[map.mapFunction(map.player.positionX, map.player.positionY + 1)];
                    break;
                case Direction.West:
                    block = map.mapGrid[map.mapFunction(map.player.positionX - 1, map.player.positionY)];
                    break;
                case Direction.East:
                    block = map.mapGrid[map.mapFunction(map.player.positionX + 1, map.player.positionY)];
                    break;
                default:
                    block = null;
                    break;
            }
            if (block != null && block.Type == Type.Rock)
            {
                if (codeBlock.stamina > staminaUse)
                {
                    await Task.Delay(500);
                    block.ImagePath = "Images/grass.png";
                    block.Type = Type.Grass;
                    codeBlock.stamina -= staminaUse;
                    if (map.player.Items.Count < 36)
                    {
                        map.player.Items.Add(new Item("rock", "Images/Items/item-rocks.png"));
                    }
                }
                else
                {
                    codeBlock.addTextToConsole("No stamina for mining", "red");
                }
            }
            else
            {
                codeBlock.addTextToConsole("Error mine", "red");
            }
            map.refresh();

        }
    }
}
