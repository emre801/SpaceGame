using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BlankGame
{
		public class Shield: Interact
		{
				//bool isShieldActive=false;
				
				public Shield(Game g)
				:base(g)
				{
					image = g.getSprite("shield");
					this.pos = g.player.pos + new Vector2(0, 5);
					updateBBox();
				}

				public override void Update()
				{
					g.shieldActive = false;
					TouchCollection tc=	TouchPanel.GetState();
					if(g.fireMode == SpaceShipPlayer.FireMode.SHIELD && tc.Count==1) 
					{
						this.pos = g.player.pos + new Vector2(-10, 50);
						g.shieldActive = true;
						updateBBox();
					}
				}

				public override bool collidesWith(Interact inter)
				{
					TouchCollection tc=	TouchPanel.GetState();
					if(g.fireMode == SpaceShipPlayer.FireMode.SHIELD && tc.Count==1) 
					{
						if(inter is Bullet) 
						{
							Bullet bull = (Bullet)inter;
							if(!bull.isGoodBullet && bull.bbox.Intersects(this.bbox)) 
							{
								bull.isVisible = false;
								Vector2 reverseVec = bull.direct * new Vector2(-1, -1);
								Bullet newBull = new Bullet(g, bull.pos, reverseVec,true);
								g.entitToAdd.Add(newBull);
							}
						}
					}
					return false;
				}

				public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				{
					TouchCollection tc=	TouchPanel.GetState();
					if(g.fireMode == SpaceShipPlayer.FireMode.SHIELD && tc.Count==1) 
					{
						base.Draw(spriteBatch, gameTime);
					}
				}
				 


		}
}

