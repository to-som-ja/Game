using Game.Web.Pages;
using Game.Web.Shared.GameWindow.Enemies;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        protected override void OnInitialized()
        {
            appState.Action = click;
            appState.ActionWithParameters = clickWithParameters;
            PropertyChanged += (o, e) => StateHasChanged();
            StateHasChanged();
        }
        private void click()
        {
            text = "text";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
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
            visible = "visible";
            data.Add("veigar", enemy);
            type = enemy.ComponentType();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
