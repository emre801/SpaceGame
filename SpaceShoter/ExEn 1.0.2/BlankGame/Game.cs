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
		public enum GameState {TITLE,GAMETIME};
		public GameState gameState;
		public DrawingTool drawingTool;
		Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
		public List<Entity> entitToRemove = new List<Entity>();
		public List<Entity> entitToAdd = new List<Entity>();
		public SpaceShipPlayer player;

		TouchScreenObj tso;
		EnemySpawner es;
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

		public SpaceShipPlayer.FireMode fireMode= SpaceShipPlayer.FireMode.NORMAL;


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
		}

		protected override void LoadContent()
		{
			addSprite("Ship", "Ship");
			addSprite("Bullet", "Bullet");
			addSprite("Enemy", "Enemy");
			addSprite("partical", "partical");
			addSprite("blueGUI", "blueGUI");
			drawingTool.initialize();

			mp.addNewSound("shoot");
			mp.addNewSound("explosion");

			player = new SpaceShipPlayer(this,getSprite("Ship"));
			entities.Add(player);


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
			}
			foreach(Entity e in entitToRemove) 
			{
				entities.Remove(e);
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
			foreach(Entity a in entities)
				foreach(Entity b in entities) 
			{
				if(a != b && a is Interact && b is Interact) 
				{
					Interact ai = (Interact)a;
					Interact bi = (Interact)b;

					ai.collidesWith(bi);
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
			Console.WriteLine("rotation rate x: {0}, y: {1}, z: {2}", 
			                  gyroData.RotationRate.x, gyroData.RotationRate.y, gyroData.RotationRate.z);
		}



		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			if(gameState == GameState.TITLE) 
			{
				drawingTool.drawTitle(ts,gameTime);
				return;
			}


			drawingTool.drawEntities(entities, gameTime);
			//base.Draw(gameTime);
		}

	}
}
