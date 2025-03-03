using ScoundrelCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelCore.Data
{
    public enum Suit
    {
        Spades,
        Hearts,
        Clubs,
        Diamonds
    }
    public class Card
    {
        public int Value { get; set; }
        public Suit Suit { get; set; }
        public override string ToString()
        {
            var text = "";
            if (Value > 10)
            {
                switch (Value)
                {
                    case 11:
                        text = $"Jack {SuitToUnicode(Suit)}";
                        break;
                    case 12:
                        text = $"Queen {SuitToUnicode(Suit)}";
                        break;
                    case 13:
                        text = $"King {SuitToUnicode(Suit)}";
                        break;
                    case 14:
                        text = $"Ace {SuitToUnicode(Suit)}";
                        break;
                    default:
                        text = $"Skibidi {SuitToUnicode(Suit)}";
                        break;
                }
            }
            else
            {
                text = $"{Value} {SuitToUnicode(Suit)}";
            }
            return text;
        }

        private string SuitToUnicode(Suit suit)
        {
            var unicode = "";
            switch (suit)
            {
                case Suit.Spades:
                    unicode = "\u2660";
                    break;

                case Suit.Clubs:
                    unicode = "\u2663";
                    break;

                case Suit.Hearts:
                    unicode = "\u2665";
                    break;

                case Suit.Diamonds:
                    unicode = "\u2666";
                    break;

            }
            return unicode;
        }
    }
}
