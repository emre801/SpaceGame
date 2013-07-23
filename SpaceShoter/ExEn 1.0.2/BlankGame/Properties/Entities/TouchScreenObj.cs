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
		public class TouchScreenObj:Entity
		{
				int prevCount=0;
				public TouchScreenObj(Game g)
				:base(g)
				{

				}
				public override void Update()
				{
					TouchCollection tc=	TouchPanel.GetState();
					if(tc.Count==1 && prevCount!=1) 
					{
						g.player.fireBullet();
			
					}
					if(tc.Count == 2 && prevCount!=2) 
					{
						g.player.changeMode();
					}

					if(tc.Count == 3 && prevCount != 3) 
					{
						g.isPaused = !g.isPaused;

					}
					foreach(TouchLocation tl in tc) 
					{

							//Vector2 mousePosition = tl.Position;
							//Rectangle mouseRect = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 15, 15);


					}
					prevCount = tc.Count;
				}
		}
}


