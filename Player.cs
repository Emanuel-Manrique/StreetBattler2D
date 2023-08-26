using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;

namespace StreetFighterGame
{
    public class Player
    {
        private readonly Control _controlToUpdate;

        /// Constructor to initialize the Player class with the control to be updated.
        public Player(Control controlToUpdate)
        {
            _controlToUpdate = controlToUpdate;
        }

        /// Handler for the OnFrameChanged event, which updates the control when an animation frame changes.
        private void OnFrameChangedHandler(object sender, EventArgs e)
        {
            _controlToUpdate.Invalidate();
        }

        /// Configures the player's action, like punch or kick.
        public void SetPlayerAction(string animation, ref int actionStrength, int strength, ref Image player, ref int totalFrame, ref int endFrame, ref bool playingAction)
        {
            player = Image.FromFile(animation);
            actionStrength = strength;
            SetUpAnimation(ref player, ref totalFrame, ref endFrame);
            playingAction = true;
        }

        /// Sets up the player to shoot a fireball.
        public void ShootFireball(ref Image fireball, ref int fireballX, ref int fireballY, Image player, int playerX, int playerY, ref bool shootFireBall)
        {
            fireball = Image.FromFile("../../Resources/FireBallFinal.gif");
            ImageAnimator.Animate(fireball, this.OnFrameChangedHandler);
            fireballX = playerX + player.Width - 50;
            fireballY = playerY - 33;
            shootFireBall = true;
        }

        /// Controls the player's movement animation.
        public void MovePlayerAnimation(string direction, ref bool goLeft, ref bool goRight, ref bool directionPressed, ref bool playingAction, ref Image player, ref int totalFrame, ref int endFrame)
        {
            if (direction == "left")
            {
                goLeft = true;
                player = Image.FromFile("../../Resources/backwards.gif");
            }
            if (direction == "right")
            {
                goRight = true;
                player = Image.FromFile("../../Resources/forwards.gif");
            }

            directionPressed = true;
            playingAction = false;
            SetUpAnimation(ref player, ref totalFrame, ref endFrame);
        }

        /// Sets up the player's animation properties.
        public void SetUpAnimation(ref Image player, ref int totalFrame, ref int endFrame)
        {
            ImageAnimator.Animate(player, this.OnFrameChangedHandler);
            FrameDimension dimensions = new FrameDimension(player.FrameDimensionsList[0]);
            totalFrame = player.GetFrameCount(dimensions);
            endFrame = totalFrame - 3;
        }

        /// Resets the player's animation to a standing position.
        public void ResetPlayer(ref Image player, ref float num, ref bool playingAction, ref int totalFrame, ref int endFrame)
        {
            player = Image.FromFile("../../Resources/standing.gif");
            SetUpAnimation(ref player, ref totalFrame, ref endFrame);
            num = 0;
            playingAction = false;
        }
    }
}
