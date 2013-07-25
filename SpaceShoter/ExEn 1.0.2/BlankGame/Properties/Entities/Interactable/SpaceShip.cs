using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.CoreMotion;

namespace BlankGame
{
		public class SpaceShipPlayer: Interact
		{
			public enum FireMode {NORMAL,FAST,TWO,THREE,CHARGESHOT,CIRCLE};
			public FireMode fireMode=FireMode.NORMAL;
			//Sprite image;
			public int health= 20;
			public int lives=2;
			Queue<Vector2> trail = new Queue<Vector2>();
			Sprite partical;
			public SpaceShipPlayer(Game g,Sprite sprite)
				:base(g)
			{
				this.image = sprite;
				this.bbox = new Microsoft.Xna.Framework.Rectangle((int)0, (int)0, 
			                                                  sprite.index.Width, sprite.index.Height);
				this.pos = new Vector2(200, 0);
				partical = g.getSprite("partical");
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
				g.mp.playSound("shoot");

			}
			public void changeMode(FireMode newFireMode)
			{
				fireMode = newFireMode;
			}

			public override void Update()
			{
				updateGyro();
				updateBBox();
				updateTrail();
				
				
			}
			public void updateTrail()
			{
				trail.Enqueue(pos + new Vector2(image.index.Width/4f,0));
				if(trail.Count > 10)
					trail.Dequeue();

			}
			public override bool collidesWith(Interact inter)
			{
				if(inter.bbox.Intersects(this.bbox))
				{
					if(inter is Bullet) {
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
					inter.isVisible = false;
					g.health--;
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
					float newX = x + this.pos.X;

					if(newY<=0)
						newY=0;
					if(newX<=0)
						newX=0;
					if(newX >= (280-image.index.Width)*g.scale)
						newX = (280-image.index.Width)*g.scale;
					if(newY >= (200-image.index.Height)*g.scaleH)
						newY = (200-image.index.Height)*g.scaleH;

	
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
			}

		}
}

