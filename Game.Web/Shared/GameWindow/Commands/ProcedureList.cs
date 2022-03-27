using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;
using System.Linq;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class ProcedureList : ICommands
    {
        public CodeBlockBase codeBlock;
        public ProcedureList(CodeBlockBase codeBlock)
        {
            this.codeBlock = codeBlock;
        }

        public async Task execute()
        {
            string list="";
            if(codeBlock.Procedures.Count > 0)
            {
                for(int i = 0; i < codeBlock.Procedures.Count; i++)
                {
                    list += codeBlock.Procedures.ElementAt(i).Key;
                    list += " ";
                }
                codeBlock.addTextToConsole(list, "green");
            }
            else
            {
                codeBlock.addTextToConsole("No procedures", "green");
            }
        }
    }
}
