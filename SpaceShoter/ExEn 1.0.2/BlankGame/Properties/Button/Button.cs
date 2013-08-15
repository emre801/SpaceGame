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
				int xOri,yOri;
				public Button(Game g, Rectangle demi)
				:base(g)
				{
					this.demi = demi;
					this.xOri = demi.X;
					this.yOri = demi.Y;
				}
				public override void Update()
				{
					TouchCollection tc=	TouchPanel.GetState();
					isPressed = false;
					foreach(TouchLocation tl in tc) 
					{
								
						Vector2 mousePosition = tl.Position;
						Vector2 worldMousePosition = Vector2.Transform(mousePosition, Matrix.Invert(g.drawingTool.cam._transform));
						Rectangle worldRec= new Rectangle((int)worldMousePosition.X-10,(int)worldMousePosition.Y-10,20,20);
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
					//if(g.xAnimation>0)
					//if(g.ignoreDraw) 
					//{
					//	this.demi = new Rectangle(xOri + 10000, yOri, demi.Width, demi.Height);
					//	return;
					//}
					if(g.isOpening || g.isClosing)
						this.demi = new Rectangle(xOri+g.xAnimation, yOri, demi.Width, demi.Height);
					
					else
						this.demi = new Rectangle(xOri, yOri, demi.Width, demi.Height);
					

				}

				

				public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
				{
					if(g.ignoreDraw) 
					{
						//g.ignoreDraw = false;
						return;
					}
					base.Draw(spriteBatch, gameTime);
				}
		}
}

