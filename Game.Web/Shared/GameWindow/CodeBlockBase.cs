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
        [Parameter]
        public MapBase mapBase { get; set; }
        public double stamina =70;

        public List<ICommands> Commands = new List<ICommands>();
        protected Code codeLines;
        public Stack forLoops;
        //public List<Dictionary<int,string>> integers;
        public Dictionary<string, int> integers;
        public string visibleSettings = "hidden";
        public string visibleInfo = "hidden";
        public string consoleColor;
        public bool disabledSubmit = false;
        public bool disabledStop = true;
        public bool stopped = false;
        public string textStop = "Stop";
        public int line { get; set; }
        protected string line1;
        protected string line2;
        protected string line3;
        protected string line4;
        protected string line1Color;
        protected string line2Color;
        protected string line3Color;
        protected string line4Color;

        protected override void OnInitialized()
        {
            codeLines = new Code();
            forLoops = new Stack();
            integers = new Dictionary<string, int>();
            Stamina();
        }
        protected async override void OnParametersSet()
        {
            
        }
        public async Task Stamina()
        {
            while (true)
            {
                if (stamina < 100)
                {
                    stamina++;
                    await Task.Delay(400);
                }
                else
                {
                    await Task.Delay(1000);
                }
            }       
        }
        public async Task AddStamina(int time)
        {
            for (int i = 0; i < time * 20; i++)
            {
                if (stamina < 100)
                {
                    stamina++;
                    await Task.Delay(50);
                }
            }
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
                            Commands.Add(new Move(this,mapBase, getDir(line.Split(' ')[1]), line.Split(' ')[2]));
                            break;
                        case "forloop":
                            int count;
                            string s = line.Split(' ')[1];
                            if (!Int32.TryParse(s, out count))
                            {
                                count = integers[s];
                            }
                            forLoops.Push(new ForLoop(this, Commands.Count - 1, count));
                            Commands.Add((ICommands)forLoops.Peek());
                            break;
                        case "endfor":
                            Commands.Add((ICommands)forLoops.Pop());
                            break;
                        case "chop":
                            Commands.Add(new Chop(this,mapBase));
                            break;
                        case "mine":
                            Commands.Add(new Mine(this,mapBase));
                            break;
                        case "sleep":
                            Commands.Add(new Sleep(this, int.Parse(line.Split(' ')[1])));
                            break;
                        case "int":
                            integers.Add(line.Split(' ')[1], int.Parse(line.Split(' ')[2]));
                            break;
                        case "inc":
                            Commands.Add(new Int(this, true,line.Split(' ')[1]));
                            break;
                        case "dec":
                            Commands.Add(new Int(this, false, line.Split(' ')[1]));
                            break;
                        case "set":
                            Commands.Add(new Set(this, line.Split(' ')[1], int.Parse(line.Split(' ')[2])));
                            break;
                    }
                }
            }
            if(forLoops.Count != 0)
            {
                addTextToConsole("Missing endfor", "red");
                Commands.Clear();
                forLoops.Clear();
                integers.Clear();
            }
            else
            {
                await execute();
            }           
            disabledSubmit = false;
            disabledStop = true;
        }
        public void stop()
        {
            stopped = true;
            textStop = "Stopped";
            addTextToConsole("Stopped", "black");
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
            integers.Clear();
            consoleColor = "green";
            addTextToConsole("complete","green");
            line = 0;
            stopped = false;
            textStop = "Stop";
        }
        public void showSettings()
        {   
            visibleSettings = "visible";
            disabledSubmit = true;
        }
        public void showInfo()
        {
            visibleInfo = "visible"; 
            disabledSubmit = true;
        }
        public void addTextToConsole(string text,string color) 
        {
            line4 = line3;
            line4Color = line3Color;
            line3 = line2;
            line3Color = line2Color;
            line2 = line1;
            line2Color = line1Color;
            line1 = text;
            line1Color = color;
        }
    }
}
