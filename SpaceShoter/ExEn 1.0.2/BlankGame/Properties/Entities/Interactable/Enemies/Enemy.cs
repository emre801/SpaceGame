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
		public class Enemy: Interact
		{
				//protected Vector2 direct;
				//Sprite image;
				protected Random r;
				
				public Enemy(Game g, Vector2 pos,Vector2 direct)
				:base(g)
				{
					this.direct = direct;
					this.image = g.getSprite("Enemy");
					this.pos = pos;
					this.r = new Random();
				}
				public override bool collidesWith(Interact inter)
				{
					if(inter is Bullet) 
					{
						Bullet b = (Bullet)(inter);
							if(b.isGoodBullet && this.bbox.Intersects(inter.bbox))
							{
								this.isVisible = false;
								inter.isVisible = false;
								Explosion exp = new Explosion(g, b.pos);
								g.mp.playSound("explosion");
								g.entitToAdd.Add(exp);
								//g.es.numEnemies--;
								g.es.removeEnemyFromList(this);
								return true;
							}
					}

					return false;
				}				

				public override void Update()
				{
					this.pos = this.pos + direct;

					if(this.pos.Y < -20)
						this.isVisible = false;
					updateBBox();
				}
				public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				{
					spriteBatch.Draw(image.index, bbox, Color.White);
				}

		}
}

