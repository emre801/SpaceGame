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
				Random r2;
				//public int numEnemies=0;
				Color hatColor=Color.White;
				public EnemySpawner(Game g)
				{
					this.g = g;
					r = new Random(801);
					r2 = new Random();
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
						SpaceShipPlayer.FireMode fireMode = randFireMode();
						Color ranColor = hatColor;
						PowerUp e= new PowerUp(g,fireMode,
				                       new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH),ranColor);
						g.entitToAdd.Add(e);

					}
				}

				public SpaceShipPlayer.FireMode randFireMode()
				{
					switch(r2.Next(0, 8)) 
					{
						case 0:
							hatColor = Color.Blue;
							return SpaceShipPlayer.FireMode.NORMAL;
						case 1:
							hatColor= Color.Green;
							return SpaceShipPlayer.FireMode.TWO;
						case 2:
							hatColor = Color.Yellow;
							return SpaceShipPlayer.FireMode.THREE;
						case 3:
							hatColor = Color.Purple;
							return SpaceShipPlayer.FireMode.SHIELD;
						case 4:
							hatColor= Color.DarkBlue;
							return SpaceShipPlayer.FireMode.CIRCLE;
						case 5:
							hatColor= Color.DarkRed;
							return SpaceShipPlayer.FireMode.CHARGESHOT;
						case 6:
							hatColor= Color.Pink;
							return SpaceShipPlayer.FireMode.PORTAL;
						case 7:
							hatColor= Color.Orange;
							return SpaceShipPlayer.FireMode.FAST;
					}
						return SpaceShipPlayer.FireMode.NORMAL;
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

