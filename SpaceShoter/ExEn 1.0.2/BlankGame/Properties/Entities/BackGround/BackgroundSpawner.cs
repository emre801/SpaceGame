using System;
using System.Collections.Generic;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BlankGame
{
		public class BackgroundSpawner
		{
			Game game;
			Random r;
			public BackgroundSpawner(Game game)
			{
				this.game = game;
						this.r = new Random(801);
			}
			
			public void Update()
			{
				if(r.Next(50)<=5) 
				{
					int xPos = r.Next(20, 300);
					StarParticle sp= new StarParticle(game,new Vector2(xPos,500));
				    game.entitToAdd.Add(sp); 
				}
			}
		}
}

