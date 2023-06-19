using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using RPGProject.Inventory.Enchantment;
using RPGProject.Inventory.Items.QuestItem;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.Inventory.Items.EquippableItem
{
    public class EquippableItem : ItemStack
    {
        private readonly string _configPath = 
            $"{Environment.CurrentDirectory}\\PropertyJson\\EquippableItemCharacteristics.json";

        private struct EquippableItemProperty
        {
            public ItemStackProperty ItemStackProperty { get; set; }
            public TypeEquippableItem TypeEquippableItem { get; set; }
            public NameEquippableItem NameEquippableItem { get; set; }
            public int Durability { get; set; }
        }
        
        private EquippableItemProperty _equippableItemProperty;
        private List<Enchantment.Enchantment> _enchantments;
        private const double EnchantmentChance = 0.2;

        public EquippableItem(NameEquippableItem nameEquippableItem) 
        {
            try
            {
                var data = File.ReadAllText(_configPath);
                var equippableItemProperties = JsonConvert.DeserializeObject<Dictionary<string, EquippableItemProperty>>(data);
                if (equippableItemProperties != null && equippableItemProperties.ContainsKey(nameEquippableItem.ToString()))
                {
                    _equippableItemProperty = equippableItemProperties[nameEquippableItem.ToString()];
                    _equippableItemProperty.Durability = 100;
                    _equippableItemProperty.NameEquippableItem = nameEquippableItem;
                    _itemStackProperty = _equippableItemProperty.ItemStackProperty;
                    _itemStackProperty.TypeItemStack = TypeItemStack.EquipmentItem;
                    _enchantments = new List<Enchantment.Enchantment>();

                    GenerateRandomEnchantment();

                    _itemStackProperty.Description += GenerateDescription();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        private void GenerateRandomEnchantment()
        {
            while (GameEngine.Random.NextDouble() <= EnchantmentChance)
            {
                var nameEnchantment = Enum.GetValues(typeof(NameEnchantment))
                    .Cast<NameEnchantment>()
                    .Where(enchantment =>
                    {
                        var enchantmentTypeAttribute = enchantment.GetType()
                            .GetField(enchantment.ToString())
                            .GetCustomAttributes(typeof(EnchantmentTypeAttribute), false)
                            .FirstOrDefault() as EnchantmentTypeAttribute;

                        return enchantmentTypeAttribute != null && enchantmentTypeAttribute.EquipmentTypes.Contains(_equippableItemProperty.TypeEquippableItem);
                    })
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefault();
                EnchantItem(nameEnchantment);
            }
        }

        public bool EnchantItem(NameEnchantment nameEnchantment)
        {
            var enumField = nameEnchantment.GetType().GetField(nameEnchantment.ToString());

            if (enumField?.GetCustomAttributes(typeof(EnchantmentTypeAttribute), false)
                    .FirstOrDefault() is EnchantmentTypeAttribute attribute)
            {
                TypeEquippableItem[] equipmentTypes = attribute.EquipmentTypes;
                if (equipmentTypes.Contains(_equippableItemProperty.TypeEquippableItem))
                {
                    if (_enchantments.Any(enchantment => enchantment.GetNameEnchantment() == nameEnchantment))
                    {
                        return false;
                    }
                    _enchantments.Add(new Enchantment.Enchantment(nameEnchantment));
                    return true;
                }
            }

            return false;
        }
         
        protected override void Use(LivingEntity.LivingEntity target)
        {
            Equip(target, this);
        }
        
        public string GenerateDescription()
        {
            var description = "\n";
            description += $"Durability: {_equippableItemProperty.Durability}\n";
            foreach (var enchantment in _enchantments)
            {
                description += enchantment.GetDescription() + "\n";
            }
            return description;
        }
        
        public int GetDurability() => _equippableItemProperty.Durability;

        private void Equip(LivingEntity.LivingEntity target, EquippableItem item)
        {
            foreach (var enchantment in _enchantments)
            {
                enchantment.Apply(target, item);
            }
        }
        
        public void Unequip(LivingEntity.LivingEntity target)
        {
            foreach (var enchantment in _enchantments)
            {
                enchantment.Remove(target);
            }
        }
        
        public void Repair() => _equippableItemProperty.Durability = 100;

        public TypeEquippableItem GetTypeEquippableItem() => _equippableItemProperty.TypeEquippableItem;

        public NameEquippableItem GetNameEquippableItem()
        {
            return _equippableItemProperty.NameEquippableItem;
        }
        
        public void DecriesDurability(LivingEntity.LivingEntity target)
        {
            _equippableItemProperty.Durability -= 1;
            if (_equippableItemProperty.Durability <= 0)
            {
                Unequip(target);
                SetToNull();
            }
        }
    }
}