using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp1.GameEngine
{
    public class Render
    {        
        static Vector _resolution;
        static IScene _scene;
        
        public static void SetResolution(int x, int y) => _resolution = new Vector(x, y);
        
        public static void SetScene(IScene customScene) => _scene = customScene;
        
        public static Image DrawFrame()
        {
            var bitmap = new Bitmap(_resolution.X, _resolution.Y);
            var g = Graphics.FromImage(bitmap);
            _scene.DrawBack(g, _resolution.X, _resolution.Y);
            _scene.DrawObjects(g);
            if (GameEngine.GetIsGameOver())
                DrawGameOverScreen(g);
            return bitmap;
        }
        
        public static Vector Resolution => _resolution;
        
        private static void DrawGameOverScreen(Graphics g)
        {
            var textX = _resolution.X / 2 - 100;
            var textY = _resolution.Y / 2 - 100;
            g.DrawString("GAME OVER", new Font("Stencil", 40),
                new SolidBrush(Color.Red), textX, textY);
        }
    }
}