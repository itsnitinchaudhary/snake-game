
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        int board_width = 20;
        int board_height = 10;

        // 0 = Empty, 1 = Snake, 2 = Food
        int[,] board = new int[board_height, board_width];

        int snake_X = board_width / 2;
        int snake_Y = board_height / 2;

        int food_X = 5;
        int food_Y = 3;

        int dx = 1;
        int dy = 0;

        bool gameOver = false;
        int score = 0;

        Console.CursorVisible = false;

        while (!gameOver)
        {
            // Read keyboard input
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (dy != 1)
                        {
                            dx = 0;
                            dy = -1;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (dy != -1)
                        {
                            dx = 0;
                            dy = 1;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (dx != 1)
                        {
                            dx = -1;
                            dy = 0;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (dx != -1)
                        {
                            dx = 1;
                            dy = 0;
                        }
                        break;
                }
            }

            // Move the snake
            snake_X += dx;
            snake_Y += dy;

            // Check wall collision
            if (snake_X < 0 || snake_X >= board_width ||
                snake_Y < 0 || snake_Y >= board_height)
            {
                gameOver = true;
                break;
            }

            // Clear the board
            Array.Clear(board, 0, board.Length);

            // Place snake and food
            board[snake_Y, snake_X] = 1;
            board[food_Y, food_X] = 2;

            // Eat food
            if (snake_X == food_X && snake_Y == food_Y)
            {
                score++;

                food_X = new Random().Next(0, board_width);
                food_Y = new Random().Next(0, board_height);
            }

            // Draw the board
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("SNAKE GAME | Score: " + score);

            for (int y = 0; y < board_height; y++)
            {
                for (int x = 0; x < board_width; x++)
                {
                    if (board[y, x] == 1)
                        Console.Write("O ");
                    else if (board[y, x] == 2)
                        Console.Write("* ");
                    else
                        Console.Write(". ");
                }

                Console.WriteLine();
            }

            Thread.Sleep(250);
        }

        Console.CursorVisible = true;
        Console.WriteLine();
        Console.WriteLine("Game Over!");
        Console.WriteLine("Final Score: " + score);
    }
}
