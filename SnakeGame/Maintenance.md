# Maintenance

Bellow is a list of classes and functions and there docs. 
Since there is only one namespace I will not include it bellow.
SnakeGame namespace contains all the classes for the program.

# ENUMS
## Direction

Direction contains four constant values. 

    UP,
    DOWN,
    LEFT,
    RIGHT

These map to w, a, s, d and Up, Down, Left, Right arrows on your keyboard.

# Classes
## Start

```
static void Main()
{
    Console.CursorVisible = false;

    CreateGame game = new CreateGame();
    game.StartGame();
}
```

Class start contains the function Main this is the entery point of the application.
This creates a new instance of the CreateGame class.

## CreateGame
### State

```
private int width = 20;
private int height = 20;
private int score;
private Snake snake;
private Food food;
private bool deid;
private ScoreDatabase scoreDatabase;
```

These are the width and height of the board, user score (apples), Instances of snake and food class,
user deid state and database class instance.

### StartGame

```
public void StartGame()
{
    snake = new Snake(width / 2, height / 2);
    food = new Food(width, height);
    scoreDatabase = new ScoreDatabase();
    score = 0;
    deid = false;

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
```

This function contains the main game loop which calls all the main
functions in the program.

### Input

```
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
```

This takes the inputs from the user from the console and converts it
into direction enum values which can be understood by the rest of the program.

### Draw

```
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
```

This function draws the board this includes the walls, apples and the snake.

### GameLogic

```
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
```

This function contains the main logic for the game
and controlls whether the snake is dead or not.

## Snake
### State

```
private Queue<(int, int)> board;
private Direction direction;
private bool grow;
```

board is a Queue of X, Y cowardenents this is the possition of each
part of the snake on the board.

direction is Direction enum

Grow tells the game loop weather to not destroy the tail of the snake
for one tick.

### ChangeDirection

```
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
```

This function first makes sure to not let the snake to a 180 deg turn.
Then it sets the new direction of the snake.

### Move

```
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
    
    if (!grow) board.Dequeue();
    else grow = false;
}
```

This converts the Direction enum value in direction to a -(X, Y) value.
These values will be subtracted from the head of the snake to get the new 
possition of the snakes head.

Then it checks grow and either takes one off the end of the snake or it waits
untill next tick which grows the snake.

### Helper Functions

```
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
    int count = board.Count(item => item == SnakePos());

    if (count > 1) return true;
    else return false;
}
```

These functions are here to help other classes because snake state is private to
Snake class.

## Food
### State

```
private Random random;
public int X { get; private set; }
public int Y { get; private set; }
```

These are the X, and Y values of the apple.

### GenerateNewPosition

```
public void GenerateNewPosition(int width, int height)
{
    do 
    {
        X = random.Next(0, width);
        Y = random.Next(0, height);
    }
    while (X != width / 2 && Y != height / 2);
}
```

The X and Y values are checked to make sure they dont spawn
on the snakes inital possiton.

## ScoreDatabase

```
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
```

This code exacutes the SQL which is needed to create the database.
Add score contains SQL command to add score to the database.
List lists out all the scores in the database by the highest amout of apples.
