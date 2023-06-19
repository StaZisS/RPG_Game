using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.EnemyStrategy;

namespace RPGProject.LivingEntity
{
    public class Skeleton : Enemy, IHasTargetStrategy
    {
        public Skeleton()
        {
            InitializeEnemy(NameEnemy.Skeleton);
            TargetStrategy = new FireBones();
        }

        public TargetStrategy TargetStrategy { get; set; }
    }
}