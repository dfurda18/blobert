using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QbertCSharp
{
    /**
     * This class represents A Qbert player.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    internal class QbertPlayer : QbertBeing
    {
        private int lives;         // The player's lives
        /**
         * Creates a new QbertPlayer.
         * @param texture The spritesheet
         * @return A new instance of the QbertPlayer
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public QbertPlayer(Texture2D texture) : base(texture, 0, 8)
        {
            this.lives = 3;                     // Set the initial lives to 3
        }
        /**
         * Return the amount of lives the player has.
         * @return The amount of lives the player has.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public int GetLives()
        {
            return this.lives;      // Return the amount of lives the player has.
        }
        /**
         * Make the player lose a life
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void LoseLives()
        {
            this.lives--;           // Reduce the player's lives by 1
        }
        /**
         * Return true if the player is dead
         * @return True if the player is dead.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool IsDead()
        {
            return this.lives <= 0;     // Return true if the player's lives are less or equal than 0
        }
        /**
         * Respawns the player in the top position.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Respawn()
        {
            base.Respawn();                             // Call the parent respawn method
            this.SetPosition(new Vector2(0, 0));        // Set the player's position to the top position

        }
        /**
         * Adds one life to the player
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void OneUp()
        {
            this.lives++;       // Increase the player's lives by 1
        }
    }
}
