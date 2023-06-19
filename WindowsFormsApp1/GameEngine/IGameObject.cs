using System.Drawing;

namespace WindowsFormsApp1.GameEngine
{
    interface IGameObject
    {
        void Draw(Graphics g);
        //bool Collision(int x, int y);
    }
}