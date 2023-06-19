using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using RPGProject.LivingEntity;

namespace WindowsFormsApp1.GameEngine
{
    public abstract class DialogWindow : Form
    {
        private readonly Button _nextButton;
        private readonly Label _greetingSpeechLabel;
        private readonly Label _closingSpeechLabel;
        public Npc Npc { get; set; }
        
        protected DialogWindow(string text)
        {
            Text = text;
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(800, 800);
            FormClosed += DialogWindow_FormClosed;
            _greetingSpeechLabel = new Label
            {
                Text = $"Hello, I'm a {text}. What do you want?",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.LightGray,
                AutoSize = true,
                Size = new Size(800, 800),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            
            _closingSpeechLabel = new Label
            {
                Text = "Goodbye",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.LightGray,
                AutoSize = true,
                Size = new Size(800, 800),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            
            _nextButton = new Button
            {
                Text = "Next",
                Location = new Point(350, 700),
                Size = new Size(100, 50)
            };
            
            Controls.Add(_greetingSpeechLabel);
            Controls.Add(_nextButton);
            _nextButton.Click += NextButton_Click;
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        private void DialogWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        
        private void NextButton_Click(object sender, System.EventArgs e)
        {
            _nextButton.Enabled = false;
            SwitchToDialogScene();
        }
        
        private void SwitchToDialogScene()
        {
            _greetingSpeechLabel.Dispose();
            _nextButton.Dispose();
            DialogScene();
        }
        
        protected abstract void DialogScene();
        
        protected void CloseDialogWindow()
        {
            Controls.Add(_closingSpeechLabel);
            Task.Delay(2000).Wait();
            Close();
        }

    }
}