using Project.BattleShip.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.BattleShip
{
    class Program
    {
        static string s;
        static string columns => "ABCDEFGHIJK";

        static void Draw(int width, List<Square> points)
        {
            for (int x = width; x > 0; x--)
            {
                s += "  " + ((x) < 10 ? "0" : "") + (x) + "   ";

                for (int y = 1; y <= width; y++)
                {
                    var item = points.Where(c => c.Row == x && c.Column == columns[y - 1]).FirstOrDefault();
                    if (item.ShipIdentifier > 0 && item.Occupied == Status.Hit)
                    {
                        s += "" + "X ";
                    }
                    else if (item.ShipIdentifier == 0 && item.Occupied == Status.Hit)
                    {
                        s += "" + "? ";
                    }
                    else
                    {
                        s += "" + $"0 ";
                    }
                }

                s += "\n";
            }
            Console.WriteLine(s);
            s = "";
            s += "   ";

            for (int x = 0; x < width; x++)
            {
                s += columns[x] + " ";
            }
            Console.WriteLine("    " + s);
            s = "";
        }


        static void Main(string[] args)
        {
            newGame:
            var board = new BoardDimension(10, 10);
            Game game = new Game(board);
            game.RaiseEvent += notifyUser;

            menu:
            Console.WriteLine("Battleship v1");
            Console.WriteLine();

            Draw(10, game.coordinates);

            Console.WriteLine("\n Please enter your move e.g A5 and press enter ?");
            string coordinate = Console.ReadLine();

            char[] keys = coordinate.Trim().ToCharArray();

            if (keys.Length > 0 && game.axis.Contains(keys[0]))
            {
                if (int.TryParse(coordinate.Substring(1, coordinate.Length - 1), out int row))
                {
                    var result = game.TakeShot(keys[0], row);

                    if (result == Shot.Hit)
                    {
                        Console.Write($"\n Shot hit {keys[0].ToString()} ");
                        Console.ReadLine();
                    }
                    else if (result == Shot.Miss)
                    {
                        Console.Write($"\n Shot missed {keys[0].ToString()} ");
                        Console.ReadLine();
                    }
                    else if (result == Shot.Wins)
                    {
                        Console.WriteLine("\n Would you like to play again ? Y/N");
                        var response = Console.ReadKey(false).Key;
                        if (response == ConsoleKey.Y)
                        {
                            Console.Clear();
                            goto newGame;
                        }
                        else if (response == ConsoleKey.N)
                        {
                            Environment.Exit(0);
                        }
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.Clear();
                    goto menu;
                }
            }
            Console.Clear();
            goto menu;
        }

        private static void notifyUser(string notice)
        {
            Console.Write($"\n {notice}");
        }
    }
}