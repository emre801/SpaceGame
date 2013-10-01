using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace BlankGame
{
	class EnemyWallShooter: Enemy
	{
		Ticker shotTicker;
		public EnemyWallShooter(Game g, Vector2 pos, Vector2 direct, float timer)
			:base(g,pos,direct,timer)
		{
			shotTicker = new Ticker(800);

		}
		public override void Update()
		{
			Vector2 playPos = g.player.pos;
			float distance = Vector2.Distance(playPos, this.pos);
			if (distance <= 100*g.scale)
			{
				shotTicker.updateTick();
				if (shotTicker.hasTicked)
				{
					Vector2 direction = playPos - pos;
					direction = Vector2.Normalize(direction);
					direction = direction * 4f;
					g.entitToAdd.Add(new Bullet(g, pos, direction,false));
				}
				shotTicker.setTickBeat((int)(800f / (g.gameSpeed)));
			}
			base.Update();
		}
	}
}
