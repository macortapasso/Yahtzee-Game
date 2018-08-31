using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game
{
    /// <summary>
    /// This class represents a playr in the game
    /// </summary>
    [Serializable]
    class Player {
        private string name;
        private int combinationsToDo = 13;
        private Score[] scores = new Score[19];
        private int grandTotal;
        
        /// <summary>
        /// This constructor will assign the name to the player
        /// and the labels will represent the socre totals on the GUI
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="scoreTotal"></param>
        public Player(string playerName, Label[] scoreTotal) {
            name = playerName;

            //instantiate score in scores array
            for (ScoreType index = ScoreType.Ones; index <= ScoreType.GrandTotal; index++) {
                switch (index) {
                    case ScoreType.Ones:
                    case ScoreType.Twos:
                    case ScoreType.Threes:
                    case ScoreType.Fours:
                    case ScoreType.Fives:
                    case ScoreType.Sixes:
                        scores[(int)index] = new CountingCombination(index, scoreTotal[(int)index]);
                        break;
                    case ScoreType.ThreeOfAKind:
                    case ScoreType.FourOfAKind:
                    case ScoreType.Chance:
                        scores[(int)index] = new TotalOfDice(index, scoreTotal[(int)index]);
                        break;
                    case ScoreType.LargeStraight:
                    case ScoreType.SmallStraight:
                    case ScoreType.Yahtzee:
                    case ScoreType.FullHouse:
                        scores[(int)index] = new FixedScore(index, scoreTotal[(int)index]);
                        break;
                    default:
                        scores[(int)index] = new BonusOrTotal(scoreTotal[(int)index]);
                        break;
                }
            }
        }//end Player

        /// <summary>
        /// This method is implemented as a C# property for name
        /// </summary>
        public string Name {
            get
            {
                return name;
            }
        }//end Name

        /// <summary>
        /// This method will calculate the socre for a specified scoring combination
        /// </summary>
        /// <param name="type">clicked scoretype</param>
        /// <param name="diceValue">array of int show the dice values</param>
        public void ScoreCombination(ScoreType type, int[] diceValue) {
            combinationsToDo--;

            Combination newScores = (Combination)scores[(int)type];

            newScores.CheckForYahtzee(diceValue);
            bool yahtzeeAgain = newScores.IsYahtzee && scores[(int)ScoreType.Yahtzee].Points == 50;

            //handling yahtzee bonus
            if (yahtzeeAgain)
            {
                scores[(int)ScoreType.YahtzeeBonus].Points = 100;
            }

            //default calculate score combination
            newScores.CalculateScore(diceValue);

            //handle yahtzee joker
            if (yahtzeeAgain && scores[newScores.YahtzeeNumber - 1].Done)
            {
                if (Equals(newScores.GetType(), typeof(FixedScore)))
                {
                    ((FixedScore)newScores).PlayYahtzeeJoker();
                }
            }

            //for subtotal, check bonus 63+, and section A total
            int upperTotal = 0;
            for (ScoreType index = ScoreType.Ones; index <= ScoreType.Sixes; index++) {
                if (scores[(int)index].Points != -1) {
                    upperTotal += scores[(int)index].Points;
                }
                
            }
            scores[(int)ScoreType.SubTotal].Points = upperTotal;
            if (upperTotal >= 63) {
                scores[(int)ScoreType.BonusFor63Plus].Points = 35;
            }
            if (scores[(int)ScoreType.BonusFor63Plus].Points != -1) {
                upperTotal += scores[(int)ScoreType.BonusFor63Plus].Points;
                
            }
            scores[(int)ScoreType.SectionATotal].Points = upperTotal;

            //for section B total
            int lowerTotal = 0;

            for (ScoreType index = ScoreType.ThreeOfAKind; index <= ScoreType.Yahtzee; index++) {
                if (scores[(int)index].Points != -1) {
                    lowerTotal += scores[(int)index].Points;
                }
            }
            scores[(int)ScoreType.SectionBTotal].Points = lowerTotal;

            if (scores[(int)ScoreType.YahtzeeBonus].Points != -1)
            {
                lowerTotal += scores[(int)ScoreType.YahtzeeBonus].Points;
            }

            //for grand total
            grandTotal = upperTotal + lowerTotal;
            scores[(int)ScoreType.GrandTotal].Points = grandTotal;


            ShowScores();
        }//end ScoreCombination

        /// <summary>
        /// This method is a C# property for grandTotal
        /// </summary>
        public int GrandTotal {
            get {
                return grandTotal;
            }
        }//end GrandTotal

        /// <summary>
        /// This method checks whether or not the player has
        /// attempted the specified ScoreType.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool IsAvailable(ScoreType score) {
            bool result = !scores[(int)score].Done;
            return result;
        }// end IsAvailable

        /// <summary>
        /// This method will display all the socres for the player
        /// </summary>
        public void ShowScores() {
            foreach (Score score in scores) {
                score.ShowScore();
            }
        }// end ShowScores

        /// <summary>
        /// This method checks if the player has attempted
        /// all of the combination
        /// </summary>
        /// <returns></returns>
        public bool IsFinished() {
            if (combinationsToDo == 0) {
                return true;
            } else {
                return false;
            }
        }// end IsFinished

        /// <summary>
        /// load player in saved game
        /// </summary>
        /// <param name="scoreTotals"></param>
        public void Load(Label[] scoreTotals) {
            for (int i = 0; i < scores.Length; i++) {
                scores[i].Load(scoreTotals[i]);
            }
        }//end Load

    }
}
