using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
using MonoTouch.AudioToolbox;

namespace BlankGame
{
		public class MusicPlayer
		{
				Game g;
			Dictionary<string, SystemSound> sounds = new Dictionary<string, SystemSound>();
				public MusicPlayer(Game g)
				{
					this.g = g;
				}
				public void addNewSound(String name)
				{
					//var mediafile=NSUrl.FromFilename(@"Content/"+name+".wav");
					//AVAudioPlayer sound=AVAudioPlayer.FromUrl(new NSUrl(name));
					SystemSound sound = SystemSound.FromFile(@"Content/" + name + ".wav");
					sounds.Add(name, sound);
				}
				public void playSound(String name)
				{
					SystemSound sound = sounds [name];
						sound.PlaySystemSound();
				}
		}
}

