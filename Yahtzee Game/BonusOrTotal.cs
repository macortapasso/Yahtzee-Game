using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game
{
    /// <summary>
    /// This is a subclass of the Score class. 
    /// </summary>
    [Serializable]
    class BonusOrTotal:Score
    {
        /// <summary>
        /// This is a constructor that calls the parent’s constructor.
        /// </summary>
        /// <param name="label">the label parameter</param>
        public BonusOrTotal(Label label) : base(label) {
            
        }
    }
}
