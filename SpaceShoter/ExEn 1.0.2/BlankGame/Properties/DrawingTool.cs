using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTouch.UIKit;

namespace BlankGame
{
	public class DrawingTool
	{
		readonly GraphicsDeviceManager gdm;
		//BasicEffect effect;
		SpriteBatch spriteBatch;
		Game game;
		public Camera2d cam;

		public static DrawingTool instance { get; private set; }
		public int ActualScreenPixelWidth { get; private set; }
		public int ActualScreenPixelHeight { get; private set; }

		public int RenderingAreaWidth { get; private set; }
		public int RenderingAreaHeight { get; private set; }

		public float GameToScreen { get; private set; }
		public float ScreenToGame { get; private set; }

		public bool isLetterBoxing { get; private set; }
		public float ScreenAspectRatio { get; private set; } // This is the aspect ratio of the screen, not the aspect ratio of the game world. For that, use Constants.desired_aspect_ratio
		public int TopPadding { get; private set; }
		public int BottomPadding { get; private set; }
		public int LeftPadding { get; private set; }
		public int RightPadding { get; private set; }

		public int TopRenderingEdge { get; private set; }
		public int BottomRenderingEdge { get; private set; }
		public int LeftRenderingEdge { get; private set; }
		public int RightRenderingEdge { get; private set; }

		//private PrimitiveDrawingElement letterBox;

		public float zoomRatio = 1.75f;
		public Texture2D rectangle;


		public float zoomRatioValue = 0.75f;
		TitleScreen ts;

		public DrawingTool(Game game)
		{
			this.game = game;
			instance = this;
			gdm = new GraphicsDeviceManager(game);


			gdm.PreferredBackBufferWidth = 320;
			gdm.PreferredBackBufferHeight = 480;
			this.ActualScreenPixelWidth = 320;
			this.ActualScreenPixelHeight = 480;




			cam = new Camera2d(Constants.GAME_WORLD_HEIGHT ,Constants.GAME_WORLD_WIDTH);
			cam.Pos = new Vector2(Constants.GAME_WORLD_HEIGHT/2f, Constants.GAME_WORLD_WIDTH/2f);
			cam.Rotation = 1.57079633f;
			ts = new TitleScreen(game);

		}

		public void updateCameraRotation()
		{
			//GraphicsDevice.PresentationParameters.DisplayOrientation

		}

		public GraphicsDevice getGraphicsDevice()
		{
			return gdm.GraphicsDevice;
		}

		public void initialize()
		{
			spriteBatch = new SpriteBatch(gdm.GraphicsDevice);          

		}
		public void resetCamera()
		{


		}
		private void initializeLetterBox()
		{}

		public void drawLetterBox()
		{

		}

		private void calculateRenderingArea()
		{

		}


		#region coordinate conversions

		// Returns a Point (x and y integers corresponding to a pixel) based on a Vector in game space
		public Point gameCoordsToScreenCoords(Vector2 loc)
		{
			return gameCoordsToScreenCoords(loc.X, loc.Y);
		}

		public int gameXCoordToScreenCoordX(float gc)
		{
			return LeftRenderingEdge + (int)gameToScreen(gc);
		}

		public int gameYCoordToScreenCoordY(float gc)
		{
			return TopRenderingEdge + (int)gameToScreen(gc);
		}

		// Returns a Point (x and y integers corresponding to a pixel) based on x and y game space coordinates
		public Point gameCoordsToScreenCoords(float x, float y)
		{
			return new Point(LeftRenderingEdge + (int)gameToScreen(x), TopRenderingEdge + (int)gameToScreen(y));
		}

		// Returns a Vector representing a location in game space given a pixel in screen space
		public Vector2 screenCoordsToGameCoords(Point pixel)
		{
			return screenCoordsToGameCoords(pixel.X, pixel.Y);
		}

		public Vector2 screenCoordsToGameCoords(int x, int y)
		{
			return new Vector2(screenToGame(x - LeftRenderingEdge), screenToGame(y - TopRenderingEdge));
		}

		public float gameToScreen(float c)
		{
			//return (int)Math.Round(c * Constants.GAME_TO_SCREEN, 0);
			return c * GameToScreen;
		}

		public Vector2 getDrawingCoords(Vector2 gameWorldCoords)
		{
			return new Vector2(LeftRenderingEdge + gameToScreen(gameWorldCoords.X), TopRenderingEdge + gameToScreen(gameWorldCoords.Y));
		}

		public float screenToGame(int c)
		{
			return c * ScreenToGame;
		}

		#endregion

		public void drawLoadingText()
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.get_transformation(gdm.GraphicsDevice /*Send the variable that has your graphic device here*/));
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			//Sprite spalshArt = game.getSprite("c801");
			//spriteBatch.Draw(spalshArt.index, new Rectangle((int)(ActualScreenPixelHeight*0.25f),
			 //                                               (int)(ActualScreenPixelWidth*0.85f),
			//                                                (int)(spalshArt.index.Width*0.15f*game.scale), (int)(spalshArt.index.Height*0.15f*game.scale)), null, Color.White*game.splashFadeOut, 0, new Vector2((spalshArt.index.Width*0.15f*game.scale)/2f, (spalshArt.index.Height*0.15f*game.scale)/2f), SpriteEffects.None, 0f);


			//Sprite spalshArt2 = game.getSprite("start");
			//spriteBatch.Draw(spalshArt2.index, new Rectangle((int)(ActualScreenPixelHeight*0.25f),
			//                                                (int)(ActualScreenPixelWidth*0.85f),
			//                                                (int)(spalshArt2.index.Width*0.15f*game.scale), (int)(spalshArt2.index.Height*0.15f*game.scale)), null, Color.White*game.splashFadeOut, 0, new Vector2((spalshArt.index.Width*0.15f*game.scale)/2f, (spalshArt.index.Height*0.15f*game.scale)/2f), SpriteEffects.None, 0f);


			spriteBatch.End();
		}
		private void endBatch()
		{
			spriteBatch.End();
		}

		public Vector2 oldPosition=Vector2.Zero;
		private void followPlayer()
		{
			/*

			float widthRatio = 0.85f;
			float heightRatio = 0.85f;//
			float heightRatio2 = 0.70f;//
			float width = widthRatio * cam.ViewportWidth;// *zoomRatio;
			float height = heightRatio * cam.ViewportHeight;// *zoomRatio;
			float height2 = heightRatio2 * cam.ViewportHeight;// *zoomRatio;
			//This allows the camera to follow the player
			PlayableCharacter p1 = game.Arena.player1;
			game.moveBackGround = new Vector2(0, 0);
			oldPosition = cam.Pos;
			bool hasCamMoved = false;
			/*
			if(Constants.ON_IPHONE)
				cam.Pos = p1.Position-  new Vector2(120,280);
			else
				cam.Pos= p1.Position-  new Vector2(800,280);
				*/
			/*
			if (!(game.Arena.maxLeft > p1.Position.X || game.Arena.maxRight < p1.Position.X))
			{
				if (p1.Position.X + width > cam._pos.X + cam.ViewportWidth / 1)
				{
					hasCamMoved = true;
					cam.Move(new Vector2((p1.Position.X + width) - (cam._pos.X + cam.ViewportWidth / 1), 0));
					//cam.Move(new Vector2(10, 0));
					if (Math.Abs(game.Arena.player1.body.LinearVelocity.X) > 0.1f)
					{
						float moveAmount = (p1.Position.X + width) - (cam._pos.X + cam.ViewportWidth / 1) / 10000f;
						//game.moveBackGround -= new Vector2(moveAmount, 0);
					}
				}
				if (p1.Position.X - width < cam._pos.X - cam.ViewportWidth / 1)
				{
					hasCamMoved = true;
					cam.Move(new Vector2((p1.Position.X - width) - (cam._pos.X - cam.ViewportWidth / 1), 0));
					//cam.Move(new Vector2(-, 0));
					if (Math.Abs(game.Arena.player1.body.LinearVelocity.X) > 0.1f)
					{
						float moveAmount = (p1.Position.X + width) - (cam._pos.X + cam.ViewportWidth / 1) / 10000f;
						//game.moveBackGround += new Vector2(moveAmount, 0);
					}
				}
			}
			if (!(game.Arena.maxTop > p1.Position.Y || game.Arena.maxButtom < p1.Position.Y))
			{
				if (p1.Position.Y + height2 > cam._pos.Y + cam.ViewportHeight / 1)
				{
					hasCamMoved = true;
					cam.Move(new Vector2(0, (p1.Position.Y + height2) - (cam._pos.Y + cam.ViewportHeight / 1)));
					if (Math.Abs(game.Arena.player1.body.LinearVelocity.Y) > 0.1f)
					{
						float moveAmount = (p1.Position.Y + height2) - (cam._pos.Y + cam.ViewportHeight / 1);
						//game.moveBackGround -= new Vector2(0, moveAmount);
					}
				}
				if (p1.Position.Y - height < cam._pos.Y - cam.ViewportHeight / 1)
				{
					hasCamMoved = true;
					cam.Move(new Vector2(0, (p1.Position.Y - height) - (cam._pos.Y - cam.ViewportHeight / 1)));
					if (Math.Abs(game.Arena.player1.body.LinearVelocity.Y) > 0.1f)
					{
						float moveAmount = (p1.Position.Y - height) - (cam._pos.Y - cam.ViewportHeight / 1);
						//game.moveBackGround += new Vector2(0, moveAmount);
					}
				}
			}*/

			//Player dies if they go out of the camera bounds
			/*
			float zoomAdjustments =  zoomRatioValue/cam.Zoom ;
						if(p1.Position.X > cam.Pos.X + cam.ViewportWidth * zoomAdjustments || p1.Position.X < cam.Pos.X - cam.ViewportWidth * zoomAdjustments
								|| p1.Position.Y > cam.Pos.Y + cam.ViewportHeight * zoomAdjustments || p1.Position.Y < cam.Pos.Y - cam.ViewportHeight * zoomAdjustments) {
								game.PlayerDies();
						} else {


			if(Constants.ON_IPHONE)
				cam.Pos = p1.Position-  new Vector2(120,280);
			else
				cam.Pos= p1.Position-  new Vector2(750,-150);


			//}
			//Vector2 moveAmount = p1.Position-cam.Pos
			//game.moveBackGround -= new Vector2(moveAmount, 0);
			game.moveBackGround = (cam.Pos - oldPosition)*5;
			*/
		}


		public void moveBackgrounCamera()
		{

		}

		public void followTitle()
		{


		}
		public void controlCamera()
		{

		}

		private void beginBatch()
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		}
		internal void drawEntities(List<Entity> entities, GameTime gameTime)
		{
			beginBatch();
			if(this.game.gameState = Game.GameState.TITLE) 
			{
				ts.Update();
				ts.Draw(spriteBatch);
			} 
			else 
			{
				foreach(Entity e in entities) 
				{
					e.Draw(spriteBatch, gameTime);

				}
			}
			endBatch();

		}

		public void drawBGGradient(Texture2D bgGradient)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, cam.get_transformation(gdm.GraphicsDevice /*Send the variable that has your graphic device here*/));
			spriteBatch.Draw(bgGradient, new Rectangle(0, 0, ActualScreenPixelWidth, ActualScreenPixelHeight), Color.White * 0.65f);
			spriteBatch.End();
		}
		public void drawBorderImageForButtons(SpriteBatch spriteBatch)
		{

		}

		public void drawBorderImage(float x, float y, int height, int width, SpriteBatch spriteBatch)
		{
		}

		public void drawBorderImageFromPos(float x, float y, int height, int width, SpriteBatch spriteBatch)
		{

		}


		//Draws a simple rectangle based on given location and color
		public void drawRectangle(Rectangle rect, Color color)
		{
			spriteBatch.Draw(rectangle, rect, color);
		}

		// Draw a sprite based on a center position and an x and y radius. All parameters are in terms of game world sizes. Does not support sprite rotation
		public void drawSprite(Texture2D sprite, Vector2 center, float radiusX, float radiusY, Color blendColor)
		{

		}

		// Draws a sprite based on a center position, and a rectangle centered at that position. Parameters are in terms of game world sizes. Does not support sprite rotation
		public void drawSprite(Texture2D sprite, Vector2 center, Rectangle rect, Color blendColor)
		{

		}
		//DrawsLine be careful, to many lines cause lag....
		public void DrawLine(SpriteBatch batch,
		                     float width, Color color, Vector2 point1, Vector2 point2)
		{

		}
		public void DrawText(SpriteBatch spriteBatch, float x, float y, String text, float size)
		{
			DrawText(spriteBatch, x, y, text, size, 1);
		}

		public static int MeasureString(String s, UIFont font)
		{
			int len = s.Length * 20;

			return 0;

		}



		public void DrawText(SpriteBatch spriteBatch, float x, float y, String text, float size,float fadePercent)
		{






		}
	}
}