using System.Drawing;

namespace StreetFighterGame
{
    public class GameLogic
    {
        /// Controls the movement of the player and the background.
        public void MovePlayerandBackground(
            bool goLeft, bool goRight,
            ref int playerX, ref int enemyX, ref int backgroundPosition,
            Image player, Image background, int clientWidth)
        {
            if (goLeft && playerX > 0)
            {
                playerX -= 6;

                if (backgroundPosition < 0 && playerX < 100)
                {
                    backgroundPosition += 5;
                    enemyX += 5;
                }
            }

            if (goRight && playerX + player.Width < clientWidth)
            {
                playerX += 6;

                if (backgroundPosition + background.Width > clientWidth + 5 && playerX > 650)
                {
                    backgroundPosition -= 5;
                    enemyX -= 5;
                }
            }
        }

        /// Detects if two objects collide based on their position and dimensions.
        public bool DetectCollision(
            int object1X, int object1Y, int object1Width, int object1Height,
            int object2X, int object2Y, int object2Width, int object2Height)
        {
            return !(object1X + object1Width <= object2X || object1X >= object2X + object2Width ||
                     object1Y + object1Height <= object2Y || object1Y >= object2Y + object2Height);
        }

        /// Checks if the player's punch hits the enemy.
        public bool CheckPunchHit(
            int playerX, int playerY, Image player,
            int enemyX, int enemyY, Image enemy,
            bool playingAction, float num, int endFrame,
            ref int enemyCurrentHealth, int actionStrength)
        {
            bool hasPlayerWon = false;

            bool collision = DetectCollision(playerX, playerY, player.Width, player.Height, enemyX, enemyY, enemy.Width, enemy.Height);

            if (collision && playingAction && num > endFrame)
            {
                enemyCurrentHealth -= actionStrength;

                if (enemyCurrentHealth <= 0)
                {
                    enemyCurrentHealth = 0;
                    hasPlayerWon = true;
                }
            }

            return hasPlayerWon;
        }

        /// Checks if the player's fireball hits the enemy.
        public bool CheckFireballHit(
            int fireballX, int fireballY, Image fireball,
            int enemyX, int enemyY, Image enemy,
            ref int enemyCurrentHealth, int actionStrength)
        {
            if (fireball == null || enemy == null)
                return false;

            bool hasPlayerWon = false;

            bool collision = DetectCollision(fireballX, fireballY, fireball.Width, fireball.Height, enemyX, enemyY, enemy.Width, enemy.Height);

            if (collision)
            {
                enemyCurrentHealth -= actionStrength;

                if (enemyCurrentHealth <= 0)
                {
                    enemyCurrentHealth = 0;
                    hasPlayerWon = true;
                }
            }

            return hasPlayerWon;
        }

        /// Checks if the enemy's punch hits the player.
        public void EnemyPunchHit(
            int playerX, int playerY, Image player,
            int enemyX, int enemyY, Image enemy,
            bool enemyPlayingAction, ref int playerCurrentHealth)
        {
            bool collision = DetectCollision(playerX, playerY, player.Width, player.Height, enemyX, enemyY, enemy.Width, enemy.Height);

            if (playerCurrentHealth <= 0)
            {
                playerCurrentHealth = 0;
            }

            if (collision && enemyPlayingAction)
            {
                playerCurrentHealth -= 1;
            }
        }

        /// Resets the game's variables to their initial values.
        public void RestartGame(
            ref int enemyCurrentHealth, ref int playerCurrentHealth,
            ref int playerX, ref int playerY, ref int enemyX, ref int enemyY,
            ref int fireballX, ref int fireballY, ref int drumMoveTime,
            ref int actionStrength, ref int endFrame, ref int backgroundPosition,
            ref float num, ref bool goLeft, ref bool goRight,
            ref bool directionPressed, ref bool playingAction,
            ref bool shootFireball, ref bool hasPlayerWon)
        {
            enemyCurrentHealth = 100;
            playerCurrentHealth = 100;
            playerX = 0;
            playerY = 300;
            enemyX = 550;
            enemyY = 300;
            fireballX = 0;
            fireballY = 0;
            drumMoveTime = 0;
            actionStrength = 0;
            endFrame = 0;
            backgroundPosition = 0;
            num = 0;
            goLeft = false;
            goRight = false;
            directionPressed = false;
            playingAction = false;
            shootFireball = false;
            hasPlayerWon = false;
        }
    }
}
