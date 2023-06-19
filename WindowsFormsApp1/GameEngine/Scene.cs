using System.Drawing;
using WindowsFormsApp1.Map;
namespace WindowsFormsApp1.GameEngine
{
    public class Scene : IScene
    {
        private readonly Image _backGroundWasteland;
        private readonly Image _backGroundCity;

        public Scene()
        {
            _backGroundWasteland = Resources.GetFrame("BackGroundWasteland");
            _backGroundCity = Resources.GetFrame("BackGroundCity");
        }
        
        public void DrawObjects(Graphics g)
        {
            var currentMap = Map.Map.Instance.GetCurrentCell();
            Player.Instance.Draw(g);

            foreach (var obj in currentMap.MapStrategies)
            {
                obj.Draw(g);
            }        
            
            foreach (var obj in currentMap.Enemies)
            {
                obj.Draw(g);
            }

            foreach (var obj in currentMap.Npc)
            {
                obj.Draw(g);
            }
        }
        
        public void DrawBack(Graphics g, int x, int y) => 
            g.DrawImage(
                Map.Map.Instance.GetCurrentCell().TypeCell == TypeCell.Wasteland ?
                _backGroundWasteland : 
                _backGroundCity,
                0, 0,
                x,
                y
                );
    }
}