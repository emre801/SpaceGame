using System;
using MonoTouch;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BlankGame
{
	public class EnemyWave:Enemy
	{
		Vector2 origPos;
		Stopwatch circleTimer;
		Ticker shot;
		public EnemyWave(Game g, Vector2 pos,Vector2 direct,float timer)
			:base(g,pos,direct,timer)
		{

			circleTimer= new Stopwatch();
			circleTimer.Start();
			shot= new Ticker(200);
			origPos=pos;
		}

		public override void Update()
		{

			circleTimer.Stop();
			double time=(double)circleTimer.ElapsedMilliseconds/500f;
			circleTimer.Start();
			//float yPos=pos.Y+direct.Y;
			float xPos=(float)Math.Cos(time)*50f;

			//this.pos=this.pos+new Vector2(xPos,yPos);
			this.pos = this.pos + (direct)*g.gameSpeed*g.gt;
			this.pos= new Vector2(origPos.X+xPos,this.pos.Y);
			shot.updateTick();

			if(shot.hasTicked)
			{
				g.entitToAdd.Add(new Bullet(g, pos, new Vector2(0, -4*g.scaleH),false));
			}

			if(this.pos.Y < -20)
				this.isVisible = false;
			updateBBox();
		}


	}
}

