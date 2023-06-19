using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RPGProject.Inventory.Effect;

namespace RPGProject.Inventory.Items.UsableItem
{
    public class UsableItem : ItemStack
    {
        private readonly string _configPath = 
            $"{Environment.CurrentDirectory}\\PropertyJson\\UsableItemCharacteristics.json";

        private struct UsableItemProperty
        {
            public ItemStackProperty ItemStackProperty { get; set; }
            public List<Effect.Effect.EffectPropertiesStruct> Effects { get; set; }
        }

        private UsableItemProperty _usableItemProperty;
        private List<Effect.Effect> _effects;

        public UsableItem(NameUsableItem nameUsableItem, int count)
        {
            try
            {
                var data = File.ReadAllText(_configPath);
                var usableItemProperties = JsonConvert.DeserializeObject<Dictionary<string, UsableItemProperty>>(data);
                if (usableItemProperties != null && usableItemProperties.ContainsKey(nameUsableItem.ToString()))
                {
                    _usableItemProperty = usableItemProperties[nameUsableItem.ToString()];
                    _itemStackProperty = _usableItemProperty.ItemStackProperty;
                    _itemStackProperty.TypeItemStack = TypeItemStack.UsableItem;
                    _itemStackProperty.StackSize = count;
                    _effects = new List<Effect.Effect>();
                    _usableItemProperty.Effects?.ForEach(effectProperty =>
                    {
                        switch (effectProperty.Type)
                        {
                            case EffectType.Momentary:
                                _effects?.Add(
                                    new MomentaryEffect(effectProperty)
                                    );
                                break;
                            case EffectType.Temporary:
                                _effects?.Add(
                                    new TemporaryEffect(effectProperty)
                                    );
                                break;
                        }
                    });
                    _itemStackProperty.Description += GenerateDescription();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        protected override void Use(LivingEntity.LivingEntity target)
        {
            foreach (var effect in _effects)
            {
                effect.ApplyEffect(target);
            }
        }
        
        private string GenerateDescription()
        {
            var description = "\n";
            foreach (var effect in _effects)
            {
                description += effect.GetDescription();
            }
            return description;
        }
    }
}