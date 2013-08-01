using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BlankGame
{
		public class CirclePUP: Entity
		{
				Sprite image;
				enum CIRCLESTATE {NORMAL,CHARGING,CHARGED};
				CIRCLESTATE cState=CIRCLESTATE.NORMAL;
				Rectangle bbox;
				int chargeRadius=0;
				List<Bullet> collectedBullets = new List<Bullet>();
				Vector2 chargeSpot;
				int prevTouch=0;
				
				public CirclePUP(Game g)
				:base(g)
				{
					this.image = g.getSprite("circleCharge");		
				}
				public void updateBBox()
				{
					bbox = new Rectangle((int)(g.player.pos.X-chargeRadius/2*g.scale+g.player.image.index.Width/2*g.scale),
			                     (int)(g.player.pos.Y-chargeRadius/2*g.scale+g.player.image.index.Height/2*g.scale),
			                     (int)(chargeRadius*g.scale), (int)(chargeRadius*g.scale));
				}
				public void getChargedBullets()
				{
					this.chargeSpot = g.player.pos;// + new Vector2(g.player.image.index.Width / 2, g.player.image.index.Height / 2);
					List<Interact> interactable = g.interactable;
					foreach(Interact i in interactable)
					{
						if(i is Bullet)
						{
							Bullet bull = (Bullet)i;
							if(!bull.isGoodBullet)
								addChargedBullet(bull);		
						}
					}
				}
				public void addChargedBullet(Bullet bull)
				{
					/*if(this.bbox.Contains(bull.bbox)) 
					{
						bull.isVisible = false;
						collectedBullets.Add(bull);
						

					}*/
					float x2 = bull.pos.X - (this.bbox.Center.X);
					x2 = x2 * x2;
					float y2 = bull.pos.Y - (this.bbox.Center.Y);
					y2 = y2 * y2;
					float distance = (float)Math.Sqrt(x2 + y2);
					if(distance < chargeRadius/2*g.scale) 
					{
						bull.isVisible = false;
						collectedBullets.Add(bull);
					}
				}
				
				public void fireChargedBullet()
				{
					foreach(Bullet bull in collectedBullets) 
					{
						Vector2 reverseBull = bull.direct * new Vector2(-1, -1);
						Vector2 firePos = (bull.pos - chargeSpot) + g.player.pos;
						Bullet newBull=new Bullet(g,firePos,reverseBull);
						g.entitToAdd.Add(newBull);
					}
					collectedBullets.Clear();
				}
				public override void Update()
				{
					updateBBox();
					if(g.fireMode== SpaceShipPlayer.FireMode.CIRCLE)
					{
						int currentTouch = TouchPanel.GetState().Count;
						if(cState == CIRCLESTATE.NORMAL) 
						{
							if(prevTouch == 0 && currentTouch == 1) 
							{
								cState=CIRCLESTATE.CHARGING;
							}
						}
						else if(cState == CIRCLESTATE.CHARGING) 
						{
							if(prevTouch == 1 && currentTouch == 1) 
							{
								if(chargeRadius<120)
									chargeRadius += 1;
							}
							else if(prevTouch ==1 && currentTouch==0)
							{
								getChargedBullets();
								cState = CIRCLESTATE.CHARGED;
							}
						}
						else if(cState == CIRCLESTATE.CHARGED) 
						{
							if(prevTouch==0 && currentTouch==1)
							{
								fireChargedBullet();
								cState = CIRCLESTATE.NORMAL;
								chargeRadius = 0;
							}
						}
						prevTouch = currentTouch;
						if(chargeRadius>0)
							g.gameSpeed = (120f - (float)chargeRadius) / 120f+0.1f;
						else
							g.gameSpeed=1f;
						return;
						
					}
					
					prevTouch = 0;
				}
				public void previewShot(SpriteBatch spriteBatch)
				{
					foreach(Bullet bull in collectedBullets) 
					{
						//Vector2 reverseBull = bull.direct * new Vector2(-1, -1);
						Vector2 firePos = (bull.pos - chargeSpot) + g.player.pos;
						
						spriteBatch.Draw(bull.image.index, firePos, Color.White * 0.5f);
					}

				}
				public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				{
						if(g.fireMode == SpaceShipPlayer.FireMode.CIRCLE &&  (cState == CIRCLESTATE.CHARGING || cState==CIRCLESTATE.CHARGED))
						{
							spriteBatch.Draw(image.index, bbox, Color.White * 0.5f);
							if(cState == CIRCLESTATE.CHARGED)
								previewShot(spriteBatch);
						}
				}
		}
}

