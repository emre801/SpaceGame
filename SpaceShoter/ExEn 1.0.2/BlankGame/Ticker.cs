using System;
using System.Diagnostics;
namespace BlankGame
{
		public class Ticker
		{
				int tickBeat;
				public bool hasTicked=false;
				Stopwatch stopwatch;
				public Ticker(int tickBeat)
				{
					this.tickBeat = tickBeat;
					this.stopwatch = new Stopwatch();
					stopwatch.Start();
				}

				public void updateTick()
				{
					stopwatch.Stop();
					int time = (int)stopwatch.ElapsedMilliseconds;
					stopwatch.Start();
					hasTicked = false;
					if(time >= tickBeat) 
					{
						hasTicked = true;
						stopwatch.Restart();
					}
				}

		}
}

