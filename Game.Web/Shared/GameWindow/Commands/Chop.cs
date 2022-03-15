using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Chop : ICommands
    {
        public MapBase map;

        public Chop(MapBase map)
        {
            this.map = map;
        }
        public async Task execute()
        {
            Block block;
            switch (map.player.direction)
            {
                case Models.Direction.North:
                    block = map.mapGrid[map.mapFunction(map.player.positionX,map.player.positionY-1)];
                    break;
                case Models.Direction.South:
                    block = map.mapGrid[map.mapFunction(map.player.positionX, map.player.positionY + 1)];
                    break;
                case Models.Direction.West:
                    block = map.mapGrid[map.mapFunction(map.player.positionX-1, map.player.positionY)];
                    break;
                case Models.Direction.East:
                    block = map.mapGrid[map.mapFunction(map.player.positionX+1, map.player.positionY)];
                    break;
                    default:
                    block = null;
                    break;
            }
            if (block != null && block.Type == Type.Forest)
            {
                await Task.Delay(200);
                block.ImagePath = "Images/choppedTrees.png";
                block.Type = Type.ChoppedTrees;

                if (map.player.Items.Count<36)
                {
                    map.player.Items.Add(new Item("wood", "Images/Items/item-wood.png"));
                }
            }
            map.refresh();
            
        }
    }
}
