using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BlankGame
{
		public class StarParticle: Entity
		{
			Sprite image;	
			Vector2 pos;
			Random r;
			bool hasSpawnedNew=false;
			public StarParticle(Game g,Vector2 pos)
			:base(g)
			{
				r= new Random();
				
				image = g.getSprite("Star"+r.Next()%4);
				//image = g.getSprite("ScrollStar");
				this.pos = pos;
			}
			public override void Update()
			{
				this.pos = this.pos + new Vector2(0, -3*g.scaleH)*g.gameSpeed*g.gt;
				if(this.pos.Y<0)// && !hasSpawnedNew) 
				{
					this.isVisible = false;
					//this.hasSpawnedNew=true;
					//StarParticle sp= new StarParticle(g,new Vector2(0*g.scale,500*g.scaleH));
					//g.entitToAdd.Add(sp); 
				}//
				if(this.pos.Y<-700)
				{
					//this.isVisible=false;
				}

			}
			public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
			{
				spriteBatch.Draw(image.index,new Rectangle((int)pos.X,(int)(pos.Y),(int)(image.index.Width*g.scale/2f),(int)(image.index.Height*g.scale/2f)),Color.White);
			}

			


		}
}

