using System;
using System.Drawing;
using RPGProject.Inventory.Effect;
using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.GameEngine;
using WindowsFormsApp1.Map;

namespace RPGProject.LivingEntity
{
    public class Enemy : LivingEntity, IGameObject
    {
        private readonly string _configPath = $"{Environment.CurrentDirectory}\\PropertyJson\\EnemyCharacteristics.json";

        private const double ChanceToExecuteStrategy = 0.01;
        
        protected struct EnemyPropertiesStruct
        {
            public NameEnemy NameEnemy { get; set; }
        }
        
        protected EnemyPropertiesStruct EnemyProperties { get; set; }
        
        protected override void OnDie() => Map.Instance.RemoveLivingEntity(this);

        public static Enemy GenerateRandomEnemy() => GetEnemyByName(GenerateRandomNameEnemy());

        private static NameEnemy GenerateRandomNameEnemy()
        {
            var values = Enum.GetValues(typeof(NameEnemy));
            return (NameEnemy) values.GetValue(GameEngine.Random.Next(values.Length));
        }

        private static Enemy GetEnemyByName(NameEnemy nameEnemy)
        {
            switch (nameEnemy)
            {
                case NameEnemy.Marauder:
                    return new Marauder();
                case NameEnemy.Rat:
                    return new Rat();
                case NameEnemy.Skeleton:
                    return new Skeleton();
                case NameEnemy.Wolf:
                    return new Wolf();
                case NameEnemy.CrazyRobot:
                    return new CrazyRobot();
                default:
                    return new Rat();
            }
        }
        
        public override void Action()
        {
            Effect.UpdateEffects(this);
            
            if (ChanceToExecuteStrategy > GameEngine.Random.NextDouble())
            {
                ExecuteStrategy();
            }
            
            if (LookForPlayer())
            {
                if (GetDistanceToEntity(Player.Instance) > GetAttackDistance())
                    ChasePlayer();
                else 
                    Attack(Player.Instance);
            }
            else
            {
                Idle();
            }
        }

        private void ExecuteStrategy()
        {
            (this as IHasMapStrategy)?.MapStrategy.ExecuteStrategy(
                GetPosition(),
                GetRandomPosition()
            );
            (this as IHasTargetStrategy)?.TargetStrategy.ExecuteStrategy(
                Player.Instance);
        }
        
        private bool LookForPlayer() => GetDistanceToEntity(Player.Instance) <= GetViewDistance();

        public void Draw(Graphics g)
        {
            g.DrawImage(
                Resources.GetFrame(EnemyProperties.NameEnemy.ToString()),
                (int)Math.Round(GetPosition().Item1),
                (int)Math.Round(GetPosition().Item2),
                100,
                100
                );
        }

        protected void InitializeEnemy(NameEnemy nameEnemy)
        {
            var enemyProperties = new EnemyPropertiesStruct
            {
                NameEnemy = nameEnemy
            };
            EnemyProperties = enemyProperties;
            LivingEntityProperties = LoadLivingEntityProperties(
                _configPath,
                nameEnemy.ToString()
            );
            LivingEntityProperties.EyeDirection = GameEngine.Random.NextDouble() * 360;
            LivingEntityProperties.Inventory = new Inventory.Inventory(this);
            GenerateRandomPosition();
        }
    }
}