using System;

namespace WindowsFormsApp1.GameEngine
{
    public class GameEngine
    {
        private static readonly Lazy<GameEngine> InstanceGameEngine =
            new Lazy<GameEngine>(() => new GameEngine());

        private static Form1 _form1;
        private static Scene _scene;
        private static bool _isRunning;
        private static bool _isGameOver;
        private static InventoryManager _inventoryManager;
        public static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private bool _isInventoryOpen;
        
        private GameEngine()
        {
            _isInventoryOpen = false;
        }
        
        public static void Initialize(Form1 form1)
        {
            _form1 = form1;
            Resources.InitializationResources();
            Render.SetResolution(_form1.Size.Width, _form1.Size.Height);
            _scene = new Scene();
            Render.SetScene(_scene);
            _inventoryManager = new InventoryManager(_form1);
            _isRunning = true;
        }
        
        public static GameEngine Instance => InstanceGameEngine.Value;

        public static void Update()
        {
            var map = Map.Map.Instance;
            
            map.UpdateStrategy();
            
            foreach (var npc in map.GetNpc())
            {
                npc.Action();
            }
            
            foreach (var enemy in map.GetEnemies())
            {
                enemy.Action();
            }
            
            Player.Instance.Action();
        }
        
        public static bool GetIsRunning() => _isRunning;
        
        public void SwitchIsRunning() => _isRunning = !_isRunning;

        public bool GetInventoryOpen() => _isInventoryOpen;
        
        public void SwitchInventoryOpen() => _isInventoryOpen = !_isInventoryOpen;
        
        public static void GameOver() => _isGameOver = true;

        public static bool GetIsGameOver() => _isGameOver;
    }
}