using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace BlankGame
{
		public class Entity
		{
			public Game g;
			public bool isVisible = true;
			public Entity(Game g)
			{
				this.g = g;
						
			}
			public virtual void Update()
			{



			}

			public virtual void Draw(SpriteBatch spriteBatch,GameTime gameTime)
			{

			}

		}
}

