using WindowsFormsApp1.EnemyStrategy;

namespace RPGProject.LivingEntity.EnemyStrategy
{
    public interface IHasMapStrategy
    {
        MapStrategy MapStrategy { get; set; }
    }
}