using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class ChargeBullet: Bullet
		{
				float size;
				public ChargeBullet(Game g,Vector2 pos,Vector2 direct,int size)
				:base(g,pos,direct)
				{
					this.size = size*g.scale;
				}
				public override void updateBBox()
				{
					bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(size*g.scale), (int)(size*g.scale));
				}
				public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				{
					spriteBatch.Draw(image.index, new Rectangle((int)pos.X, (int)pos.Y, (int)size, (int)size), Color.White);
				}
		}
}

