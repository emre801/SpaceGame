using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
		public enum GameState {TITLE,GAMETIME,OPTIONS};
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

		public int health=2;
		public int lives=3;
		public bool oniPad;
		public float scale = 1, scaleH=1;

		public FontRenderer fontRenderer;

		public SpaceShipPlayer.FireMode fireMode= SpaceShipPlayer.FireMode.CHARGESHOT;

		public Options opt;//= new Options();


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
			drawingTool.initialize();

			opt.LoadContent();
			mp.addNewSound("shoot");
			mp.addNewSound("explosion");

			player = new SpaceShipPlayer(this,getSprite("Ship"));
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


		protected override void Update(GameTime gameTime)
		{

			tso.Update();
			if(gameState == GameState.TITLE)
			{
				UpdateTite();
				return;
			}
			if(gameState == GameState.OPTIONS)
			{
				UpdateOptions();
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
			motionManager.GyroUpdateInterval = 1/10;
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

			drawingTool.drawEntities(entities, gameTime);
			//base.Draw(gameTime);
		}

	}
}
