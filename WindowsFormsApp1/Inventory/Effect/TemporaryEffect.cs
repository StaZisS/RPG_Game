using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RPGProject.SubscriptionService;

namespace RPGProject.Inventory.Effect
{
    public class TemporaryEffect : Effect
    {
        private struct PropertiesTemporaryEffect
        {
            public int Duration { get; set; }
            public double FoodRegeneration { get; set; }
            public double HealthRegeneration { get; set; }
            public double Speed { get; set; }
            public double Power { get; set; }
        }
        
        private DateTime _activationTime;
        private Guid _id;
        private bool _isActivated;
        private readonly PropertiesTemporaryEffect _propertiesTemporaryEffect;
        
        public TemporaryEffect(EffectPropertiesStruct propertiesStruct)
        {
            try
            {
                var data = File.ReadAllText($"{GetConfigPath()}\\TemporaryEffectProperty.json");
                _propertiesTemporaryEffect = JsonConvert.DeserializeObject<
                    Dictionary<string, PropertiesTemporaryEffect
                    >
                >(data)[propertiesStruct.Name.ToString()];
                _propertiesTemporaryEffect.Duration = propertiesStruct.EffectDuration;
                _propertiesTemporaryEffect.FoodRegeneration *= propertiesStruct.EffectLevel;
                _propertiesTemporaryEffect.HealthRegeneration *= propertiesStruct.EffectLevel;
                _propertiesTemporaryEffect.Speed *= propertiesStruct.EffectLevel;
                _propertiesTemporaryEffect.Power *= propertiesStruct.EffectLevel;
                SetEffectProperties(propertiesStruct);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public override void ApplyEffect(LivingEntity.LivingEntity target)
        {
            _activationTime = DateTime.Now;
            _id = target.AddSubscription(TypeSubscription.Effect, () =>
            {
                ExecuteEffect(target);
            });
        }
        
        private void ExecuteEffect(LivingEntity.LivingEntity target)
        {
            if (DateTime.Now - _activationTime > TimeSpan.FromSeconds(_propertiesTemporaryEffect.Duration))
            {
                RemoveEffect(target);
            }
            else
            {
                SetEffectDuration((int)(DateTime.Now - _activationTime).TotalSeconds);
                target.SetHealth(target.GetHealth() + _propertiesTemporaryEffect.HealthRegeneration);
                target.SetFood(target.GetFood() + _propertiesTemporaryEffect.FoodRegeneration);
                if (_isActivated) return;
                target.SetPower(target.GetPower() + _propertiesTemporaryEffect.Power);
                target.SetSpeed(target.GetSpeed() + _propertiesTemporaryEffect.Speed);
                _isActivated = true;
            }
        }
        private void RemoveEffect(LivingEntity.LivingEntity target)
        {
            target.RemoveSubscription(TypeSubscription.Effect, _id);
            if(!_isActivated) return;
            target.SetSpeed(target.GetSpeed() - _propertiesTemporaryEffect.Speed);
            target.SetPower(target.GetPower() - _propertiesTemporaryEffect.Power);
            _isActivated = false;
        }
        
        public override string GetDescription()
        {
            return $"Effect name: {EffectProperties.Name}\n" +
                   "Temporary effect\n" +
                   $"Effect level: {EffectProperties.EffectLevel}\n" +
                   $"Duration: {_propertiesTemporaryEffect.Duration}\n" +
                   $"{(_propertiesTemporaryEffect.HealthRegeneration == 0 ? "" : $"Health regeneration: {_propertiesTemporaryEffect.HealthRegeneration}\n")}" +
                   $"{(_propertiesTemporaryEffect.FoodRegeneration == 0 ? "" : $"Food regeneration: {_propertiesTemporaryEffect.FoodRegeneration}\n")}" +
                   $"{(_propertiesTemporaryEffect.Speed == 0 ? "" : $"Speed: {_propertiesTemporaryEffect.Speed}\n")}" +
                   $"{(_propertiesTemporaryEffect.Power == 0 ? "" : $"Power: {_propertiesTemporaryEffect.Power}\n")}";
        }
    }
}