using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game{
    /// <summary>
    /// class for the upper section 1 to 6 combination
    /// </summary>
    [Serializable]
    class CountingCombination:Combination{
        private int dieValue;

        /// <summary>
        /// This method represent the scoring combination 
        /// and represent this Score object on the GUI
        /// </summary>
        /// <param name="type">represent the scoring combinatio</param>
        /// <param name="label">represent this Score object on the GUI</param>
        public CountingCombination(ScoreType type, Label label):base(label) {
            dieValue = (int)type + 1;
        }//end CountingCombination

        /// <summary>
        /// This method will calculate the score for this combination
        /// given the dice values
        /// </summary>
        /// <param name="dice">for returning the value for calculating the score</param>
        public override void CalculateScore(int[] dice) {
            int count = 0;
            foreach(int die in dice) {
                if (die == dieValue) {
                    count += dieValue;
                }
            }
            Points = count;

        }//end CalculateScore

    }//end Class CountingCombination
}
