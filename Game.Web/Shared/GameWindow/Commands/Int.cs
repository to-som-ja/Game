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
        Procedure procedure = null;

        public Int(CodeBlockBase codeBlock,bool increment, string name)
        {
            this.codeBlock = codeBlock;
            this.increment = increment;
            this.name = name;
        }
        public Int(CodeBlockBase codeBlock, bool increment,Procedure procedure, string name)
        {
            this.codeBlock = codeBlock;
            this.increment = increment;
            this.name = name;
            this.procedure = procedure;
        }

        public Task execute()
        {
            if (procedure==null)
            {
                if (codeBlock.integers.ContainsKey(name))
                {
                    if (increment)
                    {
                        codeBlock.integers[name]++;
                    }
                    else
                    {
                        codeBlock.integers[name]--;
                    }
                }
                else
                {
                    codeBlock.addTextToConsole("Variable not found", "red");
                }
            }
            else
            {
                if (procedure.integers.ContainsKey(name))
                {
                    if (increment)
                    {
                        procedure.integers[name]++;
                    }
                    else
                    {
                        procedure.integers[name]--;
                    }
                }
                else
                {
                    codeBlock.addTextToConsole("Variable not found", "red");
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
