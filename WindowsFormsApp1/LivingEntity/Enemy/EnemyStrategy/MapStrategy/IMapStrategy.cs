using RPGProject.LivingEntity;

namespace WindowsFormsApp1.EnemyStrategy
{
    public interface IMapStrategy
    {
        void ExecuteStrategy((double, double) startPoint, (double, double) endPoint);
    }
}