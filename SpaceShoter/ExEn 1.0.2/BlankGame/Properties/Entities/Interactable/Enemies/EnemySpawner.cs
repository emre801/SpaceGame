using System;
using System.IO;
using System.Collections.Generic;

using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

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
		bool loadText=false;
		int totalObj;
		Vector2 spawnPosition;
		Random r;
		Dictionary<Vector2, Block> grid;
		List<Block> overLapping;
		public EnemySpawner(Game g)
		{
			this.g = g;
			//r = new Random(801);
			r2 = new Random();
			objsToSpawn = new PriorityQueue<float, Interact>();
			spawnPosition = new Vector2(0, 0);
			r = new Random();
			grid = new Dictionary<Vector2, Block>();
			overLapping = new List<Block>();
		}
		public void init(bool loadText)
		{
			this.loadText=loadText;
			//readLevel(r2.Next()%2);
			String newLevel=randomlyCreateLevel();
			readLevel(newLevel);
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
		public bool hasFinishedSpawning()
		{
			return objsToSpawn.Count==0;
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
				if(words[0].Equals("$"))
					upLoadText(sr,words[1],(int)System.Convert.ToSingle(words[2]));		
			}




		}
		public void readLevel(String file)
		{
			// String file = File.ReadAllText(path);
			StringReader sr = new StringReader(file);
			String line;
			char[] delimiterChars = { ' ', ',', ':', '\t' };
			while ((line = sr.ReadLine()) != null)
			{
				string[] words = line.Split(delimiterChars);
				if (words[0].Equals("en"))
					uploadEnemy(words);
				if (words[0].Equals("pw"))
					uploadPowerUp(words);
				if (words[0].Equals("bl"))
					uploadBlock(words);
				if (words[0].Equals("$"))
					upLoadText(sr, words[1], (int)System.Convert.ToSingle(words[2]));
			}


			foreach (KeyValuePair<Vector2, Block> entry in grid)
			{
				Vector2 v = entry.Key;
				Block b = entry.Value;
				updateBlockSpriteBasedOnPosition(b, v);

			}


		}

		public void updateBlockSpriteBasedOnPosition(Block b, Vector2 v)
		{
			bool above = false, below = false, right = false, left = false;
			if (grid.ContainsKey(new Vector2(v.X + 1, v.Y)))
				left = true;
			if (grid.ContainsKey(new Vector2(v.X - 1, v.Y)))
				right = true;
			if (grid.ContainsKey(new Vector2(v.X, v.Y - 1)))
				above = true;
			if (grid.ContainsKey(new Vector2(v.X, v.Y + 1)))
				below = true;

			if (above && !below && !left && right)
				b.updateSprite("tile9");
			else if (above && below && !left && right)
				b.updateSprite("tile6");
			else if (!above && below && !left && right)
				b.updateSprite("tile3");
			else if (above && !below && left && right)
				b.updateSprite("tile8");
			else if (above && below && left && right)
				b.updateSprite("tile5");
			else if (!above && below && left && right)
				b.updateSprite("tile2");
			else if (above && !below && left && !right)
				b.updateSprite("tile7");
			else if (above && below && left && !right)
				b.updateSprite("tile4");
			else if (!above && below && left && !right)
				b.updateSprite("tile1");
			else if (above && below && !left && !right)
				b.updateSprite("tile10");
			else if (!above && !below && left && right)
				b.updateSprite("tile11");
			else if (!above && below && !left && !right)
				b.updateSprite("tile12");
			else if (above && !below && !left && !right)
				b.updateSprite("tile13");
			else if (!above && !below && !left && right)
				b.updateSprite("tile14");
			else if (!above && !below && left && !right)
				b.updateSprite("tile15");
			else
				b.updateSprite("tile15");


		}



		public String randomlyCreateLevel()
		{




			LinkedList<String> lines = new LinkedList<String>();
			int length = r.Next(1400, 2800) / 10 * 10;
			int incre = 20;
			int posOfLastBlock = -1000;
			int makeMiddle = 0;
			int lastMiddle = -1000;
			int isWall = 0;
			int wallHeight = 0;
			int posOfWallEnemy = 0;
			for (int i = 0; i < length; i += incre)
			{
				//g.addEntity(new Block(new Vector2(i, 0), g, new Vector2(incre, incre)));
				//g.addEntity(new Block(new Vector2(i, 320 - incre), g, new Vector2(incre, incre)));
				String st;
				if (isWall != 0)
				{
					posOfLastBlock = i;
					int lastW = 0;
					if (isWall < 0)
					{

						isWall++; 
						for (int w = 320 - incre; w >= 320 - wallHeight; w -= incre)
						{
							// g.addEntity(new Block(new Vector2(i, w), g, new Vector2(incre, incre)));
							st = "bl 1 " + i + " " + (w) + " " + (incre + 3) + " " + incre + " tile11";
							lines.AddLast(st);
							lastW = w - incre;
						}



					}
					else
					{
						//int lastW=0;
						isWall--;
						for (int w = 0; w < wallHeight; w += incre)
						{
							//g.addEntity(new Block(new Vector2(i, w), g, new Vector2(incre, incre)));
							st = "bl 1 " + i + " " + (w) + " " + (incre + 3) + " " + incre + " tile11";
							lines.AddLast(st);
							lastW = w + incre;

						} 
						// st = "en " + 6 + " " + +(i) + " " + lastW;
						//lines.AddLast(st);



					}
					if (posOfWallEnemy == isWall && isWall!=0)
					{
						st = "en " + 6 + " " + +(i) + " " + lastW;
						lines.AddLast(st);
					}

					if (isWall == 0)
					{


					}
					st = "bl 1 " + i + " " + 0 + " " + (incre + 3) + " " + incre + " tile5";
					lines.AddLast(st);
					st = "bl 1 " + i + " " + (300 - incre) + " " + (incre + 3) + " " + incre + " tile5";
					lines.AddLast(st);
					continue;


				}
				st = "bl 1 " + i + " " + 0 + " " + (incre + 3) + " " + incre + " tile6";
				lines.AddLast(st);
				st = "bl 1 " + i + " " + (300 - incre) + " " + (incre + 3) + " " + incre + " tile4";
				lines.AddLast(st);

				int chance = r.Next(0, 100);
				if (chance <= 30)
				{
					if (i - posOfLastBlock > 70 && i - lastMiddle > 50)
					{
						int rNum = r.Next() % 3;
						if (rNum == 0 || rNum == 2)
						{
							posOfLastBlock = i;
							int width = r.Next(80, 100);
							int lastW = 0;
							for (int w = 0; w < width; w += incre)
							{
								//g.addEntity(new Block(new Vector2(i, w), g, new Vector2(incre, incre)));
								st = "bl 1 " + i + " " + (w) + " " + (incre + 3) + " " + incre +" tile5";
								lines.AddLast(st);
								lastW = w+incre;

							}
							//g.addEntity(new Enemy(new Vector2(i, lastW), g, 1));
							//st = "en " + 6 + " " + +(i) + " " + lastW;
							//lines.AddLast(st);
							int ranNum = r.Next(0,100);
							if (ranNum < 90)
							{
								isWall = r.Next(3, 9);
								wallHeight = width;
								posOfWallEnemy = r.Next(0, isWall);
								if (posOfWallEnemy == 0)
								{
									st = "en " + 6 + " " + +(i) + " " + lastW;
									lines.AddLast(st);
								}
							}


						}
						if (rNum == 1 || rNum == 2)
						{

							posOfLastBlock = i;
							int width = r.Next(80, 100);
							int lastW = 0;
							for (int w = 320 - incre; w >= 320 - width; w -= incre)
							{
								// g.addEntity(new Block(new Vector2(i, w), g, new Vector2(incre, incre)));
								st = "bl 1 " + i + " " + (w) + " " + (incre + 3) + " " + incre + " tile5";
								lines.AddLast(st);
								lastW = w-incre;
							}
							//g.addEntity(new Enemy(new Vector2(i, lastW), g, 1));

							int ranNum = r.Next(0,100);
							if (ranNum < 90)
							{
								isWall = r.Next(3, 9)*-1;
								wallHeight = width;
								posOfWallEnemy = r.Next(isWall, 0);
								if (posOfWallEnemy == 0)
								{
									st = "en " + 6 + " " + +(i) + " " + lastW;
									lines.AddLast(st);
									posOfWallEnemy = 1000;
								}
							}
						}
					}


				}

				if (makeMiddle == 0)
				{
					int mChance = r.Next(0, 100);
					if (mChance <= 10)
					{
						makeMiddle = r.Next(7, 14);
					}
				}
				else
				{
					if (i - posOfLastBlock > 70)
					{
						makeMiddle--;
						//g.addEntity(new Block(new Vector2(i, 320 / 2 - incre), g, new Vector2(incre, incre)));
						// posOfLastBlock = i;
						st = "bl 1 " + i + " " + (320 / 2 - incre) + " " + (incre + 3) + " " + (incre/2);
						lines.AddLast(st);
						lastMiddle = i;
					}
				}
				if (r.Next(0, 100) < 5)
				{
					int yPos = r.Next(100, 200);
					int randomPowerUp = r.Next(1, 5);
					st = "pw " + randomPowerUp+ " " + i + " " + yPos;
					lines.AddLast(st);

				}
			}

			String outPutFile = "";
			foreach (String line in lines)
			{
				outPutFile += System.Environment.NewLine + line;
			}
			return outPutFile;


		}
		public void upLoadText(StringReader sr,String leftAlign,int c)
		{

			List<String> text= new List<String>();
			String line;
			float tScale=g.isIpad()?0.65f:0.45f;

			while(!(line = sr.ReadLine()).Equals("$"))
			{
				int tx=0;
				for(int i=0;i<line.Length;i++)
				{
					float length=tx;
					if(length>350*g.scale)
					{
						int p=i;
						while(line[p]!=' ')
						{
							p--;
						}
						line= line.Substring(0,p)+"{"+line.Substring(p+1);
						tx=0;
					}
					char cee=line[i];
					FontChar fc= g.fontRenderer.getCharacter(cee);
					if(fc!=null)
						tx += (int)(fc.XAdvance*tScale);
				}
				text.Add(line);
			}
			String[] arr=text.ToArray();
			bool leftAl= leftAlign.Equals("l");

			TextBlock newText= new TextBlock(g,arr,c,leftAl);
			if(loadText)
				g.entitToAdd.Add(newText);
			//g.texts.Add(newText);

		}

		public void uploadEnemy(String[] info)
		{
			float time=System.Convert.ToSingle(info[2]);
			int xPos = (int)System.Convert.ToSingle(info[3]);
			int yPos = (int)System.Convert.ToSingle(info[2]);
			Enemy e=null;
			if(info [1].Equals("1"))
				e = new Enemy(g,new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH),time);
			else if(info[1].Equals("2"))
				e = new EnemyShooter(g, new Vector2(xPos * g.scale, 500 * g.scaleH), new Vector2(0, -2 * g.scaleH), time);
			else if(info[1].Equals("3"))
				e = new EnemyCircle(g, new Vector2(xPos * g.scale, 500 * g.scaleH), new Vector2(0, -2 * g.scaleH), time);
			else if(info[1].Equals("4"))
				e = new EnemyWave(g, new Vector2(xPos * g.scale, 500 * g.scaleH), new Vector2(0, -2 * g.scaleH), time);
			else if(info[1].Equals("5"))
				e = new EnemyTele(g, new Vector2(xPos * g.scale, 500 * g.scaleH), new Vector2(0, -2 * g.scaleH), time);
			else if (info[1].Equals("6"))
				e = new EnemyWallShooter(g, new Vector2(xPos * g.scale, 500 * g.scaleH), new Vector2(0, -2 * g.scaleH), time);
			objsToSpawn.Enqueue((float)yPos,e);
			// g.entitToAdd.Add(e);
			totalObj++;
		}

		public void uploadPowerUp(String[] info)
		{
			float time=System.Convert.ToSingle(info[2]);
			int xPos=(int)System.Convert.ToSingle(info[3]);
			int yPos = (int)System.Convert.ToSingle(info[2]);
			SpaceShipPlayer.FireMode mode= SpaceShipPlayer.FireMode.NORMAL;
			Color ranColor = hatColor;
			if(info [1].Equals("1"))
			{
				mode = SpaceShipPlayer.FireMode.NORMAL;
				ranColor = Color.Blue;
			}
			else if(info [1].Equals("2"))
			{
				mode = SpaceShipPlayer.FireMode.TWO;
				ranColor = Color.Red;
			} 
			else if(info [1].Equals("3"))
			{
				mode = SpaceShipPlayer.FireMode.THREE;
				ranColor = Color.Green;
			}
			else if(info [1].Equals("4"))
			{
				mode = SpaceShipPlayer.FireMode.CHARGESHOT;
				ranColor = Color.Purple;
			}
			else if(info [1].Equals("5"))
			{
				mode = SpaceShipPlayer.FireMode.CIRCLE;
				ranColor = Color.Yellow;
			}
			else if(info [1].Equals("6"))
			{
				mode = SpaceShipPlayer.FireMode.FAST;
				ranColor = Color.Orange;
			}

			PowerUp pwp= new PowerUp(g,mode,
			                         new Vector2(xPos*g.scale,500*g.scaleH),new Vector2(0,-3*g.scaleH),ranColor,time);
			//g.entitToAdd.Add(pwp);
			objsToSpawn.Enqueue((float)yPos,pwp);
		}

		public void uploadBlock(String[] info)
		{
			float time=System.Convert.ToSingle(info[2]);
			int xPos = (int)System.Convert.ToSingle(info [3]);
			int yPos = (int)System.Convert.ToSingle(info[2]);
			String spriteName = "block";
			if(info.Length>6)
				spriteName = info[6];
			if(info[1].Equals("1"))
			{
				Block block = new Block(g, time, new Vector2(xPos * g.scale, 500 * g.scaleH), new Vector2((int)System.Convert.ToSingle(info [5]), (int)System.Convert.ToSingle(info [4])), new Vector2(0, -2 * g.scale),spriteName);
				// g.entitToAdd.Add(block);
				objsToSpawn.Enqueue((float)yPos, block);
				Vector2 gridPosition = new Vector2(xPos / 20, yPos / 20);
				if (!grid.ContainsKey(gridPosition))
				{
					grid.Add(gridPosition, block);
				}
				else
				{
					if(block.pos.X>150)
						block.updateSprite("tile6");
					else
						block.updateSprite("tile4");

				}
			}




		}

		public void Update()
		{

			//Vector2 direct = new Vector2(0, -2);



			if(g.texts.Count>0)
				return;
			this.spawnPosition = this.spawnPosition + new Vector2(0,2) * g.gameSpeed;
			//float timer = objsToSpawn.Peek().Value.timer;
			while (objsToSpawn.Count > 0 && objsToSpawn.Peek().Value.timer<= spawnPosition.Y)
			{
				g.entitToAdd.Add(objsToSpawn.DequeueValue());
			}

			/*
					ticker.updateTick();
					if(ticker.hasTicked)
						numTicks++;
					float convTicks=numTicks;///50f;

					while(objsToSpawn.Count>0 && objsToSpawn.Peek().Value.timer<=convTicks) 
					{
						g.entitToAdd.Add(objsToSpawn.DequeueValue());
					}
                    if (g.gameSpeed == 1)
                        ticker.setTickBeat((int)(900f));
                    else
                    {
                        ticker.setTickBeat((int)(900f / (g.gameSpeed)));
                    }*/
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

