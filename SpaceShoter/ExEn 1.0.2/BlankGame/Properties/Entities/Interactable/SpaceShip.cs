using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.CoreMotion;

namespace BlankGame
{
		public class SpaceShipPlayer: Interact
		{
			public enum FireMode {NORMAL,FAST,TWO,THREE,CHARGESHOT,CIRCLE,SHIELD,PORTAL};
			public FireMode fireMode=FireMode.CHARGESHOT;
			//Sprite image;
			public int health= 20;
			public int lives=2;
			Queue<Vector2> trail = new Queue<Vector2>();
			Sprite partical,hat;
			Color hatColor=Color.White;
			bool playerDied=false;
			public bool isGod=true;
			Vector2 positionOfP1;int portalCounter=1;


			public SpaceShipPlayer(Game g,Sprite sprite)
				:base(g)
			{
				this.image = sprite;
				this.bbox = new Microsoft.Xna.Framework.Rectangle((int)0, (int)0, 
			                                                  sprite.index.Width, sprite.index.Height);
				this.pos = new Vector2(200, 0);
				partical = g.getSprite("partical");
				hat = g.getSprite("hat");
			}
			public void fireBullet()
			{
				if(g.fireMode == FireMode.NORMAL) 
				{
					g.entitToAdd.Add(new Bullet(g, g.player.pos, new Vector2(0, 10)));

				} 
				else if(g.fireMode == FireMode.TWO) 
				{
					g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(5,10)));
					g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(-5,10)));
				}
				else if(g.fireMode == FireMode.THREE) 
				{
					g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(5f/2f,10f/2f)));
					g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(-5f/2f,10f/2f)));
					g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(0,10f/2f)));
				}
				else if(g.fireMode == FireMode.FAST) 
				{
					g.entitToAdd.Add(new Bullet(g, g.player.pos, new Vector2(0, 5)));

				}
				else if(g.fireMode == FireMode.CHARGESHOT) 
				{
					g.entitToAdd.Add(new ChargeBullet(g, g.player.pos, new Vector2(0, 5),20));
				}
				else if(g.fireMode== FireMode.PORTAL)
				{
					if(g.numPortals<1)
					{
						if(portalCounter%2==1)
						{
							positionOfP1 = g.player.pos;
	
						} 
						else 
						{
							g.entitToAdd.Add(new Portal(g, positionOfP1, g.player.pos));
						}
					}
					portalCounter++;


				}
				g.mp.playSound("shoot");

			}
			public void changeMode(FireMode newFireMode)
			{
				fireMode = newFireMode;
			}

			public override void Update()
			{
				Vector2 oldPos = this.pos;
				if(!g.shieldActive)
					updateGyro();
				updateBBox();
				if(isColliding()) 
				{
					this.pos = oldPos;
					updateBBox();
				}
				updateTrail();
				if((playerDied || g.health<=0)&& !isGod)
				{
					g.entitToAdd.Add(new Explosion(g, this.pos));
					this.pos = new Vector2(200, 0);
					updateBBox();
					playerDied = false;
					g.lives--;
					g.health = 3;
					g.mp.playSound("explosion");
				}	
			}
			public bool isColliding()
			{
				foreach(Entity e in g.entities) 
				{
					if(e != this && e is Interact && !(e is Bullet)&& !(e is Shield)) 
					{
						Interact ai = (Interact)e;
						if(ai.bbox.Intersects(this.bbox))
							return true;
					}
				}
				return false;
			}
			


			public void updateTrail()
			{
				trail.Enqueue(pos + new Vector2(image.index.Width/4f,0));
				if(trail.Count > 10)
					trail.Dequeue();

			}
			public override bool collidesWith(Interact inter)
			{
				Rectangle hitBox = new Rectangle((int)pos.X + bbox.Width / 2, (int)pos.Y + bbox.Height / 2, hat.index.Width * 3, hat.index.Height * 3);
				if(inter.bbox.Intersects(hitBox))
				{
					if(inter is Bullet) 
					{
						Bullet bull = (Bullet)inter;
						if(!bull.isGoodBullet) 
						{
							inter.isVisible = false;
							g.health--;
						} 
						else 
						{
							//Maybe heal??
						}
						return true;
					}
					if(inter is PowerUp)
					{
						PowerUp pwp = (PowerUp)inter;
						g.fireMode = pwp.fireMode;
						this.hatColor = pwp.hatColor;
						pwp.isVisible = false;
						return true;
					}
					//inter.isVisible = false;
					//g.health--;
					this.pos = this.pos + inter.direct;
					if(pos.Y <= 0 && !(inter is Shield))
						this.playerDied=true;
					updateBBox();
				}
				return true;
			}


			public void updateGyro()
			{
				if(this.g.motionManager.GyroData!=null)
				{
					CMRotationRate r = this.g.motionManager.GyroData.RotationRate;
					float ry = (float)r.y;
					float rx = (float)r.x;
					if(Math.Abs(ry) <= 0.1f)
						ry = 0;
					if(Math.Abs(rx) <= 0.1f)
						rx = 0;
					float x = (float)(ry*10f)*g.scale;
					float y = (float)(rx*10f)*g.scaleH;

					float newY = y + this.pos.Y;
					float newX = x+ this.pos.X;

					if(newY<=0)
						newY=0;
					if(newX<=0)
						newX=0;
					if(newX >= (280-image.index.Width)*g.scale)
						newX = (280-image.index.Width)*g.scale;
					if(newY >= (480-image.index.Height)*g.scaleH)
						newY = (480-image.index.Height)*g.scaleH;

	
					this.pos = new Vector2(newX, newY);
				}
			}



			public override void Draw(SpriteBatch spriteBatch,Microsoft.Xna.Framework.GameTime gameTime)
			{
				float counter = 0.1f;
				foreach(Vector2 p in trail) 
				{
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.LightSkyBlue*0.5f);
					counter=counter+0.05f;
				}

				spriteBatch.Draw(image.index, bbox, Color.White);
				spriteBatch.Draw(hat.index, new Rectangle((int)pos.X+bbox.Width/2,(int)pos.Y+bbox.Height/2,hat.index.Width,hat.index.Height), hatColor);
			}

		}
}

