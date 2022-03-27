using Game.Web.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Set : ICommands
    {
        public CodeBlockBase codeBlock;
        string value;
        string name;
        Procedure procedure = null;

        public Set(CodeBlockBase codeBlock, string name, string value)
        {
            this.codeBlock = codeBlock;
            this.value = value;
            this.name = name;
        }
        public Set(CodeBlockBase codeBlock, Procedure procedure, string name, string value)
        {
            this.codeBlock = codeBlock;
            this.value = value;
            this.name = name;
            this.procedure = procedure;
        }

        public Task execute()
        {
            if(procedure == null)
            {
                if (codeBlock.integers.ContainsKey(name))
                {
                    int number;
                    if (Int32.TryParse(value, out number))
                        codeBlock.integers[name] = number;
                    else
                    {
                        if (codeBlock.integers.ContainsKey(value))
                        {
                            codeBlock.integers[name] = codeBlock.integers[value];
                        }
                        else
                        {
                            codeBlock.addTextToConsole("Wrong value", "red");
                        }
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
                    int number;
                    if (Int32.TryParse(value, out number))
                        procedure.integers[name] = number;
                    else
                    {
                        if (procedure.integers.ContainsKey(value))
                        {
                            procedure.integers[name] = procedure.integers[value];
                        }
                        else
                        {
                            codeBlock.addTextToConsole("Wrong value", "red");
                        }
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
