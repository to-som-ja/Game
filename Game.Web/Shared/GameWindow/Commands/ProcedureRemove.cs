using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class ProcedureRemove : ICommands
    {
        public CodeBlockBase codeBlock;
        public string procName;
        public ProcedureRemove(CodeBlockBase codeBlock, string procName)
        {
            this.codeBlock = codeBlock;
            this.procName = procName;
        }

        public async Task execute()
        {
            if (codeBlock.Procedures.ContainsKey(procName))
            {
                codeBlock.Procedures.Remove(procName);
                codeBlock.addTextToConsole("Procedure removed", "green");
            }
            else
                codeBlock.addTextToConsole("Procedure not found", "red");
        }
    }
}
