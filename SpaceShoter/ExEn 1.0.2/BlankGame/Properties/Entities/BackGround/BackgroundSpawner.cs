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
			Ticker t;
			bool first;
			public BackgroundSpawner(Game game)
			{
				this.game = game;
				this.r = new Random(801);
				t= new Ticker(50);
			first=true;
			}
			
			public void Update()
			{
				if(first)
				{
					//StarParticle sp= new StarParticle(game,new Vector2(0*game.scale,500*game.scaleH));
					//game.entitToAdd.Add(sp); 
					//first=false;

				}

				t.updateTick();
				if(t.hasTicked) 
				{
					int xPos = r.Next(20, 300);
					StarParticle sp= new StarParticle(game,new Vector2(xPos*game.scale,500*game.scaleH));
				    game.entitToAdd.Add(sp); 
				}
				t.setTickBeat((int)(50/game.gameSpeed));
			}

			public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
			{
				foreach(Entity e in game.entities) 
				{
					if(e is StarParticle)
					{
						e.Draw(spriteBatch, gameTime);
					}
				}
			}
		}
}

