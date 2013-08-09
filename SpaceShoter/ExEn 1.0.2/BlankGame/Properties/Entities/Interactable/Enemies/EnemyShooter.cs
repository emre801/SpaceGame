using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BlankGame
{
		public class EnemyShooter:Enemy
		{
				bool moveleft=true;
				bool moveForward=true;
				Stopwatch stopWatch;

				public EnemyShooter(Game g, Vector2 pos,Vector2 direct,int timer)
				:base(g,pos,direct,timer)
				{
					stopWatch= new Stopwatch();
				}
				public override void Update()
				{
					this.pos = this.pos + direct*g.gameSpeed;
					if(moveForward) 
					{
						if(this.pos.Y <= 250*g.scaleH) 
						{
							moveForward = false;
							direct = new Vector2(-1, 0);
							stopWatch.Start();
						}
					}
					else
					{
						if(moveleft) 
						{
							if(this.pos.X<=20*g.scale)	
							{
								direct = new Vector2(1, 0);
								moveleft=false;
							}
						}
						else
						{
							if(this.pos.X>=200*g.scale)	
							{
								direct = new Vector2(-1, 0);
								moveleft=true;
							}
						}

						stopWatch.Stop();
						if(stopWatch.ElapsedMilliseconds >= 500*(1/g.gameSpeed)) 
						{
							g.entitToAdd.Add(new Bullet(g, pos, new Vector2(0, -4),false));
							stopWatch.Reset();
						}
						stopWatch.Start();
						//if(r.Next(0,500)<5)
							//g.entitToAdd.Add(new Bullet(g, pos, new Vector2(0, -4),false));

					}
					updateBBox();
				}
		}
}

