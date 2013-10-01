using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
		Queue<RainbowPoint> trail = new Queue<RainbowPoint>();
		Sprite partical,hat;
		Color hatColor=Color.White;
		bool playerDied=false;
		public bool isGod=false;
		Vector2 positionOfP1;int portalCounter=1;

		Ticker flashTicker = new Ticker(90);
		Ticker dieTimer = new Ticker(5000);
		bool hasJustDied = false;

		Ticker tempImunity = new Ticker(1000);
		bool tempImmune = false;

		SpriteStripAnimationHandler idel,upAni,downAni;

		Rectangle hitBox;
		public int flightMode = 0;

		public SpaceShipPlayer(Game g,Sprite sprite)
			:base(g)
		{
			this.image = sprite;
			this.bbox = new Microsoft.Xna.Framework.Rectangle((int)0, (int)0, 
			                                                  sprite.index.Width, sprite.index.Height);
			this.pos = new Vector2(150, 0);
			partical = g.getSprite("partical");
			hat = g.getSprite("hat");
			idel = g.getAnimation("idealAni");
			upAni = g.getAnimation("upAni");
			downAni = g.getAnimation("downAni");
		}
		public void updateAni()
		{
			idel.Update();
			upAni.Update();
			downAni.Update();
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
				//Update charge bullet


				g.entitToAdd.Add(new ChargeBullet(g, g.player.pos, new Vector2(0, 5),g.chargeShotValue));
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
			updateAni();
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
				//this.pos = new Vector2(200, 0);
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
				hasJustDied = true;
				dieTimer = new Ticker(500);
			}

			if (hasJustDied)
			{
				dieTimer.updateTick();
				if (dieTimer.hasTicked)
					hasJustDied = false;


			}
			if (tempImmune)
			{
				tempImunity.updateTick();
				if (tempImunity.hasTicked)
					tempImmune = false;

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
			trail.Enqueue(new RainbowPoint(pos+new Vector2(5,15)));
			if(trail.Count > 15)
				trail.Dequeue();

		}
		public override void updateBBox()
		{
			this.hitBox = new Rectangle((int)pos.X + bbox.Width / 4, (int)pos.Y + bbox.Height / 2, hat.index.Width, hat.index.Height);
			removeFromHashSpace(hitBox);
			//bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(image.index.Width*g.scale), (int)(image.index.Height*g.scale));
			bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(idel.widthOf() * g.scale), (int)(idel.heightOf() * g.scale));
			addToHashSpace(hitBox);
		}


		public override bool collidesWith(Interact inter)
		{
			//this.hitBox = new Rectangle((int)pos.X + bbox.Width / 2, (int)pos.Y + bbox.Height / 2, hat.index.Width * 3, hat.index.Height * 3);
			if (tempImmune)
				return false;


			if (inter is Bullet)
			{
				if (inter.bbox.Intersects(hitBox))
				{
					Bullet bull = (Bullet)inter;
					if (!bull.isGoodBullet && !hasJustDied)
					{
						inter.isVisible = false;
						g.entitToAdd.Add(new Explosion(g, this.pos));
						g.mp.playSound("explosion");
						g.health--;
						tempImunity = new Ticker(1000);
						tempImmune = true;
					}
					else
					{
						//Maybe heal??
					}
				}
				return true;
			}
			if(inter.bbox.Intersects(bbox))
			{

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
				if(pos.Y!=inter.pos.Y)
					this.pos = this.pos + inter.direct*g.gameSpeed;
				if(pos.Y <= 0 && !(inter is Shield))
					this.playerDied=true;
				//updateBBox(); //temperary remove
				//tempUpdateBBox();
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

				if (x> 0.5f)
					flightMode = 1;
				else if (x< -0.5f)
					flightMode = 2;
				else
					flightMode = 0;

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

		public void drawRainBow(RainbowPoint p1, RainbowPoint p2, float alpha)
		{

			g.drawingTool.DrawLine(1f, Color.Red*alpha, p1.point[0], p2.point[0]);
			g.drawingTool.DrawLine(1f, Color.Orange*alpha, p1.point[1], p2.point[1]);
			g.drawingTool.DrawLine(1f, Color.Yellow*alpha, p1.point[2], p2.point[2]);
			g.drawingTool.DrawLine(1f, Color.Green*alpha, p1.point[3], p2.point[3]);
			g.drawingTool.DrawLine(1f, Color.SkyBlue*alpha, p1.point[4], p2.point[4]);
			g.drawingTool.DrawLine(1f, Color.Blue*alpha,p1.point[5], p2.point[5]);
		}


		public override void Draw(SpriteBatch spriteBatch,Microsoft.Xna.Framework.GameTime gameTime)
		{

			flashTicker.updateTick();
			if (hasJustDied || (flashTicker.hasTicked && tempImmune))
				return;

			float counter = 0.5f;
			RainbowPoint[] trailArray = trail.ToArray();
			for (int i = 0; i < trailArray.Length - 1; i++)
			{
				//drawRainBow(trailArray[i], trailArray[i + 1], counter);
				counter += 0.05f;
			}
			Color color = Color.White;
			if (g.isChargingShot && flashTicker.hasTicked)
				color = Color.Black;
			//spriteBatch.Draw(image.index, bbox, color);
			if(flightMode==0)
				idel.draw(spriteBatch, new Rectangle(bbox.X,bbox.Y,idel.widthOf(),idel.heightOf()),color);
			if (flightMode == 1)
				downAni.draw(spriteBatch, new Rectangle(bbox.X, bbox.Y, idel.widthOf(), idel.heightOf()), color);
			else
				upAni.draw(spriteBatch, new Rectangle(bbox.X, bbox.Y, idel.widthOf(), idel.heightOf()), color);
			//spriteBatch.Draw(hat.index, new Rectangle((int)pos.X+bbox.Width/2,(int)pos.Y+bbox.Height/2,hat.index.Width,hat.index.Height), hatColor);    
			spriteBatch.Draw(hat.index, hitBox, hatColor); 
		}

	}
}

