using Game.Models;
using Game.Web.Pages;
using Microsoft.AspNetCore.Components;

namespace Game.Web.Shared.GameWindow
{
    public class InventoryBase : ComponentBase
    {   
        [Parameter]
        public Player player { get; set; }
        protected override void OnInitialized()
        {
            player = new Player(0, 0, "haloo");
        }
    }
}
