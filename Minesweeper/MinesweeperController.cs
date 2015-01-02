using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
   class MinesweeperController
    {        
        internal Board board;
        private Minesweeper gui;
        
        public MinesweeperController(Minesweeper gui, Board board)
        {
            this.board = board;
            this.gui = gui;
            SetUpButtonHandlers();
        }

        private void SetUpButtonHandlers()
        {
            for(int i = 0; i < gui.buttonArray.GetLength(0); i++)
                for (int j = 0; j < gui.buttonArray.GetLength(0); j++)
                {
                    gui.buttonArray[i, j].Click += button_Click;
                }
        }

        private void button_Click(object sender, EventArgs e)
        {
            MinesweeperButton button = (MinesweeperButton) sender;
            Point buttonLocation = button.Coordinates;
            board.ClickSquare(board.GetSquare(buttonLocation.X, buttonLocation.Y));
            updateView();
        }

        private void updateView()
        {
            BoardSquare square;
            for(int i = 0; i < board.Width; i++)
               for (int j = 0; j < board.Height; j++)
               {
                   square = board.GetSquare(i, j);
                   if (square.isOpen)
                   {
                       gui.buttonArray[i, j].Text = square.value.ToString();
                       gui.buttonArray[i, j].Enabled = false;
                   }
               } 
        }

    }
}
