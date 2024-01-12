using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeTest
{
    internal class Position
    {
        public Position(int snakeCol, int snakeRow)
        {
            this.SnakeCol = snakeCol;
            this.SnakeRow = snakeRow;
            bodypartAge = 0;
        }
        private int snakeCol, snakeRow;
        private int bodypartAge;
        public int SnakeCol { get => snakeCol; set => snakeCol = value; }
        public int SnakeRow { get => snakeRow; set => snakeRow = value; }
        public int BodypartAge { get => bodypartAge; set => bodypartAge = value; }
    }
}
