using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoTouch.CoreMotion;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#if MONOTOUCH || ANDROID
using OpenTK.Graphics.ES11;
#endif

namespace BlankGame
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		protected GraphicsDeviceManager graphics;
		public List<Entity> entities = new List<Entity>();
		public List<Interact> interactable = new List<Interact>();
		public List<TextBlock> texts= new List<TextBlock>();

		public enum GameState {TITLE,GAMETIME,OPTIONS,GAMEOVER,CONTROLS};
		public GameState gameState;
		public DrawingTool drawingTool;
		Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
		public List<Entity> entitToRemove = new List<Entity>();
		public List<Entity> entitToAdd = new List<Entity>();
		public SpaceShipPlayer player;

		Dictionary<string, SpriteStripAnimationHandler> spriteAnimation = new Dictionary<string, SpriteStripAnimationHandler>();


		TouchScreenObj tso;
		public EnemySpawner es;
		public BackgroundSpawner bs;
		public TitleScreen ts;
		public MusicPlayer mp;
		public Controls cont;

		public bool isPaused=false;
		public SpriteFont sFont;

		public int health=1;
		public int lives=0;
		public bool oniPad;
		public bool shieldActive=false;
		public float scale = 1, scaleH=1;

		public FontRenderer fontRenderer;
		public GameOver go;

		public SpaceShipPlayer.FireMode fireMode= SpaceShipPlayer.FireMode.CIRCLE;

		public Options opt;//= new Options();
		public float gameSpeed = 1f;
		public int numPortals = 0;

		public float points = 0;
		public bool restart=false;

		public HighScoreData hsd;
		public string currentPlayerName = "Santa";
		public int currentLevel=0;

		public int xAnimation=0;
		public bool ignoreDraw=false; 
		public bool tempIgnore=false;
		public bool isOpening=false;
		public bool isClosing=false;
		public Ticker tick;
		public int titlePress=0;

		public bool isSingleTab=false;

		public int curTextNum=0;public bool changeTextNum=false;
		public HashSet<Interact>[,] spaceSqure;
		public bool testLag=true;
		public int maxMoveThing=450;
		Ticker garbageTick;
		public float gt;

		public float chargeShotValue=0;
		public bool isChargingShot = false;
		public Game()
		{
			//graphics = new GraphicsDeviceManager(this);
			//graphics.PreferredBackBufferWidth = 320;
			//graphics.PreferredBackBufferHeight = 480;

			IsMouseVisible = true;
			gameState = GameState.TITLE;
			Content.RootDirectory = "Content";
			drawingTool = new DrawingTool(this);
			StartGyro();
			loadGameInfo();
			tso = new TouchScreenObj(this);
			es = new EnemySpawner(this);
			bs = new BackgroundSpawner(this);
			mp = new MusicPlayer(this);
			ts = new TitleScreen(this);
			opt = new Options(this);
			go=new GameOver(this);
			cont = new Controls(this);
			tick = new Ticker(2);
			garbageTick= new Ticker(10000);

		}

		protected override void LoadContent()
		{

			oniPad = isIpad();
			if(oniPad) 
			{
				scale = 2.4f;
				scaleH = 2.09166666f;
			}
			newSpaceList();

			addSprite("Ship", "Ship");
			addSprite("Bullet", "Bullet");
			addSprite("Enemy", "Enemy");
			addSprite("partical", "partical");
			addSprite("blueGUI", "blueGUI");
			addSprite("incre", "incre");
			addSprite("hat", "hat");
			addSprite("shield", "shield");
			addSprite("block", "block");
			addSprite("circleCharge","circleCharge");
			addSprite("blue","ColorBlocks/blue");
			addSprite("face","HeadShots/face");


			addSprite("Star0","Stars/Star0");
			addSprite("Star1","Stars/Star1");
			addSprite("Star2","Stars/Star2");
			addSprite("Star3","Stars/Star3");
			addSprite("ScrollStar","Stars/ScrollStar");

			addSprite("tile1", "Tiles1/1");
			addSprite("tile2", "Tiles1/2");
			addSprite("tile3", "Tiles1/3");
			addSprite("tile4", "Tiles1/4");
			addSprite("tile5", "Tiles1/5");
			addSprite("tile6", "Tiles1/6");
			addSprite("tile7", "Tiles1/7");
			addSprite("tile8", "Tiles1/8");
			addSprite("tile9", "Tiles1/9");
			addSprite("tile10", "Tiles1/10");
			addSprite("tile11", "Tiles1/11");
			addSprite("tile12", "Tiles1/12");
			addSprite("tile13", "Tiles1/13");
			addSprite("tile14", "Tiles1/14");
			addSprite("tile15", "Tiles1/15");

			addSprite("Title","Title/Title");
			addSprite("TitleBig","Title/TitleBig");
			drawingTool.initialize();

			addAnimation("idealAni","CatAni/idealAni", 3, 0.5f);
			addAnimation("upAni", "CatAni/downAni", 3, 0.5f);
			addAnimation("downAni", "CatAni/upAni", 3, 0.5f);

			addAnimation("circleEnemy", "Enemy/CircleEnemy", 4, 1f);
			addAnimation("normalEnemy", "Enemy/NormalEnemy", 1, 1f);
			addAnimation("waveEnemy", "Enemy/WaveEnemy", 1, 1f);


			opt.LoadContent();
			mp.addNewSound("shoot");
			mp.addNewSound("explosion");
			mp.addNewSound("powerUp");
			mp.addNewSound("menu");
			mp.addNewSound("blip");

			player = new SpaceShipPlayer(this,getSprite("Ship"));
			Shield shield = new Shield(this);
			entitToAdd.Add(shield);
			interactable.Add(shield);

			CirclePUP cPUP = new CirclePUP(this);
			entitToAdd.Add(cPUP);

			entities.Add(player);
			interactable.Add(player);


			FontFile fontFile = FontLoader.Load("Content/Fonts/2pFont.fnt");
			Texture2D fontText=Content.Load<Texture2D>("Fonts\\2p");
			FontRenderer fr = new FontRenderer(fontFile, fontText);
			this.fontRenderer = fr;
			drawingTool.addFontRender(fr);

			//Adding songs to mediaPlayer
			mp.addNewSong("Alpha Black Magic");
			mp.addNewSong("Anxiety Attack");
			mp.addNewSong("Melt Yourself");
			mp.addNewSong("Nostalgia");
			mp.addNewSong("Pins and Needles");
			mp.addNewSong("Pirate Empire");
			mp.addNewSong("Saturday Supernova");
			mp.addNewSong("Nostalgia");
			mp.addNewSong("Syntax Error");
			mp.addNewSong("Tokyo Escapade");
			mp.initPlayer();

			initHighScore();
			es.init(true);

		}

		public Sprite getSprite(String fName)
		{
			if(sprites.ContainsKey(fName))
				return sprites[fName];
			return sprites ["Ship"];

		}
		public void addAnimation(String name,String direct,int count,float frameRate)
		{
			spriteAnimation.Add(name, new SpriteStripAnimationHandler(new Sprite(Content, direct), count, frameRate));
		}
		public SpriteStripAnimationHandler getAnimation(String name)
		{
			if (spriteAnimation.ContainsKey(name))
				return spriteAnimation[name];
			return null;

		}

		public String getFireMode()
		{
			if(fireMode == SpaceShipPlayer.FireMode.NORMAL)
				return "N";
			if(fireMode == SpaceShipPlayer.FireMode.TWO)
				return "2";
			if(fireMode == SpaceShipPlayer.FireMode.THREE)
				return "3";
			if(fireMode == SpaceShipPlayer.FireMode.FAST)
				return "F";
			if(fireMode == SpaceShipPlayer.FireMode.CIRCLE)
				return "C";
			return "S";
		}

		public void initHighScore()
		{
			String loadedPath = "Content/high.txt";
			String documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			String path = Path.Combine(documents, "high.txt");
			if(Constants.START_WITH_FRESH_FILE)
				File.Delete(path);
			// Check to see if the save exists, if it doesn't use the initial one
			if (!File.Exists(path))
				hsd = new HighScoreData(loadedPath,this);
			
			else
				hsd= new HighScoreData(path,this);
				
		}

		public void loadGameInfo()
		{
			String loadedPath = "Content/GameInfo/gameInfo.txt";
			String documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			String path = Path.Combine(documents, "gameInfo.txt");
			if(Constants.START_WITH_FRESH_FILE)
				File.Delete(path);
			String pathToUse;
			if(File.Exists(path))
			{
				pathToUse = path;
			}
			else
			{
				pathToUse = loadedPath;
			}
			string file=File.ReadAllText(pathToUse);
			StringReader sr = new StringReader(file);
			String line;
			char[] delimiterChars = { ' ', ',', ':', '\t' };
			while((line = sr.ReadLine()) != null) 
			{
				string[] words = line.Split(delimiterChars);
				if(words[0].Equals("name"))
				{
					this.currentPlayerName=words[1];
				}
			}
		}

		public void writeGameInfo()
		{
			LinkedList<String> lines = new LinkedList<String>();
			lines.AddLast("name " + this.currentPlayerName);
			String documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			String outPutFile = "";
			foreach(String line in lines)
			{
				outPutFile += System.Environment.NewLine + line;
			}
			var filly= Path.Combine(documents,"gameInfo.txt");
			File.WriteAllText(filly, outPutFile);
		}


		public bool isIpad()
		{
			return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
		}

		public void UpdateTite()
		{
			ts.Update();
			UpdateStarParticles();
		}
		public void UpdateOptions()
		{
			opt.Update();
			UpdateStarParticles();

		}

		public void UpdateControls()
		{
			cont.Update();
			UpdateStarParticles();
		}
		public void UpdateGameOver()
		{
			go.Update();
			UpdateStarParticles();
		}

		public void UpdateStarParticles()
		{
			doCollisions();
			foreach(Entity e in entities)
			{
				if(e is StarParticle)
				{
					if(!e.isVisible) 
					{
						entitToRemove.Add(e);
					} 
					else 
					{
						e.Update();
					}
				}
			}
			bs.Update();
			foreach(Entity e in entitToAdd) 
			{
				entities.Add(e);
				if(e is Interact)
				{
					Interact inter = (Interact)e;
					interactable.Add(inter);
				}
				if(e is TextBlock)
				{
					TextBlock text = (TextBlock)e;
					texts.Add(text);
				}
			}
			foreach(Entity e in entitToRemove) 
			{
				entities.Remove(e);
				if(e is Interact)
				{
					Interact inter = (Interact)e;
					removeFromHashSpace(inter.bbox,inter);
					interactable.Remove(inter);
				}
				if(e is TextBlock)
				{
					TextBlock text = (TextBlock)e;
					texts.Remove(text);
				}
			}
			entitToRemove = new List<Entity>();
			entitToAdd = new List<Entity>();

		}

		public void clearAllEntitiesThatArentBG()
		{
			entitToRemove = new List<Entity>();
			foreach(Entity e in entities)
			{
				if(!(e is StarParticle))
				{
					entitToRemove.Add(e);
				}
			}
			foreach(Entity e in entitToRemove)
				entities.Remove(e);


		}

		public void restartGame()
		{
			curTextNum=0;
			restart=false;
			interactable = new List<Interact>();
			texts= new List<TextBlock>();
			//entities= new List<Entity>();
			clearAllEntitiesThatArentBG();
			entitToRemove = new List<Entity>();
			entitToAdd = new List<Entity>();
			es = new EnemySpawner(this);
			player = new SpaceShipPlayer(this,getSprite("Ship"));
			Shield shield = new Shield(this);
			entitToAdd.Add(shield);
			interactable.Add(shield);

			CirclePUP cPUP = new CirclePUP(this);
			entitToAdd.Add(cPUP);

			entities.Add(player);
			interactable.Add(player);
			this.points = 0;
			es.init(true);


		}
		public void newSpaceList()
		{
			spaceSqure = new HashSet<Interact>[(int)(Constants.NUM_BLOCKS_WIDTH*scale),(int)(Constants.NUM_BLOCKS_HEIGHT*scaleH)];
			for(int x=0;x<(int)(Constants.NUM_BLOCKS_WIDTH*scale);x++)
				for(int y=0;y<(int)(Constants.NUM_BLOCKS_HEIGHT*scaleH);y++)
					spaceSqure[x,y]=new HashSet<Interact>();
			foreach(Entity e in entities)
			{
				if(!(e is SpaceShip) && !(e is StarParticle) && !(e is Shield)) 
				{
					e.isVisible = false;
				}

			}

		}

		protected override void Update(GameTime gameTime)
		{
			//mp.playMusic();
			this.gt=1f;//(float)((1f/60f)/gameTime.ElapsedGameTime.TotalSeconds);
			//Console.WriteLine(gt);
			tick.updateTick();
			if(gameState == GameState.TITLE)
			{
				UpdateTite();
				tso.Update();
				return;
			}

			if(gameState == GameState.GAMEOVER)
			{
				UpdateGameOver();
				tso.Update();
				return;
			}
			if(gameState == GameState.OPTIONS)
			{
				UpdateOptions();
				tso.Update();
				return;
			}
			if(gameState == GameState.CONTROLS)
			{
				UpdateControls();
				tso.Update();
				return;
			}
			tso.Update();

			//mp.playMusic();

			if(restart)
			{
				restartGame();
				return;
			}


			if(isPaused)
					return;
			doCollisions();
			//updateSpace();
			changeTextNum=false;
			int numEnemies=0;
			foreach(Entity e in entities)
			{
				if(!e.isVisible) 
				{
					entitToRemove.Add(e);

				} 
				else 
				{
					e.Update();
					//if(e is Interact && !(e is Shield) && !(e is Bullet) && !(e is Block))
					if(e is Enemy)
						numEnemies++;
				}
			}
			es.Update();
			bs.Update();
			foreach(Entity e in entitToAdd) 
			{
				entities.Add(e);
				if(e is Interact)
				{
					Interact inter = (Interact)e;
					interactable.Add(inter);
				}
				if(e is TextBlock)
				{
					TextBlock text = (TextBlock)e;
					texts.Add(text);
				}
			}
			foreach(Entity e in entitToRemove) 
			{
				entities.Remove(e);
				if(e is Interact)
				{
					Interact inter = (Interact)e;
					removeFromHashSpace(inter.bbox,inter);
					interactable.Remove(inter);
				}
				if(e is TextBlock)
				{
					TextBlock text = (TextBlock)e;
					texts.Remove(text);
				}
			}
			if(changeTextNum)
			{
				curTextNum++;
			}
			if(numEnemies<=0 && es.hasFinishedSpawning()&& entitToAdd.Count==0)
			{
				es = new EnemySpawner(this);
				es.init(false);
			}
			entitToRemove = new List<Entity>();
			entitToAdd = new List<Entity>();

			garbageTick.updateTick();
			if(garbageTick.hasTicked)
			{
				System.GC.Collect();
				System.GC.WaitForPendingFinalizers();
			}
		}

		public void addSprite(String name,String direct)
		{
			sprites.Add(name, new Sprite(Content, direct));
		}
		public void removeFromHashSpace(Rectangle cool,Interact lol)
		{
			int point1 = cool.X/10;
			int point2 = cool.Y/10;
			int point3=(cool.X+cool.Width)/10;
			int point4= (cool.Y+cool.Height)/10;
			for(int x=point1;x<=point3;x++)
			{
				for(int y=point2;y<=point4;y++)
				{
					if(x<(int)(Constants.NUM_BLOCKS_WIDTH*scale) && y< (int)(Constants.NUM_BLOCKS_HEIGHT*scaleH) && x>0 && y>0)
						spaceSqure[x,y].Remove(lol);

				}
			}

		}

		public void doCollisions()
		{
			/*
			if(true)
			{
				foreach(Interact a in interactable)
					foreach(Interact b in interactable) 
					{
						if(a != b) 
						{
							a.collidesWith(b);
						}
					}
				return;
			}
			*/

			for(int x=0;x<(int)(Constants.NUM_BLOCKS_WIDTH*scale);x++)
			{
				for(int y=0;y<(int)(Constants.NUM_BLOCKS_HEIGHT*scaleH);y++)
				{
					HashSet<Interact> elementsInSpace = spaceSqure [x, y];
					if(elementsInSpace.Count<=1)
						continue;
					foreach(Interact a in elementsInSpace)
					{		
						foreach(Interact b in elementsInSpace)
						{
							if(a!=b && a.isVisible && b.isVisible)
							{

								a.collidesWith(b);
							}
						}
					}
				}
			}//*/ //// In theory this is faster, but because you have to constantly update the elements space each it just wastes more time 

		}

		public void updateSpace()
		{
			for(int x=0;x<(int)(Constants.NUM_BLOCKS_WIDTH*scale);x++)
			{
				for(int y=0;y<(int)(Constants.NUM_BLOCKS_HEIGHT*scaleH);y++)
				{
					HashSet<Interact> elementsInSpace = spaceSqure [x, y];
					HashSet<Interact> newSet = new HashSet<Interact>();
					foreach(Interact a in elementsInSpace)
					{		
							if(a.isVisible)
								newSet.Add(a);
					}
					spaceSqure [x, y] = newSet;
				}
			}


		}

		public CMMotionManager motionManager;
		private void StartGyro()
		{
			motionManager = new CMMotionManager();
			motionManager.GyroUpdateInterval = 1d/10000d;
			if (motionManager.GyroAvailable)
			{
				motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, GyroData_Received);
			}

		}

		public void GyroData_Received(CMGyroData gyroData, NSError error)
		{
			//Could print information here but I don't need anything right now.
		}


		public void drawFrameRate(GameTime gameTime)
		{
			drawingTool.drawFrameRate(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			drawingTool.updateCamera();
			if(gameState == GameState.TITLE) 
			{
				drawingTool.drawTitle(ts,gameTime);
				return;
			}
			if(gameState == GameState.OPTIONS)
			{
				drawingTool.drawOptions(opt, gameTime);
				return;
			}
			if(gameState == GameState.GAMEOVER)
			{
				drawingTool.drawGameOver(go, gameTime);
				return;
			}
			if(gameState == GameState.CONTROLS)
			{
				drawingTool.drawControls(cont, gameTime);
				return;
			}


			if(this.tick.hasTicked && this.xAnimation > 0 && this.isOpening) 
			{
				this.xAnimation -= 50;
				if(this.xAnimation <= 0)
					this.isOpening = false;

			}
			else if(this.tick.hasTicked && this.xAnimation < this.maxMoveThing && this.isClosing)
			{
				this.xAnimation += 50;
				if(this.xAnimation >= this.maxMoveThing) 
				{
					this.gameState = Game.GameState.TITLE;
					this.isClosing = false;
					this.isOpening = true;
					this.tempIgnore = true;mp.pauseUnpauseMusic();
				}
			}
			if(xAnimation < 0)
				xAnimation = 0;
			if(xAnimation > maxMoveThing)
				xAnimation = maxMoveThing;
			//if(!testLag)
				drawingTool.drawEntities(entities, gameTime);
			//else
			//	drawingTool.drawBS();

			//drawFrameRate(gameTime);
			//base.Draw(gameTime);
		}

	}
}
