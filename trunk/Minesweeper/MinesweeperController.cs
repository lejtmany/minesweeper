using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{

    class MinesweeperController
    {

        internal Board board;
        private Minesweeper gui;
        readonly Image bombImage;
        readonly Image flagImage;
        private int BombsRemaining;
        private List<BoardSquare> SquareList;


        public MinesweeperController(Minesweeper gui, Board board)
        {
            this.board = board;
            this.gui = gui;
            BombsRemaining = board.AmountOfBombs;
            SquareList = board.GetSquaresList();
            bombImage = new Bitmap(new Bitmap(@"..\..\bomb-icon.png"), gui.buttonArray[0, 0].Width - 5, gui.buttonArray[0, 0].Height - 5);
            flagImage = new Bitmap(new Bitmap(@"..\..\flag-icon.png"), gui.buttonArray[0, 0].Width - 5, gui.buttonArray[0, 0].Height - 5);
            SetUpButtonHandlers();
            gui.setLabelText("Bombs Remaining: " + BombsRemaining);
        }

        private void SetUpButtonHandlers()
        {
            foreach (var square in SquareList)
            {
                gui.buttonArray[square.X, square.Y].Click += button_Click;
                gui.buttonArray[square.X, square.Y].MouseUp += MinesweeperController_MouseUp;
            }
        }

        private void MinesweeperController_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (board.gameOver)
                    return;
                BoardSquare modelSquare = GetSelectedButton(sender);
                UpdateBombCounter(modelSquare);
                ToggleFlag(modelSquare);
                UpdateFlagImage(modelSquare);
                UpdateBombLabel();
            }
        }

        private BoardSquare GetSelectedButton(object sender)
        {
            MinesweeperButton button = (MinesweeperButton)sender;
            Point buttonLocation = button.Coordinates;
            return board.GetSquare(buttonLocation.X, buttonLocation.Y);
        }

        private static void ToggleFlag(BoardSquare modelSquare)
        {
            modelSquare.IsFlag = !modelSquare.IsFlag;
        }

        private void UpdateBombCounter(BoardSquare modelSquare)
        {
            if (modelSquare.IsFlag)
                BombsRemaining++;
            else
                BombsRemaining--;
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (board.gameOver)
                return;
            BoardSquare modelSquare = GetSelectedButton(sender);
            board.ClickSquare(modelSquare);
            UpdateView(modelSquare);
        }

        private void UpdateView(BoardSquare selectedSquare)
        {
            if (board.gameOver)
                DisplayGameOver(selectedSquare);
            else
                foreach (var square in SquareList)
                {
                    UpdateSquareImage(square);
                }
        }

        private void UpdateSquareImage(BoardSquare square)
        {
            SetButtonImage(square, null);
            UpdateFlagImage(square);
            if (square.isOpen)
            {
                OpenButton(square);
            }
        }

        private void UpdateFlagImage(BoardSquare square)
        {
            if (square.IsFlag)
                SetButtonImage(square, flagImage);
            else
                SetButtonImage(square, null);

        }

        private void DisplayGameOver(BoardSquare selectedSquare)
        {
            gui.buttonArray[selectedSquare.X, selectedSquare.Y].BackColor = Color.Red;
            RevealBombs();
        }

        private void RevealBombs()
        {
            foreach (var square in SquareList)
                if (square.value == BoardSquare.BOMB)
                    SetButtonImage(square, bombImage);
        }

        private void UpdateBombLabel()
        {
            gui.setLabelText("Bombs Remaining: " + BombsRemaining);
        }

        private void OpenButton(BoardSquare square)
        {
            gui.buttonArray[square.X, square.Y].Text = square.value.ToString();
            gui.buttonArray[square.X, square.Y].Enabled = false;
        }

        private void SetButtonImage(BoardSquare square, Image image)
        {
            gui.buttonArray[square.X, square.Y].Image = image;
        }

    }
}
