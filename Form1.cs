using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace to_do_list
{
    public partial class Form1 : Form
    {
        string filePath = "tasks.json";

        public Form1()
        {
            InitializeComponent();
            CustomizeDesign();
        }

        private void CustomizeDesign()
        {
            this.BackColor = Color.FromArgb(245, 245, 245);

            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(60, 60, 60);

            txtTask.Font = new Font("Segoe UI", 12);
            txtTask.Width = 250;

            var buttons = new Button[] { btnAdd, btnDelete, btnClearAll };
            Color[] colors = { Color.MediumSeaGreen, Color.Tomato, Color.SteelBlue };

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].BackColor = colors[i];
                buttons[i].ForeColor = Color.White;
                buttons[i].FlatStyle = FlatStyle.Flat;
                buttons[i].FlatAppearance.BorderSize = 0;
                buttons[i].Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }

            chkTasks.Font = new Font("Segoe UI", 11);
            chkTasks.BackColor = Color.White;
            chkTasks.BorderStyle = BorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTasksFromFile();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTask.Text))
            {
                chkTasks.Items.Add(txtTask.Text);
                txtTask.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = chkTasks.CheckedIndices.Count - 1; i >= 0; i--)
            {
                chkTasks.Items.RemoveAt(chkTasks.CheckedIndices[i]);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("All tasks will be deleted, are you sure?", "Yes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                chkTasks.Items.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTasksToFile();
        }

        private void SaveTasksToFile()
        {
            List<string> tasks = new List<string>();
            foreach (var item in chkTasks.Items)
            {
                tasks.Add(item.ToString());
            }

            File.WriteAllText(filePath, JsonConvert.SerializeObject(tasks, Newtonsoft.Json.Formatting.Indented));
        }

        private void LoadTasksFromFile()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<string> tasks = JsonConvert.DeserializeObject<List<string>>(json);
                chkTasks.Items.Clear();
                foreach (var task in tasks)
                {
                    chkTasks.Items.Add(task);
                }
            }
        }
    }
}
