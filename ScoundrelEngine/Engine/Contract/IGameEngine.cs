using ScoundrelCore.Data;
using ScoundrelCore.Enum;
using ScoundrelCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelCore.Engine.Contract
{
    public interface IGameEngine
    {
        public GameState GameState { get; }
        public Player Player { get; }
        public int RoomNbr { get; }
        public List<Card> Dungeon { get; }
        public List<Card> Room { get; }
        public List<Card> Discard { get; }
        public bool IsRunawayAvailable { get; }
        public void StartGame();
        public void Heal(Card card);
        public void EquipWeapon(Card card);
        public void HandFight(Card card);
        public void WeaponFight(Card card);
        public void RunawayFromRoom();
    }
}
