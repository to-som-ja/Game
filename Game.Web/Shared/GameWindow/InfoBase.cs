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
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
            CommandsNameList.Add("Go Direction Value");
            CommandsInfoList.Add((MarkupString)$"Direction-up,right,down,left <br />Value-number of tiles");
        }
    }
}
