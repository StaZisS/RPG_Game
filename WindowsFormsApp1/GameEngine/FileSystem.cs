using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;

namespace WindowsFormsApp1.GameEngine
{
    public class FileSystem
    {
        public static Dictionary<string,Image> LoadFrames(string path)
        {
            Dictionary<string, Image> res = new Dictionary<string, Image>();
            StreamReader sr = new StreamReader(path);
            while(!sr.EndOfStream)
            {
                string[] lines = sr.ReadLine().Split('|');
                res.Add(lines[0], Image.FromFile(lines[1]));
            }
            sr.Close();
            return res;
        }
        public static Dictionary<string, SoundPlayer> LoadSound(string path)
        {
            Dictionary<string, SoundPlayer> res = new Dictionary<string, SoundPlayer>();
            StreamReader sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                string[] lines = sr.ReadLine().Split('|');
                res.Add(lines[0], new SoundPlayer(lines[1]));
            }
            sr.Close();
            return res;
        }
    }
}