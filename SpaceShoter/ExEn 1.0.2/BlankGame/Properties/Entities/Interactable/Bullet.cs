using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
namespace BlankGame
{
		public class Bullet: Interact
		{
			public bool isGoodBullet=true;
			//Sprite image;
			//Vector2 direct;
			
			public Bullet(Game g,Vector2 pos,Vector2 direct)
			:base(g)
			{
				this.direct = direct;
				this.pos = pos;
				image = g.getSprite("Bullet");
			}
			public Bullet(Game g,Vector2 pos,Vector2 direct,bool isGoodBullet)
			:base(g)
			{
				this.direct = direct;
				this.pos = pos;
				image = g.getSprite("Bullet");
				this.isGoodBullet = isGoodBullet;
			}
			public override void Update()
			{
				this.pos = this.pos + direct*g.gameSpeed;
				if(pos.Y > 500*g.scaleH || pos.Y<-10) 
				{
					this.isVisible = false;
				}
				updateBBox();
			}

			public override bool collidesWith(Interact inter)
			{
				if(inter.bbox.Intersects(bbox))
				{
					if(inter is Block)
					{
						this.isVisible=false;
						Explosion exp = new Explosion(g, pos);
						g.mp.playSound("explosion");
						g.entitToAdd.Add(exp);
					}
				}
				return false;
			}

			
		}
}

