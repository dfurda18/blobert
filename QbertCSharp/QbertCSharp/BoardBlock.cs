using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace QbertCSharp
{
    /**
     * This class represents a graphical block object for the gameplay in qbert.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    internal class BoardBlock
    {
        private BasicEffect basicEffect;                    // The front effect
        private VertexPositionColor[] verticesBase;         // The front and back polygons
        private VertexPositionColor[] verticesTopOn;        // The Top On polygon
        private VertexPositionColor[] verticesTopOff;       // The Top off polygon
        private int[] indicesBase;                          // The indices to draw the front and back polygons
        private int[] indicesTop;                           // The indices to draw the top polygon

        /**
         * BoardBlock constructor given its location.
         * @param location The location of this instance.
         * @return A new instance of the BoardBlock class.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public BoardBlock(Vector2 location, GraphicsDevice graphicsDevice)
        {
            this.verticesBase = new VertexPositionColor[8];                                                                                 // Create the back and fron vertex arrays
            verticesBase[0] = new VertexPositionColor(new Vector3(0 + location.X, 1080 - location.Y, 0), new Color(222, 173, 190));         // Vertex 0         3--2
            verticesBase[1] = new VertexPositionColor(new Vector3(80 + location.X, 1060 - location.Y, 0), new Color(222, 173, 190));        // Vertex 1         | /|
            verticesBase[2] = new VertexPositionColor(new Vector3(80 + location.X, 980 - location.Y, 0), new Color(222, 173, 190));         // Vertex 2         |/ |
            verticesBase[3] = new VertexPositionColor(new Vector3(0 + location.X, 1000 - location.Y, 0), new Color(222, 173, 190));         // Vertex 3         0--1
            verticesBase[4] = new VertexPositionColor(new Vector3(85 + location.X, 1060 - location.Y, 0), new Color(102, 119, 136));        // Vertex 0         3--2
            verticesBase[5] = new VertexPositionColor(new Vector3(165 + location.X, 1080 - location.Y, 0), new Color(102, 119, 136));       // Vertex 1         | /|
            verticesBase[6] = new VertexPositionColor(new Vector3(165 + location.X, 1000 - location.Y, 0), new Color(102, 119, 136));       // Vertex 2         |/ |
            verticesBase[7] = new VertexPositionColor(new Vector3(85 + location.X, 980 - location.Y, 0), new Color(102, 119, 136));         // Vertex 3         0--1
            this.verticesTopOn = new VertexPositionColor[4];                                                                                // Create the back and fron vertex arrays
            verticesTopOn[0] = new VertexPositionColor(new Vector3(83 + location.X, 1104 - location.Y, 0), new Color(34, 255, 136));         // Vertex 0         3--2
            verticesTopOn[1] = new VertexPositionColor(new Vector3(165 + location.X, 1084 - location.Y, 0), new Color(34, 255, 136));        // Vertex 1         | /|
            verticesTopOn[2] = new VertexPositionColor(new Vector3(83 + location.X, 1063 - location.Y, 0), new Color(34, 255, 136));         // Vertex 2         |/ |
            verticesTopOn[3] = new VertexPositionColor(new Vector3(0 + location.X, 1084 - location.Y, 0), new Color(34, 255, 136));          // Vertex 3         0--1
            this.verticesTopOff = new VertexPositionColor[4];                                                                               // Create the back and fron vertex arrays
            verticesTopOff[0] = new VertexPositionColor(new Vector3(83 + location.X, 1104 - location.Y, 0), new Color(255, 34, 136));        // Vertex 0         3--2
            verticesTopOff[1] = new VertexPositionColor(new Vector3(165 + location.X, 1084 - location.Y, 0), new Color(255, 34, 136));       // Vertex 1         | /|
            verticesTopOff[2] = new VertexPositionColor(new Vector3(83 + location.X, 1063 - location.Y, 0), new Color(255, 34, 136));        // Vertex 2         |/ |
            verticesTopOff[3] = new VertexPositionColor(new Vector3(0 + location.X, 1084 - location.Y, 0), new Color(255, 34, 136));         // Vertex 3         0--1
            this.indicesBase = new int[12];                                                                                                 // Get the Front and back indices
            indicesBase[0] = 0;                                                                                                             // Sequence for the vertex
            indicesBase[1] = 1;                                                                                                             // Sequence for the vertex
            indicesBase[2] = 2;                                                                                                             // Sequence for the vertex
            indicesBase[3] = 0;                                                                                                             // Sequence for the vertex
            indicesBase[4] = 2;                                                                                                             // Sequence for the vertex
            indicesBase[5] = 3;                                                                                                             // Sequence for the vertex
            indicesBase[6] = 4;                                                                                                             // Sequence for the vertex
            indicesBase[7] = 5;                                                                                                             // Sequence for the vertex
            indicesBase[8] = 6;                                                                                                             // Sequence for the vertex
            indicesBase[9] = 4;                                                                                                             // Sequence for the vertex
            indicesBase[10] = 6;                                                                                                            // Sequence for the vertex
            indicesBase[11] = 7;                                                                                                            // Sequence for the vertex
            this.indicesTop = new int[12];                                                                                                  // Get the Front and back indices
            indicesTop[0] = 0;                                                                                                              // Sequence for the vertex
            indicesTop[1] = 1;                                                                                                              // Sequence for the vertex
            indicesTop[2] = 2;                                                                                                              // Sequence for the vertex
            indicesTop[3] = 0;                                                                                                              // Sequence for the vertex
            indicesTop[4] = 2;                                                                                                              // Sequence for the vertex
            indicesTop[5] = 3;                                                                                                              // Sequence for the vertex
            basicEffect = new BasicEffect(graphicsDevice);                                                                                  // Create the basic effect for to associate the texture
            basicEffect.World = Matrix.CreateOrthographicOffCenter(0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0, 0, 1);  // Create the world matrix based on the camera view
            basicEffect.TextureEnabled = false;                                                                                             // Disables the texture
            basicEffect.FogEnabled = false;                                                                                                 // Disables the fog
            basicEffect.LightingEnabled = false;                                                                                            // Disables the lightning
            basicEffect.World = Matrix.Identity;                                                                                            // Default world
            basicEffect.View = Matrix.Identity;                                                                                             // Default view
            basicEffect.Projection = Matrix.Identity;                                                                                       // Default projection
            basicEffect.VertexColorEnabled = true;                                                                                          // Default projection
        }
        /**
         * Deletes the graphical elements from memory.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Dispose()
        {
            this.basicEffect.Dispose();         // Dispose the front effect
        }
        /**
         * Makes a texture from size, format and color.
         * @param width The texture width.
         * @param height The texture height.
         * @param format The texture format.
         * @param color The texture color.
         * @return The newly created texture. 
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private Texture2D MakeTextureBox(Color color, GraphicsDevice graphicsDevice)
        {
            Texture2D texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);    // Create the semi transparent texture
            Color[] colorArray = new Color[1];                                                      // Create a color array
            colorArray[0] = color;                                                                  // Set the color
            texture.SetData<Color>(colorArray);                                                     // Set the color array to the texture
            return texture;                                                                         // Return the new texture
        }
        /**
         * Draws the block.
         * @param polyBatch The {@link PolygonSpriteBatch} that will render the block.
         * @param active If the block should be on or off.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Draw(GraphicsDevice graphicDevice, bool active)
        {
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, graphicDevice.Viewport.Width, 0, graphicDevice.Viewport.Height, 0f, 1f);     // Set the view projection
            foreach (EffectPass effectCounter in basicEffect.CurrentTechnique.Passes)                                                                   // Loop through the effects
            {
                effectCounter.Apply();                                                                                                                  // Apply the effect
                graphicDevice.DrawUserIndexedPrimitives<VertexPositionColor>(                                                                           // Draw the front edge
                    PrimitiveType.TriangleList, this.verticesBase, 0, this.verticesBase.Length, this.indicesBase, 0, this.indicesBase.Length / 3);
                if (active)
                {
                    graphicDevice.DrawUserIndexedPrimitives<VertexPositionColor>(                                                                           // Draw the Top On edge
                        PrimitiveType.TriangleList, this.verticesTopOn, 0, this.verticesTopOn.Length, this.indicesTop, 0, this.indicesTop.Length / 3);
                }
                else
                {
                    graphicDevice.DrawUserIndexedPrimitives<VertexPositionColor>(                                                                           // Draw the Top Off edge
                        PrimitiveType.TriangleList, this.verticesTopOff, 0, this.verticesTopOff.Length, this.indicesTop, 0, this.indicesTop.Length / 3);
                }
            }
        }
    }
}
