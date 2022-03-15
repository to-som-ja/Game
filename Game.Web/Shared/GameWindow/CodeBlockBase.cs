using Game.Models;
using Game.Web.Shared.GameWindow.Commands;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
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
        public Stack forLoops;
        public string visible = "hidden";
        public int line { get; set; }

        protected override void OnInitialized()
        {
            codeLines = new Code();
            forLoops = new Stack();
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
                        case "forloop":
                            forLoops.Push(new ForLoop(this, Commands.Count - 1, int.Parse(line.Split(' ')[1])));
                            Commands.Add((ICommands)forLoops.Peek());
                            break;
                        case "endfor":
                            Commands.Add((ICommands)forLoops.Pop());
                            break;
                        case "chop":
                            Commands.Add(new Chop(mapBase));
                            break;
                        case "mine":
                            Commands.Add(new Mine(mapBase));
                            break;
                    }
                }
            }
            await execute();
            disabledSubmit = false;
            disabledStop = true;
        }
        public void stop()
        {
            stopped = true;
            textStop = "Stopped";
        }
        public Direction getDir(string stringDir)
        {
            switch (stringDir.ToLower())
            {
                case "up":
                    return Direction.North;
                case "down":
                    return Direction.South;
                case "left":
                    return Direction.West;
                case "right":
                    return Direction.East;
            }
            return Direction.North;
        }
        private async Task execute()
        {
            if (Commands.Count != 0)
            {
                while (line < Commands.Count)
                {
                    if (!stopped)
                    {
                        await Commands[line].execute();
                    }
                    line++;
                }
            }
            Commands.Clear();
            line = 0;
            stopped = false;
            textStop = "Stop";
        }
        public void showSettings()
        {
            visible = "visible";
        }
    }
}
