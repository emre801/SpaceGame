using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
namespace BlankGame
{
		public class Block : Interact
		{
				Vector2 wh;
				public Block(Game g,int t, Vector2 pos,Vector2 wh,Vector2 vel)
				:base(g,t)
				{
					this.pos = pos;
					this.wh = wh;
					this.direct = vel;
				}
				public override void updateBBox()
				{
					bbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(wh.X*g.scale), (int)(wh.Y*g.scale));
				}
				
				public override void Update()
				{
					this.pos= this.pos+this.direct;
				}
	}
}

