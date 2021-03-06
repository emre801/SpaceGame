using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class TitleScreen
		{
			Sprite start;//,options,other;
			Game game;
			Sprite title;
			public Button startB,optionsB,controlsB;
			
			public TitleScreen(Game game)
			{
				this.game = game;
				if(game.isIpad()) 
				{
					//startB= new Button(game,new Rectangle(300,-140,(int)(100*game.scale),(int)(40*game.scale)));
					//optionsB = new Button(game, new Rectangle(300, -100, (int)(100*game.scale),(int)(40*game.scale)));
				//controlsB = new Button(game, new Rectangle(300, -60, (int)(100*game.scale),(int)(40*game.scale)));

					startB= new Button(game,new Rectangle(180+200,150-215,(int)(130*2.5f),(int)(40*2.5f)));
					optionsB = new Button(game, new Rectangle(180+200, 200-150, (int)(130*2.5f), (int)(40*2.5f)));
					controlsB = new Button(game, new Rectangle(180+200, 265-100, (int)(130*2.5f), (int)(40*2.5f)));
				} 
				else 
				{
					startB= new Button(game,new Rectangle(180,150,130,40));
					optionsB = new Button(game, new Rectangle(180, 200, 130, 40));
					controlsB = new Button(game, new Rectangle(180, 265, 130, 40));
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
					//spriteBatch.Draw(start.index, controlsB.demi, Color.White);
				}
				if(title==null)
				{
					if(!game.isIpad())
						title=game.getSprite("Title");
					else
						title=game.getSprite("TitleBig");
				}
				if(game.oniPad) 
				{
					//game.fontRenderer.DrawText(spriteBatch, 300-game.xAnimation, -180, "Commander Cat Hat", 0.85f, Color.White);
					//game.fontRenderer.DrawText(spriteBatch, 300-game.xAnimation, -140, "Start", 0.65f, Color.White);
					//game.fontRenderer.DrawText(spriteBatch, 300-game.xAnimation, -100, "Options", 0.65f, Color.White);
					//game.fontRenderer.DrawText(spriteBatch, 300-game.xAnimation, -60, "Controls", 0.65f, Color.White);
					spriteBatch.Draw(title.index,new Rectangle(175-game.xAnimation,-350,(int)(title.index.Width),(int)(title.index.Height)),Color.White);
				} 
				else 
				{
					spriteBatch.Draw(title.index,new Rectangle(0-game.xAnimation,-20,(int)(title.index.Width),(int)(title.index.Height)),Color.White);

					//game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 100, "Commander Cat Hat", 0.65f, Color.White);
					//game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 140, "Start", 0.45f, Color.White);
					//game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 180, "Options", 0.45f, Color.White);
					//game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 220, "Controls", 0.45f, Color.White);
				}
				if(start != null) 
				{
					//spriteBatch.Draw(start.index, new Rectangle((int)startB.mP.X, (int)startB.mP.Y, 100, 100), Color.White);
				}
			}

		}
}

