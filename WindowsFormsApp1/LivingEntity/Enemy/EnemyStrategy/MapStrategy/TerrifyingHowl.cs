using RPGProject.LivingEntity.EnemyStrategy;

namespace WindowsFormsApp1.EnemyStrategy
{
    public class TerrifyingHowl : MapStrategy
    {
        private const int Damage = 20;
        
        public TerrifyingHowl()
        {
            Speed = 40;
            DamageRadius = 400;
            NameStrategy = NameStrategy.TerrifyingHowl;
        }

        protected override void Action()
        {
            var listLivingEntity = Map.Map.Instance.GetLivingEntitiesInRadius(CurrentPositionOnMap, DamageRadius);
            
            foreach (var livingEntity in listLivingEntity)
            {
                livingEntity.TakeDamage(Damage, 0);
            }
        }
    }
}