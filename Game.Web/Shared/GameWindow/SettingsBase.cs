using Game.Web.Pages;
using Microsoft.AspNetCore.Components;

namespace Game.Web.Shared.GameWindow
{
    public class SettingsBase : ComponentBase
    {
        [Parameter]
        public CodeBlockBase codeBlock { get; set; }
        [Parameter]
        public MapBase map { get; set; }
        public int width;
        public int height;
        protected override void OnInitialized()
        {
            codeBlock = new CodeBlockBase();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                width = map.renderWidthEnd - map.renderWidthStart;
                height = map.renderHeightEnd - map.renderHeightStart;
            }
        }
        public void exitSettings()
        {
            width = map.renderWidthEnd - map.renderWidthStart;
            height = map.renderHeightEnd - map.renderHeightStart;
            codeBlock.visibleSettings = "hidden";
            codeBlock.disabledSubmit = false;
        }
        public void saveSettings()
        {
            //map.top = map.top - 40 * ((map.renderHeightEnd - map.renderHeightStart - height) / 2);
            if (width % 2 == 0)
            {
                map.renderWidthEnd = map.player.positionX + width / 2;
            }
            else
            {
                map.renderWidthEnd = map.player.positionX + width / 2 + 1;
            }
            if (height % 2 == 0)
            {
                map.renderHeightEnd = map.player.positionY + height / 2;
            }
            else
            {
                map.renderHeightEnd = map.player.positionY + height / 2 + 1;
            }
            map.renderHeightStart = map.player.positionY - height / 2;
            map.renderWidthStart = map.player.positionX - width / 2;

            map.left = 3 + 40 * (map.player.positionX - map.renderWidthStart);
            map.top = 3 + 40 * (map.player.positionY - map.renderHeightStart);
            map.renderHeight = height;
            map.renderWidth = width;

            map.refresh();
            map.chechEnemies();
            map.refresh();
            codeBlock.visibleSettings = "hidden";
            codeBlock.disabledSubmit = false;
        }
    }
}
