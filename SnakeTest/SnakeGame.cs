using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace SnakeTest
{
    internal class SnakeGame
    {
        public void SetDirection()
        {
            _previousDirection = Direction;
            Direction = Console.ReadKey().Key;
            
        }
        private void GameOverScreen()
        {
            Console.SetCursorPosition(_map.Width+1, _map.Height+4);
            Console.WriteLine("Game over! Press any button to play again, n to exit.");            
        }
        public void InitializeGame(Map map)
        {
            _interval = 15;
            _limit = 15;
            Console.Clear();
            map.SeeMap();
            SpawnApple(map);
            ScoreBoard(map);
            _map = map;
            _tickTimer = new Timer(_interval);
            _tickTimer.Elapsed += OnTimedEvent;
            _tickTimer.AutoReset = true;
            _tickTimer.Enabled = true;
        }
        private void ScoreBoard(Map map)
        {
            Console.SetCursorPosition(map.Width + 6, 4);
            Console.WriteLine("+-----------------+");
            Console.SetCursorPosition(map.Width + 6, 5);
            Console.WriteLine($"|     Score: {_score}    |");
            Console.SetCursorPosition(map.Width + 6, 6);
            Console.WriteLine("+-----------------+");
            //aligned to the map
        }
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            _tickTime++;
            if (_tickTime > _limit)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(_interval);
                DrawSnake();
                Position current = new Position(_col, _row);
                _snakeBody.Add(current);
                AgeAll();
                CheckAge(_score);
                if (IsAppleEaten())
                {
                    SpawnApple(_map);
                    UpdateScoreboard();
                }
                if (GameOver(_map))
                {
                    _tickTimer.Stop();
                    GameOverScreen();
                }
                _tickTime = 0;
                _setDirection = false;
            }
        }
        private void DrawSnake()
        {
            if (!_setDirection)
            {
                switch (Direction)
                {
                    case ConsoleKey.UpArrow:
                        _row--;
                        break;
                    case ConsoleKey.DownArrow:
                        _row++;
                        break;
                    case ConsoleKey.RightArrow:
                        _col++;
                        break;
                    case ConsoleKey.LeftArrow:
                        _col--;
                        break;
                    default:
                        switch (_previousDirection)
                        {
                            case ConsoleKey.UpArrow:
                                _row--;
                                break;
                            case ConsoleKey.DownArrow:
                                _row++;
                                break;
                            case ConsoleKey.RightArrow:
                                _col++;
                                break;
                            case ConsoleKey.LeftArrow:
                                _col--;
                                break;
                        }
                        break;
                }
                _setDirection = true;
            }
            Console.SetCursorPosition(_col, _row);
            Console.WriteLine("o");
            if (_snakeBody.Count > 1)
            {
                Position secondPart = new Position(_snakeBody[_snakeBody.Count - 2].SnakeCol, _snakeBody[_snakeBody.Count - 2].SnakeRow); //the first non-head bodypart
                Console.SetCursorPosition(secondPart.SnakeCol, secondPart.SnakeRow);
                Console.WriteLine("O");
            }
        }
        public bool IsAppleEaten()
        {
            if (_appleCol == Head.SnakeCol && _appleRow == Head.SnakeRow)
            {
                _score++;
                FreshenAll();
                if (_interval > 1 || _limit>1)
                {
                    _interval--;
                    _limit--;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SpawnApple(Map map)
        {
            Random r = new Random();
            int appleCol = 0;
            int appleRow = 0;
            do
            {
                appleCol = r.Next(4, map.Width - 1); //fixed position relative to the map
                appleRow = r.Next(4, map.Height - 1);
                if (IsFree(appleCol, appleRow))
                {
                    _appleCol = appleCol;
                    _appleRow = appleRow;
                    Console.SetCursorPosition(_appleCol, _appleRow);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("X");
                    Console.ResetColor();
                }
            } while (!IsFree(appleCol, appleRow));
        }
        public void AgeAll()
        {
            foreach (Position item in _snakeBody)
            {
                item.BodypartAge++;
            }
        }
        private void CheckAge(int score)
        {
            if (Tail.BodypartAge - 1 > score)
            {
                Console.SetCursorPosition(Tail.SnakeCol, Tail.SnakeRow);
                Console.WriteLine(" ");
                _snakeBody.Remove(Tail);
            }
        }

        public bool IsFree(int appleCol, int appleRow)
        {
            foreach (Position item in _snakeBody)
            {
                if (item.SnakeCol == appleCol && item.SnakeRow == appleRow)
                {
                    return false;
                }
            }
            return true;
        }

        public void FreshenAll()
        {
            foreach (Position item in _snakeBody)
            {
                item.BodypartAge--;
            }
        }

        private void UpdateScoreboard()
        {
            if (_score<9)
            {
                Console.SetCursorPosition(_map.Width + 6, 5);
                Console.WriteLine($"|     Score: {_score}    |");
            }
            else
            {
                Console.SetCursorPosition(_map.Width + 6, 5);
                Console.WriteLine($"|     Score: {_score}   |");
            }
          
        }

        public bool GameOver(Map mapsize)
        {
            return CrossedSnake() || ReachedBoundary(_map);
        }

        private bool CrossedSnake()
        {
            for (int i = 0; i < _snakeBody.Count - 1; i++)
            {
                if (_snakeBody[i].SnakeRow == Head.SnakeRow && _snakeBody[i].SnakeCol == Head.SnakeCol)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(Head.SnakeCol, Head.SnakeRow);
                    Console.WriteLine("o");
                    Console.ResetColor();
                    return true;
                }
            }
            return false;
        }

        private bool ReachedBoundary(Map mapsize)
        {
            if (ReachedTopOrBottomWall(mapsize) || ReachedRightOrLeftWall(mapsize))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(Head.SnakeCol, Head.SnakeRow);
                Console.WriteLine("o");
                Console.ResetColor();
                return true;
            }
            return false;
        }
     
        private bool ReachedTopOrBottomWall(Map map)
        {
            return (Head.SnakeRow < 4 || Head.SnakeRow -1 > map.Height);
        }
        private bool ReachedRightOrLeftWall(Map map)
        {
            return (Head.SnakeCol < 4 || Head.SnakeCol -1 > map.Width);
        }
     
        /* private void Countdown( Map map)
          {
              switch (_countDown)
              {
                  default:
                      Console.WriteLine("  ");
                      break;
                  case 2:
                      Console.SetCursorPosition(map.Width / 2, map.Height / 2);
                      Console.WriteLine("Ready...");
                      break;
                  case 1:
                      Console.SetCursorPosition(map.Width / 2, map.Height / 2);
                      Console.WriteLine("Set..");
                      break;
                  case 0:
                      Console.SetCursorPosition(map.Width / 2, map.Height / 2);
                      Console.WriteLine("Go...");
                      break;
              }
          }*/
        public ConsoleKey Direction = ConsoleKey.RightArrow;
        private ConsoleKey _previousDirection = ConsoleKey.RightArrow;
        public bool anotherGame = true;
        private static Timer _tickTimer;
        private static int _interval = 15;
        private static int _limit = 15;
        private int _tickTime = 0;
        private int _score = 0;
        private int _highScore = 0;
        private int _appleCol;
        private int _appleRow;
        private bool _setDirection = false;
        private Map _map;
        private List<Position> _snakeBody = new List<Position>();
        private Position Tail { get => _snakeBody.FirstOrDefault(); }
        private Position Head { get => _snakeBody.LastOrDefault(); }
        //  private static int _countDown = 3;
        private int _row = 5;
        private int _col = 5;

    }
}
