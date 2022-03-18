using Game.Web.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Set : ICommands
    {
        public CodeBlockBase codeBlock;
        int value;
        string name;

        public Set(CodeBlockBase codeBlock, string name, int value)
        {
            this.codeBlock = codeBlock;
            this.value = value;
            this.name = name;
        }

        public Task execute()
        {
            codeBlock.integers[name]=value;
            return Task.CompletedTask;
        }
    }
}
