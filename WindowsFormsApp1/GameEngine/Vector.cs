namespace WindowsFormsApp1.GameEngine
{
    public class Vector
    {
        int x;
        int y;
        public Vector()
        {
            this.x = 0;
            this.y = 0;
        }
        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int X => x;
        public int Y => y;

        public static Vector operator +(Vector a, Vector b) => new Vector(a.x + b.x,a.y + b.y);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.x - b.x,a.y - b.y);

    }
}