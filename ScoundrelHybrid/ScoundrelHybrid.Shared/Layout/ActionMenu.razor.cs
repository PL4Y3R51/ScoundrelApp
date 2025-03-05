using Microsoft.AspNetCore.Components;
using ScoundrelCore.Engine.Contract;
using ScoundrelHybrid.Shared.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScoundrelHybrid.Shared.Layout
{
    public partial class ActionMenu
    {
        [Inject]
        public required IGameEngine GameEngine { get; set; }

        [Parameter]
        public EventCallback OnAction { get; set; }

        private Popup DeckInfo { get; set; }
        private Popup Settings { get; set; }

        public async Task ActionRestartGame()
        {
            GameEngine.StartGame();

            if (OnAction.HasDelegate)
            {
                await OnAction.InvokeAsync();
            }
        }

        public async Task ActionShowInfoGame()
        {
            DeckInfo.OpenModal();

            if (OnAction.HasDelegate)
            {
                await OnAction.InvokeAsync();
            }
        }

        public async Task ActionShowSettings()
        {
            Settings.OpenModal();

            if (OnAction.HasDelegate)
            {
                await OnAction.InvokeAsync();
            }
        }

    }
}
