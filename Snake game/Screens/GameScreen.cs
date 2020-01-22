using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameSystemServices;
using System.Media;
using System.Threading;

namespace Snake_game
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown;

        Random randGen = new Random();

        //TODO create your global game variables here
        int snakeX, snakeY, snakeSize, snakeSpeed;
        string snakeDirection;
        SolidBrush snakeBrush = new SolidBrush(Color.ForestGreen);
        int greenX, greenY, greenSize, redSize, blueX, blueY, blueSize, yellowX, yellowY, yellowSize;

        List<int> redX = new List<int>();
        List<int> redY = new List<int>();

        string winLose = "";

        private void ScoreLabel_Click(object sender, EventArgs e)
        {
            
        }

        Pen greenPen = new Pen(Color.Green, 5);
        Pen redPen = new Pen(Color.Red, 9);
        Pen bluePen = new Pen(Color.Blue, 5);
        Pen yellowPen = new Pen(Color.Yellow, 5);
        SolidBrush scoreBrush = new SolidBrush(Color.Gold);

        Boolean blueOn = false;
        int blueCounter = 0;

        Boolean yellowOn = false;
        int yellowCounter = 0;

        int redCounter = 0;

        int score = 0;

        public GameScreen()
        {
            InitializeComponent();
            InitializeGameValues();
        }

        public void InitializeGameValues()
        {
            //TODO - setup all your initial game values here. Use this method
            // each time you restart your game to reset all values.
            snakeX = 10;
            snakeY = 10;
            snakeSize = 20;
            snakeSpeed = 4;
            snakeDirection = "right";

            //for green block
            greenX = randGen.Next(5, 255);
            greenY = randGen.Next(5, 255);
            greenSize = 5;
            //for red block
            redX.Add(randGen.Next(5, 255));
            redY.Add(randGen.Next(5, 255));

            redX.Add(randGen.Next(5, 255));
            redY.Add(randGen.Next(5, 255));

            redSize = 5;
            //for blue block
            blueX = randGen.Next(5, 255);
            blueY = randGen.Next(5, 255);
            blueSize = 5;
            //for yellow block
            yellowX = randGen.Next(5, 255);
            yellowY = randGen.Next(5, 255);
            yellowSize = 5;


        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // opens a pause screen is escape is pressed. Depending on what is pressed
            // on pause screen the program will either continue or exit to main menu
            if (e.KeyCode == Keys.Escape && gameTimer.Enabled)
            {
                gameTimer.Enabled = false;
                rightArrowDown = leftArrowDown = upArrowDown = downArrowDown = false;

                DialogResult result = PauseForm.Show();

                if (result == DialogResult.Cancel)
                {
                    gameTimer.Enabled = true;
                }
                else if (result == DialogResult.Abort)
                {
                    MainForm.ChangeScreen(this, "MenuScreen");
                }
            }

            //TODO - basic player 1 key down bools set below. Add remainging key down
            // required for player 1 or player 2 here.

            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    snakeDirection = "left";
                    break;
                case Keys.Down:
                    snakeDirection = "down";
                    break;
                case Keys.Right:
                    snakeDirection = "right";
                    break;
                case Keys.Up:
                    snakeDirection = "up";
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //TODO - basic player 1 key up bools set below. Add remainging key up
            // required for player 1 or player 2 here.

            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
            }
        }

        /// <summary>
        /// This is the Game Engine and repeats on each interval of the timer. For example
        /// if the interval is set to 16 then it will run each 16ms or approx. 50 times
        /// per second
        /// </summary>
        private void gameTimer_Tick(object sender, EventArgs e)
        {


            //TODO move main character 
            if (snakeDirection == "left")
            {
                snakeX = snakeX - snakeSpeed;
            }
            if (snakeDirection == "down")
            {
                snakeY = snakeY + snakeSpeed;
            }
            if (snakeDirection == "right")
            {
                snakeX = snakeX + snakeSpeed;
            }
            if (snakeDirection == "up")
            {
                snakeY = snakeY - snakeSpeed;
            }

            //TODO move npc characters


            //TODO collisions checks 
        
            Rectangle snakeRec = new Rectangle(snakeX, snakeY, snakeSize, snakeSize);
            Rectangle greenRec = new Rectangle(greenX, greenY, greenSize, greenSize);
            Rectangle blueRec = new Rectangle(blueX, blueY, blueSize, blueSize);
            Rectangle yellowRec = new Rectangle(yellowX, yellowY, yellowSize, yellowSize);

            if (snakeRec.IntersectsWith(greenRec))
            {
                score = score + 1;

                greenX = randGen.Next(5, 255);
                greenY = randGen.Next(5, 255);
                SoundPlayer player = new SoundPlayer(Properties.Resources.Green_Tone);

                player.Play();
            }

            for (int i = 0; i < redX.Count(); i++)
            {
                Rectangle redRec = new Rectangle(redX[i], redY[i], redSize, redSize);

                if (snakeRec.IntersectsWith(redRec))
                {
                    score = score - 1;
                    SoundPlayer player = new SoundPlayer(Properties.Resources.Red_Beep);
                    player.Play();

                    redX.RemoveAt(i);
                    redY.RemoveAt(i);

                    redX.Add(randGen.Next(5, 255));
                    redY.Add(randGen.Next(5, 255));

                    break;
                }
            }
            redCounter++;

            if (redCounter == 80)
            {
                redCounter = 0;
                snakeSpeed = 4;

                redX.Add(randGen.Next(5, 255));
                redY.Add(randGen.Next(5, 255));
            }

  


            if (snakeRec.IntersectsWith(blueRec))
            {
                snakeSpeed = 6;
                blueOn = true;

                blueX = randGen.Next(5, 255);
                blueY = randGen.Next(5, 255);

                SoundPlayer player = new SoundPlayer(Properties.Resources.Speed_Sound);

                player.Play();
            }

            if (blueOn)
            {
                blueCounter++;

                if (blueCounter == 60)
                {
                    blueCounter = 0;
                    blueOn = false;
                    snakeSpeed = 4;
                }
            }


            if (snakeRec.IntersectsWith(yellowRec))
            {
                snakeSpeed = 2;
                yellowOn = true;

                yellowX = randGen.Next(5, 255);
                yellowY = randGen.Next(5, 255);

                SoundPlayer player = new SoundPlayer(Properties.Resources.Slow_sound);

                player.Play();
            }

            if (yellowOn)
            {
                yellowCounter++;

                if (yellowCounter == 60)
                {
                    yellowCounter = 0;
                    yellowOn = false;
                    snakeSpeed = 4;
                }
            }


            if (score == 20)
            {

                gameTimer.Enabled = false;
                winLose = "You Win!";

                Refresh();
                Thread.Sleep(5000);
                MainForm.ChangeScreen(this, "MenuScreen");
                return;
            }


            if (score == -1)
            {

                gameTimer.Enabled = false;
                winLose = "You lose!";

                Refresh();
                Thread.Sleep(5000);
                MainForm.ChangeScreen(this, "MenuScreen");
                return;
            }


            //calls the GameScreen_Paint method to draw the screen.
            Refresh();
        }


        //Everything that is to be drawn on the screen should be done here
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw rectangle to screen
            e.Graphics.FillRectangle(snakeBrush, snakeX, snakeY, snakeSize, snakeSize);
            e.Graphics.DrawRectangle(greenPen, greenX, greenY, greenSize, greenSize);
            e.Graphics.DrawRectangle(bluePen, blueX, blueY, blueSize, blueSize);
            e.Graphics.DrawRectangle(yellowPen, yellowX, yellowY, yellowSize, yellowSize);

            for (int i = 0; i < redX.Count(); i++)
            {
                e.Graphics.DrawRectangle(redPen, redX[i], redY[i], redSize, redSize);
            }

            if (gameTimer.Enabled == true)
            {
                e.Graphics.DrawString("score: " + score, new Font("Freefrm721 Blk BT", 12), scoreBrush, 125, 260);
            }
            else
            {
                e.Graphics.DrawString(winLose, new Font("Freefrm721 Blk BT", 12), scoreBrush, 125, 260);
            }



            

            

        }
    }

}
