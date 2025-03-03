using ScoundrelCore.Data;
using ScoundrelCore.Engine;
using ScoundrelCore.Enum;
using System.Net.WebSockets;
using System.Numerics;

namespace ScoundrelConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            var input = "";
            while (input != "Q")
            {
                Console.WriteLine(
@"
Commands
--------
P to play
Q to Quit
R for Rules.
");

                input = Console.ReadLine();
                Console.Clear();

                if (input == "P")
                {
                    PlayGame();
                }
                if (input == "R")
                {
                    ShowRules();
                }

            }
        }

        public static void PlayGame()
        {
            GameEngine gameEngine = new GameEngine();

            gameEngine.StartGame();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Game Started");

            var input = "";
            while (input != "Q" && gameEngine.GameState != GameState.Success && gameEngine.GameState != GameState.Failure)
            {
                var result = 0;
                if (int.TryParse(input, out result) && result < 5 && result > 0)
                {
                    var cardPicked = gameEngine.Room[result - 1];

                    switch (cardPicked.Suit)
                    {
                        case Suit.Hearts:
                            gameEngine.Heal(cardPicked);
                            break;
                        case Suit.Diamonds:
                            gameEngine.EquipWeapon(cardPicked);
                            break;
                        case Suit.Spades:
                        case Suit.Clubs:
                            if (gameEngine.Player.Weapon == null)
                            {
                                gameEngine.HandFight(cardPicked);
                            }
                            else
                            {
                                if (gameEngine.Player.PreviousMonsterForWeapon == null || gameEngine.Player.PreviousMonsterForWeapon.Value > cardPicked.Value)
                                {
                                    var fightResult = 0;
                                    while (!(fightResult < 3 && fightResult > 0))
                                    {
                                        Console.WriteLine("1 : Hand Fight \n2 : Weapon Fight");
                                        var fightInput = Console.ReadLine();

                                        if (int.TryParse(fightInput, out fightResult) && fightResult < 3 && fightResult > 0)
                                        {
                                            if (fightResult == 1)
                                            {
                                                gameEngine.HandFight(cardPicked);
                                            }
                                            else if (fightResult == 2)
                                            {
                                                gameEngine.WeaponFight(cardPicked);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    gameEngine.HandFight(cardPicked);
                                }
                            }
                            break;
                    }
                }
                else if (input == "X")
                {
                    if (gameEngine.IsRunawayAvailable && gameEngine.Room.Count == 4)
                    {
                        gameEngine.RunawayFromRoom();
                    }
                }
                Console.Clear();

                Console.WriteLine($"Current room [{gameEngine.RoomNbr}]");
                Console.WriteLine($"Remaining cards in dungeon [{gameEngine.Dungeon.Count}]\n");



                int i = 0;
                gameEngine.Room.ForEach(card =>
                {
                    i++;
                    if (card.Suit == Suit.Diamonds || card.Suit == Suit.Hearts)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine($"{i} : {card}");
                });

                Console.ResetColor();

                Console.WriteLine($"\nPick a card by selecting 1-4{(gameEngine.IsRunawayAvailable && gameEngine.Room.Count == 4 ? ", use X to runaway" : "")} or Q to quit\n");

                Console.WriteLine("Player Health: " + gameEngine.Player.Health);
                Console.WriteLine("Player Weapon: " + gameEngine.Player.Weapon);
                Console.WriteLine("Last weapon kill: " + gameEngine.Player.PreviousMonsterForWeapon);

                if (gameEngine.GameState == GameState.Playing)
                {
                    input = Console.ReadLine();
                }
            }

            Console.Clear();

            if (gameEngine.GameState == GameState.Success)
            {
                Console.WriteLine("YOU WIN !");
            }

            if (gameEngine.GameState == GameState.Failure)
            {
                Console.WriteLine("YOU LOSE !");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nDUNGEON\n");
            foreach (var card in gameEngine.Dungeon)
            {
                if (card.Suit == Suit.Diamonds || card.Suit == Suit.Hearts)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine(card);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"TOTAL = {gameEngine.Dungeon.Count}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nROOM\n");
            foreach (var card in gameEngine.Room)
            {
                if (card.Suit == Suit.Diamonds || card.Suit == Suit.Hearts)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine(card);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"TOTAL = {gameEngine.Room.Count}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nDISCARD\n");

            foreach (var card in gameEngine.Discard)
            {
                if (card.Suit == Suit.Diamonds || card.Suit == Suit.Hearts)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine(card);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"TOTAL = {gameEngine.Discard.Count}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPLAYER CARDS\n");

            var playerCardsCount = 0;

            if (gameEngine.Player.Weapon != null)
            {
                playerCardsCount++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(gameEngine.Player.Weapon);
            }
            if (gameEngine.Player.Weapon != null)
            {
                playerCardsCount++;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(gameEngine.Player.PreviousMonsterForWeapon);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"TOTAL = {playerCardsCount}");

        }

        private static void ShowRules()
        {
            Console.WriteLine(
@"SCOUNDREL
---------                                
***Scoundrel*** is a single player rogue-like card game for use with a standard deck of playing cards. It was developed by Zach Gage and Kurt Bieg.

All Jokers, red face cards, and red aces are removed from the deck. The remaining cards serve as monsters (spades and clubs), weapons (diamonds), and health potions (hearts) valued from 2–14 (with face cards being valued 11–14). The rooms of the dungeon are created by laying out cards in groups of four. Progress is made through the dungeon by resolving three of the four dungeon cards in each room. The fourth card serves as the foundation for a new room. Rooms can be deferred (RUN) if needed, but never twice in a row — and the player will eventually need to come back to deferred rooms.

Weapons may be equipped to attack multiple monsters, as long as each defeated monster is of lower value than the last monster defeated with that same weapon. To attack a monster of higher value, a new weapon must be equipped. Monsters can also always be attacked barehanded, but this depletes the player's health for the full value of the monster. Every fight is in one turn, when the player has a weapon the amount of damages he receive is equal to the power of the monster minus the power of the weapon. (This amount can not be lower than 0). Health potions may be used to recover life points up to the maximum starting value of 20. Playing a second potion in the same room nullifies the effect of the second potion.

The game ends in a loss when life points reach zero or in a victory if the player manages to clear all rooms of the dungeon.
");
        }
    }
}