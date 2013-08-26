using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BlankGame
{
		public class EnemyTele :Enemy
		{
				Ticker teleTimer,shot;
				Vector2[] teleLocations;
				int teleCounter=0;
				public EnemyTele(Game g, Vector2 pos,Vector2 direct,float timer)
				:base(g,pos,direct,timer)
				{
					teleTimer= new Ticker(2000);
					shot= new Ticker(200);
					teleLocations = new Vector2[4];
					teleLocations[0]=new Vector2(100,200);
					teleLocations[1]=new Vector2(100,300);
					teleLocations[2]=new Vector2(200,300);
					teleLocations[3]=new Vector2(200,200);
				}

				public override void Update()
				{
					teleTimer.updateTick();
					shot.updateTick();
					if(teleTimer.hasTicked)
					{
						teleCounter++;		
						this.pos= teleLocations[teleCounter%4];
					}
					if(shot.hasTicked)
					{
						g.entitToAdd.Add(new Bullet(g, pos, new Vector2(0, -4*g.scaleH),false));
					}
					
					updateBBox();
				}
		}
}

