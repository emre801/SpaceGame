using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class Interact:Entity
		{
			public Vector2 pos;
			bool ignoreCollision;
			public Rectangle bbox;
			public Sprite image;
			public Vector2 direct;
			public Interact(Game g)
				:base(g)
			{
					
			}
			public virtual void updateBBox()
			{
				bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(image.index.Width*g.scale), (int)(image.index.Height*g.scale));
			}
			
			public virtual bool collidesWith(Interact inter)
			{
				return false;
			}
			public override void Draw(SpriteBatch spriteBatch,Microsoft.Xna.Framework.GameTime gameTime)
			{
				spriteBatch.Draw(image.index, bbox, Color.White);
			}

			
		}
}

