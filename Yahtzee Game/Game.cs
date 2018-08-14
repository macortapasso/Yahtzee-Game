using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Yahtzee_Game {

    public enum ScoreType {
        Ones, Twos, Threes, Fours, Fives, Sixes,
        SubTotal, BonusFor63Plus, SectionATotal,
        ThreeOfAKind, FourOfAKind, FullHouse,
        SmallStraight, LargeStraight, Chance, Yahtzee,
        YahtzeeBonus, SectionBTotal, GrandTotal
    }

    /// <summary>
    /// This class represents a Yahtzee game
    /// </summary>
    [Serializable]
    class Game {

        public static string defaultPath = Environment.CurrentDirectory;
        private static string savedGameFile = defaultPath + "\\YahtzeeGame.dat";

        private BindingList<Player> players;
        private int currentPlayerIndex = 0;
        private Player currentPlayer;
        private Die[] dice = new Die[5];
        private int playersFinished = 0;
        private int numRolls;
        [NonSerialized]
        private Form1 form;
        [NonSerialized]
        private Label[] dieLabels = new Label[5];

        /// <summary>
        /// This method initialise all the instance variables of Game 
        /// in preparation for a game to begin in the correct state
        /// </summary>
        /// <param name="formOneObject"></param>
        public Game(Form1 formOneObject) {
            form = formOneObject;
            dieLabels = form.GetDice();
            players = new BindingList<Player>();
            players.Add(new Player("Player 1", form.GetScoresTotals()));
            for (int index = 0; index < 5; index++) {
                dice[index] = new Die(dieLabels[index]);
            }
            numRolls = 1;
        }//end Game

        /// <summary>
        /// This method updates currentPlayer and CurrentPlayerIndex
        /// and end game is all player finished all combinations
        /// </summary>
        public void NexTurn() { 

            currentPlayer = players[(currentPlayerIndex) % players.Count];
            playersFinished = 0;
            
            form.ShowPlayerName(currentPlayer.Name);
            form.DisableAndClearCheckBoxes();
            form.EnableRollButton();
            numRolls = 1;

            currentPlayer.ShowScores();

            //check end game and finish the game
            foreach (Player player in players) {
                if (player.IsFinished()) {
                    playersFinished++;
                }
            }
            if (playersFinished == players.Count)
            {

                string name = "";
                int grandTotal = 0;
                foreach (Player player in players)
                {
                    if (player.GrandTotal > grandTotal)
                    {
                        grandTotal = player.GrandTotal;
                        name = player.Name;
                    }
                }
                DialogResult newGame = MessageBox.Show("Game has ended and the winner is " + name + ".\nDo you want to start a new game?", "Game finished",
                MessageBoxButtons.YesNo);
                if (newGame == DialogResult.Yes)
                {
                    form.StartNewGame();
                }
                else
                {
                    form.Close();
                }
            }
            currentPlayerIndex++;

        }//end NextTurn


        /// <summary>
        /// This method will roll all of the active dice
        /// and show unscored buttons
        /// </summary>
        public void RollDice()
        {
            string[] messages = new string[4] {"Roll 1", "Roll 2 or choose a combination to score",
            "Roll 3 or choose a combination to score", "Choose a combination to score"};
            for (int index = 0; index < 5; index++) {
                if (dice[index].Active) {
                    dice[index].Roll();
                }
                
            }
            
            form.ShowMessage(messages[numRolls]);
            numRolls++;
            if (numRolls > 3) {
                form.DisableRollButton();
            }

            //enable unscored buttons
            for (int index = (int)ScoreType.Ones; index <= (int)ScoreType.Sixes; index++) {
                if (currentPlayer.IsAvailable((ScoreType)index)) {
                    form.EnableScoreButton((ScoreType)index);

                }
            }

            for (int index = (int)ScoreType.ThreeOfAKind; index <= (int)ScoreType.Yahtzee; index++) {
                if (currentPlayer.IsAvailable((ScoreType)index)) {
                    form.EnableScoreButton((ScoreType)index);
                }
            }

        }//end RollDice

        /// <summary>
        /// property for players
        /// </summary>
        public BindingList<Player> Players {
            get {
                return players;
            }
        }//end Players

        /// <summary>
        /// This method makes the specified index of the Die inactive
        /// </summary>
        /// <param name="Index">index for the die</param>
        public void HoldDie(int Index) {
            dice[Index].Active = false;
        }//end HoldDie

        /// <summary>
        /// This method makes the specified index of the Die active
        /// </summary>
        /// <param name="Index">index for the die</param>
        public void ReleaseDie(int Index) {
            dice[Index].Active = true;
        }//end ReleaseDie

        /// <summary>
        /// This method calculates the score for the specified scoring combination
        /// </summary>
        /// <param name="combo">the score combination</param>
        public void ScoreCombination(ScoreType combo) {
            int[] dieValue = new int[5];
            for (int index = 0; index < 5; index++) {
                dieValue[index] = dice[index].FaceValue;
            }
            currentPlayer.ScoreCombination(combo, dieValue);
            form.ShowOKButton();
        }//end ScoreCombination

        /// <summary>
        /// Load a saved game from the default save game file
        /// </summary>
        /// <param name="form">the GUI form</param>
        /// <returns>the saved game</returns>
        public static Game Load(Form1 form) {
            Game game = null;
            if (File.Exists(savedGameFile)) {
                try {
                    Stream bStream = File.Open(savedGameFile, FileMode.Open);
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    game = (Game)bFormatter.Deserialize(bStream);
                    bStream.Close();
                    game.form = form;
                    game.ContinueGame();
                    return game;
                } catch {
                    MessageBox.Show("Error reading saved game file.\nCannot load saved game.");
                }
            } else {
                MessageBox.Show("No current saved game.");
            }
            return null;
        }//end Load

        /// <summary>
        /// Save the current game to the default save file
        /// </summary>
        public void Save() {
            try {
                Stream bStream = File.Open(savedGameFile, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(bStream, this);
                bStream.Close();
                MessageBox.Show("Game saved");
            } catch (Exception e) {

                //   MessageBox.Show(e.ToString());
                MessageBox.Show("Error saving game.\nNo game saved.");
            }
        }//end Save

        /// <summary>
        /// Continue the game after loading a saved game
        /// 
        /// Assumes game was saved at the start of a player's turn before they had rolled dice.
        /// </summary>
        private void ContinueGame() {
            LoadLabels(form);
            for (int i = 0; i < dice.Length; i++) {
                //uncomment one of the following depending how you implmented Active of Die
                // dice[i].SetActive(true);
                // dice[i].Active = true;
            }

            form.ShowPlayerName(currentPlayer.Name);
            form.EnableRollButton();
            form.EnableCheckBoxes();
            // can replace string with whatever you used
            form.ShowMessage("Roll 1");
            currentPlayer.ShowScores();
        }//end ContinueGame

        /// <summary>
        /// Link the labels on the GUI form to the dice and players
        /// </summary>
        /// <param name="form"></param>
        private void LoadLabels(Form1 form) {
            Label[] diceLabels = form.GetDice();
            for (int i = 0; i < dice.Length; i++) {
                dice[i].Load(diceLabels[i]);
            }
            for (int i = 0; i < players.Count; i++) {
                players[i].Load(form.GetScoresTotals());
            }
        }//end LoadLabels

    }// end class Game
}
