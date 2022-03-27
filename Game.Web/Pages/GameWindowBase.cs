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
        protected MapBase map;
        protected CodeBlockBase codeBlock;
        protected Player player;
        protected CombatBase combat;
        
    }
}

