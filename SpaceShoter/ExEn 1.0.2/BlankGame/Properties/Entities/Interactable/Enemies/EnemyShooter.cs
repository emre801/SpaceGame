using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class EnemyShooter:Enemy
		{
				bool moveleft=true;
				bool moveForward=true;
				public EnemyShooter(Game g, Vector2 pos,Vector2 direct)
				:base(g,pos,direct)
				{
				
				}
				public override void Update()
				{
					this.pos = this.pos + direct;
					if(moveForward) 
					{
						if(this.pos.Y <= 250) 
						{
							moveForward = false;
							direct = new Vector2(-1, 0);
						}
					}
					else
					{
						if(moveleft) 
						{
							if(this.pos.X<=20)	
							{
								direct = new Vector2(1, 0);
								moveleft=false;
							}
						}
						else
						{
							if(this.pos.X>=200)	
							{
								direct = new Vector2(-1, 0);
								moveleft=true;
							}
						}
						if(r.Next(0,500)<5)
							g.entitToAdd.Add(new Bullet(g, pos, new Vector2(0, -4),false));

					}
					updateBBox();
				}
		}
}

