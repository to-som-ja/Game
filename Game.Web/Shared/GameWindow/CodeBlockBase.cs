using Game.Models;
using Game.Web.Shared.GameWindow;
using Game.Web.Shared.GameWindow.Commands;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Game.Web.Pages
{
    public class CodeBlockBase : ComponentBase
    {
        [Parameter]
        public MapBase mapBase { get; set; }
        [Parameter]
        public CombatBase combat { get; set; }

        public double stamina = 80;

        public List<ICommands> Commands = new List<ICommands>();
        public Dictionary<string, Procedure> Procedures = new Dictionary<string, Procedure>();
        protected Code codeLines;
        public Stack forLoops;
        public Stack ifs;
        //public List<Dictionary<int,string>> integers;
        public Dictionary<string, int> integers;
        public string visibleSettings = "hidden";
        public string visibleInfo = "hidden";
        public string consoleColor;
        public bool disabledSubmit = false;
        public bool disabledStop = true;
        public bool stopped = false;
        public string textStop = "Stop";
        public bool error = false;
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
            ifs = new Stack();
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
        public async Task Wait(int time)
        {
            await Task.Delay(time);
        }

        public async Task AddStamina(int time)
        {
            for (int i = 0; i < time * 20; i++)
            {
                if (stamina < 100 && !stopped)
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
            string procedureName = "";
            bool inProcedure = false;
            if (codeLines.text != null)
            {
                foreach (var line in codeLines.text.Split('\n'))
                {
                    string command = line.Split(' ')[0];
                    if (inProcedure&&!error)
                    {
                        if (command == "endproc")
                        {
                            inProcedure = false;
                            Procedures[procedureName].check();
                        }
                        else
                        {
                            Procedures[procedureName].insertLine(line);
                        }
                    }
                    else
                    {
                        switch (command.ToLower())
                        {
                            case "go":
                                if (line.Split(' ').Length >= 3)
                                    Commands.Add(new Move(this, mapBase, getDir(line.Split(' ')[1]), line.Split(' ')[2]));
                                else
                                    addTextToConsole("Missing arguments", "red");
                                break;
                            case "forloop":
                                int count = 0;
                                string s = "";
                                if (line.Split(' ').Length >= 2)
                                {
                                    s = line.Split(' ')[1];
                                }
                                else
                                {
                                    addTextToConsole("Missing arguments", "red");
                                    error = true;
                                }
                                if (!Int32.TryParse(s, out count))
                                {
                                    if (integers.ContainsKey(s))
                                        count = integers[s];
                                    else
                                    {
                                        addTextToConsole("Wrong value", "red");
                                        error = true;
                                    }
                                }
                                forLoops.Push(new ForLoop(this, Commands.Count - 1, count));
                                Commands.Add((ICommands)forLoops.Peek());
                                break;
                            case "endfor":
                                if (forLoops.Count > 0)
                                    Commands.Add((ICommands)forLoops.Pop());
                                else
                                {
                                    addTextToConsole("Missing forloop", "red");
                                }
                                break;
                            case "chop":
                                Commands.Add(new Chop(this, mapBase));
                                break;
                            case "mine":
                                Commands.Add(new Mine(this, mapBase));
                                break;
                            case "sleep":
                                if (line.Split(' ').Length >= 2)
                                {
                                    Commands.Add(new Sleep(this, int.Parse(line.Split(' ')[1])));
                                }
                                else
                                {
                                    addTextToConsole("Missing arguments", "red");
                                }
                                break;
                            case "int":
                                if (line.Split(' ').Length >= 3)
                                {
                                    int number = 0;
                                    if (Int32.TryParse(line.Split(' ')[2], out number))
                                    {
                                        if (!integers.TryAdd(line.Split(' ')[1], number))
                                        {
                                            addTextToConsole("Variable already defined", "red");
                                        }
                                    }
                                    else
                                    {
                                        addTextToConsole("Wrong value", "red");
                                    }
                                }
                                else                              
                                    addTextToConsole("Missing arguments", "red");                                
                                break;
                            case "inc":
                                if (line.Split(' ').Length >= 2)
                                {
                                    Commands.Add(new Int(this, true, line.Split(' ')[1]));
                                }
                                else
                                    addTextToConsole("Missing arguments", "red");
                                break;
                            case "dec":
                                if (line.Split(' ').Length >= 2)
                                {
                                    Commands.Add(new Int(this, false, line.Split(' ')[1]));
                                }
                                else
                                    addTextToConsole("Missing arguments", "red");
                                break;
                            case "set":
                                if (line.Split(' ').Length >= 3)
                                {
                                        Commands.Add(new Set(this, line.Split(' ')[1], line.Split(' ')[2]));
                                }
                                else
                                    addTextToConsole("Missing arguments", "red");                                
                                break;
                            case "attack":
                                Commands.Add(new Attack(this, mapBase));
                                Commands.Add(new Wait(this, mapBase));
                                break;
                            case "look":
                                if (line.Split(' ').Length >= 2)
                                {
                                    Commands.Add(new Look(this, mapBase, getDir(line.Split(' ')[1])));
                                }
                                else
                                    addTextToConsole("Missing arguments", "red");
                                break;
                            case "stats":
                                Commands.Add(new Stats(this, mapBase));
                                break;
                            case "inspect":
                                Commands.Add(new Inspect(this, mapBase));
                                break;
                            case "craft":
                                if (line.Split(' ').Length >= 2)
                                {
                                    Commands.Add(new Craft(this, mapBase, line.Split(' ')[1]));
                                }
                                else
                                    addTextToConsole("Missing arguments", "red");                                
                                break;
                            case "startproc":
                                if (line.Split(' ').Length >= 2)
                                {
                                    procedureName = line.Split(' ')[1];
                                    inProcedure = true;                                                                  
                                    if (procedureName != "")
                                    {
                                        if (!Procedures.TryAdd(procedureName, new Procedure(this, mapBase)))
                                        {
                                            addTextToConsole("Procedure already exists", "red");
                                        }
                                        else
                                        {
                                            if (line.Split(' ').Length > 2)
                                            {
                                                Procedures[procedureName].parameters = line.Split(' ').Length - 2;
                                                for (int i = 2; i < line.Split(' ').Length; i++)
                                                {
                                                    Procedures[procedureName].integers.TryAdd(line.Split(' ')[i], 0);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        addTextToConsole("Missing arguments", "red");
                                        error = true;
                                    }
                                                                     
                                }
                                else
                                {
                                    addTextToConsole("Missing arguments", "red");
                                    error=true;
                                }                                                                
                                break;
                            case "proc":
                                if (line.Split(' ').Length >= 2)
                                {
                                    string name = line.Split(' ')[1];
                                    if (Procedures.ContainsKey(name))
                                    {
                                        if(Procedures[name].parameters == line.Split(' ').Length - 2)
                                        {
                                            Commands.Add(Procedures[name]);
                                            for (int i = 0; i < Procedures[name].parameters; i++)
                                            {
                                                string intName = Procedures[name].integers.ElementAt(i).Key;
                                                Commands.Insert(Commands.Count-1,new Set(this, Procedures[name], intName, line.Split(' ')[i + 2]));
                                            }
                                        }
                                        else
                                            addTextToConsole("Missing arguments", "red");
                                    }                                        
                                    else
                                        addTextToConsole("Procedure not found", "red");
                                }
                                else
                                    addTextToConsole("Missing arguments", "red");
                                break;
                            case "procremove":
                                if (line.Split(' ').Length >= 2)
                                       Commands.Add(new ProcedureRemove(this, line.Split(' ')[1]));
                                else
                                    addTextToConsole("Missing arguments", "red");
                                break;
                            case "proclist":                                
                                   Commands.Add(new ProcedureList(this));
                                break;
                            case "if":
                                ifs.Push(new If(this,line));
                                Commands.Add((ICommands)ifs.Peek());
                                break;
                            case "endif":
                                ((If)ifs.Peek()).endLine=Commands.Count-1;
                                ifs.Pop();
                                break;
                            case "":
                                break;
                            default:
                                addTextToConsole("Command not found", "red");
                                break;
                        }
                    }
                }
            }
            if (forLoops.Count != 0 || ifs.Count != 0)
            {
                addTextToConsole("Missing endfor", "red");
                Commands.Clear();
                forLoops.Clear();
                integers.Clear();
            }
            else
            {
                if (error)
                {
                    //addTextToConsole("ERROR", "red");
                    Commands.Clear();
                    forLoops.Clear();
                    integers.Clear();
                    error = false;
                }
                else
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
        public Direction? getDir(string stringDir)
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
                default:
                    addTextToConsole("Wrong direction", "red");
                    break;
            }
            return null;
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
            addTextToConsole("complete", "green");
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
        public void addTextToConsole(string text, string color)
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
