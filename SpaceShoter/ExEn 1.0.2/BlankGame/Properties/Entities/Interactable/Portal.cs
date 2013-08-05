using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
namespace BlankGame
{
		public class Portal: Entity
		{
				Vector2 portal1,portal2;
				Rectangle r1,r2;
				int radius=60;
				Sprite image;
				List<Bullet> outOfP1= new List<Bullet>();
				List<Bullet> outOfP2= new List<Bullet>();
				HashSet<Bullet> bullToIgnore1 = new HashSet<Bullet>();
				HashSet<Bullet> bullToIgnore2 = new HashSet<Bullet>();
				Stopwatch stopwatch;
				bool startFlashing=false;
				float timeSpan=10000;
			
				public Portal(Game g, Vector2 portal1,Vector2 portal2)
				:base(g)
				{
					this.portal1 = portal1;
					this.portal2 = portal2;
					r1 = new Rectangle((int)portal1.X - this.radius / 2, (int)portal1.Y - this.radius / 2, radius, radius);	
					r2 = new Rectangle((int)portal2.X - this.radius / 2, (int)portal2.Y - this.radius / 2, radius, radius);
					image = g.getSprite("circleCharge");
					stopwatch = new Stopwatch();
					stopwatch.Start();
					g.numPortals++;
				}
				public void determinePortalJumps()
				{
					foreach(Interact i in g.interactable) 
					{
						if(i is Bullet) 
						{
							Bullet bull = (Bullet)i;
							if(r1.Contains(bull.bbox) && !bullToIgnore1.Contains(bull))
							{
								outOfP2.Add(bull);
								bullToIgnore2.Add(bull);
							}
							else if(r2.Contains(bull.bbox) && !bullToIgnore2.Contains(bull))
							{
								outOfP1.Add(bull);
								bullToIgnore1.Add(bull);
							}
						}
					}
				}
				
				public void portalJump()
				{
					foreach(Bullet bull in outOfP2) 
					{
						Vector2 newBullPos = (bull.pos - portal1) + portal2;
						bull.isGoodBullet = true;
						bull.pos = newBullPos;
					}
					foreach(Bullet bull in outOfP1) 
					{
						Vector2 newBullPos = (bull.pos - portal2) + portal1;
						bull.isGoodBullet = true;
						bull.pos = newBullPos;
					}
					outOfP1.Clear();
					outOfP2.Clear();
				}
			
				public void updateStopWatch()
				{
					stopwatch.Stop();
					if(stopwatch.ElapsedMilliseconds > timeSpan*0.85f)
					{
						startFlashing = !startFlashing;
					}


					if(stopwatch.ElapsedMilliseconds > timeSpan)
					{
						this.isVisible = false;
						g.numPortals--;
					}
					stopwatch.Start();

				}
				public override void Update()
				{
					determinePortalJumps();
					portalJump();
					updateStopWatch();
				}
				public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				{
					if(!startFlashing) 
					{
						spriteBatch.Draw(image.index, r1, Color.Red * 0.5f);
						spriteBatch.Draw(image.index, r2, Color.Orange * 0.5f);
					}
				}

		}
}

