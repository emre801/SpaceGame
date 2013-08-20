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
					if(g.tempIgnore)
					{
						g.tempIgnore = false;
						return;
					}
					TouchCollection tc=	TouchPanel.GetState();


					if(g.gameState == Game.GameState.TITLE) 
					{
						if(g.ts.startB.isButtonPressed) 
						{
								//g.gameState = Game.GameState.GAMETIME;
							g.mp.pauseUnpauseMusic();
							g.es.startStopTimer();
							g.mp.playSound("menu");
							g.titlePress=2;
							g.ignoreDraw = true;
							g.isClosing = true;

						}
						if(g.ts.optionsB.isButtonPressed) 
						{
							//g.gameState = Game.GameState.OPTIONS;
							//g.xAnimation = 300;
							g.isClosing = true;
							g.ignoreDraw = true;
							//g.isOpening = true;
							g.titlePress = 1;
							g.mp.playSound("menu");
						}
						if(g.ts.controlsB.isButtonPressed) 
						{
							//g.gameState = Game.GameState.OPTIONS;
							//g.xAnimation = 300;
							g.isClosing = true;
							g.ignoreDraw = true;
							//g.isOpening = true;
							g.titlePress = 3;
							g.mp.playSound("menu");
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
							g.mp.playSound("menu");
						}
						if(g.go.toTitle.isButtonPressed) 
						{
							g.gameState = Game.GameState.TITLE;
							g.restart = true;
							g.mp.playSound("menu");
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
							//g.gameState = Game.GameState.TITLE;
							g.opt.updateName();
							g.mp.updateVolume();
							g.opt.writeFile();
							g.writeGameInfo();
							g.mp.playSound("menu");
							
							g.isClosing = true;
							g.isOpening = false;
							//g.isClosing = true;
						}
						prevCount = tc.Count;
						return;	
					}


					if(g.gameState == Game.GameState.CONTROLS) 
					{
						//if(tc.Count == 1&& prevCount!=1) 
						//{
						//	g.gameState = Game.GameState.TITLE;
						//}
						if(g.cont.back.isButtonPressed) 
						{
							g.writeGameInfo();
							g.mp.playSound("menu");
							g.isClosing = true;
							g.isOpening = false;
						}
						prevCount = tc.Count;
						return;	
					}


					g.isSingleTab=false;
					if(g.fireMode == SpaceShipPlayer.FireMode.FAST && tc.Count==1) 
					{
						g.player.fireBullet();

					}
					else if(tc.Count==1 && prevCount!=1) 
					{
						g.player.fireBullet();
						g.isSingleTab=true;
					}
					if(tc.Count == 2 && prevCount != 2) 
					{
						//g.isPaused = !g.isPaused;

					}
					if(tc.Count == 3 && prevCount != 3) 
					{
						//g.gameState = Game.GameState.TITLE;
						g.es.startStopTimer();
						//g.mp.pauseUnpauseMusic();
						g.mp.playSound("menu");
						g.isClosing = true;
						g.ignoreDraw = true;
						//g.isOpening = true;
						g.titlePress = 1;

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


