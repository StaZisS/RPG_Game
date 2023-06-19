using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RPGProject.Inventory.Effect
{
    public class MomentaryEffect : Effect
    {
        private struct PropertiesMomentaryEffect
        {
            public int FoodRegeneration { get; set; }
            public int HealthRegeneration { get; set; }
        }
        private readonly PropertiesMomentaryEffect _propertiesMomentaryEffect;
        public MomentaryEffect(EffectPropertiesStruct propertiesStruct)
        {
            try
            {
                var data = File.ReadAllText($"{GetConfigPath()}\\MomentaryEffectProperty.json");
                _propertiesMomentaryEffect = JsonConvert.DeserializeObject<
                    Dictionary<string, PropertiesMomentaryEffect
                    >
                >(data)[propertiesStruct.Name.ToString()];
                _propertiesMomentaryEffect.FoodRegeneration *= propertiesStruct.EffectLevel;
                _propertiesMomentaryEffect.HealthRegeneration *= propertiesStruct.EffectLevel;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public override void ApplyEffect(LivingEntity.LivingEntity target)
        {
            target.SetHealth(target.GetHealth() + _propertiesMomentaryEffect.HealthRegeneration);
            target.SetFood(target.GetFood() + _propertiesMomentaryEffect.FoodRegeneration);
        }
        
        public override string GetDescription()
        {
            return $"Effect name: {EffectProperties.Name}\n" +
                   "Momentary effect\n" +
                   $"Effect level: {EffectProperties.EffectLevel}\n" +
                   $"{(_propertiesMomentaryEffect.HealthRegeneration == 0 ? "" : $"Health regeneration: {_propertiesMomentaryEffect.HealthRegeneration}\n")}" +
                   $"{(_propertiesMomentaryEffect.FoodRegeneration == 0 ? "" : $"Food regeneration: {_propertiesMomentaryEffect.FoodRegeneration}\n")}";
        }
    }
}