using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Neurotec.Samples
{
	public partial class SettingsForm : Form
	{
		public static void ShowSettingsForm(IWin32Window parent, SettingsGroup group, string pageName)
		{
			SettingsForm settings = new SettingsForm();
/*
			settings.AddSettingsPanel(SettingsGroup.Extraction, new Fingers.ExtractorParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Extraction, new Faces.ExtractorParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Extraction, new Irises.ExtractorParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Extraction, new Palms.ExtractorParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Extraction, new Voices.ExtractorParametersPanel());

			settings.AddSettingsPanel(SettingsGroup.Matching, new Matching.GeneralParametersPanel());
			if (!WizardData.UseLocalDb)
			{
				settings.AddSettingsPanel(SettingsGroup.Matching, new Matching.RemoteMatcherParametersPanel());
			}
			settings.AddSettingsPanel(SettingsGroup.Matching, new Fingers.MatcherParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Matching, new Faces.MatcherParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Matching, new Irises.MatcherParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Matching, new Palms.MatcherParametersPanel());
			settings.AddSettingsPanel(SettingsGroup.Matching, new Voices.MatcherParametersPanel());
			settings.ShowPage(group, pageName);
			settings.ShowDialog(parent);
*/
		}

		public SettingsForm()
		{
			InitializeComponent();
		}

		public enum SettingsGroup
		{
			General,
			Extraction,
			Matching,
		}

		private readonly List<SettingsPanelInterface> _panels = new List<SettingsPanelInterface>();
		private readonly List<TreeNode> _groupNodes = new List<TreeNode>();

		private TreeNode GetGroupNode(SettingsGroup group)
		{
			foreach (TreeNode tn in _groupNodes)
			{
				if (tn.Text == group.ToString())
				{
					return tn;
				}
			}
			TreeNode newTn = new TreeNode(group.ToString());
			_groupNodes.Add(newTn);
			tvSettingsPageSelection.Nodes.Add(newTn);
			return newTn;
		}

		public void AddSettingsPanel(SettingsGroup group, SettingsPanelInterface panel)
		{
			_panels.Add(panel);
			TreeNode groupNode = GetGroupNode(group);
			TreeNode tn = new TreeNode(panel.DisplayName);
			tn.Tag = panel;
			groupNode.Nodes.Add(tn);
			groupNode.Expand();
		}

		public void ShowPage(SettingsGroup group, string pageName)
		{
			TreeNode node = GetGroupNode(group);
			foreach (TreeNode nodeChild in node.Nodes)
			{
				Control ctrl = nodeChild.Tag as Control;
				SettingsPanelInterface panel = nodeChild.Tag as SettingsPanelInterface;
				if (panel == null) continue;
				if (panel.DisplayName.ToLower() == pageName.ToLower())
				{
					tvSettingsPageSelection.SelectedNode = nodeChild;
					_lastPanel = panel;
					if (ctrl != null && _lastPanel != null)
					{
						_lastParentNode = nodeChild.Parent;
						ShowPanel(ctrl, _lastParentNode.Text, _lastPanel.DisplayName);
					}
					else
					{
						if (_lastParentNode != nodeChild)
						{
							if (nodeChild.Nodes.Count > 0)
							{
								ctrl = nodeChild.Nodes[0].Tag as Control;
								_lastPanel = ctrl as SettingsPanelInterface;
								_lastParentNode = nodeChild;
								if (ctrl != null && _lastPanel != null)
								{
									ShowPanel(ctrl, _lastParentNode.Text, _lastPanel.DisplayName);
								}
							}
						}
					}
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (_panels != null)
			{
				foreach (SettingsPanelInterface panel in _panels)
				{
					panel.SaveSettings();
				}
			}
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			if (_panels != null)
			{
				foreach (SettingsPanelInterface panel in _panels)
				{
					panel.SaveSettings();
				}
			}
		}

		private void btnDefaults_Click(object sender, EventArgs e)
		{
			if (_lastPanel != null)
			{
				_lastPanel.LoadDefaultSettings();
			}
		}

		private TreeNode _lastParentNode = null;
		private SettingsPanelInterface _lastPanel = null;

		private void ShowPanel(Control ctrl, string groupTitle, string panelTitle)
		{
			panelSettingsPage.Controls.Clear();
			panelSettingsPage.Controls.Add(ctrl);
			ctrl.Dock = DockStyle.Fill;
			lblTitle.Text = string.Format("{0}:{1}", groupTitle, panelTitle);
		}

		private void tvSettingsPageSelection_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Control ctrl = e.Node.Tag as Control;
			_lastPanel = e.Node.Tag as SettingsPanelInterface;
			if (ctrl != null && _lastPanel != null)
			{
				_lastParentNode = e.Node.Parent;
				ShowPanel(ctrl, _lastParentNode.Text, _lastPanel.DisplayName);
			}
			else
			{
				if (_lastParentNode != e.Node)
				{
					if (e.Node.Nodes.Count > 0)
					{ 
						ctrl = e.Node.Nodes[0].Tag as Control;
						_lastPanel = ctrl as SettingsPanelInterface;
						_lastParentNode = e.Node;
						if (ctrl != null && _lastPanel != null)
						{
							ShowPanel(ctrl, _lastParentNode.Text, _lastPanel.DisplayName);
						}
					}
				}
			}
		}

		private void SettingsForm_Shown(object sender, EventArgs e)
		{
			if (Visible)
			{
				if (tvSettingsPageSelection.SelectedNode == null)
				{
					if (tvSettingsPageSelection.Nodes.Count > 0)
					{
						tvSettingsPageSelection.SelectedNode = tvSettingsPageSelection.Nodes[0];
					}
				}
			}
		}
	}
}
