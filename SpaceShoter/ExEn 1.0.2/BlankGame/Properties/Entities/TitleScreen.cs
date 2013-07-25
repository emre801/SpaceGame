using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class TitleScreen
		{
			Sprite start,options,other;
			Game game;
			public TitleScreen(Game game)
			{
				this.game = game;
			}
			public void Update()
			{


			}

			public void Draw(SpriteBatch spriteBatch)
			{
				if(game.oniPad) 
				{
					game.fontRenderer.DrawText(spriteBatch, 300, -180, "Commander Cat Hat", 0.85f, Color.White);
					game.fontRenderer.DrawText(spriteBatch, 300, -140, "Start", 0.65f, Color.White);
					game.fontRenderer.DrawText(spriteBatch, 300, -100, "Options", 0.65f, Color.White);

				} 
				else 
				{
					game.fontRenderer.DrawText(spriteBatch, 100, 100, "Commander Cat Hat", 0.65f, Color.White);
					game.fontRenderer.DrawText(spriteBatch, 100, 140, "Start", 0.45f, Color.White);
					game.fontRenderer.DrawText(spriteBatch, 100, 180, "Options", 0.45f, Color.White);
				}
			}

		}
}

