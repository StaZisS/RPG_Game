namespace RPGProject.Inventory.Items.NullItem
{
    public class NullItem : ItemStack
    {
        public NullItem()
        {
            _itemStackProperty.Name = null;
            _itemStackProperty.Price = 0;
            _itemStackProperty.StackSize = 0;
            _itemStackProperty.MaxStackSize = 0;
            _itemStackProperty.TypeItemStack = TypeItemStack.Null;
        }
        protected override void Use(LivingEntity.LivingEntity target)
        {
        
        }
    }
}
