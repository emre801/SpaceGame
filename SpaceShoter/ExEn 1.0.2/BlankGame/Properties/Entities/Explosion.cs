using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BlankGame
{
		public class Explosion: Entity
		{
				Vector2 p1,p2,p3,p4,p5,p6,p7,p8,pos;
				Sprite image;
				public Explosion(Game g, Vector2 pos)
				:base(g)
				{
					this.p1 = pos;
					this.p2 = pos;
					this.p3 = pos;
					this.p4 = pos;
					this.p5 = pos;
					this.p6 = pos;
					this.p7 = pos;
					this.p8 = pos;
					this.pos = pos;
					image = g.getSprite("partical");
				}
				public override void Update()
				{
					this.p1 = this.p1 + new Vector2(-20, 0);
					this.p2 = this.p2 + new Vector2(20, 0);
					this.p3 = this.p3 + new Vector2(0, 20);
					this.p4 = this.p4 + new Vector2(0, -20);
					this.p5 = this.p5 + new Vector2(-10, 10);
					this.p6 = this.p6 + new Vector2(10, 10);
					this.p7 = this.p7 + new Vector2(10, -10);
					this.p8 = this.p8 + new Vector2(-10, -10);	

					if(Vector2.Distance(p1, pos)>=400) 
					{
						this.isVisible = false;
					}
				}
				public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				{
					spriteBatch.Draw(image.index, p1, Color.White);
					spriteBatch.Draw(image.index, p2, Color.White);
					spriteBatch.Draw(image.index, p3, Color.White);
					spriteBatch.Draw(image.index, p4, Color.White);
					spriteBatch.Draw(image.index, p5, Color.White);
					spriteBatch.Draw(image.index, p6, Color.White);
					spriteBatch.Draw(image.index, p7, Color.White);
					spriteBatch.Draw(image.index, p8, Color.White);
				}
		}
}

