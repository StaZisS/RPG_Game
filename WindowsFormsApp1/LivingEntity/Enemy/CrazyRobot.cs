using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.EnemyStrategy;

namespace RPGProject.LivingEntity
{
    public class CrazyRobot : Enemy, IHasMapStrategy
    {
        public CrazyRobot()
        {
            InitializeEnemy(NameEnemy.CrazyRobot);
            MapStrategy = new ElectricWave();
        }

        public MapStrategy MapStrategy { get; set; }
    }
}