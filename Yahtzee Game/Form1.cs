using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {
    public partial class Form1: Form {

        const int DICE_LABELS = 5;
        const int SCORE_BUTTONS = 16;
        const int SCORE_LABELS = 19;
        const int DICE_CHECKBOXES = 5;

        private Label[] dice = new Label[DICE_LABELS];
        private Button[] scoreButtons = new Button[SCORE_BUTTONS];
        private Label[] scoreTotals = new Label[SCORE_LABELS];
        private CheckBox[] checkBoxes = new CheckBox[DICE_CHECKBOXES];
        private Game game;

        public Form1() {
            InitializeComponent();
            InitializeLabelsAndButtons();
        }
        
        /// <summary>
        /// initialize all widgets
        /// </summary>
        private void InitializeLabelsAndButtons() {
            dice = new Label[DICE_LABELS] { die1, die2, die3, die4, die5 };
            scoreButtons = new Button[SCORE_BUTTONS] {button1, button2, button3, button4, button5, button6,
            null, null, null, button7, button8, button9, button10, button11, button12, button13};
            scoreTotals = new Label[SCORE_LABELS] {scoreLabel1, scoreLabel2, scoreLabel3, scoreLabel4, scoreLabel5,
            scoreLabel6, scoreLabel14, scoreLabel15, scoreLabel16, scoreLabel7, scoreLabel8, scoreLabel9,
            scoreLabel10, scoreLabel11, scoreLabel12, scoreLabel13, scoreLabel17, scoreLabel18, label_GrandTotal2};
            checkBoxes = new CheckBox[DICE_CHECKBOXES] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5 };
        }

        /// <summary>
        /// return dice label
        /// </summary>
        /// <returns>Label[] dice</returns>
        public Label[] GetDice() {
            return dice;
        }

        /// <summary>
        /// return score labels
        /// </summary>
        /// <returns>Label[] scoreTotals</returns>
        public Label[] GetScoresTotals() {
            return scoreTotals;
        }

        /// <summary>
        /// change player label to name
        /// </summary>
        /// <param name="name">name for the player</param>
        public void ShowPlayerName(string name) {
            label_Player.Text = name;
        }

        /// <summary>
        /// enable roll dice button
        /// </summary>
        public void EnableRollButton() {
            button_RollDice.Enabled = true;
        }

        /// <summary>
        /// disable roll dice button
        /// </summary>
        public void DisableRollButton() {
            button_RollDice.Enabled = false;
        }

        /// <summary>
        /// enable all checkboxes
        /// </summary>
        public void EnableCheckBoxes() {
            foreach (CheckBox checkbox in checkBoxes) {
                checkbox.Enabled = true;
            }
        }

        /// <summary>
        /// disable all checkboxes and unticked them
        /// </summary>
        public void DisableAndClearCheckBoxes() {
            foreach (CheckBox checkbox in checkBoxes) {
                checkbox.Enabled = false;
                checkbox.Checked = false;
            }
        }

        /// <summary>
        /// enable score button according to index
        /// </summary>
        /// <param name="combo">ScoreType enum as index</param>
        public void EnableScoreButton(ScoreType combo) {
            scoreButtons[(int)combo].Enabled = true;
        }

        /// <summary>
        /// disable score button according to index
        /// </summary>
        /// <param name="combo">ScoreType enum as index</param>
        public void DisableScoreButton(ScoreType combo) {
            scoreButtons[(int)combo].Enabled = false;
        }

        /// <summary>
        /// check checkbox with required index
        /// </summary>
        /// <param name="index">index for checkbox</param>
        public void CheckCheckBox(int index) {
            checkBoxes[index].Checked = true;
        }

        /// <summary>
        /// change label text to message
        /// </summary>
        /// <param name="message">the message want to be shown</param>
        public void ShowMessage(string message) {
            label_Message.Text = message;
        }

        /// <summary>
        /// make the OK button visible
        /// </summary>
        public void ShowOKButton() {
            buttonOK.Visible = true;
        }

        /// <summary>
        /// start new game
        /// </summary>
        public void StartNewGame() {
            
            game = new Game(this);

            EnableRollButton();
            ShowPlayerName(game.Players[0].Name);
            numericUpDownPlayer.Enabled = true;
            playerBindingSource.DataSource = game.Players;
            saveToolStripMenuItem.Enabled = true;
            numericUpDownPlayer.Value = 1;

            foreach (Label label in scoreTotals)
            {
                label.Text = "";
            }
        }

        /// <summary>
        /// event handler for new menu in menu strip
        /// </summary>
        /// <param name="sender">object button</param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            StartNewGame();
            
        }

        /// <summary>
        /// handle if roll button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RollDice_Click(object sender, EventArgs e) {
            if (numericUpDownPlayer.Enabled) {
                game.NexTurn();
                numericUpDownPlayer.Enabled = false;
            }
            game.RollDice();
            EnableCheckBoxes();

        }

        /// <summary>
        /// handle if checkbox is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxes_CheckedChanged(object sender, EventArgs e) {
            for (int index = 0; index < 5; index++) {
                if (checkBoxes[index].Checked) {
                    game.HoldDie(index);
                }
            }
        }

        /// <summary>
        /// handle if any of the scoring combination is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttons_Click(object sender, EventArgs e) {
            game.ScoreCombination((ScoreType)Array.IndexOf(scoreButtons, (Button)sender));
            ShowOKButton();
            DisableRollButton();
            ShowMessage("Your turn has ended - click OK");
            DisableScoreButton((ScoreType)Array.IndexOf(scoreButtons, (Button)sender));
            UpdatePlayersDataGridView();

        }

        /// <summary>
        /// handle if button ok is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e) {
            game.NexTurn();
            for (int index = 0; index < 5; index++) {
                game.ReleaseDie(index);
            }
            ShowMessage("Roll 1");
            EnableRollButton();
            buttonOK.Visible = false;
            for (int index = 0; index < 6; index++) {
                DisableScoreButton((ScoreType)index);
            }

            for (int index = 9; index < 16; index++) {
                DisableScoreButton((ScoreType)index);
            }

            foreach (Label die in dice) {
                die.Text = "";
            }
        }

        /// <summary>
        /// handle if the num of player is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownPlayer_ValueChanged(object sender, EventArgs e) {
            game.Players.Clear();
            for (int count = 0; count < numericUpDownPlayer.Value; count++) {
                game.Players.Add(new Player("Player " + (count + 1).ToString(), scoreTotals));
            }

        }

        /// <summary>
        /// for updating player data grid view
        /// </summary>
        private void UpdatePlayersDataGridView() {
            game.Players.ResetBindings();
        }

        /// <summary>
        /// to save the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            game.Save();
        }

        /// <summary>
        /// to laod saved game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e) {
            game = Game.Load(this);
            playerBindingSource.DataSource = game.Players;
            UpdatePlayersDataGridView();
            newToolStripMenuItem.Enabled = false;

        }
        
    }
}