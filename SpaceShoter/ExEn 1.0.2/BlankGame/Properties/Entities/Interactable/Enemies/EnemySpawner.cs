using System;
using System.Collections.Generic;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BlankGame
{
		public class EnemySpawner
		{
				Game g;
				List<Enemy> enemies = new List<Enemy>();
				Random r;
				public EnemySpawner(Game g)
				{
					this.g = g;
					r = new Random(801);
				}
				public void Update()
				{
					if(r.Next(1000)<=5) 
					{
						int xPos = r.Next(20, 300);
						Enemy e= new Enemy(g,new Vector2(xPos,500),new Vector2(0,-1));
						g.entitToAdd.Add(e);
					}
				}
		}
}
