using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BlankGame
{
		public class Button : Entity
		{
				public Rectangle demi;
				public bool isButtonPressed = false;
				public bool oldPressed=false,isPressed=false;
				public Vector2 mP= Vector2.Zero;
				public Button(Game g, Rectangle demi)
				:base(g)
				{
					this.demi = demi;
				}
				public override void Update()
				{
					TouchCollection tc=	TouchPanel.GetState();
					isButtonPressed = false;
					isPressed = false;
					foreach(TouchLocation tl in tc) 
					{
								
						Vector2 mousePosition = tl.Position;
						Vector2 worldMousePosition = Vector2.Transform(mousePosition, Matrix.Invert(g.drawingTool.cam._transform));
						Rectangle worldRec= new Rectangle((int)worldMousePosition.X,(int)worldMousePosition.Y,1,1);
						mP = worldMousePosition;
						if(demi.Intersects(worldRec) || demi.Contains(worldRec)) 
						{
							isPressed = true;
							//return;
						}
								
					}
					if(oldPressed && !isPressed)
						isButtonPressed = true;
					else 
						isButtonPressed = false;


					oldPressed = isPressed;
					

				}

				

				//public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				//{
				//	base.Draw(spriteBatch, gameTime);
				//}
		}
}

