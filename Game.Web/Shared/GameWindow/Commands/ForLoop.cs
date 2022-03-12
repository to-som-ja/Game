using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class ForLoop : ICommands
    {
        public CodeBlockBase codeBlock;
        public int startLine;
        public int count;
        public int maxCount;
        public bool start;

        public ForLoop(CodeBlockBase codeBlock, int startLine,int count)
        {
            this.codeBlock = codeBlock;
            this.startLine = startLine;
            this.count = count;
            this.maxCount = count;
            start = true;
        }
        public Task execute()
        {
            if (start)
            {
                count--;
                start = false;
            }
            else
            {
                if (count > 0)
                {
                    codeBlock.line = startLine;
                }
                else
                {
                    count = maxCount;
                }
                start = true;
            }       
            return Task.CompletedTask;
        }
    }
}
