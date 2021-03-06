using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class LetterButton
		{
			Button top;
			Button bottom;
			Vector2 pos;
			Game g;
			public char c;
			Sprite incre;
			Stopwatch ticker;
			public LetterButton(Game g,Vector2 pos, char c)
			{
				this.g=g;
				this.pos = pos;
				this.bottom = new Button(g, new Rectangle((int)pos.X, (int)pos.Y + 20, 20, 20));
				this.top = new Button(g, new Rectangle((int)pos.X, (int)pos.Y - 20, 20, 20));
				this.c = c;
				ticker = new Stopwatch();
				ticker.Start();
				
			}
			public void Update()
			{
				top.Update();
				bottom.Update();
				ticker.Stop();
				int time = (int)ticker.ElapsedMilliseconds;
				bool allowInput = false;
				if(time >= 250) 
				{
					ticker.Restart();
					allowInput = true;
				}
				else
					ticker.Start();
				if(top.isPressed) 
				{
					if(allowInput) 
					{
						c++;
						g.mp.playSound("menu");
					}
				}
				if(bottom.isPressed) 
				{
					if(allowInput)
					{	
						c--;
						g.mp.playSound("menu");
					}
				}
				if(c < 'A')
					c = 'Z';
				if(c > 'Z')
					c = 'A';

			}
			public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
			{
				if(g.ignoreDraw) 
				{
					//g.ignoreDraw = false;
					return;
				}
				if(incre==null)
					incre = g.getSprite("incre");
				spriteBatch.Draw(incre.index, top.demi, Color.White);
				spriteBatch.Draw(incre.index, bottom.demi, Color.White);
				
				g.fontRenderer.DrawText(spriteBatch, (int)pos.X+g.xAnimation, (int)pos.Y, c + "", 0.45f, Color.White);

				
			}
		}
}

