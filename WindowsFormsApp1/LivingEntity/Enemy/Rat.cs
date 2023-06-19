using RPGProject.LivingEntity.EnemyStrategy;
using WindowsFormsApp1.EnemyStrategy;

namespace RPGProject.LivingEntity
{
    public class Rat : Enemy, IHasMapStrategy
    {
        public Rat()
        {
            InitializeEnemy(NameEnemy.Rat);
            MapStrategy = new CallOfRats();
        }

        public MapStrategy MapStrategy { get; set; }
    }
}