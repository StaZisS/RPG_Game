using RPGProject.LivingEntity;
using RPGProject.LivingEntity.EnemyStrategy;

namespace WindowsFormsApp1.EnemyStrategy
{
    public class CallOfRats : MapStrategy
    {
        private const int CountOfRats = 5;
        
        public CallOfRats()
        {
            Speed = 20;
            DamageRadius = 300;
            NameStrategy = NameStrategy.CallOfRats;
        }
        
        protected override void Action()
        {
            for (var i = 0; i < CountOfRats; i++)
            {
                var enemy = new Rat();
                enemy.SetPosition(
                    CurrentPositionOnMap.Item1,
                    CurrentPositionOnMap.Item2
                    );
                Map.Map.Instance.AddEnemy(enemy);
            }
        }
    }
}