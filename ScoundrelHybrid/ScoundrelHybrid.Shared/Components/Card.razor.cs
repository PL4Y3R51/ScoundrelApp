using Microsoft.AspNetCore.Components;
using ScoundrelCore.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelHybrid.Shared.Components
{
    public partial class Card
    {
        [EditorRequired]
        [Parameter]
        public ScoundrelCore.Data.Card CardData { get; set; } = new ScoundrelCore.Data.Card();

        [Parameter]
        public bool IsWeapon { get; set; } = false;

        public void PlayCard()
        {
            switch (CardData.Suit)
            {

                case ScoundrelCore.Data.Suit.Spades:
                    GameEngine.
                    break;
            }
        }
    }
}
