using ScoundrelCore.Data;
using ScoundrelCore.Engine.Contract;
using ScoundrelCore.Enum;
using ScoundrelCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelCore.Engine
{
    public class GameEngine : IGameEngine
    {
        public GameState GameState { get; private set; } = GameState.Unknown;
        public Player Player { get; private set; } = new Player();
        public int RoomNbr { get; private set; }
        public List<Card> Dungeon { get; private set; } = new List<Card>();
        public List<Card> Room { get; private set; } = new List<Card>();
        public List<Card> Discard { get; private set; } = new List<Card>();
        public bool IsRunawayAvailable { get; private set; } = true;

        private bool isPotionUsedInRoom { get; set; }

        public void StartGame()
        {
            GameState = GameState.Starting;
            Player = new Player();

            // Create the dungeon
            Dungeon.Clear();
            Dungeon = DefaultCards();
            Dungeon.Shuffle();

            //Empty the room
            Room.Clear();

            //Empty the discard
            Discard.Clear();

            //Pick 4 card for the dungeon
            for (int i = 0; i < 4; i++)
            {
                DrawCard();
            }
            RoomNbr = 1;

            IsRunawayAvailable = true;
            isPotionUsedInRoom = false;

            GameState = GameState.Playing;
        }
        #region Card Actions
        public void Heal(Card card)
        {
            IsRunawayAvailable = false;

            if (!isPotionUsedInRoom)
            {
                Player.Health += card.Value;
            }
            if (Player.Health > 20)
            {
                Player.Health = 20;
            }
            Discard.Add(card);

            isPotionUsedInRoom = true;

            EndOfTurn(card);

        }

        public void EquipWeapon(Card card)
        {
            IsRunawayAvailable = false;

            if (Player.PreviousMonsterForWeapon != null)
            {
                Discard.Add(Player.PreviousMonsterForWeapon);
            }
            if (Player.Weapon != null)
            {
                Discard.Add(Player.Weapon);
            }
            Player.Weapon = card;
            Player.PreviousMonsterForWeapon = null;

            EndOfTurn(card);

        }

        public void HandFight(Card card)
        {
            IsRunawayAvailable = false;

            Player.Health -= card.Value;

            Discard.Add(card);

            EndOfTurn(card);

        }

        public void WeaponFight(Card card)
        {
            IsRunawayAvailable = false;

            try
            {
                if (Player.Weapon == null)
                {
                    throw new Exception("This player has no weapon");
                }
                if (!(card.Value < (Player.PreviousMonsterForWeapon?.Value ?? Int32.MaxValue)))
                {
                    throw new Exception("This weapon cannot fight this ennemy");
                }
                var damage = card.Value - Player.Weapon!.Value;
                Player.Health -= damage >= 0 ? damage : 0;

                if (Player.Health < 0)
                {
                    Player.Health = 0;
                }
                if (Player.PreviousMonsterForWeapon != null)
                {
                    Discard.Add(Player.PreviousMonsterForWeapon);
                }
                Player.PreviousMonsterForWeapon = card;

                EndOfTurn(card);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public void RunawayFromRoom()
        {
            Room.Shuffle();

            foreach (var card in Room)
            {
                Dungeon.Add(card);
            }

            Room.Clear();

            ChangeRoom(true);
        }

        private void EndOfTurn(Card card)
        {
            Room.Remove(card);

            if (Room.Count == 1)
            {
                RoomNbr++;
                ChangeRoom(false);
            }

            if (Player.Health < 1)
            {
                GameState = GameState.Failure;
            }
            if (Dungeon.Count == 0)
            {
                GameState = GameState.Success;
            }
        }

        private void ChangeRoom(bool fromRunaway)
        {
            isPotionUsedInRoom = false;

            while (Room.Count < 4 && Dungeon.Count > 0)
            {
                DrawCard();
            }
            if (fromRunaway)
            {
                IsRunawayAvailable = false;
            }
            else
            {
                IsRunawayAvailable = true;
            }
        }

        private List<Card> DefaultCards()
        {
            List<Card> cards = new List<Card>();
            for (int i = 2; i < 15; i++)
            {
                Card card = new Card();
                card.Value = i;
                card.Suit = Suit.Club;
                cards.Add(card);
            }
            for (int i = 2; i < 15; i++)
            {
                Card card = new Card();
                card.Value = i;
                card.Suit = Suit.Spade;
                cards.Add(card);
            }
            for (int i = 2; i < 10; i++)
            {
                Card card = new Card();
                card.Value = i;
                card.Suit = Suit.Diamond;
                cards.Add(card);
            }
            for (int i = 2; i < 10; i++)
            {
                Card card = new Card();
                card.Value = i;
                card.Suit = Suit.Heart;
                cards.Add(card);
            }
            return cards;
        }

        private void DrawCard()
        {
            Room.Add(Dungeon[0]);
            Dungeon.RemoveAt(0);
        }
    }
}
