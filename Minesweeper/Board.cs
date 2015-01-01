using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    class Board
    {
        public int amountOfBombs{get; private set;}
        public int width {get; private set;}
        public int height{get; private set;}
        public BoardSquare[,] squares{get; private set;}
        public bool[,] wasChecked;
        public bool gameOver { get; private set; }

        public Board(int width, int height)
        {
            this.width = width;
            this.height = height;
            amountOfBombs = 9;
            squares = new BoardSquare[width, height];
            wasChecked = new bool[width, height];
            FillBoardSquares();
            SeedBoard();
        }

        internal void FillBoardSquares()
        {
            for (int width = 0; width < squares.GetLength(0); width++)
                for (int height = 0; height < squares.GetLength(1); height++)
                    squares[width, height] = new BoardSquare(width, height);
        }

        public BoardSquare GetSquare(int x, int y)
        {
           //(0,0) is top left
            try
            {
                return squares[x, y];
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException("Given values for square were outside the range of the board");
            }
        }

        internal List<BoardSquare> GetSquaresList()
        {
            List<BoardSquare> squaresList = new List<BoardSquare>();
            for (int width = 0; width < squares.GetLength(0); width++)
                for (int height = 0; height < squares.GetLength(1); height++)
                    squaresList.Add(GetSquare(width, height));
            return squaresList;
        }

        internal void SeedBoard()
        {
            SeedBombs();
            FillBoardValues();
        }

        internal void SeedBombs()
        {
            List<BoardSquare> randomSquares = GetRandomSquares();
            PlaceBombs(randomSquares);
        }

        internal List<BoardSquare> GetRandomSquares()
        {
            Random generator = new Random();
            return GetRandomSquares(generator.Next());
        }

        internal List<BoardSquare> GetRandomSquares(int seed)
        {
            int randomX, randomY, randomSquareCounter = 0;
            bool[,] squares = new bool[width, height];
            var randomSquares = new List<BoardSquare>();
            Random generator = new Random(seed);

            while (randomSquareCounter < amountOfBombs)
            {
                randomX = generator.Next() % width;
                randomY = generator.Next() % height;
                if (!squares[randomX, randomY])
                {
                    squares[randomX, randomY] = true;
                    randomSquares.Add(GetSquare(randomX, randomY));
                    randomSquareCounter++;
                }
            }
            return randomSquares;
        }


        internal void PlaceBombs(List<BoardSquare> randomSquares)
        {
           foreach(var square in randomSquares)
               square.value = BoardSquare.BOMB;                 
        }

        internal void FillBoardValues()
        {
            List<BoardSquare> squares = GetSquaresList();
            foreach(BoardSquare square in squares)
                {
                    if (square.value != BoardSquare.BOMB)
                    {
                        FillSquareValue(square);
                    }
                }
        }

        private List<BoardSquare> FillSquareValue(BoardSquare square)
        {
            var adjacentSquares = GetAdjacentSquares(square);
            square.value = CountBombs(adjacentSquares);
            return adjacentSquares;
        }

        

        internal List<BoardSquare> GetAdjacentSquares(BoardSquare square)
        {
            List<BoardSquare> adjacentSquares = new List<BoardSquare>();
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                    TryAddAdjacentSquare(square, adjacentSquares, i, j);
            adjacentSquares.Remove(square);
            return adjacentSquares;
        }

        private void TryAddAdjacentSquare(BoardSquare square, List<BoardSquare> adjacentSquares, int i, int j)
        {
            try
            {
                adjacentSquares.Add(GetSquare(square.X + i, square.Y + j));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                //Do nothing. Don't add square.
            }
        }

        
        internal int CountBombs(List<BoardSquare> adjacentSquares)
        {
            int bombCount = 0;
            foreach(BoardSquare square in adjacentSquares){
                if (square.value == BoardSquare.BOMB)
                    bombCount++;
            }
            return bombCount;
        }

        internal void OpenZeros(BoardSquare square)
        {           
            if (square.value == 0)
            {
                var nonOpenButtons = new List<BoardSquare>();
                foreach (var adjacentSquare in GetAdjacentSquares(square))
                {
                    if (!wasChecked[adjacentSquare.X, adjacentSquare.Y])
                        nonOpenButtons.Add(adjacentSquare);
                    wasChecked[adjacentSquare.X, adjacentSquare.Y] = true;
                }
                foreach (var nonOpenSquare in nonOpenButtons)
                {
                    ClickSquare(nonOpenSquare);
                }
            }                              
            }              

        internal void ClickSquare(BoardSquare square)
        {
            if (square.value == BoardSquare.BOMB)
                GameOver();
            else if (square.value == BoardSquare.FLAG || square.isOpen)
                return;
            else
            {
                square.isOpen = true;
                OpenZeros(square);
             //   ResetCheckedArray();
            }
        }

        private void ResetCheckedArray()
        {
            for (int i = 0; i < wasChecked.GetLength(0); i++)
                for (int j = 0; j < wasChecked.GetLength(1); j++)
                    wasChecked[i, j] = false;
        }

        private void GameOver()
        {
            gameOver = true;
        }

        public string ToString()
        {
            String s = "";
            for (int height = 0; height < squares.GetLength(0); height++)
            {
                s += "\n";
                for (int width = 0; width < squares.GetLength(1); width++)
                    s +=   String.Format(" {0} ",(GetSquare(width, height).value == BoardSquare.BOMB) ? "X" : GetSquare(width, height).value.ToString());
            }
            return s;
        }
    }
}
