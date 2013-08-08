using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.CoreMotion;

namespace BlankGame
{
		public class GameOver
		{
				public Button contin,toTitle;
				Game game;
				public GameOver(Game game)
				{
					this.game = game;
					if(game.oniPad) 
					{
						contin= new Button(game,new Rectangle(300,-140,(int)(100*game.scale),(int)(20*game.scale)));
						toTitle = new Button(game, new Rectangle(300, -100, (int)(100*game.scale),(int)(20*game.scale)));
					} 
					else 
					{
						contin= new Button(game,new Rectangle(90,130,130,40));
						toTitle = new Button(game, new Rectangle(90, 170, 130, 40));
					}
				}
				public void Update()
				{
					contin.Update();
					toTitle.Update();
				}

				public void Draw(SpriteBatch spriteBatch)
				{
					if(game.oniPad) 
					{
						game.fontRenderer.DrawText(spriteBatch, 300, -220, "GAME OVER", 0.85f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 300, -180, "Score "+game.points, 0.85f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 300, -140, "Continue", 0.65f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 300, -100, "Quit", 0.65f, Color.White);

					} 
					else 
					{
						game.fontRenderer.DrawText(spriteBatch, 100, 60, "GAME OVER", 0.65f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 100, 100, "Score "+game.points, 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 100, 140, "Continue", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 100, 180, "Quit", 0.45f, Color.White);
					}

				}
		}
}

