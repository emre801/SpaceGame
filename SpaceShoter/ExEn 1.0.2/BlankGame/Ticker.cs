using System;
using System.Diagnostics;
namespace BlankGame
{
		public class Ticker
		{
				float tickBeat;
				public bool hasTicked=false;
				Stopwatch stopwatch;
				public Ticker(float tickBeat)
				{
					this.tickBeat = tickBeat;
					this.stopwatch = new Stopwatch();
					stopwatch.Start();
				}

				public void setTickBeat(int tickBeat)
				{
					if(tickBeat>0)
						this.tickBeat = tickBeat;
				}

				public void pauseUnpause()
				{
					if(stopwatch.IsRunning)
						stopwatch.Stop();
					else 
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

