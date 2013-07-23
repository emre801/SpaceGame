using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.CoreMotion;
using MonoTouch.Foundation;
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
		public bool isPaused=false;
		public Game()
		{
			//graphics = new GraphicsDeviceManager(this);
			//graphics.PreferredBackBufferWidth = 320;
			//graphics.PreferredBackBufferHeight = 480;

			IsMouseVisible = true;
			gameState = GameState.GAMETIME;
			Content.RootDirectory = "Content";
			drawingTool = new DrawingTool(this);
			StartGyro();
			tso = new TouchScreenObj(this);
		}

		protected override void LoadContent()
		{
			drawingTool.initialize();
			addSprite("Ship", "Ship");
			addSprite("Bullet", "Bullet");
			player = new SpaceShipPlayer(this,getSprite("Ship"));
			entities.Add(player);


		}

		public Sprite getSprite(String fName)
		{
			if(sprites.ContainsKey(fName))
				return sprites[fName];
			return sprites ["Ship"];

		}


		protected override void Update(GameTime gameTime)
		{

			tso.Update();
			if(isPaused)
					return;
			doCollisions();
			foreach(Entity e in entities)
			{
				e.Update();
					if(!e.isVisible)
						entitToRemove.Add(e);
			}
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
			motionManager.GyroUpdateInterval = 1/2;
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
			drawingTool.drawEntities(entities, gameTime);
			//base.Draw(gameTime);
		}

	}
}
