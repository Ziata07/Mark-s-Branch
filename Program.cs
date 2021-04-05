using System;
using System.Collections.Generic;

namespace HyperSpace_Cheese_Battle
{
    class Program
    {
        static int[] diceValues = new int[] { 1, 2, 3, 3 };
        static int diceValuePos = 0;
        static Random diceRandom = new Random();
        const int MaxOnBoard = 7;
        const int MinOnBoard = 0;
        static bool gameOver = false;
        static bool testMode = false;

        static PlayerMovement[,] board = new PlayerMovement[,]
       {
            {PlayerMovement.arrowUp,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowDown}, //Row 1 (0)
            {PlayerMovement.arrowUp,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight},
            {PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight,PlayerMovement.arrowRight},
            {PlayerMovement.arrowUp,PlayerMovement.arrowDown,PlayerMovement.arrowUp,PlayerMovement.arrowRight,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowLeft,PlayerMovement.arrowRight},
            {PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowLeft,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowRight},
            {PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowLeft,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowUp,PlayerMovement.arrowLeft,PlayerMovement.arrowRight},
            {PlayerMovement.arrowUp,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowDown},
            {PlayerMovement.arrowUp,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.arrowLeft,PlayerMovement.win} //Row 8 (7)
       };

        static BoardTile[,] board_new = new BoardTile[,]
        {
            { new BoardTile(PlayerMovement.arrowUp)}

        };
        

        static List<Player> players = new List<Player>(); //For dynamic changes!

        class BoardTile
        {
            public BoardTile(PlayerMovement direction, bool isCheese = false)
            {
                Direction = direction;
                IsCheese = isCheese;
            }
           public PlayerMovement Direction;
            public bool IsCheese;

        }
        class Player
        {
            public int Number;
            public int X = 0;
            public int Y = 0;
        }
        static void ResetGame()
        {
            int playerNumber = 0;
            players.Clear();

            do
            {
                Console.Write("How many players are playing? ");
                string playerNumberString = Console.ReadLine();
                try
                {
                    playerNumber = int.Parse(playerNumberString);
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
                    for (int i = 0; i < playerNumber; i++)//create player function
                    {
                        players.Add(new Player() {Number = i + 1 });//Adds new player to list
                    }
                    break;
                }                    
                
            } while (true);

           
        }

        static void ShowStatus()
        {
            Console.WriteLine("Hyperspace Cheese Battle Status Report\n" +
                              "======================================");
            Console.WriteLine($"There are {players.Count} players in the game.");
            foreach (Player p in players)//USE foreach!
            {
                //Console.WriteLine("Player {0}'s current position is ({1}.{2})\n------------", players.IndexOf(p) + 1, p.X, p.Y);//IndexOf!
                Console.WriteLine($"Player {p.Number}'s current position is ({p.X}.{p.Y})\n------------");//IndexOf!

            }

            Console.WriteLine("Press any key to Progress.");
            Console.ReadKey();
        }
        static void MakeMoves()
        {
            while (!gameOver)//name bools to make sense!
            {
                
                foreach (Player p in players)
                {
                    PlayerTurn(p);
                    if (gameOver)
                    {
                        break;
                    }
                }
            }
        }

        enum PlayerMovement
        {
            arrowUp,
            arrowDown,
            arrowLeft,
            arrowRight,
            win
        }

        static bool cheesePowerSquare(int x, int y)
        {
            if (x == 4 && y == 1)
            {
                return true;
            }
            else if (x == 0 && y == 3)
            {
                return true;
            }
            else if (x == 6 && y == 4)
            {
                return true;
            }
            else if (x == 3 && y == 5)
            {
                return true;
            }
            else
                return false;
        }
        static bool RocketInSquare(int x, int y)
        {

            for (int i = 0; i < players.Count; i++)//foreach
            {
                if (players[i].X == x && players[i].Y == y)
                    return true;
            }
            return false;
        }

        static int DiceThrow()
        {
            int spots = diceValues[diceValuePos];
            diceValuePos = diceValuePos + 1;
            if (diceValuePos == diceValues.Length)
                diceValuePos = 0;
            return spots;
        }
        static int PresetDiceThrow()
        {
            return diceRandom.Next(1, 7);
        }
        private static void PlayerTurn(Player player)
        {
            ShowStatus();
            int displayNum = player.Number;
            int result = PresetDiceThrow();
            bool bounce = true;

            /*if (testMode == false)
            {
                Console.WriteLine("Test mode is off!");
                result = DiceThrow();
                testMode = true;
            }
            else
            {
                Console.WriteLine("Test mode is on!");
                result = PresetDiceThrow();
                testMode = false;
            }*/
            Console.WriteLine("\nPlayer {0}, it's your turn. Press any key to roll the dice!", displayNum);
            Console.ReadLine();
            Console.WriteLine("Player {0} rolled a {1}", displayNum, result);
            while (bounce)
            {
                switch (board[player.X, player.Y])
                {
                    case PlayerMovement.arrowUp:
                        if (RocketInSquare(player.X, player.Y + result))//check for a bounce here/see if another rocket is at the location.
                        {
                            bounce = true;
                        }
                        else
                        {
                            bounce = false;
                        }
                        if (result + player.Y <= MaxOnBoard)//whether the above is true or false, see if we're on or off the board.
                        {
                            player.Y += result;
                            Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, player.X, player.Y);
                            if (bounce) //if the above lands us on a point where another player is...
                            {
                                Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, player.X, player.Y);
                                result = 1; //??
                            }
                            else
                            {
                                Console.WriteLine("No bounce!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                            bounce = false;
                        }
                        break;
                    case PlayerMovement.arrowDown:
                        if (RocketInSquare(player.X, playera.Y - result))
                        {
                            bounce = true;
                        }
                        else
                        {
                            bounce = false;
                        }
                        if (players[playerNo].Y - result >= MinOnBoard)
                        {
                            players[playerNo].Y -= result;
                            Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                            if (bounce)
                            {
                                Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                result = 1; //??
                            }
                            else
                            {
                                Console.WriteLine("No Bounce!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                        }
                        break;
                    case PlayerMovement.arrowLeft:
                        if (RocketInSquare(players[playerNo].X - result, players[playerNo].Y))
                        {
                            bounce = true;
                        }
                        else
                        {
                            bounce = false;
                        }
                        if (players[playerNo].X - result >= MinOnBoard)
                        {
                            players[playerNo].X -= result;
                            Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                            if (bounce)
                            {
                                Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                result = 1; //??
                            }
                            else
                            {
                                Console.WriteLine("No Bounce!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                        }

                        break;
                    case PlayerMovement.arrowRight:
                        if (RocketInSquare(players[playerNo].X + result, players[playerNo].Y))
                        {
                            bounce = true;
                        }
                        else
                        {
                            bounce = false;
                        }
                        if (result + players[playerNo].X <= MaxOnBoard)
                        {
                            players[playerNo].X += result;
                            Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                            if (bounce)
                            {
                                Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                result = 1; //??
                            }
                            else
                            {
                                Console.WriteLine("No Bounce!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (board[players[playerNo].X, players[playerNo].Y] == PlayerMovement.win)
            {
                Console.WriteLine("Game is over!");
                gameOver = true;
                bounce = false;
            }
            else if (cheesePowerSquare(players[playerNo].X, players[playerNo].Y) == true)
            {
                Console.WriteLine("Option 1: Roll the dice again!\nOption 2: Send a player back down");
                Console.Write("Press 1 or 2: ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    result = PresetDiceThrow();
                    Console.WriteLine("Player {0} rolled a {1}", displayNum, result);
                    bounce = true;
                    while (bounce) //we set up ANOTHER turn here. Is there any easier way?
                    {
                        switch (board[players[playerNo].X, players[playerNo].Y])
                        {
                            case PlayerMovement.arrowUp:
                                if (RocketInSquare(players[playerNo].X, players[playerNo].Y + result))
                                {
                                    bounce = true;
                                }
                                else
                                {
                                    bounce = false;
                                }
                                if (result + players[playerNo].Y <= MaxOnBoard)
                                {
                                    players[playerNo].Y += result;
                                    Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                    if (bounce) //if the above lands us on a point where another player is...
                                    {
                                        Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                        result = 1; //??
                                    }
                                    else
                                    {
                                        Console.WriteLine("No bounce!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                    bounce = false;
                                }
                                break;
                            case PlayerMovement.arrowDown:
                                if (RocketInSquare(players[playerNo].X, players[playerNo].Y - result))
                                {
                                    bounce = true;
                                }
                                else
                                {
                                    bounce = false;
                                }
                                if (players[playerNo].Y - result >= MinOnBoard)
                                {
                                    players[playerNo].Y -= result;
                                    Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                    if (bounce)
                                    {
                                        Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                        result = 1; //??
                                    }
                                    else
                                    {
                                        Console.WriteLine("No Bounce!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                }
                                break;
                            case PlayerMovement.arrowLeft:
                                if (RocketInSquare(players[playerNo].X - result, players[playerNo].Y))
                                {
                                    bounce = true;
                                }
                                else
                                {
                                    bounce = false;
                                }
                                if (players[playerNo].X - result >= MinOnBoard)
                                {
                                    players[playerNo].X -= result;
                                    Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                    if (bounce)
                                    {
                                        Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                        result = 1; //??
                                    }
                                    else
                                    {
                                        Console.WriteLine("No Bounce!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                }

                                break;
                            case PlayerMovement.arrowRight:
                                if (RocketInSquare(players[playerNo].X + result, players[playerNo].Y))
                                {
                                    bounce = true;
                                }
                                else
                                {
                                    bounce = false;
                                }
                                if (result + players[playerNo].X <= MaxOnBoard)
                                {
                                    players[playerNo].X += result;
                                    Console.WriteLine("Player {0} is now at position ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                    if (bounce)
                                    {
                                        Console.WriteLine("Player {0} is making a bounce off of another player at ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                        result = 1; //??
                                    }
                                    else
                                    {
                                        Console.WriteLine("No Bounce!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Roll was too high! Player {0} will remain on their square ({1}.{2})", displayNum, players[playerNo].X, players[playerNo].Y);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (input == "2")
                {
                    Console.WriteLine("Choose a player to send back down!");
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (i == playerNo)
                        {
                            Console.WriteLine("Your current position is ({0}.{1})", players[i].X, players[i].Y);
                        }
                        Console.WriteLine("Player {0}'s current position is ({1}.{2})", i + 1, players[i].X, players[i].Y);
                    }

                    do
                    {
                        int numInput = 0;
                        Console.Write("Choose a player: ");
                        string inputPlayer = Console.ReadLine();

                        try
                        {
                            numInput = int.Parse(inputPlayer);
                        }
                        catch (FormatException)
                        {
                            Console.Write("Please enter a number from the list above: ");
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine("That submitted number was too long. Please try again.");
                        }
                        if (numInput > players.Length || numInput <= 0)
                        {
                            Console.WriteLine("There are currently {0} players in the game. Please choose a player!.", players.Length);
                            inputPlayer = Console.ReadLine();
                            numInput = int.Parse(inputPlayer);
                        }
                        else
                        {
                            Console.WriteLine("Okay! Sending player {0} back down.", numInput);
                            players[numInput - 1].X = 0;
                            Console.WriteLine("Player's {0} new position is ({1}.{2})", numInput, players[numInput - 1].X, players[numInput - 1].Y);
                            break;
                        }
                    } while (true);
                }
            }
            else
                Console.WriteLine("No Winner Yet!");
        }
        static void Main(string[] args)
        {
            ResetGame();
            MakeMoves();
            Console.ReadLine();
        }
    }
}
