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
     * This class represents A Qbert enemy.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    internal class QbertEnemy : QbertBeing
    {
        private const long lowestTimeToAct = 3;   // The shortest time to make a movement
        private const long highestTimeToAct = 8;  // The longest time to make a movement
        private double timeToAct;                    // The time to wait for the next act
        private double timer;                        // The timer
        private bool readyToMove;                   // A variable if it is ready to move
        /**
         * Creates a new instance of a QbertEnemy.
         * @param texture The spritesheet
         * @return A new instance of the QbertEnemy
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public QbertEnemy(Texture2D texture) : base(texture, 5, 8)
        {
            this.timer = 0;                         // Set the timer to 0
            this.readyToMove = false;               // Start by not being ready to move.
        }
        /**
         * Notifies if the enemy is ready to move.
         * @return True if the enemy is ready to move.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool IsReadyToMove()
        {
            return this.readyToMove;        // Return true if the enemy is ready to move
        }
        /**
         * Respawns the enemy.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Respawn()
        {
            base.Respawn();        // Call the parent's respawn method
        }
        /**
         * Updates the enemy.
         * @param timeSlice The time passed since last update
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);                  // Call the parent update
            if (!this.CanBeDrawn())        // If the enemy doesnt have a position
            {
                this.pleaseRespawn = true;          // Request to be spawned
                this.ResetTimeToAct();              // Reset the time to act
            }
            this.timer += gameTime.ElapsedGameTime.TotalSeconds;                // Add the time slice to the timer
            if (this.timer >= this.timeToAct)       // Check if the timer is larger than the time to act
            {
                this.ResetTimeToAct();              // Reset the time to act
                this.readyToMove = true;            // The enemy is ready to move
            }
        }
        /**
         * Notifies if the enemy can be drawn.
         * @return True if the enemy has a position so it can be drawn.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool CanBeDrawn()
        {
            return this.GetPosition() != null;      // Return true if the enemy has a position
        }
        /**
         * Moves an enemy towards a direction
         * @rparam direction The direction to move.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Move(int direction)
        {
            base.Move(direction);      // Call the parent's move method
            this.readyToMove = false;   // The enemy is no longer ready to move
            this.ResetTimeToAct();      // Reset the time to act
        }
        /**
         * Resets the time to act
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void ResetTimeToAct()
        {
            Random random = new Random();                   // Create a random generator
            this.timer = 0;                                 // Set the timer to 0
            this.timeToAct = (float)random.NextInt64(QbertEnemy.highestTimeToAct - QbertEnemy.lowestTimeToAct);  // Gets a random number between the difference of the shortest and longest
            this.timeToAct += QbertEnemy.lowestTimeToAct;   // Adds the shortest to the new time to act
        }
    }
}
