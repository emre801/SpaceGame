using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BlankGame
{
		public class StarParticle: Entity
		{
			Sprite image;	
			Vector2 pos;
			public StarParticle(Game g,Vector2 pos)
			:base(g)
			{
				image = g.getSprite("partical");
				this.pos = pos;
			}
			public override void Update()
			{
				this.pos = this.pos + new Vector2(0, -2*g.scaleH)*g.gameSpeed;
				if(this.pos.Y<0) 
				{
					this.isVisible = false;
				}
			}
			public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
			{
				spriteBatch.Draw(image.index,new Rectangle((int)pos.X,(int)(pos.Y),(int)(image.index.Width/2*g.scale),(int)(image.index.Width/2*g.scale)),Color.White);
			}

			


		}
}

