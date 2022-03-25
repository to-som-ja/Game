using Game.Models;
using Game.Web.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow.Commands
{
    public class Craft : ICommands
    {
        public MapBase map;
        public CodeBlockBase codeBlock;
        public string itemName;
        public Craft(CodeBlockBase codeBlock, MapBase map, string itemName)
        {
            this.map = map;
            this.codeBlock = codeBlock;
            this.itemName = itemName;
        }

        public async Task execute()
        {
            bool findedItem = true;
            List<Item> inventory = map.player.Items;
            List<Item> neededItems = new List<Item>();
            List<string> neededItemsName = new List<string>();
            string imgPath="";
            string name="";
            switch (itemName.ToLower())
            {
                case "planks":
                    imgPath = "Images/Items/item-planks.png";
                    name = "planks";
                    neededItemsName.Add("wood");
                    neededItemsName.Add("wood");
                    break;
                default:
                    codeBlock.addTextToConsole("Item not found", "red");
                    findedItem = false;
                    break;
            }
            if(findedItem)
            {
                foreach (Item item in inventory)
                {
                    for (int i = neededItemsName.Count-1; i >= 0; i--)
                    {
                        if (neededItemsName[i] == item.name)
                        {
                            neededItems.Add(item);
                            neededItemsName.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (neededItemsName.Count == 0)
                {
                    foreach (Item item in neededItems)
                    {
                        inventory.Remove(item);
                    }
                    await Task.Delay(500);
                    inventory.Add(new Item(name, imgPath));
                    map.refresh();
                }
            }
            
        }
    }
}
