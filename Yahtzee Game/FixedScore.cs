using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game
{
    /// <summary>
    /// class for fixed score combination
    /// </summary>
    [Serializable]
    class FixedScore:Combination
    {
        private ScoreType scoreType;

        /// <summary>
        /// This method is used to combinate score 
        /// and return the score on the label
        /// </summary>
        /// <param name="type"> scoring combination</param>
        /// <param name="label">representing this score on the GUI</param>
        public FixedScore(ScoreType type, Label label) : base(label) {
            scoreType = type;
        }//end FixedScore

        /// <summary>
        /// This method will calculate the score for this Combination given the dice values
        /// </summary>
        /// <param name="dice">getting the value of the dice</param>
        public override void CalculateScore(int[] dice) {
            int result = 0;
            Sort(dice);
            List<int> diceList = dice.OfType<int>().ToList();
            switch (scoreType)
            {
                case ScoreType.LargeStraight:
                    List<int> check1 = new List<int> {1, 2, 3, 4, 5};
                    List<int> check2 = new List<int> {2, 3, 4, 5, 6};
                    if (!check1.Except(diceList).Any() || !check2.Except(diceList).Any())
                    {
                        result = 40;
                    }
                    break;
                case ScoreType.Yahtzee:
                    if (dice[dice.Length - 1] == dice[0])
                    {
                        result = 50;
                    }
                    break;
                case ScoreType.SmallStraight:
                    check1 = new List<int> { 1, 2, 3, 4 };
                    check2 = new List<int> { 2, 3, 4, 5 };
                    List<int> check3 = new List<int> {  3, 4, 5, 6 };
                    if (!check1.Except(diceList).Any() || !check2.Except(diceList).Any() || !check3.Except(diceList).Any())
                    {
                        result = 30;
                    }
                    break;
                case ScoreType.FullHouse:
                    if ((dice[0] == dice[2] || dice[0] == dice[1]) && dice[3] == dice[4])
                    {
                        result = 25;
                    }
                    break;
            }
            Points = result;
        }//end CalculateScore

        /// <summary>
        ///for play yahtzee joker
        /// </summary>
        public void PlayYahtzeeJoker() {
            int result = 0;
            switch (scoreType)
            {
                case ScoreType.LargeStraight:
                    result = 40;
                    break;
                case ScoreType.Yahtzee:
                    result = 50;
                    break;
                case ScoreType.SmallStraight:
                    result = 30;
                    break;
                case ScoreType.FullHouse:
                    result = 25;
                    break;
            }

            Points = result;
        }//end PlayYahtzeeJoker

    }//end Class FixedScore
}
