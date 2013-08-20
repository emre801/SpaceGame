using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class Controls
		{
			Game game;
			public Button back;
			Sprite stack;
			public Controls(Game game)
			{
				this.game = game;
				if(game.isIpad()) 
				{
					back= new Button(game,new Rectangle(300,-80,(int)(100*game.scale),(int)(50*game.scale)));
				} 
				else 
				{
					back= new Button(game,new Rectangle(90,170,130,40));
				}
			}
			
			public void Update()
			{
				if(stack==null)
				{
					stack=game.getSprite("Enemy");

				}

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
						game.gameState=Game.GameState.TITLE;
						game.isClosing = false;
						game.isOpening = true;
						game.tempIgnore = true;
					}
				}

				if(game.xAnimation < 0)
					game.xAnimation = 0;
				if(game.xAnimation > game.maxMoveThing)
					game.xAnimation = game.maxMoveThing;
				back.Update();



			}
			
			public void Draw(SpriteBatch spriteBatch)
			{
				if(stack != null) 
				{
					//spriteBatch.Draw(stack.index, back.demi, Color.White);

				}
				if(game.oniPad) 
				{
					
					game.fontRenderer.DrawText(spriteBatch, back.demi.X-game.xAnimation, back.demi.Y, "Back", 0.65f, Color.White);
				} 
				else 
				{
					
					game.fontRenderer.DrawText(spriteBatch, 100-game.xAnimation, 170, "Back", 0.45f, Color.White);
				}


			}

		}
}

