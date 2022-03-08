using Game.Models;
using Game.Web.Shared.GameWindow.Commands;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Web.Pages
{
    public class CodeBlockBase : ComponentBase
    {
        protected Code codeLines;
        public List<ICommands> Commands = new List<ICommands>();
        [Parameter]
        public MapBase mapBase { get; set; }
        protected override void OnInitialized ()
        {
            codeLines = new Code();
        }
        protected async void sendCode()
        {
            if (Commands.Count==0)
            {
                Commands.Add(new Move(mapBase, Direction.East, 3));
                Commands.Add(new Move(mapBase, Direction.North, 3));
            }
           // Commnads.Add(new Move(mapBase, Direction.West, 3));
            //Commnads.Add(new Move(mapBase, Direction.South, 3));
            await execute();
        }
        private async Task execute()
        {
            foreach (ICommands command in Commands)
            {
                await command.execute();
            }
        }
    }
}
