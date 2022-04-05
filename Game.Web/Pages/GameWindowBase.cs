using Game.Models;
using Game.Web.Shared.GameWindow;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Type = Game.Models.Type;

namespace Game.Web.Pages
{   
    
    public class GameWindowBase : ComponentBase
    {
        [Parameter]
        public int seed { get; set; }
        [Parameter]
        public int dificulty { get; set; }
        [Parameter]
        public int gameId { get; set; }

        protected MapBase map;
        protected CodeBlockBase codeBlock;
        protected Player player;
        protected CombatBase combat;
        
    }
}

