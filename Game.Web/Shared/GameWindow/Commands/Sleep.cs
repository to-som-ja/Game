using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Sleep : ICommands
    {
        public CodeBlockBase codeBlock;
        int time;
        public Sleep(CodeBlockBase codeBlock,int time)
        {
            this.codeBlock = codeBlock;
            this.time = time;
        }

        public async Task execute()
        {
            await codeBlock.AddStamina(time);
        }
    }
}
