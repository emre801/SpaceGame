using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
using MonoTouch.AudioToolbox;

namespace BlankGame
{
		public class MusicPlayer
		{
			
			Dictionary<string, AVAudioPlayer> sounds = new Dictionary<string, AVAudioPlayer>();
			ArrayList songs = new ArrayList();
			Random r;
			AVAudioPlayer audioPlayer=null;

			Game g;

				public MusicPlayer(Game g)
				{
					this.r = new Random();
					this.g = g;
				}
				public void updateVolume()
				{
					if(audioPlayer!=null)
						audioPlayer.Volume=(float)g.opt.musicVolume;

				}
				public void addNewSound(String name)
				{
					var mediafile=NSUrl.FromFilename(@"Content/Sounds/"+ name+".wav");
					sounds.Add(name,AVAudioPlayer.FromUrl(mediafile));
				}
				public void playSound(String name)
				{
					AVAudioPlayer sound = sounds[name];
					sound.Volume = (float)g.opt.sfxVolume;
					sound.Play();
				}

				public void playMusic()
				{
					if(audioPlayer==null || !audioPlayer.Playing)
					{	
						int index = r.Next() % songs.Count;
						this.audioPlayer = (AVAudioPlayer)songs [index];
						audioPlayer.Volume = (float)g.opt.musicVolume;
						audioPlayer.Play();
					}
				}
				
				public void pauseUnpauseMusic()
				{
					if(audioPlayer == null)
						return;

					if(audioPlayer.Playing)
						audioPlayer.Pause();
					else
						audioPlayer.Play();
				}

				public void addNewSong(String name)
				{
					var mediafile=NSUrl.FromFilename(@"Content/Music/"+ name+".mp3");
					songs.Add(AVAudioPlayer.FromUrl(mediafile));
				}


		}
}

