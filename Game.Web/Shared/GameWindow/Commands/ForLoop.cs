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
        Procedure procedure;

        public ForLoop(CodeBlockBase codeBlock, int startLine,int count)
        {
            this.codeBlock = codeBlock;
            this.startLine = startLine;
            this.count = count;
            this.maxCount = count;
            start = true;            
        }
        public ForLoop(CodeBlockBase codeBlock,Procedure procedure, int startLine, int count)
        {
            this.codeBlock = codeBlock;
            this.startLine = startLine;
            this.count = count;
            this.maxCount = count;
            start = true;
            this.procedure = procedure;
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
                    if (procedure==null)
                    {
                        codeBlock.line = startLine;
                    }
                    else
                    {
                        procedure.line = startLine;
                    }                 
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
