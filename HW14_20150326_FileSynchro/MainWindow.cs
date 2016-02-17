using HW14_20150326_FileSynchro.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW14_20150326_FileSynchro
{
    public partial class MainWindow : Form, IView
    {
        public MainWindow()
        {
            InitializeComponent();

            ///Selecting "Equivalent" direction for synchronization from start;
            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// Property for 1st folder (source)
        /// </summary>
        public string SourceFolder
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        /// <summary>
        /// Property for 2nd folder (destination)
        /// </summary>
        public string DestinationFolder
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        /// <summary>
        /// Getter of Synchronization direction: 0-Equivalent, 1-source:1st folder, 2-source:2nd folder
        /// </summary>
        public int Direction
        {
            get { return comboBox1.SelectedIndex; }
        }
        /// <summary>
        /// An event for Synchronization
        /// Raises when "Synchronize" button is pressed
        /// </summary>
        public event EventHandler<EventArgs> Synchronize;

        /// <summary>
        /// Receiving 1st folder and writing result to textbox using FolderBrowserDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose source folder:";
            fbd.SelectedPath = SourceFolder;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;

            }
            ///Described below: Function to make Synchronize button enabled/disabled
            CheckSyncPossible(sender, e);
        }

        /// <summary>
        /// Receiving 2nd folder and writing result to textbox using FolderBrowserDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose destination folder:";
            fbd.SelectedPath = DestinationFolder;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = fbd.SelectedPath;

            }
            CheckSyncPossible(sender, e);
        }

        /// <summary>
        /// Check whether Sync-button should be enabled/disabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckSyncPossible(object sender, EventArgs e)
        {
            ///Check Source Folder, destination folder existance
            if (!string.IsNullOrWhiteSpace(SourceFolder)
                && !string.IsNullOrWhiteSpace(DestinationFolder)
                && Directory.Exists(SourceFolder)
                && Directory.Exists(DestinationFolder))
            {
                button3.Enabled = true;
            }
            else
                button3.Enabled = false;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            ///For example: set our source directory as working directory
            ///---used to easy access test folder: testSource and testDest
            ///---can be commented or deleted
            SourceFolder = Directory.GetCurrentDirectory();

            ///same for Destination folder.
            DestinationFolder = Directory.GetCurrentDirectory();

            ///check possibility to enable synch button
            CheckSyncPossible(sender, e);
        }

        /// <summary>
        /// Synchronize button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            ///if Synchronize event has subscribers - call Synchronize event
            if (Synchronize != null)
                Synchronize(sender, e);
        }

        /// <summary>
        /// In current case is used for Exception information display
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="MessageName"></param>
        public void ShowMessage(string Text, string MessageName)
        {
            MessageBox.Show(Text, MessageName);
        }

        /// <summary>
        /// Direction selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    ///add some "style" for labels
                    label1.Text = "Choose 1st folder:";
                    label2.Text = "Choose 2nd folder:";
                    label3.Text = "Files will be added to both directories";
                    label1.Font = new Font(label1.Font, FontStyle.Regular);
                    label2.Font = new Font(label2.Font, FontStyle.Regular);
                    break;
                case 1:
                    ///add some "style" for labels
                    label1.Text += " (main)";
                    label2.Text = "Choose 2nd folder:";
                    label3.Text = "Files will be added/updated/deleted in 2nd directory";
                    label1.Font = new Font(label1.Font, FontStyle.Bold);
                    label2.Font = new Font(label2.Font, FontStyle.Regular);
                    break;
                case 2:
                    ///add some "style" for labels
                    label2.Text += " (main)";
                    label1.Text = "Choose 1st folder:";
                    label3.Text = "Files will be added/updated/deleted in 1st directory";
                    label1.Font = new Font(label1.Font, FontStyle.Regular);
                    label2.Font = new Font(label2.Font, FontStyle.Bold);
                    break;
            }
        }

        public void SetProgress(string TooltipText)
        {
            toolStripStatusLabel1.Text = TooltipText;
        }
    }
}
