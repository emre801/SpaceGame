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
		public enum GameState {TITLE,GAMETIME,OPTIONS,GAMEOVER};
		public GameState gameState;
		public DrawingTool drawingTool;
		Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
		public List<Entity> entitToRemove = new List<Entity>();
		public List<Entity> entitToAdd = new List<Entity>();
		public SpaceShipPlayer player;

		TouchScreenObj tso;
		public EnemySpawner es;
		public BackgroundSpawner bs;
		public TitleScreen ts;
		public MusicPlayer mp;

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
			tso = new TouchScreenObj(this);
			es = new EnemySpawner(this);
			bs = new BackgroundSpawner(this);
			mp = new MusicPlayer(this);
			ts = new TitleScreen(this);
			opt = new Options(this);
			go=new GameOver(this);
		}

		protected override void LoadContent()
		{
			addSprite("Ship", "Ship");
			addSprite("Bullet", "Bullet");
			addSprite("Enemy", "Enemy");
			addSprite("partical", "partical");
			addSprite("blueGUI", "blueGUI");
			addSprite("incre", "incre");
			addSprite("hat", "hat");
			addSprite("shield", "shield");
			addSprite("circleCharge","circleCharge");

			drawingTool.initialize();

			opt.LoadContent();
			mp.addNewSound("shoot");
			mp.addNewSound("explosion");
			mp.addNewSound("powerUp");
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
			oniPad = isIpad();
			if(oniPad) 
			{
				scale = 2.4f;
				scaleH = 2.09166666f;
			}
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

			initHighScore();
		}

		public Sprite getSprite(String fName)
		{
			if(sprites.ContainsKey(fName))
				return sprites[fName];
			return sprites ["Ship"];

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
			// Get the path of the save game
			//LoadHighScores("Content/HighScores.xml");
			// Get the path of the save game
			String HighScoresFilename = "Content/high.txt";
			string fullpath = Path.Combine(StorageContainer.TitleLocation, HighScoresFilename);

			// Check to see if the save exists
			if (!File.Exists(fullpath))
			{
				//If the file doesn't exist, make a fake one...
				// Create the data to save*
				/*
				HighScoreData data = new HighScoreData(5);
				data.PlayerName[0] = "Neil";
				data.Level[0] = 10;
				data.Score[0] = 200500;

				data.PlayerName[1] = "Shawn";
				data.Level[1] = 10;
				data.Score[1] = 187000;

				data.PlayerName[2] = "Mark";
				data.Level[2] = 9;
				data.Score[2] = 113300;

				data.PlayerName[3] = "Cindy";
				data.Level[3] = 7;
				data.Score[3] = 95100;

				data.PlayerName[4] = "Sam";
				data.Level[4] = 1;
				data.Score[4] = 1000;

				SaveHighScores(data, HighScoresFilename);*/
			}
			else
				hsd= new HighScoreData(fullpath);
				
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
			}
			foreach(Entity e in entitToRemove) 
			{
				entities.Remove(e);
			}
			entitToRemove = new List<Entity>();
			entitToAdd = new List<Entity>();

		}
		public void restartGame()
		{
			restart=false;
			interactable = new List<Interact>();
			entities= new List<Entity>();
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


		}


		protected override void Update(GameTime gameTime)
		{

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
			tso.Update();




			if(gameState == GameState.OPTIONS)
			{
				UpdateOptions();
				return;
			}

			if(restart)
			{
				restartGame();
				return;
			}

			mp.playMusic();
			if(isPaused)
					return;
			doCollisions();
			foreach(Entity e in entities)
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
			}
			foreach(Entity e in entitToRemove) 
			{
				entities.Remove(e);if(e is Interact)
				{
					Interact inter = (Interact)e;
					interactable.Remove(inter);
				}
			}

			entitToRemove = new List<Entity>();
			entitToAdd = new List<Entity>();

		}

		public void addSprite(String name,String direct)
		{
			sprites.Add(name, new Sprite(Content, direct));
		}

		public void doCollisions()
		{
			foreach(Interact a in interactable)
				foreach(Interact b in interactable) 
				{
					if(a != b) 
					{
						a.collidesWith(b);
					}
				}

		}

		public CMMotionManager motionManager;
		private void StartGyro()
		{
			motionManager = new CMMotionManager();
			motionManager.GyroUpdateInterval = 1/100;
			if (motionManager.GyroAvailable)
			{
				motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, GyroData_Received);
			}

		}

		public void GyroData_Received(CMGyroData gyroData, NSError error)
		{

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


			drawingTool.drawEntities(entities, gameTime);
			//base.Draw(gameTime);
		}

	}
}
