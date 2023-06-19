using System;
using System.Linq;
using RPGProject.SubscriptionService;

namespace RPGProject.Inventory.Effect
{
    public abstract class Effect
    {
        private readonly string _configPath =  $"{Environment.CurrentDirectory}\\PropertyJson";
        
        public struct EffectPropertiesStruct
        {
            public EffectType Type { get; set; }
            public EffectName Name { get; set; }
            public int EffectLevel { get; set; }
            public int EffectDuration { get; set; }
        }
        
        protected EffectPropertiesStruct EffectProperties;
        
        protected string GetConfigPath() => _configPath;

        protected void SetEffectProperties(EffectPropertiesStruct effectPropertiesStruct) => EffectProperties = effectPropertiesStruct;

        public EffectPropertiesStruct GetEffectProperties() => EffectProperties;

        protected void SetEffectDuration(int duration) => EffectProperties.EffectDuration = duration;

        public abstract string GetDescription();
        
        public abstract void ApplyEffect(LivingEntity.LivingEntity target);

        public static void UpdateEffects(LivingEntity.LivingEntity target)
        {
            var effects = target.GetSubscriptions(TypeSubscription.Effect);
            foreach (var eff in effects.ToList())
            {
                eff.Invoke();
            }
        }
    }
}