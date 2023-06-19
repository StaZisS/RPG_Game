using System.Drawing;
using System.Windows.Forms;
using RPGProject.Inventory.Enchantment;
using RPGProject.Inventory.Items;
using RPGProject.Inventory.Items.EquippableItem;
using RPGProject.Inventory.Items.NullItem;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.LivingEntity
{
    public class DialogWindowForBlacksmith : DialogWindow
    {
        private readonly ComboBox _itemComboBox;
        private readonly Label _dialogLabel;
        private readonly Label _itemCostLabel;
        private readonly Button _buttonRepairItem;
        private readonly Button _buttonEnhanceItem;
        private readonly Button _buttonLeave;
        
        public DialogWindowForBlacksmith(string text) : base(text)
        {
            _buttonLeave = new Button
            {
                Text = "Leave Forge",
                Size = new Size(100, 50),
                Location = new Point(50, this.ClientSize.Height - 70),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            
            _buttonRepairItem = new Button
            {
                Text = "Repair Item",
                Size = new Size(100, 50),
                Location = new Point(this.ClientSize.Width - 150, this.ClientSize.Height - 70),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            
            _buttonEnhanceItem = new Button
            {
                Text = "Enhance Item",
                Size = new Size(100, 50),
                Location = new Point(this.ClientSize.Width - 150, this.ClientSize.Height - 140),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            
            _dialogLabel = new Label
            {
                Text = $"You have so much money {Player.Instance.GetMoney()}, choose the item you want repair or enhance",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.LightGray,
                AutoSize = true,
                Size = new Size(800, 800),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            
            _itemCostLabel = new Label
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.LightGray,
                AutoSize = true,
                Size = new Size(800, 800),
                Location = new Point(50, this.ClientSize.Height - 170),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            
            _itemComboBox = new ComboBox
            {
                Location = new Point(10, 50),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            _itemComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            _itemComboBox.DrawItem += ItemComboBox_DrawItem;
            _buttonLeave.Click += ButtonLeave_Click;
            _buttonRepairItem.Click += ButtonRepairItem_Click;
            _buttonEnhanceItem.Click += ButtonEnhanceItem_Click;
            _itemComboBox.SelectedIndexChanged += ItemComboBox_SelectedIndexChanged;
        }

        protected override void DialogScene()
        {
            UpdateComboBox();
            Controls.Add(_buttonRepairItem);
            Controls.Add(_buttonEnhanceItem);
            Controls.Add(_buttonLeave);
            Controls.Add(_itemComboBox);
            Controls.Add(_itemCostLabel);
            Controls.Add(_dialogLabel);
        }
        
        private void UpdateComboBox()
        {
            _dialogLabel.Text = $"You have so much money {Player.Instance.GetMoney()}, choose the item you want repair or enhance";
            _itemComboBox.Items.Clear();
            foreach (var item in Player.Instance.GetInventory().GetItems())
            {
                if (item is EquippableItem && item.GetTypeItemStack() != TypeItemStack.Null)
                {
                    _itemComboBox.Items.Add(item);
                }
            }
        }

        private void ButtonRepairItem_Click(object sender, System.EventArgs e)
        {
            if (!(_itemComboBox.SelectedItem is ItemStack itemStack)) return;
            if (Player.Instance.SubtractMoney(itemStack.GetCount() * itemStack.GetPrice()))
            {
                (itemStack as EquippableItem).Repair();
                UpdateComboBox();
            }
            else
            {
                MessageBox.Show("You don't have enough money");
            }
        }
        
        private void ButtonEnhanceItem_Click(object sender, System.EventArgs e)
        {
            if (!(_itemComboBox.SelectedItem is ItemStack itemStack)) return;
            if (Player.Instance.SubtractMoney(itemStack.GetCount() * itemStack.GetPrice()))
            {
                if (itemStack is EquippableItem equippableItem)
                {
                    equippableItem.EnchantItem(
                        Enchantment.GetAppropriateNameForItem(equippableItem.GetTypeEquippableItem())
                    );
                    UpdateComboBox();
                }
            }
            else
            {
                MessageBox.Show("You don't have enough money");
            }
        }
        
        private void ButtonLeave_Click(object sender, System.EventArgs e)
        {
            ClearDialogWindow();
            CloseDialogWindow();
        }
        
        private void ClearDialogWindow()
        {
            _buttonLeave.Dispose();
            _buttonEnhanceItem.Dispose();
            _buttonRepairItem.Dispose();
            _itemComboBox.Dispose();
            _dialogLabel.Dispose();
            _itemCostLabel.Dispose();
        }
        
        private void ItemComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_itemComboBox.SelectedItem is ItemStack itemStack)
            {
                _itemCostLabel.Text = $"Cost: {itemStack.GetPrice()}";
            }
        }
        
        private void ItemComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                var itemStack = _itemComboBox.Items[e.Index] as ItemStack;
                
                if (!(itemStack is NullItem))
                {
                    e.Graphics.DrawImage(
                        Resources.GetFrame(itemStack.GetName()),
                        e.Bounds.Left,
                        e.Bounds.Top,
                        e.Bounds.Height,
                        e.Bounds.Height
                    );
                }

                var text = itemStack != null ? $"{itemStack.GetName()}, Durability:{(itemStack as EquippableItem).GetDurability()}" : string.Empty;
                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, _itemComboBox.Font, brush, e.Bounds.Left + e.Bounds.Height, e.Bounds.Top);
                }
            }

            e.DrawFocusRectangle();
        }
    }
}