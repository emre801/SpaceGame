using System;
using System.IO;
using System.Collections.Generic;
using MonoTouch;
using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BlankGame
{
		public class EnemySpawner
		{
				Game g;
				HashSet<Enemy> enemies = new HashSet<Enemy>();
				//Random r;
				Random r2;
				//public int numEnemies=0;
				Color hatColor=Color.White;
				Stopwatch timer;
				PriorityQueue<float,Interact> objsToSpawn;
				Ticker ticker;
				int numTicks;
				public EnemySpawner(Game g)
				{
					this.g = g;
					//r = new Random(801);
					r2 = new Random();
					objsToSpawn = new PriorityQueue<float, Interact>();
				}
				public void init()
				{
					readLevel(0);
					timer = new Stopwatch();
					ticker = new Ticker(1);
					numTicks = 0;
				}
				
				public String returnCurrentGameTime()
				{
					timer.Stop();
					int totalSec = (int)(timer.ElapsedMilliseconds/1000);
					int elapsedMilSec=(int)(timer.ElapsedMilliseconds%1000)/10;
					timer.Start();
					int totalMinutes = totalSec / 60;
					int secLeftOver = totalSec % 60;
					return totalMinutes + ":" + secLeftOver+":"+elapsedMilSec;
				}
				
				public void startStopTimer()
				{
					if(timer.IsRunning)
						timer.Stop();
					else 
						timer.Start();
					ticker.pauseUnpause();
				}
				public void readLevel(int levelNum)
				{
					String path = Path.Combine("Content/Levels", "Level" + levelNum + ".txt");
					String file = File.ReadAllText(path);
					StringReader sr = new StringReader(file);
					String line;
					char[] delimiterChars = { ' ', ',', ':', '\t' };
					while((line = sr.ReadLine()) != null) 
					{
						string[] words = line.Split(delimiterChars);
						if(words[0].Equals("en"))
							uploadEnemy(words);
						if(words[0].Equals("pw"))				   		
							uploadPowerUp(words);
						if(words[0].Equals("bl"))
							uploadBlock(words);

					}
					String[] text={"Hello Commander","Cat Hat ","How are you doing today"};
					TextBlock fuck= new TextBlock(g,text,0,true);
					g.entitToAdd.Add(fuck);
					String[] text2={"LOL","Cat","PotatoeMan"};
					fuck= new TextBlock(g,text2,1,false);
					g.entitToAdd.Add(fuck);

				}

				public void uploadEnemy(String[] info)
				{
					float time=System.Convert.ToSingle(info[2]);
					int xPos=(int)System.Convert.ToSingle(info[3]);
					Enemy e=null;
					if(info [1].Equals("1"))
						e = new EnemyShooter(g,new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH),time);
					objsToSpawn.Enqueue(time,e);
				}

				public void uploadPowerUp(String[] info)
				{
					float time=System.Convert.ToSingle(info[2]);
					int xPos=(int)System.Convert.ToSingle(info[3]);
					SpaceShipPlayer.FireMode mode= SpaceShipPlayer.FireMode.NORMAL;
					Color ranColor = hatColor;
					if(info [1].Equals("1"))
					{
						mode = SpaceShipPlayer.FireMode.NORMAL;
						ranColor = Color.Blue;
					}


					PowerUp pwp= new PowerUp(g,mode,
					                         new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH),ranColor,time);
					objsToSpawn.Enqueue(time,pwp);
				}

				public void uploadBlock(String[] info)
				{
					float time=System.Convert.ToSingle(info[2]);
					int xPos = (int)System.Convert.ToSingle(info [3]);
					if(info[1].Equals("1"))
					{
						Block block = new Block(g, time, new Vector2(xPos * g.scale, 500 * g.scaleH), new Vector2((int)System.Convert.ToSingle(info [4]), (int)System.Convert.ToSingle(info [5])), new Vector2(0, -2 * g.scale));
						objsToSpawn.Enqueue(time, block);
					}
				}

				public void Update()
				{
					if(g.texts.Count>0)
						return;


					ticker.updateTick();
					if(ticker.hasTicked)
						numTicks++;
					float convTicks=numTicks/50f;

					while(objsToSpawn.Count>0 && objsToSpawn.Peek().Value.timer<=convTicks) 
					{
						g.entitToAdd.Add(objsToSpawn.DequeueValue());
					}
					
					ticker.setTickBeat((int)(1 / g.gameSpeed));
				}

				public SpaceShipPlayer.FireMode randFireMode()
				{
					switch(r2.Next(0, 8)) 
					{
						case 0:
							hatColor = Color.Blue;
							return SpaceShipPlayer.FireMode.NORMAL;
						case 1:
							hatColor= Color.Green;
							return SpaceShipPlayer.FireMode.TWO;
						case 2:
							hatColor = Color.Yellow;
							return SpaceShipPlayer.FireMode.THREE;
						case 3:
							hatColor = Color.Purple;
							return SpaceShipPlayer.FireMode.SHIELD;
						case 4:
							hatColor= Color.DarkBlue;
							return SpaceShipPlayer.FireMode.CIRCLE;
						case 5:
							hatColor= Color.DarkRed;
							return SpaceShipPlayer.FireMode.CHARGESHOT;
						case 6:
							hatColor= Color.Pink;
							return SpaceShipPlayer.FireMode.PORTAL;
						case 7:
							hatColor= Color.Orange;
							return SpaceShipPlayer.FireMode.FAST;
					}
						return SpaceShipPlayer.FireMode.NORMAL;
				}

				public int numEnemies()
				{
					return enemies.Count;
				}
				public void addEnemyToList(Enemy e)
				{
					enemies.Add(e);	
				}
				public void removeEnemyFromList(Enemy e)
				{
					enemies.Remove(e);
				}
		}
}

