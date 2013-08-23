using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class Interact:Entity
		{
			public Vector2 pos;
			bool ignoreCollision;
			public Rectangle bbox;
			public Sprite image;
			public Vector2 direct;
			public float timer;
			public Interact(Game g)
				:base(g)
			{
				timer = 0;		
			}
			public Interact(Game g,float timer)
			:base(g)
			{
				this.timer=timer;
			}
			public virtual void updateBBox()
			{
				Rectangle oldBBox=bbox;
				
				bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(image.index.Width*g.scale), (int)(image.index.Height*g.scale));
				if(!oldBBox.Equals(bbox))
				{
					removeFromHashSpace(oldBBox);
					addToHashSpace(bbox);
				}
			}


			public void addToHashSpace(Rectangle cool)
			{
				int point1 = cool.X/10;
				int point2 = cool.Y/10;
				int point3=(cool.X+cool.Width)/10;
				int point4= (cool.Y+cool.Height)/10;
				for(int x=point1;x<=point3;x++)
				{
					for(int y=point2;y<=point4;y++)
					{
						if(x<(int)(Constants.NUM_BLOCKS_WIDTH) && x>0 && y>0 && y< (int)(Constants.NUM_BLOCKS_HEIGHT))
							g.spaceSqure[x,y].Add(this);

					}
				}

			}

			public void removeFromHashSpace(Rectangle cool)
			{
				int point1 = cool.X/10;
				int point2 = cool.Y/10;
				int point3=(cool.X+cool.Width)/10;
				int point4= (cool.Y+cool.Height)/10;
				for(int x=point1;x<=point3;x++)
				{
					for(int y=point2;y<=point4;y++)
					{
						if(x<(int)(Constants.NUM_BLOCKS_WIDTH) && y< (int)(Constants.NUM_BLOCKS_HEIGHT) && x>0 && y>0)
							g.spaceSqure[x,y].Remove(this);

					}
				}

			}


			
			public virtual bool collidesWith(Interact inter)
			{
				return false;
			}
			public override void Draw(SpriteBatch spriteBatch,Microsoft.Xna.Framework.GameTime gameTime)
			{
				spriteBatch.Draw(image.index, bbox, Color.White);
			}

			
		}
}

