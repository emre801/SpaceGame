using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BlankGame
{
		public class GUI
		{
			Game game;
			Sprite blueGUI;
			public GUI(Game game)
			{
				this.game = game;
				this.blueGUI = game.getSprite("blueGUI");
			}
			
			public void Draw(SpriteBatch spriteBatch)
			{
				spriteBatch.Draw(blueGUI.index, new Vector2(280, 0), Color.White);
			}

		}
}

