
using Game.Models;
using Game.Web.Pages;
using System;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Move : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        int staminaUse = 3;
        public Direction? direction = null;
        public string textDistance;
        Procedure procedure;

        public Move(CodeBlockBase codeBlock, MapBase map, Direction? direction, string textDistance)
        {
            this.map = map;
            this.direction = direction;
            this.textDistance = textDistance;
            this.codeBlock = codeBlock;
        }
        public Move(CodeBlockBase codeBlock, MapBase map, Procedure procedure, Direction? direction, string textDistance)
        {
            this.map = map;
            this.direction = direction;
            this.textDistance = textDistance;
            this.codeBlock = codeBlock;
            this.procedure = procedure;
        }
        public async Task execute()
        {
            int distance=0;
            if (!Int32.TryParse(textDistance, out distance))
            {
                if (procedure==null) 
                {
                    if (codeBlock.integers.ContainsKey(textDistance))
                        distance = codeBlock.integers[textDistance];
                    else
                        codeBlock.addTextToConsole("wrong value", "red");
                }
                else 
                {                   
                    if (procedure.integers.ContainsKey(textDistance))
                        distance = procedure.integers[textDistance];
                    else
                        codeBlock.addTextToConsole("wrong value", "red");
                }
            }
            bool noStamina = false;
            int dir = 0;
            switch (direction)
            {
                case Direction.North:
                    dir = 0;
                    break;
                case Direction.East:
                    dir = 1;
                    break;
                case Direction.South:
                    dir = 2;
                    break;
                case Direction.West:
                    dir = 3;
                    break;
                default:
                    dir = -1;
                    break;
            }
            if (map.canMove(dir))
            {
                for (int i = 0; i < distance; i++)
                {
                    if (codeBlock.stamina >= staminaUse)
                    {
                        if (!codeBlock.stopped)
                        {
                            codeBlock.stamina -= staminaUse;
                            await map.moveTo(dir);
                        }                       
                    }
                    else
                    {
                        noStamina = true;
                    }
                }
            }
            else
            {
                codeBlock.addTextToConsole("Cant go there", "red");
            }
            if (noStamina)
            {
                codeBlock.addTextToConsole("No stamina", "red");
            }
        }
    }
}
