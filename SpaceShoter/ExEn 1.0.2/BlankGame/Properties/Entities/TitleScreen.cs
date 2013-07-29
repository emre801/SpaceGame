using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class TitleScreen
		{
			Sprite start,options,other;
			Game game;
			public Button startB,optionsB;
			
			public TitleScreen(Game game)
			{
				this.game = game;
				if(game.oniPad) 
				{
					startB= new Button(game,new Rectangle(300,-140,(int)(100*game.scale),(int)(20*game.scale)));
					optionsB = new Button(game, new Rectangle(300, -100, (int)(100*game.scale),(int)(20*game.scale)));
				} 
				else 
				{
					startB= new Button(game,new Rectangle(90,130,130,40));
					optionsB = new Button(game, new Rectangle(90, 170, 130, 40));

				}
				//start = game.getSprite("Enemy");
			}
			public void Update()
			{
				startB.Update();
				optionsB.Update();
				start = game.getSprite("Enemy");
			}

			public void Draw(SpriteBatch spriteBatch)
			{
				if(start != null) 
				{
					//spriteBatch.Draw(start.index, startB.demi, Color.White);
					//spriteBatch.Draw(start.index, optionsB.demi, Color.White);
				}

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
				if(start != null) 
				{
					//spriteBatch.Draw(start.index, new Rectangle((int)startB.mP.X, (int)startB.mP.Y, 100, 100), Color.White);
				}
			}

		}
}

