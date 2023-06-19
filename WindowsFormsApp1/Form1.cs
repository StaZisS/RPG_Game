using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApp1.GameEngine;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int _hotBarRowIndexOfSelectedCell;
        private int _hotBarColumnIndexOfSelectedCell;
        private bool _isMovingLeft;
        private bool _isMovingRight;
        private bool _isMovingUp;
        private bool _isMovingDown;
        
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            UpdateStyles();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GameEngine.GameEngine.Initialize(this);
            timer1.Start();
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateProgressBar();
            if(GameEngine.GameEngine.GetIsGameOver()) return;
            Invalidate();
            if (!GameEngine.GameEngine.GetIsRunning()) return;
            GameEngine.GameEngine.Instance.SwitchIsRunning();
            GameEngine.GameEngine.Update();
            GameEngine.GameEngine.Instance.SwitchIsRunning();
            BackgroundImage = Render.DrawFrame();
        }
            
        private void UpdateProgressBar()
        {
            progressBarForHealth.Value = (int)Player.Instance.GetHealth();
            progressBarForFood.Value = (int)Player.Instance.GetFood();
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(GameEngine.GameEngine.GetIsGameOver()) return;
            
            switch (e.KeyCode)
            {
                case Keys.I:
                    GameEngine.GameEngine.Instance.SwitchInventoryOpen();
                    SetDataGridViewVisibility( GameEngine.GameEngine.Instance.GetInventoryOpen());
                    break;
                case Keys.E:
                    Player.Instance.TryInteractWithNpc();
                    Player.Stop();
                    break;
                case Keys.Escape:
                    Close();
                    break;
                case Keys.W:
                    _isMovingUp = true;
                    UpdateMovement();
                    break;
                case Keys.S:
                    _isMovingDown = true;
                    UpdateMovement();
                    break;
                case Keys.D:
                    _isMovingRight = true;
                    UpdateMovement();
                    break;
                case Keys.A:
                    _isMovingLeft = true;
                    UpdateMovement();
                    break;
            }
            
            
            if (!GameEngine.GameEngine.Instance.GetInventoryOpen())
            {
                SwitchCellInHotBar(e);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(GameEngine.GameEngine.GetIsGameOver()) return;
            switch (e.KeyCode)
            {
                case Keys.W:
                    _isMovingUp = false;
                    UpdateMovement();
                    break;
                case Keys.S:
                    _isMovingDown = false;
                    UpdateMovement();
                    break;
                case Keys.D:
                    _isMovingRight = false;
                    UpdateMovement();
                    break;
                case Keys.A:
                    _isMovingLeft = false;
                    UpdateMovement();
                    break;
            }
        }
        
        private void UpdateMovement()
        {
            if (_isMovingLeft)
            {
                if (_isMovingUp)
                    Player.MoveUpLeft();
                else if (_isMovingDown)
                    Player.MoveDownLeft();
                else
                    Player.MoveLeft();
            }
            else if (_isMovingRight)
            {
                if (_isMovingUp)
                    Player.MoveUpRight();
                else if (_isMovingDown)
                    Player.MoveDownRight();
                else
                    Player.MoveRight();
            }
            else if (_isMovingUp)
            {
                Player.MoveUp();
            }
            else if (_isMovingDown)
            {
                Player.MoveDown();
            }
            else
            {
                Player.Stop();
            }
        }
        
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if(GameEngine.GameEngine.GetIsGameOver()) return;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Player.Instance.TryAttack();
                    break;
                case MouseButtons.Right:
                    if (!GameEngine.GameEngine.Instance.GetInventoryOpen())
                    {
                        InventoryManager.UseItemInHotBar(
                            _hotBarRowIndexOfSelectedCell + 3,
                            _hotBarColumnIndexOfSelectedCell
                        );
                    }
                    break;
            }
        }

        private void SwitchCellInHotBar(KeyEventArgs e)
        {
            if(GameEngine.GameEngine.GetIsGameOver()) return;
            if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
            {
                var index = e.KeyCode - Keys.D0 - 1;
                dataGridViewHotBar.CurrentCell = dataGridViewHotBar[index, 0];
                _hotBarColumnIndexOfSelectedCell = index;
                InventoryManager.SwitchItemInHand(index);
            }
        }

        private void SetDataGridViewVisibility(bool visible)
        {
            dataGridViewInventory.ClearSelection();
            dataGridViewHotBar.ClearSelection();
            dataGridViewEquipment.ClearSelection();
            dataGridViewHotBar.Visible = !visible;
            dataGridViewInventory.Visible = visible;
            dataGridViewEquipment.Visible = visible;
            if (!GameEngine.GameEngine.Instance.GetInventoryOpen())
            {
                dataGridViewHotBar.CurrentCell = 
                    dataGridViewHotBar[
                        _hotBarColumnIndexOfSelectedCell,
                        _hotBarRowIndexOfSelectedCell
                    ];
            }
            else
            {
                InventoryManager.UpdateImageTextData();
                _hotBarRowIndexOfSelectedCell = dataGridViewHotBar.CurrentCell.RowIndex;
                _hotBarColumnIndexOfSelectedCell = dataGridViewHotBar.CurrentCell.ColumnIndex;
            }
            GameEngine.GameEngine.Instance.SwitchIsRunning();
        }

        public DataGridView GetDataGridViewInventory() => dataGridViewInventory;

        public DataGridView GetDataGridViewHotBar() => dataGridViewHotBar;

        public DataGridView GetDataGridViewEquipment() => dataGridViewEquipment;
    }
}