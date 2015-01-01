using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
     partial class Minesweeper : Form
    {
        private Board board;
        private Size buttonSize = new Size(25, 25);
        public MinesweeperButton[,] buttonArray {get; private set;}
        
         public Minesweeper(Board board)
        {
            this.board = board;
            buttonArray = new MinesweeperButton[board.width, board.height];
            InitializeComponent();       
            this.SuspendLayout();
            AddButtons();
            this.AutoSize = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ResumeLayout(true);          
        }

     
        private void AddButtons()
        {
            MinesweeperButton button;
            
            for(int i = 0; i < board.height; i++)
                for (int j = 0; j < board.width; j++)
                {
                    buttonArray[j, i] = new MinesweeperButton();
                    buttonArray[j, i].Location = new System.Drawing.Point(i * buttonSize.Height, j * buttonSize.Width);
                    buttonArray[j, i].Name = "button" + (j + (i * j));
                    buttonArray[j, i].Coordinates = new Point(j, i);
                    buttonArray[j, i].Size = buttonSize;
                    buttonArray[j, i].UseVisualStyleBackColor = false;
                    this.Controls.Add(buttonArray[j, i]);                    
                }

        }

       
        private void Minesweeper_Load(object sender, EventArgs e)
        {

        }
    }

     public class MinesweeperButton : Button
     {
         public Point Coordinates { get; set; }
     }
}
