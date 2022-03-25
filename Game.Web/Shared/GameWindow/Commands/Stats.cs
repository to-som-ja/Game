using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Stats : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public Stats(CodeBlockBase codeBlock, MapBase map)
        {
            this.map = map;
            this.codeBlock = codeBlock;
        }

        public async Task execute()
        {
            string line=$"X:{map.player.positionX} Y:{map.player.positionY}";
            codeBlock.addTextToConsole(line,"black");
            line = $"DAMAGE:{map.player.damage} LEVEL:{map.player.level}";
            codeBlock.addTextToConsole(line, "black");
            int nextlvlxp = 0;
            if ( map.player.level * 10 - map.player.experience  >  0)
            {
                nextlvlxp = map.player.level * 10 - map.player.experience;
            }
            line = $"XP:{map.player.experience} XpForNextLevel:{nextlvlxp}";
            codeBlock.addTextToConsole(line, "black");
        }
    }
}
