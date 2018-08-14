using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {

    /// <summary>
    /// Jovi (n10101381)	
    /// Kiki (n10031014)
    /// ITD121 Class Project, TP1 2018
    /// Implementation of a GUI Yahtzee Dice Game
    /// </summary>
    static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }//end Main
    }
}
