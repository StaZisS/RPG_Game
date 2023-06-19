using System;
using RPGProject.Inventory.Items.EquippableItem;

namespace RPGProject.Inventory.Enchantment
{
    
    public class EnchantmentTypeAttribute : Attribute
    {
        public TypeEquippableItem[] EquipmentTypes { get; set; }

        public EnchantmentTypeAttribute(params TypeEquippableItem[] equipmentTypes)
        {
            EquipmentTypes = equipmentTypes;
        }
    }

    public enum NameEnchantment
    {
        [EnchantmentType(TypeEquippableItem.Armor)]
        Protection,
    
        [EnchantmentType(TypeEquippableItem.Weapon)]
        Sharpness,

        [EnchantmentType(TypeEquippableItem.Weapon, TypeEquippableItem.Armor)]
        InfiniteDurability,
    }

}