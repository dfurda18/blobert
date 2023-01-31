package com.urdapilleta.qbert;

import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.Pixmap.Format;
import com.badlogic.gdx.graphics.g2d.PolygonRegion;
import com.badlogic.gdx.graphics.g2d.PolygonSprite;
import com.badlogic.gdx.graphics.g2d.PolygonSpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.Vector2;
/**
 * This class represents a graphical block object for the gameplay in qbert.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class BoardBlock {
	private PolygonSprite frontPolygon;		// The front polygon
	private PolygonSprite backPolygon;		// The back polygon
	private PolygonSprite topOnPolygon;		// The top polygon when it is on
	private PolygonSprite topOffPolygon;	// The top polygon when it is off
	private Texture textureFront;			// The front texture
	private Texture textureBack;			// The back texture
	private Texture textureOn;				// The top texture when it is on
	private Texture textureOff;				// The top texture when it is off
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
	public BoardBlock(Vector2 location) {
		textureFront = makeTextureBox(1, 1, Pixmap.Format.RGBA8888,  0xDEADBEFF); 			// Make the texture for the front edge
		textureBack = makeTextureBox(1, 1, Pixmap.Format.RGBA8888,  0x667788FF);			// Make the texture for the back edge
		textureOn = makeTextureBox(1, 1, Pixmap.Format.RGBA8888,  0x22FF88FF);				// Make the texture for the top edge when it is on
		textureOff = makeTextureBox(1, 1, Pixmap.Format.RGBA8888,  0xFF2288FF);				// Make the texture for the top edge when it is off
		PolygonRegion polyReg = new PolygonRegion(new TextureRegion(textureFront),			// Create a polygon for the front square
		  new float[] {      	// Four vertices
		    0, 20,            	// Vertex 0         3--2
		    80, 0,          	// Vertex 1         | /|
		    80, 80,        		// Vertex 2         |/ |
		    0, 100           	// Vertex 3         0--1
		}, new short[] {
		    0, 1, 2,         // Two triangles using vertex indices.
		    0, 2, 3          // Take care of the counter-clockwise direction. 
		});
		frontPolygon = new PolygonSprite(polyReg);											// Create the front polygon sprite
		frontPolygon.setOrigin(0, 0);														// Set the origin
		frontPolygon.translate(location.x, location.y);										// Translate it to its position
		PolygonRegion polyRegBack = new PolygonRegion(new TextureRegion(textureBack),		// Create a polygon for the back square
		  new float[] {      	// Four vertices
		    85, 0,            	// Vertex 0         3--2
		    165, 20,          	// Vertex 1         | /|
		    165, 100,        	// Vertex 2         |/ |
		    85, 80           	// Vertex 3         0--1
		}, new short[] {
		    0, 1, 2,         // Two triangles using vertex indices.
		    0, 2, 3          // Take care of the counter-clockwise direction. 
		});
		backPolygon = new PolygonSprite(polyRegBack);										// Create the back polygon sprite
		backPolygon.setOrigin(0, 0);														// Set the origin
		backPolygon.translate(location.x, location.y);										// Translate it to its position
		PolygonRegion polyRegOff = new PolygonRegion(new TextureRegion(textureOff),			// Create a polygon for the top-off square
		  new float[] {      	// Four vertices
		    83, 84,            	// Vertex 0         3--2
		    165, 104,          	// Vertex 1         | /|
		    83, 125,        	// Vertex 2         |/ |
		    0, 104           	// Vertex 3         0--1
		}, new short[] {
		    0, 1, 2,         // Two triangles using vertex indices.
		    0, 2, 3          // Take care of the counter-clockwise direction. 
		});
		topOffPolygon = new PolygonSprite(polyRegOff);										// Create the top-off polygon sprite
		topOffPolygon.setOrigin(0, 0);														// Set the origin
		topOffPolygon.translate(location.x, location.y);									// Translate it to its position
		PolygonRegion polyRegOn = new PolygonRegion(new TextureRegion(textureOn),			// Create a polygon for the top-on square
		  new float[] {      	// Four vertices
		    83, 84,            	// Vertex 0         3--2
		    165, 104,          	// Vertex 1         | /|
		    83, 125,        	// Vertex 2         |/ |
		    0, 104           	// Vertex 3         0--1
		}, new short[] {
		    0, 1, 2,         // Two triangles using vertex indices.
		    0, 2, 3          // Take care of the counter-clockwise direction. 
		});
		topOnPolygon = new PolygonSprite(polyRegOn);										// Create the top-on polygon sprite
		topOnPolygon.setOrigin(0, 0);														// Set the origin
		topOnPolygon.translate(location.x, location.y);										// Translate it to its position
	}
	/**
	 * Deletes the graphical elements from memory.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void dispose() {
		this.textureFront.dispose();		// Dispose the front texture
		this.textureBack.dispose();			// Dispose the back texture
		this.textureOn.dispose();			// Dispose the top on texture
		this.textureOff.dispose();			// Dispose the top off texture
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
	private Texture makeTextureBox(int width, int height, Format format, int color) {
		Pixmap pix = new Pixmap(1, 1, format);		// Create a pixmap
		pix.setColor(color);						// Set its color
		pix.fill();									// Fill the pixmap
		return new Texture(pix);					// Return the new texture
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
	public void draw(PolygonSpriteBatch polyBatch, boolean active) {
		this.frontPolygon.draw(polyBatch);			// Draw the front edge
		this.backPolygon.draw(polyBatch);			// Draw the back edge
		if(active) {								// Check if the block should be on
			this.topOnPolygon.draw(polyBatch);		// Draw the top on edge
		} else {
			this.topOffPolygon.draw(polyBatch);		// Draw the top off edge
		}
	}
}
