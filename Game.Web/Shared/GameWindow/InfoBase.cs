using Game.Web.Pages;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Game.Web.Shared.GameWindow
{
    public class InfoBase : ComponentBase
    {
        [Parameter]
        public CodeBlockBase codeBlock { get; set; }
        public List<string> CommandsNameList = new List<string>();
        public List<MarkupString> CommandsInfoList = new List<MarkupString>();
        protected override void OnInitialized()
        {
            codeBlock = new CodeBlockBase();
            fillCommandsLists();
        }
        public void exitInfo()
        {
            codeBlock.visibleInfo = "hidden";
            codeBlock.disabledSubmit = false;
        }
        public void fillCommandsLists()
        {
            CommandsNameList.Add("go \"direction\" \"value\"");
            CommandsInfoList.Add((MarkupString)$"direction-up,right,down,left <br />value-number of tiles");

            CommandsNameList.Add("forloop \"value\"");
            CommandsInfoList.Add((MarkupString)$"code between \"forloop\" and \"endfor\" will repeat \"value\" times");
            
            CommandsNameList.Add("chop");
            CommandsInfoList.Add((MarkupString)$"trees in front of the player will be chopped <br />uses 15 stamina");

            CommandsNameList.Add("mine");
            CommandsInfoList.Add((MarkupString)$"rocks in front of the player will be mined <br />uses 20 stamina");

            CommandsNameList.Add("sleep \"time\"");
            CommandsInfoList.Add((MarkupString)$"player will wait for \"time\" seconds with faster stamina regen");

            CommandsNameList.Add("int \"name\" \"value\"");
            CommandsInfoList.Add((MarkupString)$"defines variable that can be used in commands: go, forloop, set");

            CommandsNameList.Add("inc \"name\"");
            CommandsInfoList.Add((MarkupString)$"increments variable value by 1");

            CommandsNameList.Add("dec \"name\"");
            CommandsInfoList.Add((MarkupString)$"decrement variable value by 1");

            CommandsNameList.Add("set \"name\" \"value\"");
            CommandsInfoList.Add((MarkupString)$"set value of variable to new value");

            CommandsNameList.Add("attack");
            CommandsInfoList.Add((MarkupString)$"enemy in front of the player will be attacked");

            CommandsNameList.Add("look \"direction\"");
            CommandsInfoList.Add((MarkupString)$"player will change direction<br />direction-up,right,down,left");

            CommandsNameList.Add("stats");
            CommandsInfoList.Add((MarkupString)$"basic stats about player will be shown in console");

            CommandsNameList.Add("inspect");
            CommandsInfoList.Add((MarkupString)$"stats about block in front of player will be shown in console");

            CommandsNameList.Add("craft \"name\"");
            CommandsInfoList.Add((MarkupString)$"player will craft item");

            CommandsNameList.Add("startproc \"name\"");
            CommandsInfoList.Add((MarkupString)$"commands between \"startproc\" and \"endproc\" will be saved in procedure for next code runs");

            CommandsNameList.Add("procremove \"name\"");
            CommandsInfoList.Add((MarkupString)$"remove rpocedure with \"name\"");

            CommandsNameList.Add("proclist");
            CommandsInfoList.Add((MarkupString)$"all saved procedures will be shown in console");



        }
    }
}
