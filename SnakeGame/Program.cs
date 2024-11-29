public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

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
        private Snake snake;
        private Food food;

        public void StartGame()
        {
            snake = new Snake(width / 2, height / 2);
            food = new Food(width, height);
            score = 0;

            while (true) 
            {
                Input();
                snake.Move();
                Draw();
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
                        snake.ChangeDirection(Direction.UP);
                        break;
                    case ConsoleKey.A:
                        snake.ChangeDirection(Direction.LEFT);
                        break;
                    case ConsoleKey.D:
                        snake.ChangeDirection(Direction.RIGHT);
                        break;
                    case ConsoleKey.S:
                        snake.ChangeDirection(Direction.DOWN);
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.ChangeDirection(Direction.LEFT);
                        break;
                    case ConsoleKey.UpArrow:
                        snake.ChangeDirection(Direction.UP);
                        break;
                    case ConsoleKey.DownArrow:
                        snake.ChangeDirection(Direction.DOWN);
                        break;
                    case ConsoleKey.RightArrow:
                        snake.ChangeDirection(Direction.RIGHT);
                        break;
                }
            }
        }

        public void Draw()
        {
            Console.Clear();
            for (int i = 0; i < width; i++) Console.Write("🟧");

            Console.WriteLine();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j == 0 || j == width - 1) Console.Write("🟧");
                    else if (snake.IsSnakePosition(j, i)) Console.Write("🟩");
                    else if (food.X == j && food.Y == i) Console.Write("🟥");
                    else Console.Write("🟫");
                }

                Console.WriteLine();
            }

            for (int i = 0; i < width; i++) Console.Write("🟧");

            Console.WriteLine();
            Console.WriteLine($"Score: {score}");
        }
    }

    class Snake
    {
       private Queue<(int, int)> board;
       private Direction direction;

        public Snake(int startX, int startY)
        {
            board = new Queue<(int, int)>();
            board.Enqueue((startX, startY));
            direction = Direction.RIGHT; // Initial direction (moving right)
        }

       public void ChangeDirection(Direction newDirection) 
       {
            // Prevent the snake from reversing
            if ((direction == Direction.UP && newDirection != Direction.DOWN) ||
                (direction == Direction.DOWN && newDirection != Direction.UP) ||
                (direction == Direction.LEFT && newDirection != Direction.RIGHT) ||
                (direction == Direction.RIGHT && newDirection != Direction.LEFT))
            {
                direction = newDirection;
            }
       }

        public void Move()
        {
            (int x, int y) = board.Last();
            (int dx, int dy) = direction switch
            {
                Direction.UP => (0, -1),
                Direction.DOWN => (0, 1),
                Direction.LEFT => (-1, 0),
                Direction.RIGHT => (1, 0),
                _ => (0, 0)
            };

            (int newX, int newY) = (x + dx, y + dy);

            board.Enqueue((newX, newY));
            board.Dequeue();

            Console.WriteLine(board);
        }

        public bool IsSnakePosition(int x, int y)
        {
            return board.Contains((x, y));
        }
    }

    public class Food
    {
        private Random random;
        public int X { get; private set; }
        public int Y { get; private set; }

        public Food(int width, int height)
        {
            random = new Random();
            GenerateNewPosition(width, height);
        }

        public void GenerateNewPosition(int width, int height)
        {
            X = random.Next(0, width);
            Y = random.Next(0, height);
        }
    }
}