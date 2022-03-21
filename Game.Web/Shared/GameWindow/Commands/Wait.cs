using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Wait : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public Wait(CodeBlockBase codeBlock, MapBase map)
        {
            this.map = map;
            this.codeBlock = codeBlock;
        }

        public async Task execute()
        {
            while (codeBlock.combat.waiting)
            {
                await codeBlock.Wait(200);
            }            
        }
    }
}
