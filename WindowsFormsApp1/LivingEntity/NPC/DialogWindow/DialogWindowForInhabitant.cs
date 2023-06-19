using System.Drawing;
using System.Windows.Forms;
using RPGProject.Inventory.Items.QuestItem;
using WindowsFormsApp1.GameEngine;


namespace RPGProject.LivingEntity
{
    public class DialogWindowForInhabitant : DialogWindow
    {
        
        private readonly Label _dialogLabel;
        private readonly Button _acceptQuestButton;
        private readonly Button _declineQuestButton;
        
        public DialogWindowForInhabitant(string text) : base(text)
        {
            _acceptQuestButton = new Button
            {
                Text = "Accept Quest",
                Size = new Size(100, 50),
                Location = new Point(this.ClientSize.Width - 150, this.ClientSize.Height - 70), 
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            _declineQuestButton = new Button
            {
                Text = "Decline Quest",
                Size = new Size(100, 50),
                Location = new Point(50, this.ClientSize.Height - 70),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            
            _dialogLabel = new Label
            {
                Text = $"I can give you a quest. What do you want?",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.LightGray,
                AutoSize = true,
                Size = new Size(800, 800),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            
            _acceptQuestButton.Click += AcceptQuestButton_Click;
            _declineQuestButton.Click += DeclineQuestButton_Click;
        }

        protected override void DialogScene()
        {
            Controls.Add(_dialogLabel);
            Controls.Add(_acceptQuestButton);
            Controls.Add(_declineQuestButton);
        }
        
        private void AcceptQuestButton_Click(object sender, System.EventArgs e)
        {
            var questItem = new QuestItem(NameQuestItem.KillTenEnemy);
            questItem.UseItem(Player.Instance);
            ClearDialogWindow();
            CloseDialogWindow();
        }
        
        private void DeclineQuestButton_Click(object sender, System.EventArgs e)
        {
            ClearDialogWindow();
            CloseDialogWindow();
        }
        
        private void ClearDialogWindow()
        {
            _dialogLabel.Dispose();
            _acceptQuestButton.Dispose();
            _declineQuestButton.Dispose();
        }
    }
}