using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace StreetFighterGame
{
    public partial class Form1 : Form
    {
        Image player;
        Image background;
        Image enemy;
        Image fireball;

        readonly GameLogic gameLogic = new GameLogic();
        readonly Player playerInstance;
        readonly Enemy enemyInstance;

        float enemyNum;
        int enemyTotalFrame;

        int playerX = 0;
        int playerY = 300;

        int enemyX = 550;
        int enemyY = 300;

        readonly int playerMaxHealth = 100;
        int playerCurrentHealth = 100;

        readonly int enemyMaxHealth = 100;
        int enemyCurrentHealth = 100; 
        bool enemyPlayingAction = false;

        readonly int healthBarWidth = 100;

        private int playerScore = 0;

        int enemyAttackCooldown = 0; 
        const int ENEMY_MAX_COOLDOWN = 100; 

        int fireballX;
        int fireballY;

        int drumMoveTime = 0;
        int actionStrenght = 0;
        int endFrame = 0;
        int backgroundPosition = 0;
        int totalFrame = 0;
        int bg_number = 0;

        float num;

        bool goLeft, goRight;
        bool directionPressed;
        bool playingAction;
        bool shootFireBall;

        string enemyState = "standing";
        bool hasPlayerWon = false; 

        List<string> background_images = new List<string>();

        private readonly System.Media.SoundPlayer bgMusicPlayer;


        private readonly Image enemyWalkingLeft = Image.FromFile("../../Resources/enemy/forwards.gif");
        private readonly Image enemyWalkingRight = Image.FromFile("../../Resources/enemy/backwards.gif");

        public Form1()
        {
            InitializeComponent();
            playerInstance = new Player(this);
            enemyInstance = new Enemy(this);
            SetUpForm();

            // Create the SoundPlayer objects
            bgMusicPlayer = new System.Media.SoundPlayer("../../Resources/backgroundMusic.wav");

            // Play the background music
            bgMusicPlayer.PlayLooping();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // ----------------- KEYS MANAGEMENT -----------------
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && !directionPressed)
            {

                playerInstance.MovePlayerAnimation("left", ref goLeft, ref goRight, ref directionPressed, ref playingAction, ref player, ref totalFrame, ref endFrame);

                
            }
            if (e.KeyCode == Keys.Right && !directionPressed)
            {
                playerInstance.MovePlayerAnimation("right", ref goLeft, ref goRight, ref directionPressed, ref playingAction, ref player, ref totalFrame, ref endFrame);
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                if (bg_number < background_images.Count - 1)
                {
                    bg_number++;
                }
                else
                {
                    bg_number = 0;
                }
                background = Image.FromFile(background_images[bg_number]);
                backgroundPosition = 0;
                drumMoveTime = 0;
                enemyX = 300;
            }

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
            {
                goLeft = false;
                goRight = false;
                directionPressed = false;
                playerInstance.ResetPlayer(ref player, ref num, ref playingAction, ref totalFrame, ref endFrame);
            }

            if (e.KeyCode == Keys.Z && !playingAction && !goLeft && !goRight)
            {
                playerInstance.SetPlayerAction("../../Resources/punch2.gif", ref actionStrenght, 2, ref player, ref totalFrame, ref endFrame, ref playingAction);
                
            }

            if (e.KeyCode == Keys.X && !playingAction && !goLeft && !goRight)
            {
                playerInstance.SetPlayerAction("../../Resources/punch1.gif", ref actionStrenght, 5, ref player, ref totalFrame, ref endFrame, ref playingAction);
            }
            if (e.KeyCode == Keys.C && !playingAction && !goLeft && !goRight && !shootFireBall)
            {
                playerInstance.SetPlayerAction("../../Resources/fireball.gif", ref actionStrenght, 20, ref player, ref totalFrame, ref endFrame, ref playingAction);
            }
        }



        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(background, new Point(backgroundPosition, 0));
            e.Graphics.DrawImage(player, new Point(playerX, playerY));
            e.Graphics.DrawImage(enemy, new Point(enemyX, enemyY));

            // Dibuja la barra de vida del player
            float healthPlayerRatio = (float)playerCurrentHealth / playerMaxHealth;
            int currentHealPlayerthBarWidth = (int)(healthBarWidth * healthPlayerRatio);

            // Dibujar fondo de la barra de salud (color rojo)
            e.Graphics.FillRectangle(Brushes.Red, new Rectangle(playerX, playerY - 20, healthBarWidth, 10));
            // Dibujar salud actual (color verde)
            e.Graphics.FillRectangle(Brushes.Green, new Rectangle(playerX, playerY - 20, currentHealPlayerthBarWidth, 10));
            // Dibujar el puntaje del player
            e.Graphics.DrawString("Puntaje: " + playerScore, new Font("Arial", 12, FontStyle.Bold), Brushes.White, new Point(10, 10));


            // Dibujar barra de vida del enemigo:
            float healthRatio = (float)enemyCurrentHealth / enemyMaxHealth;
            int currentHealthBarWidth = (int)(healthBarWidth * healthRatio);

            // Dibujar fondo de la barra de salud (color rojo)
            e.Graphics.FillRectangle(Brushes.Red, new Rectangle(enemyX, enemyY - 20, healthBarWidth, 10));
            // Dibujar salud actual (color verde)
            e.Graphics.FillRectangle(Brushes.Green, new Rectangle(enemyX, enemyY - 20, currentHealthBarWidth, 10));



            // El dibujado de la fireball
            if (shootFireBall)
            {
                e.Graphics.DrawImage(fireball, new Point(fireballX, fireballY));
            }
        }

        // ----------------- GAMELOGIC MANAGEMENT -----------------
        private void GameTimerEvent(object sender, EventArgs e)
        {
            this.Invalidate();
            ImageAnimator.UpdateFrames();
            gameLogic.MovePlayerandBackground(goLeft, goRight, ref playerX, ref enemyX, ref backgroundPosition, player, background, this.ClientSize.Width);
            bool didPlayerWinByPunch = gameLogic.CheckPunchHit(playerX, playerY, player, enemyX, enemyY, enemy, playingAction, num, endFrame, ref enemyCurrentHealth, actionStrenght);

            gameLogic.CheckPunchHit(playerX, playerY, player, enemyX, enemyY, enemy, playingAction, num, endFrame, ref enemyCurrentHealth, actionStrenght);
            enemyInstance.EnemyAI(playerX, ref enemyX, ref enemy, ref enemyPlayingAction, ref enemyNum, ref enemyTotalFrame, ref endFrame, ref enemyState, enemyWalkingLeft, enemyWalkingRight, ref enemyAttackCooldown, ENEMY_MAX_COOLDOWN);
            gameLogic.EnemyPunchHit(playerX, playerY, player, enemyX, enemyY, enemy, enemyPlayingAction, ref playerCurrentHealth);

            if (didPlayerWinByPunch)
            {
                playerScore += 100;  
                gameLogic.RestartGame(
    ref enemyCurrentHealth,
    ref playerCurrentHealth,
    ref playerX,
    ref playerY,
    ref enemyX,
    ref enemyY,
    ref fireballX,
    ref fireballY,
    ref drumMoveTime,
    ref actionStrenght,
    ref endFrame,
    ref backgroundPosition,
    ref num,
    ref goLeft,
    ref goRight,
    ref directionPressed,
    ref playingAction,
    ref shootFireBall,
    ref hasPlayerWon
            );
            }

            if (playerCurrentHealth <= 0)
            {
                playerScore = 0;
                gameLogic.RestartGame(
    ref enemyCurrentHealth,
    ref playerCurrentHealth,
    ref playerX,
    ref playerY,
    ref enemyX,
    ref enemyY,
    ref fireballX,
    ref fireballY,
    ref drumMoveTime,
    ref actionStrenght,
    ref endFrame,
    ref backgroundPosition,
    ref num,
    ref goLeft,
    ref goRight,
    ref directionPressed,
    ref playingAction,
    ref shootFireBall,
    ref hasPlayerWon
            );
            }

            if (enemyAttackCooldown > 0)
            {
                enemyAttackCooldown--;
            }

            if (playingAction)
            {
                if (num < totalFrame)
                {
                    num += 0.5f;
                }
            }

            if (num == totalFrame)
            {
                playerInstance.ResetPlayer(ref player, ref num, ref playingAction, ref totalFrame, ref endFrame);

            }

            if (enemyPlayingAction)
            {
                if (enemyNum < enemyTotalFrame)
                {
                    enemyNum += 0.5f;
                }
            }

            bool fireballHit = gameLogic.CheckFireballHit(fireballX, fireballY, fireball, enemyX, enemyY, enemy, ref enemyCurrentHealth, actionStrenght);

            if (fireballHit)
            {
                shootFireBall = false;
                fireballX = -100;
            }

            if (enemyNum == enemyTotalFrame)
            {
                enemyInstance.ResetEnemy(ref enemy, ref enemyNum, ref enemyPlayingAction, enemyTotalFrame, endFrame);
            }


            // Controls that the enemy does not go to the sides
            if (enemyX < 0)
            {
                enemyX = 0;
            }
            else if (enemyX + enemy.Width > this.ClientSize.Width)
            {
                enemyX = this.ClientSize.Width - enemy.Width;
            }

            if (enemyPlayingAction && num == totalFrame)
            {
                enemyInstance.ResetEnemy(ref enemy, ref enemyNum, ref enemyPlayingAction, enemyTotalFrame, endFrame); enemyPlayingAction = false; 
            }

            // fireball instructions
            if (shootFireBall)
            {
                fireballX += 10;
               gameLogic.CheckFireballHit(playerX, playerY, player, enemyX, enemyY, enemy, ref enemyCurrentHealth, actionStrenght);

            }

            if (fireballX > this.ClientSize.Width)
            {
                shootFireBall = false;
            }

            if (!shootFireBall && num > endFrame && drumMoveTime == 0 && actionStrenght == 20)
            {
                playerInstance.ShootFireball(ref fireball, ref fireballX, ref fireballY, player, playerX, playerY, ref shootFireBall);
            }

        }

        private void SetUpForm()
        {
            Timer gameTimer = new Timer
            {
                Interval = 20
            };
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Start();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            background_images = Directory.GetFiles("../../Resources/background", "*.jpg").ToList();
            background = Image.FromFile(background_images[bg_number]);
            player = Image.FromFile("../../Resources/standing.gif");
            enemy = Image.FromFile("../../Resources/enemy/standing.gif");
            playerInstance.SetUpAnimation(ref player, ref totalFrame, ref endFrame);
            enemyInstance.SetUpEnemyAnimation(ref enemy, ref enemyTotalFrame, ref endFrame);
        }
    }
}
