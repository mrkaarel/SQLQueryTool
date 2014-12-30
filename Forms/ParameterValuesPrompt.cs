using SqlQueryTool.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SqlQueryTool.Forms
{
	public partial class ParameterValuesPrompt : Form
	{
		public List<CommandParameter> Parameters { get; private set; }

		public ParameterValuesPrompt(IEnumerable<string> parameterNames, IEnumerable<CommandParameter> previousParameters)
		{
			InitializeComponent();

			this.Parameters = new List<CommandParameter>();
			AddFields(parameterNames, previousParameters);
		}

		private void AddFields(IEnumerable<string> parmNames, IEnumerable<CommandParameter> previousParameters)
		{
			foreach (var parmName in parmNames) {
				var lblName = new Label() { Anchor = AnchorStyles.Right, Text = String.Format("@{0}", parmName), AutoSize = true };
				var txtValue = new TextBox() { Anchor = (AnchorStyles.Left | AnchorStyles.Right), Tag = parmName, Name = "txtValue" };
				txtValue.TextChanged += txtValue_TextChanged;
				var selTypes = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, TabStop = false };
				selTypes.Items.AddRange(CommandParameter.SupportedTypes);
				tblParameterValues.Controls.Add(lblName);
				tblParameterValues.Controls.Add(txtValue);
				tblParameterValues.Controls.Add(selTypes);

				var previous = previousParameters.SingleOrDefault(p => p.Name == parmName);
				if (previous != null) {
					txtValue.Text = previous.OriginalValue;
				}
			}
		}

		void txtValue_TextChanged(object sender, EventArgs e)
		{
			string value = (sender as TextBox).Text;
			var selTypes = (tblParameterValues.GetNextControl((sender as TextBox), true)) as ComboBox;
			selTypes.SelectedItem = CommandParameter.GuessValueType(value);
		}



		private void btnOK_Click(object sender, EventArgs e)
		{
			bool hasProblems = false;
			this.Parameters.Clear();

			var txtValues = tblParameterValues.Controls.Find("txtValue", true).Cast<TextBox>();
			foreach (var txtValue in txtValues) {
				string parmName = txtValue.Tag as string;
				string value = txtValue.Text;
				string type = ((tblParameterValues.GetNextControl(txtValue, true)) as ComboBox).SelectedItem as string;

				try {
					this.Parameters.Add(new CommandParameter(parmName, value, type));
				}
				catch (Exception) {
					hasProblems = true;
					txtValue.BackColor = Color.LightPink;
				}
			}

			if (hasProblems) {
				return;
			}

			this.DialogResult = DialogResult.OK;
		}
	}
}
