namespace XMasLights.PlugInSystem
{
	partial class EffectManagerDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.effectsTree = new System.Windows.Forms.TreeView();
            this.infoLabel = new System.Windows.Forms.LinkLabel();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.getnewLink = new System.Windows.Forms.LinkLabel();
            this.pluginDirButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // effectsTree
            // 
            this.effectsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.effectsTree.Location = new System.Drawing.Point(12, 12);
            this.effectsTree.Name = "effectsTree";
            this.effectsTree.Size = new System.Drawing.Size(309, 328);
            this.effectsTree.TabIndex = 0;
            this.effectsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EffectsTree_AfterSelect);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.infoLabel.Location = new System.Drawing.Point(327, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(149, 358);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = "Name: {0}\r\n\r\nLib: {1}\r\n\r\nDescription: {2}\r\n\r\nWebsite : {3}";
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(401, 346);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(320, 346);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 3;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // getnewLink
            // 
            this.getnewLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.getnewLink.AutoSize = true;
            this.getnewLink.LinkArea = new System.Windows.Forms.LinkArea(21, 16);
            this.getnewLink.LinkColor = System.Drawing.SystemColors.Highlight;
            this.getnewLink.Location = new System.Drawing.Point(12, 355);
            this.getnewLink.Name = "getnewLink";
            this.getnewLink.Size = new System.Drawing.Size(191, 17);
            this.getnewLink.TabIndex = 4;
            this.getnewLink.TabStop = true;
            this.getnewLink.Text = "Get new Effects from Official Website";
            this.getnewLink.UseCompatibleTextRendering = true;
            this.getnewLink.Visible = false;
            this.getnewLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.getnewLink_LinkClicked);
            // 
            // pluginDirButton
            // 
            this.pluginDirButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginDirButton.Image = global::XMasLights.Properties.Resources.folder_icon;
            this.pluginDirButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pluginDirButton.Location = new System.Drawing.Point(327, 316);
            this.pluginDirButton.Name = "pluginDirButton";
            this.pluginDirButton.Size = new System.Drawing.Size(149, 24);
            this.pluginDirButton.TabIndex = 5;
            this.pluginDirButton.Text = "Open Plugins Directory";
            this.pluginDirButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.pluginDirButton.UseVisualStyleBackColor = true;
            this.pluginDirButton.Click += new System.EventHandler(this.pluginDirButton_Click);
            // 
            // EffectManagerDialog
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(488, 381);
            this.Controls.Add(this.pluginDirButton);
            this.Controls.Add(this.getnewLink);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.effectsTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "EffectManagerDialog";
            this.Text = "Effect Manager";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EffectManagerDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView effectsTree;
		private System.Windows.Forms.LinkLabel infoLabel;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.LinkLabel getnewLink;
        private System.Windows.Forms.Button pluginDirButton;
    }
}