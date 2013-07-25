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

					if(g.gameState == Game.GameState.TITLE) 
					{
						if(tc.Count == 1)
							g.gameState = Game.GameState.GAMETIME;
						return;
					}


					if(tc.Count==1 && prevCount!=1) 
					{
						g.player.fireBullet();
			
					}
					if(tc.Count == 2 && prevCount != 2) 
					{
						g.isPaused = !g.isPaused;

					}
					if(tc.Count == 3 && prevCount != 3) 
					{
						g.gameState = Game.GameState.TITLE;

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


