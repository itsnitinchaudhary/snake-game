class Program
{
    static void Main()
    {
        Console.CursorVisible = false;

        int board_width = 30;
        int board_height = 15;

        // 0 = Empty, 1 = Snake, 2 = Food
        int[,] game_board = new int[board_height, board_width];

        // Snake body coordinates
        int[] snake_x_cordinate = new int[board_width * board_height];
        int[] snake_y_cordinate = new int[board_width * board_height];

        int snake_length = 3;

        // Initial snake
        snake_x_cordinate[0] = board_width / 2;
        snake_y_cordinate[0] = board_height / 2;

        snake_x_cordinate[1] = snake_x_cordinate[0] - 1;
        snake_y_cordinate[1] = snake_y_cordinate[0];

        snake_x_cordinate[2] = snake_x_cordinate[0] - 2;
        snake_y_cordinate[2] = snake_y_cordinate[0];

        int dx = 1;
        int dy = 0;

        int food_x_cordinate = 0;
        int food_y_cordinate = 0;

        int total_score = 0;
        bool is_game_over = false;

        Random random = new Random();

        // Place initial food
        placeFood();

        while (!is_game_over)
        {
            // Read keyboard input
            while (Console.KeyAvailable)
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

                    case ConsoleKey.Escape:
                        is_game_over = true;
                        break;
                }
            }

            if (is_game_over)
                break;

            // Calculate next head position
            int new_x_cordinate = snake_x_cordinate[0] + dx;
            int new_y_cordinate = snake_y_cordinate[0] + dy;

            // Check wall collision
            if (new_x_cordinate < 0 || new_x_cordinate >= board_width ||
                new_y_cordinate < 0 || new_y_cordinate >= board_height)
            {
                is_game_over = true;
                break;
            }

            bool is_food_eaten = new_x_cordinate == food_x_cordinate && new_y_cordinate == food_y_cordinate;

            // Check self collision.
            int check_self_collision = is_food_eaten ? snake_length : snake_length - 1;

            for (int i = 0; i < check_self_collision; i++)
            {
                if (snake_x_cordinate[i] == new_x_cordinate &&
                    snake_y_cordinate[i] == new_y_cordinate)
                {
                    is_game_over = true;
                    break;
                }
            }

            if (is_game_over)
                break;

            // Increase length when food is eaten
            if (is_food_eaten)
            {
                snake_length++;
                total_score++;
            }

            // Move the body from tail to head
            for (int i = snake_length - 1; i > 0; i--)
            {
                snake_x_cordinate[i] = snake_x_cordinate[i - 1];
                snake_y_cordinate[i] = snake_y_cordinate[i - 1];
            }

            // Set the new head position
            snake_x_cordinate[0] = new_x_cordinate;
            snake_y_cordinate[0] = new_y_cordinate;

            // Generate new food
            if (is_food_eaten)
            {
                if (snake_length == board_width * board_height)
                {
                    is_game_over = true;
                }
                else
                {
                    placeFood();
                }
            }

            // Clear the board
            Array.Clear(game_board, 0, game_board.Length);

            // Put snake on the board
            for (int i = 0; i < snake_length; i++)
            {
                game_board[snake_y_cordinate[i], snake_x_cordinate[i]] = 1;
            }

            // Put food on the board
            if (!is_game_over)
            {
                game_board[food_y_cordinate, food_x_cordinate] = 2;
            }

            // Draw the board
            Console.SetCursorPosition(0, 0);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("          S N A K E   G A M E");
            Console.ResetColor();

            Console.WriteLine($"  Score: {total_score}    Length: {snake_length}    Esc: Quit");

            // Top border
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("+" + new string('-', board_width * 2) + "+");

            for (int y = 0; y < board_height; y++)
            {
                Console.Write("|");

                for (int x = 0; x < board_width; x++)
                {
                    if (game_board[y, x] == 1)
                    {
                        if (x == snake_x_cordinate[0] && y == snake_y_cordinate[0])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("O ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("o ");
                        }
                    }
                    else if (game_board[y, x] == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("* ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(". ");
                    }
                }

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("|");
            }

            // Bottom border
            Console.WriteLine("+" + new string('-', board_width * 2) + "+");

            Console.ResetColor();

            Thread.Sleep(150);
        }

        Console.SetCursorPosition(0, board_height + 5);
        Console.CursorVisible = true;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Game Over!");
        Console.ResetColor();
        Console.WriteLine($"Final Score: {total_score}");
        //Console.WriteLine($"Final Length: {length}");

        // Local function to place food in an empty cell
        void placeFood()
        {
            do
            {
                food_x_cordinate = random.Next(0, board_width);
                food_y_cordinate = random.Next(0, board_height);

                bool onSnake = false;

                for (int i = 0; i < snake_length; i++)
                {
                    if (snake_x_cordinate[i] == food_x_cordinate &&
                        snake_y_cordinate[i] == food_y_cordinate)
                    {
                        onSnake = true;
                        break;
                    }
                }

                if (!onSnake)
                    break;

            } while (true);
        }
    }
}
