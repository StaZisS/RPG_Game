using RPGProject.Inventory.Effect;
using RPGProject.LivingEntity;
using RPGProject.LivingEntity.EnemyStrategy;

namespace WindowsFormsApp1.EnemyStrategy
{
    public class FireBones : TargetStrategy
    {
        public FireBones()
        {
            AttackDistance = 300;
            NameStrategy = NameStrategy.FireBones;
        }

        protected override void Action()
        {
            var effect = new TemporaryEffect(
                new Effect.EffectPropertiesStruct
                {
                    Name = EffectName.Poisoning,
                    Type = EffectType.Temporary,
                    EffectDuration = 5,
                    EffectLevel = 3
                }
            );
            effect.ApplyEffect(Target);
        }
    }
}