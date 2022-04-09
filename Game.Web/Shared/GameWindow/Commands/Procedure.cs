using Game.Web.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Procedure: ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public List<ICommands> commands = new List<ICommands>();
        public Dictionary<string, int> integers = new Dictionary<string, int>();
        public int line = 0;
        public Stack forLoops=new Stack();
        bool error;
        public int parameters= 0;

        public Procedure(CodeBlockBase codeBlock, MapBase map)
        {
            this.map = map;
            this.codeBlock = codeBlock;           
        }

        public void insertLine(string line)
        {
            string command = line.Split(' ')[0];
            switch (command.ToLower())
            {
                case "go":                
                    if (line.Split(' ').Length >= 3)
                        commands.Add(new Move(codeBlock, map, this, codeBlock.getDir(line.Split(' ')[1]), line.Split(' ')[2]));
                    else
                        codeBlock.addTextToConsole("Missing arguments", "red");                   
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
                        codeBlock.addTextToConsole("Missing arguments", "red");
                        error = true;
                    }
                    forLoops.Push(new ForLoop(codeBlock, this, commands.Count - 1, s));
                    commands.Add((ICommands)forLoops.Peek());
                    break;
                case "endfor":
                    if (forLoops.Count > 0)
                        commands.Add((ICommands)forLoops.Pop());
                    else
                    {
                        codeBlock.addTextToConsole("Missing forloop", "red");
                    }
                    break;
                case "chop":
                    commands.Add(new Chop(codeBlock, map));
                    break;
                case "mine":
                    commands.Add(new Mine(codeBlock, map));
                    break;
                case "sleep":                   
                    if (line.Split(' ').Length >= 2)
                    {
                        commands.Add(new Sleep(codeBlock, int.Parse(line.Split(' ')[1])));
                    }
                    else
                    {
                        codeBlock.addTextToConsole("Missing arguments", "red");
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
                                codeBlock.addTextToConsole("Variable already defined", "red");
                            }
                            else
                            {
                                commands.Add(new Set(codeBlock, this, line.Split(' ')[1], line.Split(' ')[2]));
                            }
                        }
                        else
                        {
                            codeBlock.addTextToConsole("Wrong value", "red");
                        }
                    }
                    else
                        codeBlock.addTextToConsole("Missing arguments", "red");
                    break;
                case "inc":                   
                    if (line.Split(' ').Length >= 2)
                    {
                        commands.Add(new Int(codeBlock, true,this, line.Split(' ')[1]));
                    }
                    else
                        codeBlock.addTextToConsole("Missing arguments", "red");
                    break;
                case "dec":
                    if (line.Split(' ').Length >= 2)
                    {
                        commands.Add(new Int(codeBlock, false,this, line.Split(' ')[1]));
                    }
                    else
                        codeBlock.addTextToConsole("Missing arguments", "red");
                    break;
                case "set":                   
                    if (line.Split(' ').Length >= 3)
                    {
                        commands.Add(new Set(codeBlock,this, line.Split(' ')[1], line.Split(' ')[2]));
                    }
                    else
                        codeBlock.addTextToConsole("Missing arguments", "red");
                    break;
                case "attack":
                    commands.Add(new Attack(codeBlock, map));
                    commands.Add(new Wait(codeBlock, map));
                    break;
                case "look":                    
                    if (line.Split(' ').Length >= 2)
                    {
                        commands.Add(new Look(codeBlock, map, codeBlock.getDir(line.Split(' ')[1])));
                    }
                    else
                        codeBlock.addTextToConsole("Missing arguments", "red");
                    break;
                case "stats":
                    commands.Add(new Stats(codeBlock, map));
                    break;
                case "inspect":
                    commands.Add(new Inspect(codeBlock, map));
                    break;
                case "craft":                   
                    if (line.Split(' ').Length >= 2)
                    {
                        commands.Add(new Craft(codeBlock, map, line.Split(' ')[1]));
                    }
                    else
                        codeBlock.addTextToConsole("Missing arguments", "red");
                    break;
                case "":
                    break;
                default:
                    codeBlock.addTextToConsole("Command not found", "red");
                    break;
            }

        }
        public void check()
        {
            if (forLoops.Count != 0)
            {
                codeBlock.addTextToConsole("Missing endfor", "red");
                commands.Clear();
                forLoops.Clear();
                integers.Clear();
            }
            else
            {
                if (error)
                {
                    //addTextToConsole("ERROR", "red");
                    commands.Clear();
                    forLoops.Clear();
                    integers.Clear();
                    error = false;
                }
            }
        }
        public async Task execute()
        {
            if (commands.Count != 0)
            {
                while (line < commands.Count)
                {
                    if (!codeBlock.stopped)
                    {
                        await commands[line].execute();
                    }
                    line++;
                }
            }
            line = 0;
            error = false;
        }
    }
}
