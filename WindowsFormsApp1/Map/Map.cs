using System;
using System.Collections.Generic;
using System.Linq;
using RPGProject.LivingEntity;
using RPGProject.SubscriptionService;
using WindowsFormsApp1.EnemyStrategy;

namespace WindowsFormsApp1.Map
{
    public class Map
    {
        private static readonly Lazy<Map> InstanceMap =
            new Lazy<Map>(() => new Map());
        
        public static Map Instance => InstanceMap.Value;
        
        private Cell _currentCell;
        private const int CountEnemy = 5;
        private const double ChanceOfCity = 0.2;
        private const double ChanceOfEnemy = 0.6;
        private const double ChanceOfNpc = 0.5;
        
        public class Cell
        {
            public SubscriptionService SubscriptionService { get; set; }
            public TypeCell TypeCell { get; set; }
            public List<Enemy> Enemies { get; set; }
            public List<Npc> Npc { get; set; }
            public NeighboringCells NeighboringCells { get; set; }
            public List<MapStrategy> MapStrategies { get; set; }
        }

        public class NeighboringCells
        {
            public Cell LeftCell { get; set; }
            public Cell RightCell { get; set; }
            public Cell TopCell { get; set; }
            public Cell BottomCell { get; set; }
        }

        public Map()
        {
            _currentCell = GenerateCell(TypeCell.Wasteland); 
        }
        
        private TypeCell GenerateRandomTypeCell()
        {
            return ChanceOfCity > GameEngine.GameEngine.Random.NextDouble() ? TypeCell.City : TypeCell.Wasteland;
        }
        
        private Cell GenerateCell(TypeCell typeCell)
        {
            switch (typeCell)
            {
                case TypeCell.City:
                    return GenerateCity();
                case TypeCell.Wasteland:
                    return GenerateWasteland();
                default:
                    return GenerateWasteland();
            }
        }

        private Cell GenerateCity()
        {
            var listNpc = new List<Npc>
            {
                new Blacksmith(),
                new Dealer(),
                new Inhabitant()
            };

            while (GameEngine.GameEngine.Random.NextDouble() < ChanceOfNpc)
            {
                listNpc.Add(Npc.GenerateRandomNpc());
            }
            
            return new Cell
            {
                SubscriptionService = new SubscriptionService(),
                TypeCell = TypeCell.City,
                Npc = listNpc,
                NeighboringCells = new NeighboringCells(),
                Enemies = new List<Enemy>(),
                MapStrategies = new List<MapStrategy>()
            };
        }

        private Cell GenerateWasteland()
        {
            var listEnemy = new List<Enemy>();
            
            for (var i = 0; i < CountEnemy; i++)
            {
                listEnemy.Add(Enemy.GenerateRandomEnemy());
            }
            
            while (GameEngine.GameEngine.Random.NextDouble() < ChanceOfEnemy)
            {
                listEnemy.Add(Enemy.GenerateRandomEnemy());
            }
            
            return new Cell
            {
                SubscriptionService = new SubscriptionService(),
                TypeCell = TypeCell.Wasteland,
                Npc = new List<Npc>(),
                NeighboringCells = new NeighboringCells(),
                Enemies = listEnemy,
                MapStrategies = new List<MapStrategy>()
            };
        }
        
        public Cell GetCurrentCell() => _currentCell;

        public Cell SwitchCell(DirectionOfMove directionOfMove)
        {
            switch (directionOfMove)
            {
                case DirectionOfMove.Down:
                    _currentCell.NeighboringCells.BottomCell = 
                        _currentCell.NeighboringCells.BottomCell 
                        ?? GenerateCell(GenerateRandomTypeCell());
                    _currentCell.NeighboringCells.BottomCell.NeighboringCells.TopCell = _currentCell;
                    _currentCell = _currentCell.NeighboringCells.BottomCell;
                    break;
                case DirectionOfMove.Up:
                    _currentCell.NeighboringCells.TopCell =
                        _currentCell.NeighboringCells.TopCell 
                        ?? GenerateCell(GenerateRandomTypeCell());
                    _currentCell.NeighboringCells.TopCell.NeighboringCells.BottomCell = _currentCell;
                    _currentCell = _currentCell.NeighboringCells.TopCell;
                    break;
                case DirectionOfMove.Left:
                    _currentCell.NeighboringCells.LeftCell =
                        _currentCell.NeighboringCells.LeftCell 
                        ?? GenerateCell(GenerateRandomTypeCell());
                    _currentCell.NeighboringCells.LeftCell.NeighboringCells.RightCell = _currentCell;
                    _currentCell = _currentCell.NeighboringCells.LeftCell;
                    break;
                case DirectionOfMove.Right:
                    _currentCell.NeighboringCells.RightCell =
                        _currentCell.NeighboringCells.RightCell 
                        ?? GenerateCell(GenerateRandomTypeCell());
                    _currentCell.NeighboringCells.RightCell.NeighboringCells.LeftCell = _currentCell;
                    _currentCell = _currentCell.NeighboringCells.RightCell;
                    break;
            }
            return _currentCell;
        }

        public List<Npc> GetNpc() => _currentCell.Npc;

        public List<Enemy> GetEnemies() => _currentCell.Enemies;
        
        public void RemoveLivingEntity(LivingEntity livingEntity)
        {
            switch (livingEntity)
            {
                case Npc npc:
                {
                    var npcToRemove = _currentCell.Npc.FirstOrDefault(n => n.Id == npc.Id);
                    if (npcToRemove != null)
                    {
                        _currentCell.Npc.Remove(npcToRemove);
                    }
                    break;
                }
                case Enemy enemy:
                    var enemyToRemove = _currentCell.Enemies.FirstOrDefault(e => e.Id == enemy.Id);
                    if (enemyToRemove != null)
                    {
                        _currentCell.Enemies.Remove(enemyToRemove);
                    }
                    break;
            }
        }
        
        public Guid AddSubscription(TypeSubscription typeSubscription, Action action, MapStrategy mapStrategy)
        { 
            _currentCell.MapStrategies.Add(mapStrategy);
            return _currentCell.SubscriptionService.Subscribe(typeSubscription, action);
        }

        public void RemoveSubscription(TypeSubscription typeSubscription, Guid id)
        {
            var strategyToRemove = _currentCell.MapStrategies.FirstOrDefault(s => s.Id == id);
            if (strategyToRemove != null)
            {
                _currentCell.MapStrategies.Remove(strategyToRemove);
            }
            _currentCell.SubscriptionService.Unsubscribe(typeSubscription, id);
        }

        public void AddEnemy(Enemy enemy) => _currentCell.Enemies.Add(enemy);

        public void UpdateStrategy()
        {
            foreach (var obj in _currentCell.
                         SubscriptionService.
                         GetSubscriptions(TypeSubscription.MapStrategy).ToList())
            {
                obj.Invoke();
            }
        }

        public List<LivingEntity> GetLivingEntitiesInRadius((double, double) pointInMap, double radius)
        {
            var entitiesInRadius = new List<LivingEntity>();
            var livingEntities = new List<LivingEntity>();
            
            livingEntities.AddRange(_currentCell.Npc);
            livingEntities.AddRange(_currentCell.Enemies);
            livingEntities.Add(Player.Instance);
            
            foreach (var entity in livingEntities)
            {
                var (x, y) = entity.GetPosition();
                var distance = Math.Sqrt(Math.Pow(x - pointInMap.Item1, 2) + Math.Pow(y - pointInMap.Item2, 2));
                
                if (distance <= radius)
                {
                    entitiesInRadius.Add(entity);
                }
            }
    
            return entitiesInRadius;
        }

        public Npc GetNearestNpc()
        {
            Npc nearestNpc = null;
            var minDistance = double.MaxValue;
            foreach (var npc in _currentCell.Npc.ToList())
            {
                var distance = Player.Instance.GetDistanceToEntity(npc);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNpc = npc;
                }
            }
            return nearestNpc;
        }
    }
}