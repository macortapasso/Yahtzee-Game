using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Yahtzee_Game
{
    /// <summary>
    /// This class represents a die
    /// </summary>
    [Serializable]
    class Die {

        private static string rollFileName = Game.defaultPath + "\\basictestrolls.txt";

        private int faceValue;
        private bool active = true;
        [NonSerialized]
        private Label label;
        private static Random random = new Random();
        [NonSerialized]
        private static StreamReader rollFile = new StreamReader(rollFileName);
        private static bool DEBUG = false;

        /// <summary>
        /// This method represent the Die in Form1
        /// </summary>
        /// <param name="die">label</param>
        public Die(Label die) {
            label = die;
        }//end Die

        /// <summary>
        /// This method is an accessor property for faceValue
        /// </summary>
        public int FaceValue {
            get {
                return faceValue;
            }
        }// end faceValue

        /// <summary>
        /// This method is the acessor and mutator for the instance variable active.
        /// </summary>
        public bool Active {
            get {
                return active;
            }
            set {
                active = value;
            }
        }//end Active
        
        /// <summary>
        /// This method simulates the rolling of this Die
        /// </summary>
        public void Roll() {
            if (!DEBUG) {
                faceValue = random.Next(1, 7);
                label.Text = FaceValue.ToString();
            } else {
                faceValue = int.Parse(rollFile.ReadLine());
                label.Text = faceValue.ToString();
                label.Refresh();
            }
            
        }//end Roll

        /// <summary>
        /// load die in saved game
        /// </summary>
        /// <param name="label"></param>
        public void Load(Label label) {
            this.label = label;
            if (faceValue == 0) {
                label.Text = string.Empty;
            } else {
                label.Text = faceValue.ToString();
            }
        }//end Load
    }//end class Die
}
