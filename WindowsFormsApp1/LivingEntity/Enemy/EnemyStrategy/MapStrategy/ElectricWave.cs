using System.Drawing;
using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.GameEngine;

namespace WindowsFormsApp1.EnemyStrategy
{
    public class ElectricWave : MapStrategy
    {
        private const int Damage = 40;
        
        public ElectricWave()
        {
            Speed = 30;
            DamageRadius = 600;
            NameStrategy = NameStrategy.ElectricWave;
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