using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game
{
    /// <summary>
    /// class for total of dice combination
    /// </summary>
    [Serializable]
    class TotalOfDice:Combination
    {
        private int numberOfOneKind;

        /// <summary>
        /// TotalOfDice constructor will use the ScoreType parameter to determine what the value
        ///of numberOfAKind will be for this combination.
        /// </summary>
        /// <param name="type">the scoretype</param>
        /// <param name="labelDice">the label of the dice</param>
        public TotalOfDice(ScoreType type, Label labelDice) : base(labelDice) {
            if (type == ScoreType.ThreeOfAKind) {
                numberOfOneKind = 3;
            }else if (type == ScoreType.FourOfAKind) {
                numberOfOneKind = 4;
            } else {
                numberOfOneKind = 0;
            }
        }//end TotalOfDice

        /// <summary>
        /// This method will calculate how many points will
        /// be awarded for this combination
        /// </summary>
        /// <param name="dice">the die value is needed for the calculation</param>
        public override void CalculateScore(int[] dice) {
            int result = 0;
            Sort(dice);
            if (numberOfOneKind == 0) {
                foreach(int die in dice) {
                    result += die;
                }
            } else
            {
                foreach(int die in dice)
                {
                    if (CheckValue(die, dice) >= numberOfOneKind)
                    {
                        result = dice.Sum();
                    }
                }
            }
            Points = result;
        }//end CalculateScore

        /// <summary>
        /// helper method to count each faceValue in dice
        /// </summary>
        /// <param name="faceValue"></param>
        /// <param name="dice"></param>
        /// <returns></returns>
        private int CheckValue(int faceValue, int[] dice) {
            int count = 0;
            foreach (int die in dice)
            {
                if (die == faceValue)
                {
                    count++;
                }
            }

            return count;
        }//end CheckValue

    }//end Class TotalOfDice
}
