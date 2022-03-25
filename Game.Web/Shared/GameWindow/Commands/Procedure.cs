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
                    commands.Add(new Move(codeBlock, map,this, codeBlock.getDir(line.Split(' ')[1]), line.Split(' ')[2]));
                    break;
                case "forloop":
                    int count;
                    string s = line.Split(' ')[1];
                    if (!Int32.TryParse(s, out count))
                    {
                        count = integers[s];
                    }
                    forLoops.Push(new ForLoop(codeBlock,this, commands.Count - 1, count));
                    commands.Add((ICommands)forLoops.Peek());
                    break;
                case "endfor":
                    commands.Add((ICommands)forLoops.Pop());
                    break;
                case "chop":
                    commands.Add(new Chop(codeBlock, map));
                    break;
                case "mine":
                    commands.Add(new Mine(codeBlock, map));
                    break;
                case "sleep":
                    commands.Add(new Sleep(codeBlock, int.Parse(line.Split(' ')[1])));
                    break;
                case "int":
                    integers.Add(line.Split(' ')[1], int.Parse(line.Split(' ')[2]));
                    break;
                case "inc":
                    commands.Add(new Int(codeBlock, true, line.Split(' ')[1]));
                    break;
                case "dec":
                    commands.Add(new Int(codeBlock, false, line.Split(' ')[1]));
                    break;
                case "set":
                    commands.Add(new Set(codeBlock, line.Split(' ')[1], int.Parse(line.Split(' ')[2])));
                    break;
                case "attack":
                    commands.Add(new Attack(codeBlock, map));
                    commands.Add(new Wait(codeBlock, map));
                    break;
                case "look":
                    commands.Add(new Look(codeBlock, map, codeBlock.getDir(line.Split(' ')[1])));
                    break;
                case "stats":
                    commands.Add(new Stats(codeBlock, map));
                    break;
                case "inspect":
                    commands.Add(new Inspect(codeBlock, map));
                    break;
                case "craft":
                    commands.Add(new Craft(codeBlock, map, line.Split(' ')[1]));
                    break;
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
        }
    }
}
