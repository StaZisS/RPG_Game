using RPGProject.LivingEntity;

namespace WindowsFormsApp1.EnemyStrategy
{
    public interface ITargetStrategy
    {
        void ExecuteStrategy(LivingEntity target);
    }
}