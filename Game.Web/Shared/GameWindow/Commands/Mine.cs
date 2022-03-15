using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Mine : ICommands
    {
        public MapBase map;

        public Mine(MapBase map)
        {
            this.map = map;
        }
        public async Task execute()
        {
            Block block;
            switch (map.player.direction)
            {
                case Models.Direction.North:
                    block = map.mapGrid[map.mapFunction(map.player.positionX, map.player.positionY - 1)];
                    break;
                case Models.Direction.South:
                    block = map.mapGrid[map.mapFunction(map.player.positionX, map.player.positionY + 1)];
                    break;
                case Models.Direction.West:
                    block = map.mapGrid[map.mapFunction(map.player.positionX - 1, map.player.positionY)];
                    break;
                case Models.Direction.East:
                    block = map.mapGrid[map.mapFunction(map.player.positionX + 1, map.player.positionY)];
                    break;
                default:
                    block = null;
                    break;
            }
            if (block != null && block.Type == Type.Rock)
            {
                await Task.Delay(500);
                block.ImagePath = "Images/grass.png";
                block.Type = Type.Grass;

                if (map.player.Items.Count < 36)
                {
                    map.player.Items.Add(new Item("rock", "Images/Items/item-rocks.png"));
                }
            }
            map.refresh();

        }
    }
}
