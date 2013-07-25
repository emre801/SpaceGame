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
			
			public Interact(Game g)
				:base(g)
			{
					
			}
			public void updateBBox()
			{
				bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(image.index.Width*g.scale), (int)(image.index.Height*g.scale));
			}
			
			public virtual bool collidesWith(Interact inter)
			{
				return true;
			}
		}
}

