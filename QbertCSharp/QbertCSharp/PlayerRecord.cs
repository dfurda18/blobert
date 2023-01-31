using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QbertCSharp
{
    /**
     * This class represents a player record.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    internal class PlayerRecord
    {
        public const short NAME_SIZE = 3;                   // The record's name size
        private char[] name;                                // The name
        private int score;                                  // The score
        /**
         * PlayerRecord constructor given its name and score.
         * @param name The record's name.
         * @param score The record's score
         * @return A new instance of the PlayerRecord class.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public PlayerRecord(char[] name, int score)
        {
            this.name = new char[NAME_SIZE];                                    // Create the name array
            for (int nameCounter = 0; nameCounter < NAME_SIZE; nameCounter++)
            {   // Loop through the array
                this.name[nameCounter] = name[nameCounter];                     // Copy it from the parameter name
            }
            this.score = score;                                                 // Set the score
        }
        /**
         * Returns the record score.
         * @return The record's score.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public int GetScore()
        {
            return this.score;                      // Return the score
        }
        /**
         * Returns the name in the form of a String
         * @return The name in the form of a String
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public String GetStringName()
        {
            return this.name[0].ToString() + this.name[1].ToString() + this.name[2].ToString();
        }
    }
}
