using System.Collections.Generic;
using System.Drawing;
using System.Media;

namespace WindowsFormsApp1.GameEngine
{
    public class Resources
    {
        static Dictionary<string, Image> frames = new Dictionary<string, Image>();
        static Dictionary<string, SoundPlayer> sounds = new Dictionary<string, SoundPlayer>();
        static public void InitializationResources()
        {
            try
            {
                frames = FileSystem.LoadFrames("Res.int");
                sounds = FileSystem.LoadSound("Sound.int");
            }catch(System.Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }
        static public Image GetFrame(string key) => string.IsNullOrEmpty(key) ? null : frames[key];
        static public SoundPlayer GetSound(string key) => sounds[key];
    }
}