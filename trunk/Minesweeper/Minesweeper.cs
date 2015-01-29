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
            buttonArray = new MinesweeperButton[board.Width, board.Height];
            InitializeComponent();       
            this.SuspendLayout();
            AddButtons();
            this.label1.Width = this.Width;
            this.AutoSize = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;            
            this.ResumeLayout(true);          
        }

     
        private void AddButtons()
        {
            MinesweeperButton button;
            for(int i = 0; i < board.Height; i++)
                for (int j = 0; j < board.Width; j++)
                {
                    buttonArray[j, i] = new MinesweeperButton();
                    buttonArray[j, i].Location = new System.Drawing.Point(i * buttonSize.Height, j * buttonSize.Width + label1.Height);
                    buttonArray[j, i].Name = "button" + (j + (i * j));
                    buttonArray[j, i].Coordinates = new Point(j, i);
                    buttonArray[j, i].Size = buttonSize;
                    buttonArray[j, i].UseVisualStyleBackColor = false;
                    this.Controls.Add(buttonArray[j, i]);                    
                }

        }

        public void setLabelText(String message)
        {
            label1.Text = message;
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
