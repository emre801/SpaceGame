using System;
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

				public Button sfxVolU,sfxVolD,musVolD,musVolU,exit;
				public Options(Game game)
				{
					this.game = game;	
					if(!game.oniPad) 
					{
						sfxVolU= new Button(game,new Rectangle(360,140,20,20));
						sfxVolD= new Button(game,new Rectangle(280,140,20,20));
						
						musVolU= new Button(game,new Rectangle(360,100,20,20));
						musVolD= new Button(game,new Rectangle(280,100,20,20));
						exit= new Button(game,new Rectangle(80,180,130,40));
						
					} 
					else 
					{

					}
				}

				public void LoadContent()
				{
					incre = game.getSprite("incre");
				}
				public void Update()
				{
					sfxVolU.Update();
					sfxVolD.Update();
					musVolU.Update();
					musVolD.Update();
					exit.Update();

					if(sfxVolU.isButtonPressed) 
					{
						if(sfxVolume < 20)
							sfxVolume++;
					}
					if(sfxVolD.isButtonPressed) 
					{
						if(sfxVolume > 0)
							sfxVolume--;
					}

					if(musVolU.isButtonPressed) 
					{
						if(musicVolume < 20)
							musicVolume++;
					}
					if(musVolD.isButtonPressed) 
					{
						if(musicVolume > 0)
							musicVolume--;
					}
							


				}
				public void Draw(SpriteBatch spriteBatch)
				{
					spriteBatch.Draw(incre.index, sfxVolU.demi, Color.White);
					spriteBatch.Draw(incre.index, sfxVolD.demi, Color.White);
					spriteBatch.Draw(incre.index, musVolU.demi, Color.White);
					spriteBatch.Draw(incre.index, musVolD.demi, Color.White);
					//spriteBatch.Draw(incre.index, exit.demi, Color.White);

					if(game.oniPad) 
					{
						game.fontRenderer.DrawText(spriteBatch, 300, -140, "Music Volume", 0.65f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 300, -100, "SFX Volume", 0.65f, Color.White);

					} 
					else 
					{
						game.fontRenderer.DrawText(spriteBatch, 80, 100, "Music Volume", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 80, 140, "SFX Volume", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 80, 180, "Exit", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 320, 100, musicVolume+"", 0.45f, Color.White);
						game.fontRenderer.DrawText(spriteBatch, 320, 140, sfxVolume+"", 0.45f, Color.White);
					}
				}
		}
}

