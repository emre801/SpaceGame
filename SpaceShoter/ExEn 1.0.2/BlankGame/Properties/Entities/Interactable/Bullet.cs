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
			Vector2 direct;
			
			public Bullet(Game g,Vector2 pos,Vector2 direct)
			:base(g)
			{
				this.direct = direct;
				this.pos = pos;
				image = g.getSprite("Bullet");
			}
			public override void Update()
			{
				this.pos = this.pos + direct;
				if(pos.Y > 500) 
				{
					this.isVisible = false;
				}
			updateBBox();
			}

			public override bool collidesWith(Interact inter)
			{
				return false;
			}
			public override void Draw(SpriteBatch spriteBatch,Microsoft.Xna.Framework.GameTime gameTime)
			{
				spriteBatch.Draw(image.index, pos, Color.White);
			}

			
		}
}

