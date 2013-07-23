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
				public TouchScreenObj(Game g)
				:base(g)
				{

				}
				public override void Update()
				{
					TouchCollection tc=	TouchPanel.GetState();
					if(tc.Count==1) 
					{
						g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(0,10)));
			
					}
					if(tc.Count == 2) 
					{
						g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(5,10)));
						g.entitToAdd.Add(new Bullet(g,g.player.pos,new Vector2(-5,10)));
					}
					foreach(TouchLocation tl in tc) 
					{

							//Vector2 mousePosition = tl.Position;
							//Rectangle mouseRect = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 15, 15);


					}
				}
		}
}


