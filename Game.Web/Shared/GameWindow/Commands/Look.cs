
using Game.Models;
using Game.Web.Pages;
using System;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Look : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public Direction? direction = Direction.North;


        public Look(CodeBlockBase codeBlock, MapBase map, Direction? direction)
        {
            this.map = map;
            this.direction = direction;
            this.codeBlock = codeBlock;
            
        }
        public async Task execute()
        {
            map.player.direction=direction;
            switch (direction)
            {
                case Direction.North: map.player.ImagePath = "Images/player-up.png"; break;
                case Direction.South: map.player.ImagePath = "Images/player-down.png"; break;
                case Direction.West: map.player.ImagePath = "Images/player-left.png"; break;
                case Direction.East: map.player.ImagePath = "Images/player-right.png"; break;
            }
            map.refresh();
        }
    }
}
