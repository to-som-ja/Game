using Game.Models;
using Game.Web.Pages;
using Game.Web.Shared.GameWindow.Enemies;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Inspect : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public Inspect(CodeBlockBase codeBlock, MapBase map)
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
            string line = $"Block:{block.Type} X:{block.positionX},Y:{block.positionY}";
            codeBlock.addTextToConsole(line, "black");
            if (block != null && block.Type == Type.Grass || block.Type == Type.ChoppedTrees)
            {
                foreach (EnemyParent enemy in map.enemies)
                {
                    if (enemy.positionX == block.positionX && enemy.positionY == block.positionY)
                    {
                        line = $"ENEMY: {enemy.name} LEVEL:{enemy.level}";
                        codeBlock.addTextToConsole(line, "black");
                    }
                }
            }   
        }
    }
}
