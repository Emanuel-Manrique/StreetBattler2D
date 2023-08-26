using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace StreetFighterGame
{
    public class Enemy
    {
        private readonly Control _controlToUpdate;

        /// Constructor to initialize the Enemy class with the control to be updated.
        public Enemy(Control controlToUpdate)
        {
            _controlToUpdate = controlToUpdate;
        }

        /// Handler for the OnFrameChanged event, which updates the control when an animation frame changes.
        private void OnFrameChangedHandler(object sender, EventArgs e)
        {
            _controlToUpdate.Invalidate();
        }

        // ----------------- ANIMATION SETUP -----------------

        /// Sets up the enemy's animation properties.
        public void SetUpEnemyAnimation(ref Image enemy, ref int enemyTotalFrame, ref int endFrame)
        {
            ImageAnimator.Animate(enemy, this.OnFrameChangedHandler);
            FrameDimension dimensions = new FrameDimension(enemy.FrameDimensionsList[0]);
            enemyTotalFrame = enemy.GetFrameCount(dimensions);
            endFrame = enemyTotalFrame - 3;
        }

        /// Manages the enemy's attack animation.
        public void EnemyAttack(ref Image enemy, ref bool enemyPlayingAction, ref float enemyNum, int enemyTotalFrame, int endFrame, ref int enemyAttackCooldown, ref string enemyState, int ENEMY_MAX_COOLDOWN)
        {
            if (!enemyPlayingAction && enemyAttackCooldown == 0)
            {
                enemy = Image.FromFile("../../Resources/enemy/punch1.gif");
                SetUpEnemyAnimation(ref enemy, ref enemyTotalFrame, ref endFrame);
                enemyState = "punch";
                enemyPlayingAction = true;
                enemyAttackCooldown = ENEMY_MAX_COOLDOWN;
            }
            else if (enemyPlayingAction && enemyNum >= enemyTotalFrame)
            {
                ResetEnemy(ref enemy, ref enemyNum, ref enemyPlayingAction, enemyTotalFrame, endFrame);
            }
        }

        /// Manages the enemy's movement animations.
        private void MoveEnemyAnimation(string direction, ref Image enemy, ref string enemyState, Image enemyWalkingLeft, Image enemyWalkingRight)
        {
            if (direction == "left" && enemyState != "walkingLeft")
            {
                enemy = enemyWalkingLeft;
                enemyState = "walkingLeft";
                ImageAnimator.StopAnimate(enemy, this.OnFrameChangedHandler);
                ImageAnimator.Animate(enemy, this.OnFrameChangedHandler);
            }
            else if (direction == "right" && enemyState != "walkingRight")
            {
                enemy = enemyWalkingRight;
                enemyState = "walkingRight";
                ImageAnimator.StopAnimate(enemy, this.OnFrameChangedHandler);
                ImageAnimator.Animate(enemy, this.OnFrameChangedHandler);
            }
        }

        // ----------------- ENEMY AI -----------------

        /// Implements a basic enemy AI logic, deciding when to attack or move.
        public void EnemyAI(int playerX, ref int enemyX, ref Image enemy, ref bool enemyPlayingAction, ref float enemyNum, ref int enemyTotalFrame, ref int endFrame, ref string enemyState, Image enemyWalkingLeft, Image enemyWalkingRight, ref int enemyAttackCooldown, int ENEMY_MAX_COOLDOWN)
        {
            int distanceToPlayer = playerX - enemyX;

            if (Math.Abs(distanceToPlayer) < 50)
            {
                EnemyAttack(ref enemy, ref enemyPlayingAction, ref enemyNum, enemyTotalFrame, endFrame, ref enemyAttackCooldown, ref enemyState, ENEMY_MAX_COOLDOWN);
                return;
            }
            else if (distanceToPlayer < 0)
            {
                enemyX -= 3;
                MoveEnemyAnimation("left", ref enemy, ref enemyState, enemyWalkingLeft, enemyWalkingRight);
            }
            else
            {
                enemyX += 3;
                MoveEnemyAnimation("right", ref enemy, ref enemyState, enemyWalkingLeft, enemyWalkingRight);
            }
        }

        // ----------------- ENEMY STATE MANAGEMENT -----------------

        /// Resets the enemy's state to standing.
        public void ResetEnemy(ref Image enemy, ref float enemyNum, ref bool enemyPlayingAction, int enemyTotalFrame, int endFrame)
        {
            enemy = Image.FromFile("../../Resources/enemy/standing.gif");
            SetUpEnemyAnimation(ref enemy, ref enemyTotalFrame, ref endFrame);
            enemyNum = 0;
            enemyPlayingAction = false;
        }
    }
}
