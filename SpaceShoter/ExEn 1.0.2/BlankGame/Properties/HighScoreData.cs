using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Storage;
namespace BlankGame
{
	public class HighScoreData
	{
		public ArrayList PlayerName;
		public ArrayList Score;
		public ArrayList Level;
		public HighScoreData(String fileName)
		{
			PlayerName= new ArrayList();
			Score= new ArrayList();
			Level = new ArrayList();
			readFile(fileName);

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
					PlayerName.Add(words[1]);
					Score.Add(System.Convert.ToSingle(words[2]));
					Level.Add(System.Convert.ToSingle(words[2]));
				}

			}
		}
		public void writeFile(String fileName)
		{
			LinkedList<String> lines = new LinkedList<String>();
			for(int i=0; i<PlayerName.Count; i++) 
			{
				String name= (String)PlayerName[i];
				float score= (float)Score[i];
				float level = (float)Level[i];
				String output="HS "+name +" "+score+" "+level;
				lines.AddLast(output);
			}

						String outPutFile = "";
			foreach(String line in lines)
			{
				outPutFile += System.Environment.NewLine + line;
			}
			File.SetAttributes(fileName,FileAttributes.Normal);
			File.WriteAllText(fileName, outPutFile);


		}
	}
	
}

