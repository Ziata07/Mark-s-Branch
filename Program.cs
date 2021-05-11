using System;
using System.Collections.Generic;
namespace HyperSpace_Cheese_Battle
{
    class Program
    {
       // static int[] diceValues = new int[] { 2, 0, 1, 4 };
       // static int diceValuePos = 0;
        static Random diceRandom = new Random(1);//set seed #, predetermined dice roll.
        const int MaxOnBoard = 7;
        const int MinOnBoard = 0;
        static bool gameOver = false;

        enum PlayerMovement
        {
            arrowUp,
            arrowDown,
            arrowLeft,
            arrowRight,
            win
        }

        class BoardTile
        {
            public PlayerMovement Direction;
            public bool IsCheese;

            public BoardTile(PlayerMovement direction, bool isCheese = false)
            {
                Direction = direction;
                IsCheese = isCheese;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    BoardTile tile = (BoardTile)obj;
                    return this.Direction == tile.Direction && this.IsCheese == tile.IsCheese;
                }
            }
        }

        static List<Player> playerList = new List<Player>();

        static BoardTile[,] board = new BoardTile[,]
       {
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight){IsCheese = true},new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowDown),},//Coloumn 1 (0)
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),},
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowRight),},
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowDown),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight),new BoardTile(PlayerMovement.arrowRight){IsCheese = true},new BoardTile(PlayerMovement.arrowDown),new BoardTile(PlayerMovement.arrowRight),},
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp){IsCheese = true},new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowRight),},
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowRight),},
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft){IsCheese= true},new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowDown)},
            {new BoardTile(PlayerMovement.arrowUp),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.arrowLeft),new BoardTile(PlayerMovement.win)},//Coloumn 8 (7)                                     
       };

        class Player
        {
            public int Number;
            public int X = 0;
            public int Y = 0;

            //void move function here, takes X and Y
        }
        static void ResetGame()
        {
            gameOver = false;
            int playerNumber = 0;
            playerList.Clear();

            do
            {
                Console.Write("How many players are playing? ");
                string numP = Console.ReadLine();
                try
                {
                    playerNumber = int.Parse(numP);
                }
                catch (FormatException)
                {
                    Console.Write("Please enter a number from 2 to 4: ");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("That submitted number was too long. Please try again.");
                }
                if (playerNumber > 4 || playerNumber < 2)
                {
                    Console.WriteLine("This game can only support between 2, 3, or 4 players!");
                }
                else
                {
                    for (int i = 0; i < playerNumber; i++)
                    {
                        playerList.Add(new Player() { Number = i + 1 });
                    }
                    break;
                }
            } while (true);
        }

        static void ShowStatus()
        {
            Console.WriteLine("\nHyperspace Cheese Battle Status Report\n" +
                              "======================================");
            Console.WriteLine($"There are {playerList.Count} players in the game.");
            foreach (Player p in playerList)
            {
                Console.WriteLine($"Player {p.Number}'s position is {p.X}.{p.Y}\n");
            }
            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();
        }
        static void MakeMoves()
        {
            while (!gameOver)
            {
                foreach (Player p in playerList)
                {

                    if (gameOver)
                    {
                        break;
                    }
                    else
                    {
                        ShowStatus();
                        PlayerTurn(p);
                    }

                }
            }
        }

        static bool RocketInSquare(int x, int y)
        {
            foreach (Player p in playerList)
            {
                if (p.X == x && p.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        static void ShowCheeseOptions(Player p)
        {
            Console.WriteLine($"Player {p.Number}'s position is {p.X}.{p.Y}.\nOption 1: Roll the dice again!\nOption 2: Send a player back down");
            Console.Write("Type 1 or 2 then press enter to confirm your choice: ");

            do
            {
                string input = Console.ReadLine();
                if (input == "1")
                {
                    Console.WriteLine($"Player {p.Number} is rolling the dice again!");
                    PlayerTurn(p);
                    break;
                }
                else if (input == "2")
                {
                    PushDownPlayer();
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a 1 or 2");
                }
            } while (true);
        }

        static void PushDownPlayer()
        {
            int playerNumber = 0;

            Console.WriteLine("Choose a player to send back down!" + "\n");
            foreach (Player p in playerList)
            {
                Console.WriteLine($"Player {p.Number}'s position is {p.X}.{p.Y}");
            }

            do
            {
                Console.Write("Enter a player number: ");
                string input = Console.ReadLine();

                try
                {
                    playerNumber = int.Parse(input);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Enter a nummerical player number between 1 and {playerList.Count}");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Number was too large!" + $"Enter a numberical player number between 1 and {playerList.Count}");
                }
                if (playerNumber > playerList.Count || playerNumber < 1)
                {
                    Console.WriteLine($"Number was out of bounds! Choose a player from 1 to {playerList.Count}");
                }
                else
                {
                    Console.WriteLine($"You chose Player {playerNumber}.\nSending Player{playerNumber} back down!");
                    playerList[playerNumber - 1].X -= playerList[playerNumber - 1].X;
                    Console.WriteLine($"Player {playerNumber}'s position is now {playerList[playerNumber - 1].X}.{playerList[playerNumber - 1].Y}");
                    break;
                }

            } while (true);
        }
        //static int DiceThrow()
        //{
        //    int spots = diceValues[diceValuePos];
        //    diceValuePos = diceValuePos + 1;
        //    if (diceValuePos == diceValues.Length)
        //        diceValuePos = 0;
        //    return spots;
        //}
        static int PresetDiceThrow()
        {
            return diceRandom.Next(1, 7);
        }

        static bool IsMoveInvalid(int x, int y)
        {
            if (x > MaxOnBoard || y > MaxOnBoard)
            {
                return true;
            }
            else if (x < MinOnBoard || y < MinOnBoard)
            {
                return true;
            }
            else
                return false;
        }

        static bool SpotIsCheese(int x, int y)
        {
            if (board[x, y].Equals(new BoardTile(PlayerMovement.arrowUp, true)))
            {
                return true;
            }
            else if (board[x, y].Equals(new BoardTile(PlayerMovement.arrowLeft, true)))
            {
                return true;
            }
            else if (board[x, y].Equals(new BoardTile(PlayerMovement.arrowRight, true)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void PlayerTurn(Player player)
        {
            bool playerTurnToggle = true;
            int displayNum = player.Number;
            int result = PresetDiceThrow();

            Console.WriteLine($"It's Player {displayNum}'s turn! Press Enter to Roll Dice.");
            Console.ReadKey();
            Console.WriteLine($"Player {displayNum} rolled a {result}.");
            while (playerTurnToggle)
            {
                BoardTile currentTile = board[player.X, player.Y];
                PlayerMovement currentDirection = currentTile.Direction;
                switch (currentDirection)
                {
                    case PlayerMovement.arrowUp:
                        {
                            int newX = player.X;
                            int newY = player.Y + result;

                            if (IsMoveInvalid(newX, newY))
                            {
                                Console.WriteLine($"Move is not valid. The total roll of {newY} will move player off the board!");
                                playerTurnToggle = false;
                                break;
                            }
                            else
                            {
                                player.X = newX;
                                player.Y = newY;
                                //move player ,set coordinates
                            }

                            if (RocketInSquare(player.X, player.Y))
                            {
                                Console.WriteLine($"There is another player in coordinate {newX}.{newY}. Player {displayNum} is making a bounce!");
                                result = 1;
                                playerTurnToggle = true;
                            }
                            else
                            {
                                playerTurnToggle = false;
                            }

                            if (SpotIsCheese(player.X, player.Y))
                            {
                                Console.WriteLine($"Player {displayNum} landed on a spot of cheese!\n");
                               // player.X = newX;
                               // player.Y = newY; //better way to do this?
                                ShowCheeseOptions(player);
                               // newY = player.Y;
                               // newX = player.X;
                            }

                           // player.X = newX;
                           // player.Y = newY;
                        }
                        break;

                    case PlayerMovement.arrowDown:
                        {
                            int newX = player.X;
                            int newY = player.Y - result;

                            if (IsMoveInvalid(newX, newY))
                            {
                                Console.WriteLine($"Move is not valid. The total roll of {newY} will move player off the board!");
                                playerTurnToggle = false;
                                break;
                            }

                            if (RocketInSquare(newX, newY))
                            {
                                Console.WriteLine($"There is another player in coordinate {newX}.{newY}. Player {displayNum} is making a bounce!");
                                result = 1;
                                playerTurnToggle = true;
                            }
                            else
                            {
                                playerTurnToggle = false;
                            }

                            if (SpotIsCheese(newX, newY))
                            {
                                Console.WriteLine($"Player {displayNum} landed on a spot of cheese!\n");
                                player.X = newX;
                                player.Y = newY; //better way to do this?
                                ShowCheeseOptions(player);
                                newY = player.Y;
                                newX = player.X;
                            }

                            player.X = newX;
                            player.Y = newY;
                        }
                        break;

                    case PlayerMovement.arrowLeft:
                        {
                            int newX = player.X - result;
                            int newY = player.Y;

                            if (IsMoveInvalid(newX, newY))
                            {
                                Console.WriteLine($"Move is not valid. The total roll of {newX} will move player off the board!");
                                playerTurnToggle = false;
                                break;
                            }

                            if (RocketInSquare(newX, newY))
                            {
                                Console.WriteLine($"There is another player in coordinate {newX}.{newY}. Player {displayNum} is making a bounce!");
                                result = 1;
                                playerTurnToggle = true;
                            }
                            else
                            {
                                playerTurnToggle = false;
                            }

                            if (SpotIsCheese(newX, newY))
                            {
                                Console.WriteLine($"Player {displayNum} landed on a spot of cheese!\n");
                                player.X = newX;
                                player.Y = newY; //better way to do this?
                                ShowCheeseOptions(player);
                                newY = player.Y;
                                newX = player.X;
                            }

                            player.X = newX;
                            player.Y = newY;
                        }
                        break;

                    case PlayerMovement.arrowRight:
                        {
                            int newX = player.X + result;
                            int newY = player.Y;

                            if (IsMoveInvalid(newX, newY))
                            {
                                Console.WriteLine($"Move is not valid. The total roll of {newX} will move player off the board!");
                                playerTurnToggle = false;
                                break;
                            }

                            if (RocketInSquare(newX, newY))
                            {
                                Console.WriteLine($"There is another player in coordinate {newX}.{newY}. Player {displayNum} is making a bounce!");
                                result = 1;
                                playerTurnToggle = true;
                            }
                            else
                            {
                                playerTurnToggle = false;
                            }

                            if (SpotIsCheese(newX, newY))
                            {
                                Console.WriteLine($"Player {displayNum} landed on a spot of cheese!\n");
                                player.X = newX;
                                player.Y = newY; //better way to do this?
                                ShowCheeseOptions(player);
                                newY = player.Y;
                                newX = player.X;
                            }

                            player.X = newX;
                            player.Y = newY;
                        }
                        break;
                }
            }

            if (board[player.X, player.Y].Direction == PlayerMovement.win)
            {
                Console.WriteLine("\n======================================");
                Console.WriteLine($"Player {displayNum} wins!");

                gameOver = true;
                playerTurnToggle = false;
            }
        }
        static void Main(string[] args)
        {
            string newGame;
            do
            {
                ResetGame();
                MakeMoves();
                Console.WriteLine("Do you want to play again? Press 'Enter' to start new game or press any key to close the application!\n");
                newGame = Console.ReadLine();

            } while (String.Equals(newGame,String.Empty));
        }
    }
}
