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
				HashSet<Enemy> enemies = new HashSet<Enemy>();
				Random r;
				//public int numEnemies=0;
				public EnemySpawner(Game g)
				{
					this.g = g;
					r = new Random(801);
				}
				public void Update()
				{
					if(r.Next(2000)<=5 && numEnemies()<10) 
					{
						int xPos = r.Next(40, 280);
						Enemy e= new EnemyShooter(g,new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH));
						//Enemy e= new Enemy(g,new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH));
						g.entitToAdd.Add(e);
						//numEnemies++;
						addEnemyToList(e);
					}
					if(r.Next(5000) <= 5) 
					{
						int xPos = r.Next(40, 280);
						PowerUp e= new PowerUp(g,SpaceShipPlayer.FireMode.CIRCLE,
				                       new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH),Color.Red);
						g.entitToAdd.Add(e);

					}
				}
				public int numEnemies()
				{
					return enemies.Count;
				}
				public void addEnemyToList(Enemy e)
				{
					enemies.Add(e);	
				}
				public void removeEnemyFromList(Enemy e)
				{
					enemies.Remove(e);
				}
		}
}

