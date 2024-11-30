using Microsoft.Data.Sqlite;

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
            // Program Start
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
        private bool deid;
        private ScoreDatabase scoreDatabase;

        public void StartGame()
        {
            snake = new Snake(width / 2, height / 2);
            food = new Food(width, height);
            scoreDatabase = new ScoreDatabase();
            score = 0;
            deid = false;

            // Main Game Loop
            while (true) 
            {
                if (deid) 
                {
                    Console.Clear();
                    Console.WriteLine("You are deid");
                    Console.WriteLine($"Your final amount of 🟥 is {score}");
                    scoreDatabase.AddScore(score);
                    DisplayScores();
                    break;
                } 
                else 
                {
                    Input();
                    snake.Move();
                    Draw();
                    GameLogic();
                    Thread.Sleep(200);
                }
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
            Console.WriteLine($"🟥: {score}");
        }

        public void GameLogic()
        {
            // If snake eat food
            if (snake.SnakePos() == (food.X, food.Y))
            {
                snake.AddToSnakeTail();
                food.GenerateNewPosition(width - 2, height - 2);
                score++;
            }
            
            // If snake eats its self
            if 
            (
                snake.EatSelf() ||
                snake.SnakePos().Item1 == 0 ||
                snake.SnakePos().Item1 == width ||
                snake.SnakePos().Item2 == 0 || 
                snake.SnakePos().Item2 == height
            )
            {
                deid = true;
            }
        }

        private void DisplayScores()
        {
            var scores = scoreDatabase.GetScores();
            Console.WriteLine("High amount of 🟥:");
            foreach (var (score, date) in scores)
            {
                Console.WriteLine($"🟥 {score} - {date}");
            }
        }
    }

    class Snake
    {
       private Queue<(int, int)> board;
       private Direction direction;
       private bool grow;

        public Snake(int startX, int startY)
        {
            board = new Queue<(int, int)>();
            board.Enqueue((startX, startY));
            direction = Direction.RIGHT; // Initial direction (moving right)
            grow = false;
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
            // Converts Direction enum value to -(X, Y) which will be taken away from snake head
            // to get new snake head possiton.
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
            
            if (!grow) board.Dequeue();
            else grow = false;
        }

        public bool IsSnakePosition(int x, int y)
        {
            return board.Contains((x, y));
        }

        public (int, int) SnakePos()
        {
            return board.Last();
        }

        public void AddToSnakeTail()
        {
            grow = true;
        }

        public bool EatSelf()
        {
            // Checks to see if there are two instances of the same (X, Y) of snake
            int count = board.Count(item => item == SnakePos());

            if (count > 1) return true;
            else return false;
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
            // Checks to make sure apple is not in the initial starting possition of snake
            do 
            {
                X = random.Next(0, width);
                Y = random.Next(0, height);
            }
            while (X != width / 2 && Y != height / 2);
        }
    }

    public class ScoreDatabase
    {
        private string connectionString = "Data Source=scores.db";

        public ScoreDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS Scores (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Score INTEGER NOT NULL,
                        Date TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
            }
        }

        public void AddScore(int score)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO Scores (Score, Date)
                    VALUES ($score, $date);
                ";
                command.Parameters.AddWithValue("$score", score);
                command.Parameters.AddWithValue("$date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }

        // Lists scores from Least amount of apples to most and returns the list
        public List<(int Score, string Date)> GetScores()
        {
            var scores = new List<(int Score, string Date)>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT Score, Date
                    FROM Scores
                    ORDER BY Score DESC;
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var score = reader.GetInt32(0);
                        var date = reader.GetString(1);
                        scores.Add((score, date));
                    }
                }
            }

            return scores;
        }
    }
}