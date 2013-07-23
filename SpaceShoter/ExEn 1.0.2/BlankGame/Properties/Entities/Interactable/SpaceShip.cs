using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.CoreMotion;

namespace BlankGame
{
		public class SpaceShipPlayer: Interact
		{
			public enum FireMode {NORMAL,FAST,TWO,THREE,CHARGESHOT,CIRCLE};
			public FireMode fireMode=FireMode.NORMAL;
			Sprite image;
			public int health= 20;
			public int lives=2;
			public SpaceShipPlayer(Game g,Sprite sprite)
				:base(g)
			{
				this.image = sprite;
				this.bbox = new Microsoft.Xna.Framework.Rectangle((int)0, (int)0, 
			                                                  sprite.index.Width, sprite.index.Height);
				this.pos = new Vector2(200, 0);

			}
			public void fireBullet()
			{
				if(fireMode == FireMode.NORMAL) 
				{
					g.entitToAdd.Add(new Bullet(g, g.player.pos, new Vector2(0, 10)));

				} 
				else if(fireMode == FireMode.TWO) 
				{
					g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(5,10)));
					g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(-5,10)));
				}

			}
			public void changeMode()
			{
				if(fireMode == FireMode.NORMAL) 
				{
					fireMode = FireMode.TWO;
				}	 
				else if(fireMode == FireMode.TWO) 
				{
					fireMode = FireMode.NORMAL;
				}

			}

			public override void Update()
			{
				updateGyro();
				
			}
			public override bool collidesWith(Interact inter)
			{
				if(inter is Bullet) {
					Bullet bull = (Bullet)inter;
					if(!bull.isGoodBullet) 
					{
						///Damage step
					} else 
					{
						//Maybe heal??
					}
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
					float x = (float)(ry*10f);
					float y = (float)(rx*10f);

					float newY = y + this.pos.Y;
					float newX = x + this.pos.X;

					if(newY<=0)
						newY=0;
					if(newX<=0)
						newX=0;
					if(newX >= 320-image.index.Width)
						newX = 320-image.index.Width;
					if(newY >= 200-image.index.Height)
						newY = 200-image.index.Height;

	
					this.pos = new Vector2(newX, newY);
				}
			}



			public override void Draw(SpriteBatch spriteBatch,Microsoft.Xna.Framework.GameTime gameTime)
			{
						spriteBatch.Draw(image.index, pos, Color.White);
			}

		}
}

