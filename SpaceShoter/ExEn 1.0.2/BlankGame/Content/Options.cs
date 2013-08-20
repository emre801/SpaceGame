using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BlankGame
{
		public class Options
		{
				//This class will keep track of all possible game options
				public int sfxVolume=10;
				public int musicVolume=10;
				Sprite incre;
				Game game;
				Stopwatch ticker;
				LetterButton[] lButtons;
				String sound = "menu";
				public Button sfxVolU,sfxVolD,musVolD,musVolU,exit;
				//int xAnimation=200;
				Ticker t;
				Sprite start;
				public Options(Game game)
				{
					t = new Ticker(2);
					this.game = game;	
					if(game.isIpad()) 
					{
						sfxVolU= new Button(game,new Rectangle(740,-100,20,20));
						sfxVolD= new Button(game,new Rectangle(650,-100,20,20));
						
						musVolU= new Button(game,new Rectangle(740,-140,20,20));
						musVolD= new Button(game,new Rectangle(650,-140,20,20));
						exit= new Button(game,new Rectangle(280,10,130,40));
						
					} 
					else 
					{
						sfxVolU= new Button(game,new Rectangle(360,140,20,20));
						sfxVolD= new Button(game,new Rectangle(280,140,20,20));

						musVolU= new Button(game,new Rectangle(360,100,20,20));
						musVolD= new Button(game,new Rectangle(280,100,20,20));
						exit= new Button(game,new Rectangle(80,260,130,40));
					}
					ticker = new Stopwatch();
					ticker.Start();
					lButtons = new LetterButton[6];
					char[] cArr = toCharArray();
					int counter = 40;
					for(int i=0; i<6; i++)
					{
						if(game.isIpad())
							lButtons [i] = new LetterButton(game, new Vector2(320+counter*i,-40),cArr[i]);
						else
							lButtons [i] = new LetterButton(game, new Vector2(80+counter*i,200),cArr[i]);
					}
					initOptions();
				}
				
				public char[] toCharArray()
				{
					char[] cArr= new char[6];
					char[] nameArray= game.currentPlayerName.ToUpper().ToCharArray();
					for(int i=0; i<6; i++) 
					{
						if(i < nameArray.Length)
							cArr [i] = nameArray [i];
						else
							cArr [i] = ' ';
					}
					return cArr;

				}

				public void initOptions()
				{
					String loadedPath = "Content/option.txt";
					String documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
					String path = Path.Combine(documents, "option.txt");
					if(Constants.START_WITH_FRESH_FILE)
						File.Delete(path);
					// Check to see if the save exists, if it doesn't use the initial one
					if(!File.Exists(path))
						readFile(loadedPath);
					else
						readFile(path);

					
				}

				public void LoadContent()
				{
					incre = game.getSprite("incre");
					start=game.getSprite("Enemy");
				}
				public void Update()
				{
					t.updateTick();
					if(t.hasTicked && game.xAnimation > 0 && game.isOpening) 
					{
						game.xAnimation -= 20;
						if(game.xAnimation <= 0)
							game.isOpening = false;

					}
					else if(t.hasTicked && game.xAnimation < game.maxMoveThing && game.isClosing)
					{
						game.xAnimation += 20;
						if(game.xAnimation >= game.maxMoveThing) 
						{
							game.gameState = Game.GameState.TITLE;
							game.isClosing = false;
							game.tempIgnore = true;
							game.isOpening = true;
							//game.xAnimation = 0;
							
						}
					}
					if(game.xAnimation < 0)
						game.xAnimation = 0;
					if(game.xAnimation > game.maxMoveThing)
						game.xAnimation = game.maxMoveThing;

					sfxVolU.Update();
					sfxVolD.Update();
					musVolU.Update();
					musVolD.Update();
					for(int i=0; i<6; i++)
						lButtons [i].Update();
					exit.Update();
					
					ticker.Stop();
					int time = (int)ticker.ElapsedMilliseconds;
					bool allowInput = false;
					if(time >= 250) 
					{
						ticker.Restart();
						allowInput = true;
					}
					else
						ticker.Start();
				
					if(sfxVolU.isPressed) 
					{
						if(sfxVolume < 20 && allowInput) 
						{
							game.mp.playSound(sound);
							sfxVolume++;
						}
					}
					if(sfxVolD.isPressed&& allowInput) 
					{
						if(sfxVolume > 0) 
						{
							game.mp.playSound(sound);
							sfxVolume--;
						}
					}

					if(musVolU.isPressed&& allowInput) 
					{
						if(musicVolume < 20)
						{
							game.mp.playSound(sound);
							musicVolume++;
						}
					}
					if(musVolD.isPressed&& allowInput) 
					{
						if(musicVolume > 0)
						{
							game.mp.playSound(sound);
							musicVolume--;
						}
					}

				if(game.ignoreDraw)
					game.ignoreDraw=false;
					
						


				}
				public void updateName()
				{
					String newName = "";
					for(int i=0; i<lButtons.Length; i++)
						newName += lButtons [i].c;
					game.currentPlayerName = newName;
				}

				public void writeFile()
				{
					LinkedList<String> lines = new LinkedList<String>();
					lines.AddLast("mv " + musicVolume);
					lines.AddLast("sfx " + sfxVolume);
					String outPutFile = "";
					foreach(String line in lines)
					{
						outPutFile += System.Environment.NewLine + line;
					}

					String documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);

					var filly= Path.Combine(documents,"option.txt");
					File.WriteAllText(filly, outPutFile);
				}

				public void readFile(String fileName)
				{
					string file=File.ReadAllText(fileName);
					StringReader sr = new StringReader(file);
					String line;
					char[] delimiterChars = { ' ', ',', ':', '\t' };
					while((line = sr.ReadLine()) != null) {
						string[] words = line.Split(delimiterChars);
						if(words[0].Equals("mv"))
						{
							float tempV=System.Convert.ToSingle(words[1]);
							this.musicVolume=(int)tempV;
						}
						if(words[0].Equals("sfx"))
						{
							float tempV=System.Convert.ToSingle(words[1]);
							this.sfxVolume=(int)tempV;
						}

					}
				}


				public void Draw(SpriteBatch spriteBatch)
				{
					spriteBatch.Draw(incre.index, sfxVolU.demi, Color.White);
					spriteBatch.Draw(incre.index, sfxVolD.demi, Color.White);
					spriteBatch.Draw(incre.index, musVolU.demi, Color.White);
					spriteBatch.Draw(incre.index, musVolD.demi, Color.White);
					//spriteBatch.Draw(incre.index, exit.demi, Color.White);
					if(start != null) 
					{
						//spriteBatch.Draw(start.index, sfxVolD.demi, Color.White);
						//spriteBatch.Draw(start.index, sfxVolU.demi, Color.White);
						//spriteBatch.Draw(start.index, musVolD.demi, Color.White);
						//spriteBatch.Draw(start.index, musVolU.demi, Color.White);
						//spriteBatch.Draw(start.index, exit.demi, Color.White);
					}



					if(game.oniPad) 
					{
						game.fontRenderer.DrawText(spriteBatch, 300+game.xAnimation, -140, "Music Volume", 0.65f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 300+game.xAnimation, -100, "SFX Volume", 0.65f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 300+game.xAnimation, 20, "Exit", 0.65f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 700+game.xAnimation, -140, musicVolume+"", 0.65f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 700+game.xAnimation, -100, sfxVolume+"", 0.65f, Color.White);

					} 
					else 
					{
						game.fontRenderer.DrawText(spriteBatch, 80+game.xAnimation, 100, "Music Volume", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 80+game.xAnimation, 140, "SFX Volume", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 80+game.xAnimation, 260, "Exit", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 320+game.xAnimation, 100, musicVolume+"", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 320+game.xAnimation, 140, sfxVolume+"", 0.45f, Color.White);
						
					}
						for(int i=0; i<6; i++)
							lButtons [i].Draw(spriteBatch,null);
				}
		}
}

