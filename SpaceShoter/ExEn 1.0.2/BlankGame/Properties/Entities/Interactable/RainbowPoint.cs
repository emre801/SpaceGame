using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class RainbowPoint
		{
				public Vector2[] point;
				public RainbowPoint(Vector2 p)
				{
					point= new Vector2[6];
					point[0]=p;
					point[1]= p+new Vector2(2,0);
					point[2]= p+new Vector2(4,0);
					point[3]= p+new Vector2(6,0);
					point[4]= p+new Vector2(8,0);
					point[5]= p+new Vector2(10,0);
				}
		}
}

