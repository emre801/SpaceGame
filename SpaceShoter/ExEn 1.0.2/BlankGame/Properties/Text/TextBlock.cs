using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlankGame
{
		public class TextBlock: Entity
		{
			String[] text;
			Sprite block;
			int currText=0,stringLength=0;
			int textNum;
			bool isLeftAlign;
			Ticker ticker;
			Sprite icon;
			public TextBlock(Game game,String[] text,int textNum, bool isLeftAlign)
			:base(game)
			{
				this.text=text;
				block= game.getSprite("blue");
				ticker= new Ticker(50);
				this.textNum=textNum;
				this.isLeftAlign=isLeftAlign;
				this.icon= g.getSprite("face");
			}

			public override void Update()
			{
				if(textNum!=g.curTextNum)
					return;
				ticker.updateTick();
				if(g.isSingleTab)
				{
					stringLength=0;
					currText++;
				}
				if(ticker.hasTicked)
				{
					if( currText<text.Length && stringLength<text[currText].Length)
					{
						g.mp.playSound("blip");// change this to something better later.
						stringLength++;
					}
				}
				if(currText>=text.Length)
				{
					g.changeTextNum=true;
					this.isVisible=false;
				}
			}

			public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
			{
				if(this.isVisible && g.curTextNum==textNum)
				{
					spriteBatch.Draw(block.index,new Rectangle(0,250,400,50),Color.White);
					if(isLeftAlign)
					{
						g.fontRenderer.DrawText(spriteBatch,10,260,text[currText].Substring(0,stringLength),0.48f,Color.White);
						spriteBatch.Draw(icon.index, new Rectangle(360,260,30,30),Color.White);
					}
					else
					{
						spriteBatch.Draw(icon.index, new Rectangle(10,260,30,30),Color.White);
						g.fontRenderer.DrawText(spriteBatch,60,260,text[currText].Substring(0,stringLength),0.48f,Color.White);
					}
				}

			}

			

		}
}

