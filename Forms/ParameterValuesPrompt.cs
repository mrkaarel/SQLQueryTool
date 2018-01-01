using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SqlQueryTool.DatabaseObjects;

namespace SqlQueryTool.Forms
{
    public partial class ParameterValuesPrompt : Form
    {
        public ParameterValuesPrompt(IEnumerable<string> parameterNames,
            IEnumerable<CommandParameter> previousParameters)
        {
            InitializeComponent();

            Parameters = new List<CommandParameter>();
            AddFields(parameterNames, previousParameters);
        }

        public List<CommandParameter> Parameters { get; }

        private void AddFields(IEnumerable<string> parmNames, IEnumerable<CommandParameter> previousParameters)
        {
            var isFirstElement = true;

            foreach (var parmName in parmNames)
            {
                var lblName = new Label
                {
                    Anchor = AnchorStyles.Right,
                    Text = $"@{parmName}",
                    AutoSize = true
                };
                var txtValue = new TextBox
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Tag = parmName,
                    Name = "txtValue"
                };
                txtValue.TextChanged += txtValue_TextChanged;

                var selTypes = new ComboBox {DropDownStyle = ComboBoxStyle.DropDownList, TabStop = false};
                selTypes.Items.AddRange(CommandParameter.SupportedTypes);
                tblParameterValues.Controls.Add(lblName);
                tblParameterValues.Controls.Add(txtValue);
                tblParameterValues.Controls.Add(selTypes);

                if (isFirstElement)
                {
                    ActiveControl = txtValue;
                    isFirstElement = false;
                }

                var previous = previousParameters.SingleOrDefault(p => p.Name == parmName);
                if (previous != null) txtValue.Text = previous.OriginalValue;
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            var value = (sender as TextBox).Text;
            var selTypes = tblParameterValues.GetNextControl(sender as TextBox, true) as ComboBox;
            selTypes.SelectedItem = CommandParameter.GuessValueType(value);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var hasProblems = false;
            Parameters.Clear();

            var txtValues = tblParameterValues.Controls.Find("txtValue", true).Cast<TextBox>();
            foreach (var txtValue in txtValues)
            {
                var parmName = txtValue.Tag as string;
                var value = txtValue.Text;
                var type = (tblParameterValues.GetNextControl(txtValue, true) as ComboBox).SelectedItem as string;

                try
                {
                    Parameters.Add(new CommandParameter(parmName, value, type));
                }
                catch (Exception)
                {
                    hasProblems = true;
                    txtValue.BackColor = Color.LightPink;
                }
            }

            if (hasProblems) return;

            DialogResult = DialogResult.OK;
        }
    }
}