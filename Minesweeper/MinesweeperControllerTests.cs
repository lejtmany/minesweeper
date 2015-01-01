using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using FluentAssertions.Common;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper
{
    [TestClass]
    public class MinesweeperTests
    {

        [TestMethod]
        public void TestGetSquare()
        {
            Board board = new Board(9, 9);
            BoardSquare testSquare = new BoardSquare(3,4);
            BoardSquare square = board.GetSquare(3, 4);
            square.Should().Equals(testSquare);
        }

        [TestMethod]
        public void TestGetSquareWithBadValue()
        {
            Board board = new Board(9, 9);
            Action a = () => board.GetSquare(-1, 4);
            a.ShouldThrow<ArgumentOutOfRangeException>();
        }
        
        [TestMethod]
        public void TestGetRandomSquaresHasRightAmountOfBombs()
        {
            Board board = new Board(9,9);
            var randomSquares =  board.GetRandomSquares();
            randomSquares.Should().HaveCount(board.amountOfBombs);           
        }

        [TestMethod]
        public void TestGetRandomSquaresIsDifferentEveryTime()
        {
            Board board = new Board(9,9);
            var randomSquares1 = board.GetRandomSquares();
            //delay to give time for random to make new seed
            Thread.Sleep(100);
            var randomSquares2 = board.GetRandomSquares();
            randomSquares1.Union(randomSquares2).Count().Should().NotBe(randomSquares1.Count());   
        }

        [TestMethod]
        public void TestThatBombsSeededInRightPlace()
        {
          Board board = new Board(9, 9);
          var randomSquares = board.GetRandomSquares();
          board.PlaceBombs(randomSquares);
          randomSquares.Should().OnlyContain(s => s.value == BoardSquare.BOMB);
        }

        [TestMethod]
        public void TestFillBoardValues()
        {
            Board board = new Board(9, 9);
            BoardSquare square;
            board.PlaceBombs(board.GetRandomSquares(2));
            board.FillBoardValues();
            square = board.GetSquare(1, 6);
            square.value.Should().Be(2);
        }

        [TestMethod]
        public void TestGetAdjacentSquares()
        {
            Board board = new Board(9, 9);
            BoardSquare[] testAdjacentSquares = { board.GetSquare(2, 2), board.GetSquare(2, 3), board.GetSquare(2, 4), board.GetSquare(3, 2), board.GetSquare(3, 4), board.GetSquare(4, 2), board.GetSquare(4, 3), board.GetSquare(4, 4) };
            List<BoardSquare> adjacentSquares = board.GetAdjacentSquares(board.GetSquare(3, 3));
            adjacentSquares.Should().BeSubsetOf(testAdjacentSquares);
        }

        [TestMethod]
        public void TestGetAdjacentSquaresInCorner()
        {
            Board board = new Board(9, 9);
            BoardSquare[] testAdjacentSquares = { board.GetSquare(0, 7), board.GetSquare(1, 7), board.GetSquare(1, 8)};
            List<BoardSquare> adjacentSquares = board.GetAdjacentSquares(board.GetSquare(0, 8));
            adjacentSquares.Should().BeSubsetOf(testAdjacentSquares);
        }

        [TestMethod]
        public void TestGetSquaresList()
        {
            Board board = new Board(9, 9);
            List<BoardSquare> squares = board.GetSquaresList();
            squares.Should().OnlyHaveUniqueItems();
            squares.Should().HaveCount(9 * 9);
        }

        [TestMethod]
        public void TestCountBombs()
        {
            Board board = new Board(9, 9);
            List<BoardSquare> testSquares = new List<BoardSquare>();
            BoardSquare bombSquare = new BoardSquare(0, 0);
            
            for (int i = 0; i < 5; i++)
            {
                bombSquare.value = BoardSquare.BOMB;
                testSquares.Add(bombSquare);
            }

            testSquares.Add(new BoardSquare(0,0));
            board.CountBombs(testSquares).Should().Be(5);
        }

        [TestMethod]
        public void TestCountBombsWhenNoBombs()
        {
            Board board = new Board(9, 9);
            List<BoardSquare> testSquares = new List<BoardSquare>();
            testSquares.Add(new BoardSquare(0, 0));
            board.CountBombs(testSquares).Should().Be(0);
        }

        [TestMethod]
        public void TestOpenZeroesWhenAllZeros()
        {
            Board board = new Board(9, 9);
            board.FillBoardSquares();
            board.OpenZeros(board.GetSquare(2, 2));
            var squareOpens =
                from square in board.GetSquaresList()
                select square.isOpen;
            squareOpens.Should().OnlyContain(s => s == true);
        }

        [TestMethod]
        public void TestOpenZerosWithOneBomb()
        {
             Board board = new Board(4, 4);
            board.FillBoardSquares();
            board.GetSquare(0, 0).value = BoardSquare.BOMB;
            board.OpenZeros(board.GetSquare(2, 2));
            var openCount =
                (from square in board.GetSquaresList()
                 where square.isOpen == true
                select square.isOpen).Count();
            openCount.Should().Be(15);
        }

        [TestMethod]
        public void TestOpenZerosWithMultipeBombs()
        {
            Board board = new Board(4, 4);
            board.FillBoardSquares();
            board.GetSquare(0, 0).value = BoardSquare.BOMB;
            board.GetSquare(0, 3).value = BoardSquare.BOMB;
            board.FillBoardValues();
            board.OpenZeros(board.GetSquare(2, 2));
            var openCount =
                (from square in board.GetSquaresList()
                 where square.isOpen == true
                 select square.isOpen).Count();
            openCount.Should().Be(12);
        }
    }
}
