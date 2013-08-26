using System;
using MonoTouch;
using System.Diagnostics;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class BulletExplo: Bullet
		{
			Ticker explodTick;
			int layer;
			public BulletExplo(Game g,Vector2 pos,Vector2 direct,bool isGoodBullet,int layer)
				:base(g,pos,direct,isGoodBullet)
			{
				this.layer=layer;
				explodTick=new Ticker(1000);
			}
			
			public override void Update()
			{
				explodTick.updateTick();
				if(explodTick.hasTicked && layer>0)
				{
					this.isVisible=false;
					g.entitToAdd.Add(new BulletExplo(g,this.pos,new Vector2(0,3), isGoodBullet,layer-1));
					g.entitToAdd.Add(new BulletExplo(g,this.pos,new Vector2(0,-3), isGoodBullet,layer-1));
					g.entitToAdd.Add(new BulletExplo(g,this.pos,new Vector2(3,0), isGoodBullet,layer-1));
					g.entitToAdd.Add(new BulletExplo(g,this.pos,new Vector2(-3,0), isGoodBullet,layer-1));
					
				}
				base.Update();
			}

				
		}
}

