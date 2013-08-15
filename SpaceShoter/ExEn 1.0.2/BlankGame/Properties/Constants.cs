using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BlankGame
{
		public class Constants
		{
		public const bool ON_IPHONE = true;

		//public const Vector2 startPos=new Vector2(0,0);

		public const float GAME_WORLD_WIDTH = 320f;  // Changing this constant will require changing lots of other constants, unless we cahnge them to be relative to this
		public const float GAME_WORLD_HEIGHT = 480f;
		public const float NUM_BLOCKS_WIDTH = GAME_WORLD_WIDTH / 10;
		public const float NUM_BLOCKS_HEIGHT = GAME_WORLD_HEIGHT / 10;


		public const bool START_WITH_FRESH_FILE=true;

		}
}

