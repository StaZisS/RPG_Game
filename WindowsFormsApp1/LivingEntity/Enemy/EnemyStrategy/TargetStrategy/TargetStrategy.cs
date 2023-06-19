using RPGProject.LivingEntity;
using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.GameEngine;

namespace WindowsFormsApp1.EnemyStrategy
{
    public abstract class TargetStrategy : ITargetStrategy
    {
        protected LivingEntity Target;
        protected double AttackDistance;
        protected NameStrategy NameStrategy;
        
        protected abstract void Action();

        public void ExecuteStrategy(LivingEntity target)
        {
            Target = target;
            Action();
        }
    }
}