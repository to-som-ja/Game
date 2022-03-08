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
        public bool disabledSubmit = false;
        public bool disabledStop = true;
        public bool stopped = false;
        public string textStop = "Stop";

        protected override void OnInitialized ()
        {
            codeLines = new Code();
        }
        protected async void sendCode()
        {
            disabledSubmit = true;
            disabledStop = false;
            if (codeLines.text != null)
            {
                foreach (var line in codeLines.text.Split('\n'))
                {
                    string command = line.Split(' ')[0];
                    switch (command.ToLower())
                    {
                        case "go":
                            Commands.Add(new Move(mapBase, getDir(line.Split(' ')[1]), int.Parse(line.Split(' ')[2])));
                            break;
                    }
                }
            }
            //if (Commands.Count==0)
            //{
            //    Commands.Add(new Move(mapBase, Direction.East, 3));
            //    Commands.Add(new Move(mapBase, Direction.North, 3));
            //}
            // Commnads.Add(new Move(mapBase, Direction.West, 3));
            //Commnads.Add(new Move(mapBase, Direction.South, 3));
             await execute();
             disabledSubmit = false;
             disabledStop = true;
            //execute();
        }
        public void stop()
        {
            stopped=true;
            textStop = "Stopped";
        }
        public Direction getDir(string stringDir)
        {
            switch (stringDir.ToLower())
            {
                case "north":
                    return Direction.North;
                case "south":
                    return Direction.South;
                case "west":
                    return Direction.West;
                case "east":
                    return Direction.East;
            }
            return  Direction.North;
        }
        private async Task execute()
        {
            if (Commands.Count!=0)
            {
                foreach (ICommands command in Commands)
                {
                    if (!stopped)
                    {
                        await command.execute();
                    }
                }
            }
            Commands.Clear();
            stopped = false;
            textStop = "Stop";
        }
    }
}
