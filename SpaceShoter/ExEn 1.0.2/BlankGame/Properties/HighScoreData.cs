using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
namespace BlankGame
{


	public class HighScoreInfo : IComparable<HighScoreInfo>
	{
		public String playerName;
		public int score;
		public int level;
		public HighScoreInfo(String playerName,int score,int level)
		{
			this.playerName=playerName;
			this.score=score;
			this.level=level;
		}

		public int CompareTo(HighScoreInfo other)
		{
			if(this.score == other.score) 
			{
				return playerName.CompareTo(other.playerName);
			}

			return this.score > other.score ? 1 : -1;

		}

	}



	public class HighScoreData
	{
		//public ArrayList PlayerName;
		//public ArrayList Score;
		//public ArrayList Level;
		Game g;
		PriorityQueue<float,HighScoreInfo> queue;
		public HighScoreData(String fileName,Game g)
		{
			//PlayerName= new ArrayList();
			//Score= new ArrayList();
			//Level = new ArrayList();
			queue = new PriorityQueue<float,HighScoreInfo>();
			readFile(fileName);
			this.g = g;

		}
		public void readFile(String fileName)
		{
			string file=File.ReadAllText(fileName);
			StringReader sr = new StringReader(file);
			String line;
			char[] delimiterChars = { ' ', ',', ':', '\t' };
			while((line = sr.ReadLine()) != null) {
			string[] words = line.Split(delimiterChars);
			if(words[0].Equals("HS"))
			{
				String pName=words[1];
				float score=System.Convert.ToSingle(words[2]);
				float level=System.Convert.ToSingle(words[3]);
				addNewScore(pName,score,level);
			}

			}
		}
		public void writeFile(String fileName)
		{
			LinkedList<String> lines = new LinkedList<String>();
			int count = queue.Count;
			PriorityQueue<float,HighScoreInfo> tempQueue = new PriorityQueue<float, HighScoreInfo>();
			for(int i=0; i<count; i++) 
			{
				HighScoreInfo tempHighScore = queue.Dequeue().Value;
				tempQueue.Enqueue((int)(tempHighScore.score+tempHighScore.playerName.GetHashCode()/100000f)*-1,tempHighScore);
				String output="HS "+tempHighScore.playerName +" "+(int)tempHighScore.score+" "+(int)tempHighScore.level;
				lines.AddLast(output);
			}
			queue = tempQueue;


			String outPutFile = "";
			foreach(String line in lines)
			{
				outPutFile += System.Environment.NewLine + line;
			}
			//File.SetAttributes(fileName,FileAttributes.Normal);

			String documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);

			var filly= Path.Combine(documents,fileName);
			//String[] dirs= Directory.GetFiles(Environment.GetFolderPath (Environment.SpecialFolder.Personal),"*");
			File.WriteAllText(filly, outPutFile); 
		}

		public void drawTop10(SpriteBatch spriteBatch)
		{
			int spacing = 0;
			g.fontRenderer.DrawText(spriteBatch, 300, 20, "Top 10", 0.45f, Color.White);
			PriorityQueue<float,HighScoreInfo> tempQueue = new PriorityQueue<float, HighScoreInfo>();
			int count = queue.Count;
			for(int i=0; i<count; i++) 
			{
				//String info = (String)PlayerName [i] + " " + (float)(Score [i]);
				HighScoreInfo tempHighScore = queue.Dequeue().Value;
				tempQueue.Enqueue((int)(tempHighScore.score+tempHighScore.playerName.GetHashCode()/100000f)*-1,tempHighScore);
				String info = tempHighScore.playerName + " " + tempHighScore.score;
				g.fontRenderer.DrawText(spriteBatch, 300, 40+spacing, info, 0.35f, Color.White);
				spacing += 10;
			}
			queue = tempQueue;



		}

		public void addNewScore(String name,float score, float level)
		{
			HighScoreInfo newHighScore = new HighScoreInfo(name, (int)score, (int)level);
			queue.Enqueue((int)(score+name.GetHashCode()/100000f), newHighScore);
		}


	}
	
}

