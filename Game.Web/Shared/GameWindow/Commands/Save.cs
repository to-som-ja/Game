using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Save : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public Save(CodeBlockBase codeBlock, MapBase map)
        {
            this.map = map;
            this.codeBlock = codeBlock;
        }

        public async Task execute()
        {
            if (map.save())
            {
                codeBlock.addTextToConsole("SAVED", "green");
            }
            else
            {
                codeBlock.addTextToConsole("Cannot save", "red");
            }
        }
    }
}
