using Game.Web.Pages;
using Game.Web.Shared.GameWindow.Enemies;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Web.Shared.GameWindow
{
    public class CombatBase : ComponentBase
    {   
        [Parameter]
        public CodeBlockBase codeBlock { get; set; }
        protected Type type = typeof(Info);
        protected AppState appState = new AppState();
        protected string text= "tu";
        protected string visible = "hidden";
        public event PropertyChangedEventHandler PropertyChanged;
        protected IDictionary<string, object> data = new Dictionary<string, object>();
        public bool waiting =false;
        protected IEnemy enemy;
        protected override void OnInitialized()
        {
            appState.Action = endCombat;
            appState.ActionWithParameters = clickWithParameters;
            PropertyChanged += (o, e) => StateHasChanged();
            StateHasChanged();
        }
        private void endCombat()
        {
            // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            visible = "hidden";
            if (((EnemyParent)enemy).hp <= 0)
            {
                //codeBlock.mapBase.player.experience += ((EnemyParent)enemy).level;
                codeBlock.mapBase.player.experience += 20;
                codeBlock.mapBase.enemies.Remove(enemy);
                codeBlock.mapBase.refresh();
                codeBlock.addTextToConsole("Enemy defeated", "green");
                if (codeBlock.mapBase.player.levelUp())
                {
                    codeBlock.addTextToConsole("LEVEL UP", "green");
                }
            }
            waiting = false;
        }
        private void clickWithParameters(Dictionary<string, object> dictionaryParameters)
        {
            foreach (var param in dictionaryParameters)
            {
                text += param.Value;
            }          
        }
        public void combat(IEnemy enemy)
        {   
            this.enemy = enemy;
            waiting = true;
            data.Clear();
            visible = "visible";
            data.Add("enemy", enemy);
            data.Add("player", codeBlock.mapBase.player);
            type = enemy.ComponentType();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            codeBlock.mapBase.refresh();
        }
    }
}
