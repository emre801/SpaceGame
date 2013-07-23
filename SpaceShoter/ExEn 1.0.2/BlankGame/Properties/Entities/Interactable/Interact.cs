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
			
			public Interact(Game g)
				:base(g)
			{
					
			}
			public virtual bool collidesWith(Interact inter)
			{
				return true;
			}
		}
}

