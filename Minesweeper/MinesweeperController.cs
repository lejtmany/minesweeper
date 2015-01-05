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
        

        public MinesweeperController(Minesweeper gui, Board board)
        {
            this.board = board;
            this.gui = gui;
            BombsRemaining = board.AmountOfBombs;
            bombImage = new Bitmap(new Bitmap(@"..\..\bomb-icon.png"), gui.buttonArray[0, 0].Width - 5, gui.buttonArray[0, 0].Height - 5);
            flagImage = new Bitmap(new Bitmap(@"..\..\flag-icon.png"), gui.buttonArray[0, 0].Width - 5, gui.buttonArray[0, 0].Height - 5);
            SetUpButtonHandlers();
            gui.setLabelText("Bombs Remaining: " + BombsRemaining);
        }

        private void SetUpButtonHandlers()
        {
            for (int i = 0; i < gui.buttonArray.GetLength(0); i++)
                for (int j = 0; j < gui.buttonArray.GetLength(0); j++)
                {
                    gui.buttonArray[i, j].Click += button_Click;
                    gui.buttonArray[i, j].MouseUp += MinesweeperController_MouseUp;
                }
        }

        private void MinesweeperController_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (board.gameOver)
                    return;
                MinesweeperButton button = (MinesweeperButton)sender;
                Point buttonLocation = button.Coordinates;
                BoardSquare modelSquare = board.GetSquare(buttonLocation.X, buttonLocation.Y);
                if (modelSquare.IsFlag)
                    BombsRemaining++;
                else
                    BombsRemaining--;
                modelSquare.IsFlag = !modelSquare.IsFlag;
                updateView(modelSquare);
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (board.gameOver)
                return;
            MinesweeperButton button = (MinesweeperButton)sender;
            Point buttonLocation = button.Coordinates;
            BoardSquare modelSquare = board.GetSquare(buttonLocation.X, buttonLocation.Y);
            board.ClickSquare(modelSquare);
            updateView(modelSquare);
        }

        private void updateView(BoardSquare selectedSquare)
        {
            BoardSquare square;

            if (selectedSquare.value == BoardSquare.BOMB && board.gameOver)
                gui.buttonArray[selectedSquare.X, selectedSquare.Y].BackColor = Color.Red;

            for (int i = 0; i < board.Width; i++)
                for (int j = 0; j < board.Height; j++)
                {
                    gui.buttonArray[i, j].Image = null;
                    square = board.GetSquare(i, j);
                    if (board.gameOver)
                    {
                        if (square.value == BoardSquare.BOMB)
                        {
                            gui.buttonArray[i, j].Image = bombImage;                           
                        }

                    }
                    if (square.IsFlag)
                        gui.buttonArray[i, j].Image = flagImage;
                    
                    if (square.isOpen)
                    {
                        gui.buttonArray[i, j].Text = square.value.ToString();
                        gui.buttonArray[i, j].Enabled = false;
                    }

                    gui.setLabelText("Bombs Remaining: " + BombsRemaining);
                }
        }

    }
}
