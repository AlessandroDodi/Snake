using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    public partial class Game : Form
    {
        private Home f;
        public SoundPlayer simpleSound;
        private SnakeRear s;
        public string direction;
        public int scoreInt;
        public bool audio;
        public int difficult;
        public Game(Home f, bool a, int d)
        {
            InitializeComponent();
            audio = a;
            difficult = d;
            inizializeAll();
                f.Hide();
            this.Show();
        }
        /*---------------------------------------------------INIZIALIZE---------------------------------------------*/
        public void inizializeAll()
        {
            if(audio)
            {
                simpleSound = new SoundPlayer(snake.Properties.Resources.background);
                simpleSound.Play();
            }               
            direction = "";
            scoreInt = 0;
            inizializeSnake();
            inizializeDifficult();
            generateFood();
        }
        public void inizializeSnake()
        {
            s = new SnakeRear();
            s.add(this, direction);
        }
        public void inizializeDirecion(char c)
        {
            switch (c)
            {
                case 'w':
                    if (direction != "up" && direction != "down")
                        direction = "up";
                    break;
                case 's':
                    if (direction != "up" && direction != "down")
                        direction = "down";
                    break;
                case 'd':
                    if (direction != "right" && direction != "left")
                        direction = "right";
                    break;
                case 'a':
                    if (direction != "right" && direction != "left")
                        direction = "left";
                    break;
            }
        }
        public void inizializeDifficult()
        {
            switch (difficult)
            {
                case 0:
                    time.Interval = 110;
                    break;
                case 1:
                    time.Interval = 70;
                    break;
                case 2:
                    time.Interval = 40;
                    break;
            }
        }

        /*----------------------------------------------------------------------------------------------------------*/
        /*---------------------------------------------------FOOD---------------------------------------------------*/
        public void foodEaten()
        {
            if (s.pFront.img.Bounds.IntersectsWith(food.Bounds))
            {
                food.Visible = false;
                s.add(this, direction);
                generateFood();
                scoreInt++;
                scorePunt.Text = scoreInt.ToString();
            }
        }
        public void generateFood()
        {
            Random r = new Random();
            int x = r.Next();
            x %= (Size.Width - 100);
            int y = r.Next();
            y %= (Size.Height - 100);
            food.Location = new Point(x, y);
            food.Visible = true;
        }
        /*----------------------------------------------------------------------------------------------------------*/
        /*---------------------------------------------------CONTROLLS---------------------------------------------*/
        public bool controllBoundsForm()
        {
            if ((s.pFront.img.Location.X <= 5) || (s.pFront.img.Location.X >= this.Size.Width) || (s.pFront.img.Location.Y >= 695) || (s.pFront.img.Location.Y <= 0))
                return true;
            return false;
        }
        public bool controlls()
        {
            bool flag = false;
            s.pFront.img.BackColor = Color.Black; //colors of the head snake
            if (scoreInt < 1) return false;
            else
            {
                Node pa = s.pFront.getPtrNext();
                Node secondlast = pa;

                while (pa.getPtrNext() != null)
                {
                    pa.img.BackColor = Color.Yellow; //colors of the body snake 
                    if (pa.img.Location == s.pFront.img.Location)
                        flag = true;
                    pa = pa.getPtrNext();
                }
                pa.img.BackColor = Color.Yellow; //colors of the rear snake 
                return flag;
            }

        }
        /*----------------------------------------------------------------------------------------------------------*/
        /*----------------------------------------------------EVENTS------------------------------------------------*/
        private void time_Tick(object sender, EventArgs e)
        {
            s.move(this, direction);
            foodEaten();
            if (controllBoundsForm() || controlls())
                endGame();
        }

        private void Game_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!time.Enabled) time.Enabled = true; 
            inizializeDirecion(e.KeyChar);
        }
        /*----------------------------------------------------------------------------------------------------------*/
        /*----------------------------------------------------ENG-GAME-----------------------------------------------*/
        public void endGame()
        {
            stopTimer();
            Game_over g = new Game_over(this, scoreInt);
            simpleSound.Stop();
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                Pause p = new Pause(this);
                time.Stop();
                simpleSound.Stop();
            }
            
        }
        /*----------------------------------------------------------------------------------------------------------*/
        /*----------------------------------------------------OTHERS-----------------------------------------------*/
        public void stopTimer() => time.Stop();
        public void playTimer() => time.Start();
        public void setAudio(bool a) => audio = a;
        public bool getAudio() => audio;
        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            simpleSound.Stop();
            Environment.Exit(0);
        }
        /*----------------------------------------------------------------------------------------------------------*/
    }
}
