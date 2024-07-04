namespace LocalWebChat
{
	public partial class Form1 : Form
	{
		bool IsItWorking = false;
		public Form1()
		{
			InitializeComponent();
			MessageBox.KeyPress += MessageBox_KeyPress;
		}
		public void AddMessage(string message)
		{
			ChatBox.Items.Add(message);
		}
		private void MessageBox_KeyPress(object? sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\r' || MessageBox.Text.Length == 0) return;
			string info = MessageBox.Text;
			//throw new NotImplementedException(info);
			//Call Send(info)
		}

		private void ControlButton_Click(object sender, EventArgs e)
		{
			IsItWorking = !IsItWorking;
			if (IsItWorking)
			{
				ControlButton.Text = "Disconnect/Close Server";
				//Call Stop()
			}
			else
			{
				ControlButton.Text = "Connect/Create Server";
				if(IPInput.Text.Length == 0)
				{

				}
				else
				{

				}
				//Call Start()
			}
		}
	}
}
