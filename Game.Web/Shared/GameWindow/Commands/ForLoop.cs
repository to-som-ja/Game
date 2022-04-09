using Game.Web.Pages;
using System;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class ForLoop : ICommands
    {
        public CodeBlockBase codeBlock;
        public int startLine;
        public string s;
        public int maxCount;
        int count = 0;
        public bool start;
        Procedure procedure;

        public ForLoop(CodeBlockBase codeBlock, int startLine,string count)
        {
            this.codeBlock = codeBlock;
            this.startLine = startLine;
            this.s = count;
            this.maxCount = 0;
            start = true;            
        }
        public ForLoop(CodeBlockBase codeBlock,Procedure procedure, int startLine, string count)
        {
            this.codeBlock = codeBlock;
            this.startLine = startLine;
            this.s= count;
            this.maxCount = 0;
            start = true;
            this.procedure = procedure;
        }
        public Task execute()
        {
            if(maxCount == 0)
            {
                if (!Int32.TryParse(s, out count))
                {
                    if (procedure==null)
                    {
                        if (codeBlock.integers.ContainsKey(s))
                            count = codeBlock.integers[s];
                        else
                        {
                            codeBlock.addTextToConsole("Wrong value", "red");
                        }
                    }
                    else
                    {
                        if (procedure.integers.ContainsKey(s))
                            count = procedure.integers[s];
                        else
                        {
                            codeBlock.addTextToConsole("Wrong value", "red");
                        }
                    }
                    maxCount = count;
                }
            }
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
