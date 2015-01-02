using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    class BoardSquare
    {
        public bool isOpen{get; internal set;}
        public int value { get; internal set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsFlag { get; set; }
        public readonly static int BOMB = -1;        

        public BoardSquare(int X, int Y)
        {
            this.X = X;
            this.Y = Y;

        }
    }
}
