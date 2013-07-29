using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class PowerUp: Interact
		{
				public SpaceShipPlayer.FireMode fireMode;
				Vector2 direct;
				public PowerUp(Game g, SpaceShipPlayer.FireMode fireMode, Vector2 pos,Vector2 direct)
				:base(g)
				{
						this.fireMode = fireMode;	
						this.direct = direct;
						this.image = g.getSprite("incre");
						this.pos = pos;
				}

				public override void Update()
				{
					this.pos = this.pos + direct;
					if(pos.Y < -100*g.scaleH) 
					{
						this.isVisible = false;
					}
					updateBBox();
				}


				public override bool collidesWith(Interact inter)
				{
					if(inter.bbox.Intersects(this.bbox)) 
					{
						if(inter is SpaceShipPlayer)
							this.isVisible = false;
					}
					
					return false;
				}

		}
}

