using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    static class program
    {

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Board board = new Board(25, 25, 100);
            board.start();
            Minesweeper gui = new Minesweeper(board);
            MinesweeperController controller = new MinesweeperController(gui,board);        
            Application.Run(gui);
        }

    }
}
