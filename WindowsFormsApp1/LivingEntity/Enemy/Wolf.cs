using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.EnemyStrategy;

namespace RPGProject.LivingEntity
{
    public class Wolf : Enemy, IHasMapStrategy
    {
        public Wolf()
        {
            InitializeEnemy(NameEnemy.Wolf);
            MapStrategy = new TerrifyingHowl();
        }

        public MapStrategy MapStrategy { get; set; }
    }
}