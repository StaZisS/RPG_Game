using System.Drawing;
using System.Windows.Forms;
using RPGProject.Inventory.Items;
using RPGProject.Inventory.Items.NullItem;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.LivingEntity
{
    public class DialogWindowForDealer : DialogWindow
    {
        private readonly ComboBox _itemComboBox;
        private readonly Label _dialogLabel;
        private readonly Label _itemCostLabel;
        private readonly Button _buttonBuyItem;
        private readonly Button _buttonLeave;

        public DialogWindowForDealer(string text) : base(text)
        {
            _itemComboBox = new ComboBox
            {
                Location = new Point(10, 50),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _dialogLabel = new Label
            {
                Text = $"You have so much money {Player.Instance.GetMoney()}, choose the item you want to buy",
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

            _buttonBuyItem = new Button
            {
                Text = "Buy Item",
                Size = new Size(100, 50),
                Location = new Point(this.ClientSize.Width - 150, this.ClientSize.Height - 70),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            _buttonLeave = new Button
            {
                Text = "Leave Store",
                Size = new Size(100, 50),
                Location = new Point(50, this.ClientSize.Height - 70),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            
            _itemComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            _itemComboBox.DrawItem += ItemComboBox_DrawItem;
            _itemComboBox.SelectedIndexChanged += ItemComboBox_SelectedIndexChanged;
            _buttonBuyItem.Click += ButtonBuyItem_Click;
            _buttonLeave.Click += ButtonLeave_Click;
        }

        protected override void DialogScene()
        {
            UpdateComboBox();
            Controls.Add(_buttonBuyItem);
            Controls.Add(_buttonLeave);
            Controls.Add(_itemComboBox);
            Controls.Add(_itemCostLabel);
            Controls.Add(_dialogLabel);
        }

        private void UpdateComboBox()
        {
            _dialogLabel.Text = $"You have so much money {Player.Instance.GetMoney()}, choose the item you want repair or enhance";
            _itemComboBox.Items.Clear();
            foreach (var item in Npc.GetInventory().GetItems())
            {
                if (!(item is NullItem) && item.GetTypeItemStack() != TypeItemStack.Null)
                {
                    _itemComboBox.Items.Add(item);
                }
            }
            _itemComboBox.SelectedItem = _itemComboBox.Items[0];
        }
        
        private void ButtonBuyItem_Click(object sender, System.EventArgs e)
        {
            if (!(_itemComboBox.SelectedItem is ItemStack itemStack)) return;
            if (Player.Instance.SubtractMoney(itemStack.GetCount() * itemStack.GetPrice()))
            {
                var item = (ItemStack)itemStack.Clone();
                Player.Instance.GetInventory().AddItem(item);
                itemStack.SetToNull();
                UpdateComboBox();
            }
            else
            {
                MessageBox.Show("You don't have enough money");
            }
        }
        
        private void ItemComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_itemComboBox.SelectedItem is ItemStack itemStack)
            {
                _itemCostLabel.Text = $"Cost: {itemStack.GetPrice() * itemStack.GetCount()}";
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

                var text = itemStack != null ? $"{itemStack.GetName()} Count:{itemStack.GetCount()}" : string.Empty;
                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, _itemComboBox.Font, brush, e.Bounds.Left + e.Bounds.Height, e.Bounds.Top);
                }
            }

            e.DrawFocusRectangle();
        }
        
        private void ButtonLeave_Click(object sender, System.EventArgs e)
        {
            ClearDialogWindow();
            CloseDialogWindow();
        }
        
        private void ClearDialogWindow()
        {
            _buttonLeave.Dispose();
            _buttonBuyItem.Dispose();
            _itemComboBox.Dispose();
            _dialogLabel.Dispose();
            _itemCostLabel.Dispose();
        }
    }
}