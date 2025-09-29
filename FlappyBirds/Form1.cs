using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlappyBirds
{
    public class Form1 : Form
    {
        // Alanlar
        private PictureBox bird = null!;
        private PictureBox pipeTop = null!;
        private PictureBox pipeBottom = null!;
        private Label scoreText = null!;
        private System.Windows.Forms.Timer gameTimer = null!;

        private int gravity = 5;
        private int pipeSpeed = 7;
        private int score = 0;
        private bool gameOver = false;

        private Random rand = new Random();

        public Form1()
        {
            // Pencere ayarlarý
            this.Width = 700;
            this.Height = 700;
            this.Text = "Flappy Bird - C#";
            this.BackColor = Color.White;

            this.KeyDown += GameKeyDown;
            this.KeyUp += GameKeyUp;

            // Kuþ
            bird = new PictureBox
            {
                Size = new Size(60, 60),
                Location = new Point(100, 200),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("bird.png")
            };
            this.Controls.Add(bird);

            // Üst boru
            pipeTop = new PictureBox
            {
                Size = new Size(130, 330),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("row2.JPG"),
                Location = new Point(400, -150)
            };
            this.Controls.Add(pipeTop);

            // Alt boru
            pipeBottom = new PictureBox
            {
                Size = new Size(130, 350),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("row1.JPG"),
                Location = new Point(400, 400)
            };
            this.Controls.Add(pipeBottom);

            // Skor
            scoreText = new Label
            {
                Text = "Score: 0",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(scoreText);

            // Timer
            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 20
            };
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Start();
        }

        private void GameTimerEvent(object? sender, EventArgs e)
        {
            bird.Top += gravity;
            pipeTop.Left -= pipeSpeed;
            pipeBottom.Left -= pipeSpeed;

            // Borular ekran dýþýna çýktýysa sýfýrla ve skor ekle
            if (pipeTop.Left < -100)
            {
                ResetPipes();
                score++;
                scoreText.Text = "Score: " + score;
            }

            // Çarpma kontrolü
            if (bird.Bounds.IntersectsWith(pipeTop.Bounds) ||
                bird.Bounds.IntersectsWith(pipeBottom.Bounds) ||
                bird.Top < -25 || bird.Bottom > this.ClientSize.Height)
            {
                EndGame();
            }
        }

        private void GameKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) gravity = -8; // yukarý zýplama
            else if (e.KeyCode == Keys.R && gameOver) RestartGame(); // R ile restart
        }

        private void GameKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) gravity = 8; // yerçekimi tekrar pozitif
        }

        private void ResetPipes()
        {
            int pipeOffset = rand.Next(-150, 150);
            pipeTop.Left = 500;
            pipeTop.Top = -100 + pipeOffset;

            pipeBottom.Left = 500;
            pipeBottom.Top = pipeTop.Bottom + 150;
        }

        private void EndGame()
        {
            gameTimer.Stop();
            scoreText.Text += "         Oyun Bitti! (R ile tekrar baþlat)";
            gameOver = true;
        }

        private void RestartGame()
        {
            score = 0;
            bird.Location = new Point(100, 200);
            ResetPipes();
            gameOver = false;
            scoreText.Text = "Score: 0";
            gameTimer.Start();
        }
    }
}
