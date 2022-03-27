using Microsoft.JSInterop;
using System;
namespace Game.Web.Shared
{
    public class BrowserService
    {
        private IJSRuntime JS = null;
        int[] dimension = new int[2];
        public event EventHandler<int[]> Resize;
        public int browserWidth;
        public int browserHeight;
        public async void Init(IJSRuntime js)
        {
            // enforce single invocation            
            if (JS == null)
            {
                this.JS = js;
                await JS.InvokeAsync<string>("resizeListener", DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void SetBrowserDimensions(int jsBrowserWidth, int jsBrowserHeight)
        {
            browserWidth = jsBrowserWidth;
            browserHeight = jsBrowserHeight;
            // For simplicity, we're just using the new width
            dimension[0] = browserWidth;
            dimension[1] = browserHeight;
            this.Resize?.Invoke(this, dimension);
        }
    }
}