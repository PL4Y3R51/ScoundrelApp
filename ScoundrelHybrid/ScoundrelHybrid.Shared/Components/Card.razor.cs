using Microsoft.AspNetCore.Components;
using ScoundrelCore.Data;
using ScoundrelCore.Engine;
using ScoundrelCore.Engine.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelHybrid.Shared.Components
{
    public partial class Card
    {
        [Inject]
        public required IGameEngine GameEngine { get; set; }

        [EditorRequired]
        [Parameter]
        public ScoundrelCore.Data.Card CardData { get; set; } = new ScoundrelCore.Data.Card();

        [Parameter]
        public bool IsWeapon { get; set; } = false;

        [Parameter]
        public EventCallback OnCardPlayed { get; set; }

        private bool SelectFightType { get; set; }

        private Popup FightPopup { get; set; }

        public async Task PlayCard()
        {
            switch (CardData.Suit)
            {
                case Suit.Hearts:
                    GameEngine.Heal(CardData);
                    if (OnCardPlayed.HasDelegate)
                    {
                        await OnCardPlayed.InvokeAsync();
                    }
                    break;
                case Suit.Diamonds:
                    GameEngine.EquipWeapon(CardData);
                    if (OnCardPlayed.HasDelegate)
                    {
                        await OnCardPlayed.InvokeAsync();
                    }
                    break;
                case Suit.Spades:
                case Suit.Clubs:
                    if (GameEngine.Player.Weapon == null)
                    {
                        GameEngine.HandFight(CardData);
                        if (OnCardPlayed.HasDelegate)
                        {
                            await OnCardPlayed.InvokeAsync();
                        }
                    }
                    else
                    {
                        if (GameEngine.Player.PreviousMonsterForWeapon == null || GameEngine.Player.PreviousMonsterForWeapon.Value > CardData.Value)
                        {
                            //Test fight type
                            FightPopup.OpenModal();
                            if (OnCardPlayed.HasDelegate)
                            {
                                await OnCardPlayed.InvokeAsync();
                            }
                        }
                        else
                        {
                            GameEngine.HandFight(CardData);
                            if (OnCardPlayed.HasDelegate)
                            {
                                await OnCardPlayed.InvokeAsync();
                            }
                        }
                    }
                    break;
            }
        }

        public async Task FightWithChoice(bool useWeapon)
        {
            if (useWeapon)
            {
                GameEngine.WeaponFight(CardData);
            }
            else
            {
                GameEngine.HandFight(CardData);
            }
            await FightPopup.CloseModal();

            if (OnCardPlayed.HasDelegate)
            {
                await OnCardPlayed.InvokeAsync();
            }
        }
    }
}
