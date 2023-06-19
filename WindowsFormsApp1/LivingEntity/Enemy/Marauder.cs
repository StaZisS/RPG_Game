using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.EnemyStrategy;

namespace RPGProject.LivingEntity
{
    public class Marauder : Enemy, IHasTargetStrategy
    {
        public Marauder()
        {
            InitializeEnemy(NameEnemy.Marauder);
            TargetStrategy = new QuickRobbery();
        }

        public TargetStrategy TargetStrategy { get; set; }
    }
}