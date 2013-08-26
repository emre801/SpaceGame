using System;
using MonoTouch;
using System.Diagnostics;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class BulletWave: Bullet
		{
			Stopwatch circleTimer;
			public BulletWave(Game g,Vector2 pos,Vector2 direct,bool isGoodBullet)
				:base(g,pos,direct,isGoodBullet)
			{
				circleTimer= new Stopwatch();
				circleTimer.Start();
			}

			public override void Update()
			{
				circleTimer.Stop();
				double time=(double)circleTimer.ElapsedMilliseconds/(500f/direct.Y);
				circleTimer.Start();

				float xPos=(float)Math.Cos(time);
				float yPos=1;//(float)Math.Abs((float)Math.Sin(time));//*50f;
				
				float xDir=direct.X;
				if(direct.X==0)
					xDir=-1;	

				float yDir=direct.Y;
				if(direct.Y==0)
					yDir=-1;	
				
				Vector2 direct2= new Vector2(xDir,yDir); 


				Vector2 newDir= direct2*new Vector2(xPos,yPos);
				this.pos = this.pos + (newDir)*g.gameSpeed*g.gt;

				updateBBox();
			}
				
		}
}

