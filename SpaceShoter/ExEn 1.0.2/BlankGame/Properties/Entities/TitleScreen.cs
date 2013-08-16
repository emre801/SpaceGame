using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class TitleScreen
		{
			Sprite start;//,options,other;
			Game game;
			public Button startB,optionsB,controlsB;
			
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
					controlsB = new Button(game, new Rectangle(90, 210, 130, 40));

				}
				//start = game.getSprite("Enemy");
			}
			public void Update()
			{
				if(game.tick.hasTicked && game.xAnimation > 0 && game.isOpening) 
				{
					game.xAnimation -= 20;
					if(game.xAnimation <= 0)
						game.isOpening = false;

				}
				else if(game.tick.hasTicked && game.xAnimation < game.maxMoveThing && game.isClosing)
				{
					game.xAnimation += 20;
					if(game.xAnimation >= game.maxMoveThing) 
					{
						if(game.titlePress==1)
							game.gameState = Game.GameState.OPTIONS;
						else if(game.titlePress==2)
							game.gameState=Game.GameState.GAMETIME;
						else
							game.gameState=Game.GameState.CONTROLS;
						game.isClosing = false;
						game.isOpening = true;
						game.tempIgnore = true;
					}
				}
				
				if(game.xAnimation < 0)
					game.xAnimation = 0;
				if(game.xAnimation > game.maxMoveThing)
					game.xAnimation = game.maxMoveThing;

				startB.Update();
				optionsB.Update();
				controlsB.Update();
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
					game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 100, "Commander Cat Hat", 0.65f, Color.White);
					game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 140, "Start", 0.45f, Color.White);
					game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 180, "Options", 0.45f, Color.White);
					game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 220, "Controls", 0.45f, Color.White);
				}
				if(start != null) 
				{
					//spriteBatch.Draw(start.index, new Rectangle((int)startB.mP.X, (int)startB.mP.Y, 100, 100), Color.White);
				}
			}

		}
}

