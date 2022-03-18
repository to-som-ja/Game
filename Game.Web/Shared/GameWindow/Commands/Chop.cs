﻿using Game.Models;
using Game.Web.Pages;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Chop : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        int staminaUse = 20;
        public Chop(CodeBlockBase codeBlock, MapBase map)
        {
            this.map = map;
            this.codeBlock = codeBlock;
        }
        public async Task execute()
        {
            Block block;
            switch (map.player.direction)
            {
                case Models.Direction.North:
                    block = map.mapGrid[map.mapFunction(map.player.positionX,map.player.positionY-1)];
                    break;
                case Models.Direction.South:
                    block = map.mapGrid[map.mapFunction(map.player.positionX, map.player.positionY + 1)];
                    break;
                case Models.Direction.West:
                    block = map.mapGrid[map.mapFunction(map.player.positionX-1, map.player.positionY)];
                    break;
                case Models.Direction.East:
                    block = map.mapGrid[map.mapFunction(map.player.positionX+1, map.player.positionY)];
                    break;
                    default:
                    block = null;
                    break;
            }
            if (block != null && block.Type == Type.Forest)
            {
                if(codeBlock.stamina >= staminaUse)
                {
                    await Task.Delay(200);
                    block.ImagePath = "Images/choppedTrees.png";
                    block.Type = Type.ChoppedTrees;
                    codeBlock.stamina -= staminaUse;
                    if (map.player.Items.Count < 36)
                    {
                        map.player.Items.Add(new Item("wood", "Images/Items/item-wood.png"));
                    }
                }else
                {
                    codeBlock.addTextToConsole("No stamina for chopping", "red");
                }
            }
            else
            {
                codeBlock.addTextToConsole("Error chop", "red");
            }
            map.refresh();
            
        }
    }
}