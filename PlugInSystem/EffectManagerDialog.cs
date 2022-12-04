using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace XMasLights.PlugInSystem
{
	public partial class EffectManagerDialog : Form
	{
		public int libIx = 0, effIx = 0, prevLib = 0, prevEff = 0;
		public EffectManagerDialog()
		{
			InitializeComponent();
			foreach (IEffectCollection plugin in MainForm.plugins)
			{
				TreeNode node = new TreeNode(plugin.GetPlugInName(),0,0) { Tag = false };
				foreach (ILightsEffect effect in plugin.GetEffects())
					node.Nodes.Add(new TreeNode(effect.GetName(),1,1) { Tag = true });
					effectsTree.Nodes.Add(node);
			}
			ImageList icons = new ImageList();
			icons.ColorDepth = ColorDepth.Depth32Bit;
			icons.Images.Add(Properties.Resources.box_icon);
			icons.Images.Add(Properties.Resources.block_icon);
			effectsTree.ImageList = icons;
			prevLib = MainForm.libIndex;
			prevEff = MainForm.effectIndex;
		}
		private void OkBtn_Click(object sender, EventArgs e)
		{
			if ((bool)effectsTree.SelectedNode.Tag) 
			{
				MainForm.libIndex = libIx;
				MainForm.effectIndex = effIx;
				MainForm.prefs.Write("libIx", libIx.ToString(), "effect");
				MainForm.prefs.Write("effectIx", effIx.ToString(), "effect");
				Close();
			}
			else effectsTree.SelectedNode.Expand();
		}

        private void pluginDirButton_Click(object sender, EventArgs e)
        {
			Process.Start(Environment.CurrentDirectory + "\\plugins");
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
			MainForm.libIndex = prevLib;
			MainForm.effectIndex = prevEff;
            Close();
        }

        private void getnewLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.Visited)
				MessageBox.Show("You've been here. Nothing chaged!\nComing soon...", "XMasLights", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
				MessageBox.Show("It's not actually a release, later this link will open website!\nComing soon...", "XMasLights", MessageBoxButtons.OK, MessageBoxIcon.Information);
				e.Link.Visited = true;
            }
				
			//e.Link.Visited = true;
			//Process.Start("https://catyoutuber.github.io/projects/xmaslights/plugins");
        }

        private void EffectsTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			bool isEffect = (bool)effectsTree.SelectedNode.Tag;
			TreeNode sn = effectsTree.SelectedNode;
			IEffectCollection collection = MainForm.plugins[isEffect ? sn.Parent.Index : sn.Index];
			infoLabel.Text = string.Format("Name: {0}\r\n\nLib: {1}\r\n\nDescription: {2}" + (isEffect ? "\r\n\nFrameRate : {3}" : ""),
				isEffect ? collection.GetEffects()[sn.Index].GetName() : collection.GetPlugInName(),
				MainForm.GetFileName(MainForm.dllNames[isEffect ? sn.Parent.Index : sn.Index]),
				isEffect ? collection.GetEffects()[sn.Index].GetDescription() : collection.GetPlugInDescription(),
				isEffect ? collection.GetEffects()[sn.Index].GetRequiredFrameRate() : 0);
			okBtn.Enabled = isEffect;
			if(isEffect)
			{
				MainForm.libIndex = libIx = sn.Parent.Index;
				MainForm.effectIndex = effIx = sn.Index;
			}
		}
	}
}
