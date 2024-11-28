using System;
using System.Threading;

namespace SnakeGame 
{
    class Start
    {
        static void Main()
        {
            Console.CursorVisible = false;

            CreateGame game = new CreateGame();
            game.StartGame();
        }
    }

    class CreateGame
    {
        private int width = 20;
        private int height = 20;
        private int score;

        public void StartGame()
        {
            score = 0;

            while (true) 
            {
                Input();
                Thread.Sleep(300);
            }
        }

        private void Input()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                        Console.WriteLine("UP");
                        break;
                    case ConsoleKey.A:
                        Console.WriteLine("LEFT");
                        break;
                    case ConsoleKey.D:
                        Console.WriteLine("RIGHT");
                        break;
                    case ConsoleKey.S:
                        Console.WriteLine("DOWN");
                        break;
                    case ConsoleKey.LeftArrow:
                        Console.WriteLine("LEFT");
                        break;
                    case ConsoleKey.UpArrow:
                        Console.WriteLine("UP");
                        break;
                    case ConsoleKey.DownArrow:
                        Console.WriteLine("DOWN");
                        break;
                    case ConsoleKey.RightArrow:
                        Console.WriteLine("RIGHT");
                        break;
                }
            }
        }
    }

    class Snake
    {
        // Snake class implementation
    }
}