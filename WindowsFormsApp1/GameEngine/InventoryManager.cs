using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RPGProject.Inventory.Items;

namespace WindowsFormsApp1.GameEngine
{
    public class InventoryManager
    {
        private static Form1 _form1;
        private Rectangle _dragBoxFromMouseDown;
        private object _valueFromMouseDown;
        
        public InventoryManager(Form1 form1)
        {
            _form1 = form1;
            InitializeViewElement();
        }
        
        private void InitializeViewElement()
        {
            InitializeDataGridView(
                _form1.GetDataGridViewInventory(),
                9,
                4,
                900,
                400,
                _form1.ClientSize.Width / 2 - 900 / 2,
                _form1.ClientSize.Height / 2 - 400 / 2,
                false,
                "Inventory"
            );
            
            InitializeDataGridView(
                _form1.GetDataGridViewEquipment(),
                1,
                4,
                100,
                400,
                _form1.ClientSize.Width - 100,
                _form1.ClientSize.Height / 2 - 400 / 2,
                false,
                "Equipment"
            );

            InitializeDataGridView(
                _form1.GetDataGridViewHotBar(),
                9,
                1,
                900,
                100,
                _form1.ClientSize.Width / 2 - 900 / 2,
                _form1.ClientSize.Height - 100,
                true,
                "HotBar"
            );
            
            UpdateImageTextData();
            
            _form1.GetDataGridViewInventory().CellValueChanged += dataGridViewInventory_CellValueChanged;
            _form1.GetDataGridViewHotBar().CellValueChanged += dataGridViewHotBar_CellValueChanged;
        }
        
        private static ImageTextData CreateImageTextData(ItemStack itemStack, TypeCell typeCell)
        {
            return new ImageTextData
            {
                Image = Resources.GetFrame(itemStack.GetName()),
                Text = itemStack.GetCount() == 0 ? "" : itemStack.GetCount().ToString(),
                TypeCell = typeCell,
                Description = itemStack.GetDescription()
            };
        }

        public static void UpdateImageTextData()
        {
            var itemsStack = Player.Instance.GetInventory().GetItems();
            var armorStack = Player.Instance.GetInventory().GetArmor();
            
            for (var i = 0; i < _form1.GetDataGridViewInventory().ColumnCount; i++)
            {
                for (var j = 0; j < _form1.GetDataGridViewInventory().RowCount; j++)
                {
                    var currentItemStack = itemsStack[i + j * _form1.GetDataGridViewInventory().ColumnCount];
                    _form1.GetDataGridViewInventory()[i, j].Value = CreateImageTextData(currentItemStack, TypeCell.Bag);
                }
            }
            
            for (var i = 0; i < _form1.GetDataGridViewHotBar().ColumnCount; i++)
            {
                var currentItemStack = itemsStack[i + 3 * 9];
                _form1.GetDataGridViewHotBar()[i, 0].Value = CreateImageTextData(currentItemStack, TypeCell.Bag);
            }
            
            for (var i = 0; i < _form1.GetDataGridViewEquipment().RowCount; i++)
            {
                var currentItemStack = armorStack[i];
                _form1.GetDataGridViewEquipment()[0, i].Value = CreateImageTextData(currentItemStack, TypeCell.Armor);
            }
        }
        
        private void InitializeDataGridView(
            DataGridView dataGridView,
            int columCount,
            int rowCount,
            int width,
            int height,
            int x,
            int y,
            bool visible,
            string name
        )
        {
            dataGridView.Visible = visible;
            dataGridView.Anchor = AnchorStyles.None;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Size = new Size(width, height);
            dataGridView.Location = new Point(x, y);
            dataGridView.Name = name;
            dataGridView.TabIndex = 1;
            dataGridView.ColumnCount = columCount;
            dataGridView.RowCount = rowCount;
            dataGridView.RowHeadersVisible = false;
            dataGridView.ColumnHeadersVisible = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.ScrollBars = ScrollBars.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView.AllowDrop = true;

            dataGridView.CellStateChanged += (sender, args) =>
            {
                if (args.StateChanged == DataGridViewElementStates.Selected)
                {
                    dataGridView_CellStateChanged(sender, args);
                }
            };
            
            dataGridView.MouseDown += dataGridView_MouseDown;
            dataGridView.MouseMove += dataGridView_MouseMove;
            dataGridView.DragDrop += dataGridView_DragDrop;
            dataGridView.DragOver += dataGridView_DragOver;
            dataGridView.DragEnter += dataGridView_DragEnter;

            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].Width = dataGridView.Width / dataGridView.ColumnCount;
            }

            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                dataGridView.Rows[i].Height = dataGridView.Height / dataGridView.RowCount;
            }

            for (int i = 0; i < dataGridView.ColumnCount * dataGridView.RowCount; i++)
            {
                dataGridView[i % dataGridView.ColumnCount, i / dataGridView.ColumnCount] = new ImageTextCellTemplate();
            }
        }

        private void dataGridView_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell == null || e.Cell.State.HasFlag(DataGridViewElementStates.Selected))
            {
                e.Cell.Style.SelectionBackColor = Color.Red;
                e.Cell.Style.SelectionForeColor = Color.White;
            }
            else
            {
                e.Cell.Style.SelectionBackColor = e.Cell.Style.BackColor;
                e.Cell.Style.SelectionForeColor = e.Cell.Style.ForeColor;
            }
        }

        private void dataGridView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dataGridViewInventory_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < _form1.GetDataGridViewInventory().ColumnCount && e.RowIndex == 3)
            {
                int columnIndex = e.ColumnIndex;
                _form1.GetDataGridViewHotBar()[columnIndex, 0].Value =
                    _form1.GetDataGridViewInventory()[columnIndex, e.RowIndex].Value;
            }
        }

        private void dataGridViewHotBar_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < _form1.GetDataGridViewHotBar().ColumnCount && e.RowIndex == 0)
            {
                int columnIndex = e.ColumnIndex;
                _form1.GetDataGridViewInventory()[columnIndex, 3].Value =
                    _form1.GetDataGridViewHotBar()[columnIndex, e.RowIndex].Value;
            }
        }
        
        private void dataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is DataGridView dataGridView)
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        DragDropEffects dropEffect = dataGridView.DoDragDrop(_valueFromMouseDown, DragDropEffects.Copy);
                    }
                }
            }
        }

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is DataGridView dataGridView)
            {
                var hitTestInfo = dataGridView.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex != -1 && hitTestInfo.ColumnIndex != -1)
                {
                    _valueFromMouseDown = dataGridView.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex].Value;
                    if (_valueFromMouseDown != null)
                    {
                        Size dragSize = SystemInformation.DragSize;
                        _dragBoxFromMouseDown =
                            new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
                    }
                }
                else
                    _dragBoxFromMouseDown = Rectangle.Empty;
            }
        }

        private void dataGridView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (sender is DataGridView dataGridView)
            {
                Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));
                if (e.Effect == DragDropEffects.Copy)
                {
                    ImageTextData cellValue = (ImageTextData)e.Data.GetData(typeof(ImageTextData));
                        var hitTest = dataGridView.HitTest(clientPoint.X, clientPoint.Y);
                    if (hitTest.ColumnIndex != -1 && hitTest.RowIndex != -1)
                    {
                        var currentValue = dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Value;
                        if (((ImageTextData)dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Value).TypeCell ==
                            cellValue.TypeCell)
                        {
                            if (cellValue.TypeCell == TypeCell.Armor)
                            {
                                Player.Instance.GetInventory().SwapItemStacks(
                                    _form1.GetDataGridViewEquipment().SelectedCells[0].RowIndex,
                                    hitTest.RowIndex,
                                    cellValue.TypeCell,
                                    cellValue.TypeCell
                                );
                            }
                            else
                            {
                                Player.Instance.GetInventory().SwapItemStacks(
                                    _form1.GetDataGridViewInventory().SelectedCells[0].ColumnIndex +
                                    _form1.GetDataGridViewInventory().SelectedCells[0].RowIndex * 9,
                                    hitTest.ColumnIndex + hitTest.RowIndex * 9,
                                    cellValue.TypeCell,
                                    cellValue.TypeCell
                                );
                            }

                            dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Value = cellValue;
                            dataGridView.SelectedCells[0].Value = currentValue;
                            
                            dataGridView.SelectedCells[0].Selected = false;
                            dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Selected = true;
                        }
                        //cellValue клетка от куда мы пришли, currentValue клетка куда мы пришли
                        else if (cellValue.TypeCell == TypeCell.Armor)
                        {
                            int rowIndex = _form1.GetDataGridViewEquipment().SelectedCells[0].RowIndex;
                            int columnIndex = _form1.GetDataGridViewEquipment().SelectedCells[0].ColumnIndex;
                            
                            
                            var flag = Player.Instance.GetInventory().SwapItemStacks(
                                rowIndex,
                                hitTest.ColumnIndex + hitTest.RowIndex * 9,
                                TypeCell.Armor,
                                TypeCell.Bag
                            );
                            if (!flag)
                            {
                                MessageBox.Show("Невозможно перенести предмет");
                                return;
                            }

                            dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Value = cellValue;
                            _form1.GetDataGridViewEquipment().SelectedCells[0].Value = currentValue;
                            
                            ((ImageTextData)dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Value).TypeCell = TypeCell.Bag;
                            ((ImageTextData)_form1.GetDataGridViewEquipment().SelectedCells[0].Value).TypeCell = TypeCell.Armor;
                            
                            UpdateImageTextData();
                            
                            _form1.GetDataGridViewEquipment().SelectedCells[0].Selected = false;
                            dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Selected = true;
                        } else
                        {
                            //перетаскиваем из инвентаря в броню
                            int rowIndex = _form1.GetDataGridViewInventory().SelectedCells[0].RowIndex;
                            int columnIndex = _form1.GetDataGridViewInventory().SelectedCells[0].ColumnIndex;
                            
                            var flag = Player.Instance.GetInventory().SwapItemStacks(
                                rowIndex * 9 + columnIndex,
                                hitTest.RowIndex,
                                TypeCell.Bag,
                                TypeCell.Armor
                            );
                            if (!flag)
                            {
                                MessageBox.Show("Невозможно перенести предмет");
                                return;
                            }
                            dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Value = cellValue;
                            _form1.GetDataGridViewInventory().SelectedCells[0].Value = currentValue;
                            
                            ((ImageTextData)dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Value).TypeCell = TypeCell.Armor;
                            ((ImageTextData)_form1.GetDataGridViewInventory().SelectedCells[0].Value).TypeCell = TypeCell.Bag;
                            
                            UpdateImageTextData();
                            
                            _form1.GetDataGridViewInventory().SelectedCells[0].Selected = false;
                            dataGridView[hitTest.ColumnIndex, hitTest.RowIndex].Selected = true;
                        }
                    }
                }
            }
            
        }

        public static void UseItemInHotBar(int row, int column)
        {
            Player.Instance.GetInventory()
                .GetItems()[9 * row + column]
                .UseItem(Player.Instance);
            _form1.GetDataGridViewHotBar()[column, 0].Value =
                CreateImageTextData(
                    Player.Instance.GetInventory().GetItems()[9 * row + column],
                    TypeCell.Bag
                    );
            _form1.GetDataGridViewInventory()[column, 3].Value =
                CreateImageTextData(
                    Player.Instance.GetInventory().GetItems()[9 * row + column],
                    TypeCell.Bag
                );
        }

        public static void SwitchItemInHand(int column)
        {
            Player.Instance.SetItemStackInHand(
                Player.Instance.GetInventory().GetItems()[9 * 3 + column]
                );
        }
    }
}  