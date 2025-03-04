using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelHybrid.Shared.Components
{
    public partial class Popup
    {
        [Parameter] public string Title { get; set; } = "Modal Title";
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        private bool IsVisible { get; set; }

        public void OpenModal()
        {
            IsVisible = true;
        }

        public async Task CloseModal()
        {
            IsVisible = false;
            await OnClose.InvokeAsync();
        }
    }
}
