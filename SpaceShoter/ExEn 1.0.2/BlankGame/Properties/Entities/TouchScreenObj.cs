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
						/*
						if(tc.Count == 1&& prevCount!=1) 
						{
							g.gameState = Game.GameState.GAMETIME;
							g.mp.pauseUnpauseMusic();
						}
						if(tc.Count == 2&& prevCount!=2) 
						{
							g.gameState = Game.GameState.OPTIONS;
						}
						prevCount = tc.Count;
						*/
						
							if(g.ts.startB.isButtonPressed) 
							{
								g.gameState = Game.GameState.GAMETIME;
								g.mp.pauseUnpauseMusic();
								g.es.startStopTimer();
							}
							if(g.ts.optionsB.isButtonPressed) 
							{
								g.gameState = Game.GameState.OPTIONS;
							}
						
						return;

					}
					if(g.gameState == Game.GameState.GAMEOVER) 
					{
						if(g.go.contin.isButtonPressed) 
						{
							g.gameState = Game.GameState.GAMETIME;
							g.restart=true;
							g.mp.pauseUnpauseMusic();
						}
						if(g.go.toTitle.isButtonPressed) 
						{
							g.gameState = Game.GameState.TITLE;
							g.restart = true;
						}
				
						prevCount = tc.Count;
						return;

					}
					
					if(g.gameState == Game.GameState.OPTIONS) 
					{
						//if(tc.Count == 1&& prevCount!=1) 
						//{
						//	g.gameState = Game.GameState.TITLE;
						//}
						if(g.opt.exit.isButtonPressed) 
						{
							g.gameState = Game.GameState.TITLE;
							g.opt.updateName();
							g.mp.updateVolume();
							g.opt.writeFile();
						}
						prevCount = tc.Count;
						return;	
					}


					if(g.fireMode == SpaceShipPlayer.FireMode.FAST && tc.Count==1) 
					{
						g.player.fireBullet();

					}
					else if(tc.Count==1 && prevCount!=1) 
					{
						g.player.fireBullet();
			
					}
					if(tc.Count == 2 && prevCount != 2) 
					{
						//g.isPaused = !g.isPaused;

					}
					if(tc.Count == 3 && prevCount != 3) 
					{
						g.gameState = Game.GameState.TITLE;
						g.es.startStopTimer();
						g.mp.pauseUnpauseMusic();

					}
					if(tc.Count == 4 && prevCount != 4) 
					{
						g.restart = true;
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


