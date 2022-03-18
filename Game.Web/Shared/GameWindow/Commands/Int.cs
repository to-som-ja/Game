using Game.Web.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Int : ICommands
    {
        public CodeBlockBase codeBlock;
        bool increment;
        string name;

        public Int(CodeBlockBase codeBlock,bool increment, string name)
        {
            this.codeBlock = codeBlock;
            this.increment = increment;
            this.name = name;
        }

        public Task execute()
        {
            if (increment)
            {
                codeBlock.integers[name]++;
            }
            else
            {
                codeBlock.integers[name]--;
            }
            return Task.CompletedTask;
        }
    }
}
