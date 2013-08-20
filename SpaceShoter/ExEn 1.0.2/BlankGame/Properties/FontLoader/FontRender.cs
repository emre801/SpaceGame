using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BlankGame
{
	public class FontRenderer
	{
		public FontRenderer (FontFile fontFile, Texture2D fontTexture)
		{
			_fontFile = fontFile;
			_texture = fontTexture;
			_characterMap = new Dictionary<char, FontChar>();

			foreach(var fontCharacter in _fontFile.Chars)
			{
				char c = (char)fontCharacter.ID;
				_characterMap.Add(c, fontCharacter);
			}
		}

		private Dictionary<char, FontChar> _characterMap;
		private FontFile _fontFile;
		private Texture2D _texture;
		public void DrawText(SpriteBatch spriteBatch, int x, int y, string text, float scale, Color color)
		{
			int dx = x;
			int dy = y;
			foreach(char c in text)
			{
				FontChar fc;
				if(_characterMap.TryGetValue(c, out fc))
				{
					var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
					var position = new Vector2(dx + fc.XOffset*scale, dy + fc.YOffset*scale);
					var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 
					                                         (int)(sourceRectangle.Width * scale), (int)(sourceRectangle.Height * scale));

					spriteBatch.Draw(_texture, destinationRectangle,sourceRectangle, color);
					dx += (int)(fc.XAdvance*scale);
				}
			}
		}


		public FontChar getCharacter(char c)
		{
			FontChar fc;
			_characterMap.TryGetValue(c, out fc);
			return fc;

		}

		public void DrawText(SpriteBatch spriteBatch, int x, int y, string text, float scale, Color color,int lineLength)
		{
			int dx = x;
			int dy = y;
			int tx=x;
			/*
			for(int i=0;i<text.Length;i++)
			{
				float length=tx-x;
				if(length>lineLength)
				{
					int p=i;
					while(text[p]!=' ')
					{
						p--;
					}
					text= text.Substring(0,p)+"{"+text.Substring(p+1);
					tx=x;
				}
				FontChar fc;
				if(_characterMap.TryGetValue(text[i], out fc))
				{
					tx += (int)(fc.XAdvance*scale);

				}

			}*/


			foreach(char c in text)
			{
				if(c=='{')
				{
					dx=x;
					dy+=(int)15;
					continue;
				}

				FontChar fc;
				if(_characterMap.TryGetValue(c, out fc))
				{
					var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
					var position = new Vector2(dx + fc.XOffset*scale, dy + fc.YOffset*scale);
					var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 
					                                         (int)(sourceRectangle.Width * scale), (int)(sourceRectangle.Height * scale));

					spriteBatch.Draw(_texture, destinationRectangle,sourceRectangle, color);
					dx += (int)(fc.XAdvance*scale);

				}
			}
		}



	}
}

