using System;
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
			shot= new Ticker(800);
			origPos=pos;
			ani = g.getAnimation("waveEnemy");
		}

		public override void Update()
		{
			/*
            if (pos.Y > 500 * g.scaleH)
            {
                this.pos = this.pos + new Vector2(0, -2) * g.gameSpeed * g.gt;
                updateBBox();
                return;
            }*/


			circleTimer.Stop();
			float time=(float)(circleTimer.ElapsedMilliseconds)/(500f);
			circleTimer.Start();
			//float yPos=pos.Y+direct.Y;
			float xPos=(float)Math.Cos((double)time)*50f*g.scale;

			//this.pos=this.pos+new Vector2(xPos,yPos);
			this.pos = this.pos + (direct)*g.gameSpeed;
			this.pos= new Vector2(origPos.X+xPos,this.pos.Y);
			shot.updateTick();

			if(shot.hasTicked)
			{
				g.entitToAdd.Add(new Bullet(g, pos, new Vector2(0, -4*g.scaleH),false));
			}

			if(this.pos.Y < -20)
				this.isVisible = false;
			updateBBox();
			shot.setTickBeat((int)(800f/g.gameSpeed));
		}


	}
}

