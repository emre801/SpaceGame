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
			public bool isGod=false;
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
					if(g.lives < 0) 
					{
						g.gameState = Game.GameState.GAMEOVER;
						g.lives = 3;
						g.mp.pauseUnpauseMusic();
						g.hsd.addNewScore(g.currentPlayerName, g.points, 1f);
						g.hsd.writeFile("high.txt");
						g.newSpaceList();
					}
					g.health = 3;
					g.mp.playSound("explosion");
				}	
			}
			public bool isColliding()
			{
				foreach(Entity e in g.entities) 
				{
					if(e != this && e is Interact && !(e is Bullet)&& !(e is Shield) && !(e is PowerUp)) 
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
				trail.Enqueue(pos);
				if(trail.Count > 10)
					trail.Dequeue();

			}
			public override void updateBBox()
			{
				Rectangle hitBox = new Rectangle((int)pos.X + bbox.Width / 2, (int)pos.Y + bbox.Height / 2, hat.index.Width * 3, hat.index.Height * 3);
				removeFromHashSpace(hitBox);
				bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(image.index.Width*g.scale), (int)(image.index.Height*g.scale));
				addToHashSpace(hitBox);
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
							g.entitToAdd.Add(new Explosion(g, this.pos));
							g.mp.playSound("explosion");
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
						g.mp.playSound("powerUp");
						return true;
					}
					//inter.isVisible = false;
					//g.health--;
					this.pos = this.pos + inter.direct;
					if(pos.Y <= 0 && !(inter is Shield))
						this.playerDied=true;
					//updateBBox(); //temperary remove
					tempUpdateBBox();
				}
				return true;
			}

			public void tempUpdateBBox()
			{
			bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(image.index.Width*g.scale), (int)(image.index.Height*g.scale));



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
			
			public void drawRainBow(Vector2 p1, Vector2 p2, float alpha)
			{

			g.drawingTool.DrawLine(1f, Color.Red*alpha, p1, p2);
			g.drawingTool.DrawLine(1f, Color.Orange*alpha, p1+ new Vector2(2,0), p2+ new Vector2(2,0));
			g.drawingTool.DrawLine(1f, Color.Yellow*alpha, p1+ new Vector2(4,0), p2+ new Vector2(4,0));
			g.drawingTool.DrawLine(1f, Color.Green*alpha, p1+ new Vector2(6,0), p2+ new Vector2(6,0));
			g.drawingTool.DrawLine(1f, Color.SkyBlue*alpha, p1+ new Vector2(8,0), p2+ new Vector2(8,0));
			g.drawingTool.DrawLine(1f, Color.Blue*alpha, p1+ new Vector2(10,0), p2+ new Vector2(10,0));
			}


			public override void Draw(SpriteBatch spriteBatch,Microsoft.Xna.Framework.GameTime gameTime)
			{
				float counter = 0.5f;
				Vector2[] trailArray = trail.ToArray();
				/*foreach(Vector2 p in trail) 
				{
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.Red*0.9f);
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X+2,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.Orange*0.9f);
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X+4,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.Yellow*0.9f);
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X+6,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.Green*0.9f);
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X+8,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.SkyBlue*0.9f);
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X+10,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.Blue*0.9f);
					spriteBatch.Draw(partical.index,new Rectangle((int)p.X+12,(int)p.Y,(int)(image.index.Width*counter),(int)(image.index.Width*counter)),Color.Purple*0.9f);
					//counter=counter+0.05f;
				}*/
			if(trailArray.Length>3)
			for(int i=0;i<trailArray.Length-1;i++)
			{
				drawRainBow(trailArray [i], trailArray [i+1],counter);
				counter += 0.05f;
			}	

				spriteBatch.Draw(image.index, bbox, Color.White);
				spriteBatch.Draw(hat.index, new Rectangle((int)pos.X+bbox.Width/2,(int)pos.Y+bbox.Height/2,hat.index.Width,hat.index.Height), hatColor);
			}

		}
}

