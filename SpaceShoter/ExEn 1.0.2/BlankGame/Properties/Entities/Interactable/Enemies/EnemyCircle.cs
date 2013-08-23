using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BlankGame
{
		public class EnemyCircle:Enemy
		{
			bool doCircle;
			Vector2 circlePos,origPos;
			float radius;
			Stopwatch circleTimer;
			Ticker shot;
			public EnemyCircle(Game g, Vector2 pos,Vector2 direct,float timer)
			:base(g,pos,direct,timer)
			{
				doCircle=false;
				radius=50;
			}

			public override void Update()
			{
				if(!doCircle)
				{
					this.pos = this.pos + direct*g.gameSpeed*g.gt;
					if(this.pos.Y <= 200*g.scaleH) 
					{
						doCircle=true;
						this.origPos=pos;
						this.circlePos=new Vector2(pos.X+50,pos.Y);
						circleTimer= new Stopwatch();
						circleTimer.Start();
						shot= new Ticker(200);
					}

				}
				else
				{
					circleTimer.Stop();
					double time=(double)circleTimer.ElapsedMilliseconds/1000f;
					circleTimer.Start();
					float xPos=(float)Math.Sin(time)*50f-50f;
					float yPos=(float)Math.Cos(time)*50f-50f;

					this.pos=this.circlePos+new Vector2(xPos,yPos);
					if(this.pos.Equals(origPos))
					{
						int k=0;
						k=k+1;
					}
					shot.updateTick();
					if(shot.hasTicked)
					{
						g.entitToAdd.Add(new Bullet(g, pos, new Vector2(0, -4*g.scaleH),false));
					}

				}
				updateBBox();
			}


		}
}

