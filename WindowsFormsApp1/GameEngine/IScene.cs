using System.Drawing;

namespace WindowsFormsApp1.GameEngine
{
    public interface IScene
    {
        void DrawObjects(Graphics g);
        void DrawBack(Graphics g, int x, int y);
    }
}