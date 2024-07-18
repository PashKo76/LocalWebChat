namespace LocalWebChat
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			ChatBox = new ListBox();
			MessageBox = new TextBox();
			IPInput = new TextBox();
			ControlButton = new Button();
			IPTip = new Label();
			SuspendLayout();
			// 
			// ChatBox
			// 
			ChatBox.FormattingEnabled = true;
			ChatBox.Location = new Point(12, 12);
			ChatBox.Name = "ChatBox";
			ChatBox.Size = new Size(353, 384);
			ChatBox.TabIndex = 0;
			// 
			// MessageBox
			// 
			MessageBox.Location = new Point(12, 411);
			MessageBox.Name = "MessageBox";
			MessageBox.Size = new Size(353, 27);
			MessageBox.TabIndex = 1;
			// 
			// IPInput
			// 
			IPInput.Location = new Point(368, 55);
			IPInput.Name = "IPInput";
			IPInput.Size = new Size(150, 27);
			IPInput.TabIndex = 2;
			// 
			// ControlButton
			// 
			ControlButton.Location = new Point(368, 88);
			ControlButton.Name = "ControlButton";
			ControlButton.Size = new Size(150, 80);
			ControlButton.TabIndex = 3;
			ControlButton.Text = "Connect/Create Server";
			ControlButton.UseVisualStyleBackColor = true;
			ControlButton.Click += ControlButton_Click;
			// 
			// IPTip
			// 
			IPTip.AutoSize = true;
			IPTip.Location = new Point(371, 12);
			IPTip.Name = "IPTip";
			IPTip.Size = new Size(147, 40);
			IPTip.TabIndex = 4;
			IPTip.Text = "Keep it void if you\r\nwant to create server";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(531, 450);
			Controls.Add(IPTip);
			Controls.Add(ControlButton);
			Controls.Add(IPInput);
			Controls.Add(MessageBox);
			Controls.Add(ChatBox);
			Name = "Form1";
			Text = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ListBox ChatBox;
		private TextBox MessageBox;
		private TextBox IPInput;
		private Button ControlButton;
		private Label IPTip;
	}
}
