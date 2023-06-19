using RPGProject.LivingEntity;
using RPGProject.LivingEntity.EnemyStrategy;

namespace WindowsFormsApp1.EnemyStrategy
{
    public class QuickRobbery : TargetStrategy
    {
        public QuickRobbery()
        {
            AttackDistance = 300;
            NameStrategy = NameStrategy.QuickRobbery;
        }

        protected override void Action()
        {
            Target.GetInventory().DeleteRandomNonNullItem();
        }
    }
}