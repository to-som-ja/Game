using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class If : ICommands
    {
        public CodeBlockBase codeBlock;
        public int endLine;
        public string line;

        public If(CodeBlockBase codeBlock, string line)
        {
            this.codeBlock = codeBlock;
            this.line = line;
        }
        
        public Task execute()
        {
            bool condition = false;
            if(line.Split(' ').Length == 2)
            {
                condition = line.Split(' ')[1].ToLower() == "true";
                if (!condition)
                {
                    condition = line.Split(' ')[1] == "1";
                    int value = 0;
                    if (codeBlock.integers.TryGetValue(line.Split(' ')[1], out value))
                    {
                        condition = value == 1;
                    }
                }
            }
            else
            {
                if (line.Split(' ')[1].ToLower() == "block")
                {
                    Block block;
                    MapBase map = codeBlock.mapBase;
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
                    switch (line.Split(' ')[2].ToLower())
                    {
                        case "grass":
                            condition = block.Type==Type.Grass;
                            break;
                        case "forest":
                            condition = block.Type == Type.Forest;
                            break;
                        case "chopped":
                            condition = block.Type == Type.ChoppedTrees;
                            break;
                        case "rock":
                            condition = block.Type == Type.Rock;
                            break;
                        case "water":
                            condition = block.Type == Type.Water;
                            break;
                    }
                }
            }
            if (!condition)
            {
                codeBlock.line = endLine;
            }
            return Task.CompletedTask;
        }
    }
}
