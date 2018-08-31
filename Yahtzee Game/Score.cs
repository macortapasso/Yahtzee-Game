using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game
{
    /// <summary>
    /// A class that represents a single scoring 
    /// combination in a Yahtzee game.
    /// </summary>
    [Serializable]
    class Score {

        private int points = -1;
        [NonSerialized]
        private Label label;
        protected bool done = false;

        /// <summary>
        /// The class constructor
        /// </summary>
        /// <param name="labelName"></param>
        public Score(Label labelName){
            label = labelName;
        }//end Score

        /// <summary>
        /// This method is an acessor and mutator 
        /// property for points
        /// </summary>
        public int Points {
            get {
                return points;
            }
            set {
                points = value;
            }
        }//end Points

        /// <summary>
        /// Done is an accessor only property for done. 
        /// </summary>
        public bool Done {
            get {
                return done;
            }
        }//end Done

        /// <summary>
        /// This method will display the number of points
        /// on the associated Label on the GUI.
        /// </summary>
        public void ShowScore() {
            if (points != -1) {
                label.Text = points.ToString();
                done = true;
            } else {
                label.Text = "";
            }
        }//end ShowScor

        /// <summary>
        /// load score in saved game
        /// </summary>
        /// <param name="label"></param>
        public void Load(Label label) {
            this.label = label;
        } //end Load

    }//end class Score
}
