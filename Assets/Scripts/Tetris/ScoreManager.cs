using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using System.IO;




public class ScoreManager
{
    public class ScoreEntry : IComparable
    {
        public int score;
        public string name;

        public ScoreEntry(int score, string name)
        {
            this.score = score;
            this.name = name;
        }
        public ScoreEntry(string name, int score)
        {
            this.score = score;
            this.name = name;
        }
        public ScoreEntry()
        {
            score = -1;
            name = "";
        }

        public int CompareTo(object obj)
        {
            ScoreEntry se = obj as ScoreEntry; 
            return -1*score.CompareTo(se.score);
        }
    }


    public List<ScoreEntry> scores = new List<ScoreEntry>();

    const string path = "./score.txt";


    private static ScoreManager instance = null;

    public static ScoreManager getInstance()
    {
        if (instance == null)
            instance = new ScoreManager();

        return instance;
    }




    public string getTopPlayerString(int numPlayers = 10)
    {
        string s = "";
        scores.Sort(); 

        for(int i = 0; i<Math.Min(numPlayers, scores.Count); ++i)
        {
            s += string.Format("{0}. {1,-10} {2}\n", (i + 1), scores[i].name, scores[i].score); 
            //s += (i + 1) + ". " + scores[i].name + "\t" + scores[i].score + "\n"; 
        }
        return s; 
    }

    private ScoreManager()
    {
        if (!File.Exists(path))
        {
            var fs = File.Create(path);
            fs.Close(); 

        }
        readScoreFromFile(); 
    }



    private void writeScoreToFile()
    {
        StreamWriter sw = File.AppendText(path); 

        foreach(var entry in scores)
        {
            if (entry.name.Length > 0 && entry.score >= 0)
                writeLine(sw, entry); 
        }
        sw.Flush();
        sw.Close(); 
    }

    private void writeLine(StreamWriter sw, ScoreEntry entry)
    {
        sw.WriteLine(entry.score + ";" + entry.name);
    }

    private void readScoreFromFile()
    {
        StreamReader sr = File.OpenText(path);
        scores.Clear(); 
        while(!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            string[] seg = line.Split(';');

            ScoreEntry e = new ScoreEntry();
            e.score = int.Parse(seg[0]); 
            e.name = seg[1];

            scores.Add(e); 
        }
        sr.Close(); 
    }

    public void addEntry(string name, int score)
    {
        scores.Add(new ScoreEntry(name, score));
        StreamWriter sw = File.AppendText(path); 
        writeLine(sw, new ScoreEntry(name, score));
        sw.Flush();
        sw.Close(); 
    }

}

