using WindowsFormsApp1.EnemyStrategy;

namespace RPGProject.LivingEntity.EnemyStrategy
{
    public interface IHasTargetStrategy
    {
        TargetStrategy TargetStrategy { get; set; }
    }
}