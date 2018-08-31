using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game
{
    /// <summary>
    /// This is an abstract class which is a subclass of Score.cs
    /// that calculate score for eash combination
    /// </summary>
    [Serializable]
    abstract class Combination: Score
    {
        protected bool isyahtzee;
        protected int yahtzeeNumber;

        /// <summary>
        /// This is constructor which only needs to 
        /// pass the Label parameter
        /// </summary>
        /// <param name="label">the label for the combination</param>
        public Combination(Label label):base(label) {
            
        }//end Combination

        /// <summary>
        /// This is an abstract method which must be 
        /// implemented by all the subclasses for this class
        /// </summary>
        /// <param name="dice"></param>
        public abstract void CalculateScore(int[] dice);
        //end CalculateScore

        /// <summary>
        /// This mehthod can call the Array.Sort method
        /// </summary>
        /// <param name="dice"></param>
        public void Sort(int[] dice) {
            Array.Sort(dice);
        }//end Sort

        /// <summary>
        /// check if the current dice is yahtzee again
        /// </summary>
        public bool IsYahtzee {
            get
            {
                return isyahtzee;
            }
        }//end IsYahtzee

        /// <summary>
        /// get the faceValue for this yahtzee
        /// </summary>
        public int YahtzeeNumber {
            get
            {
                return yahtzeeNumber;
            }
        }//end YahtzeeNumber

        /// <summary>
        /// check if player got another yahtzee
        /// </summary>
        /// <param name="dice"></param>
        public void CheckForYahtzee(int[] dice) {
            Sort(dice);
            if (dice[0] == dice[4])
            {
                isyahtzee = true;
                yahtzeeNumber = dice[0];
            }
                        
        }//end CheckForYahtzee

    }//end class Combination
}
