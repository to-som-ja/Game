using Game.Models;
using Game.Web.Pages;
using Game.Web.Shared.GameWindow.Enemies;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Attack : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public Attack(CodeBlockBase codeBlock, MapBase map)
        {
            this.map = map;
            this.codeBlock = codeBlock;
        }
        public async Task execute()
        {           
            codeBlock.combat.combat(map.enemies[0]);
        }
    }
}
