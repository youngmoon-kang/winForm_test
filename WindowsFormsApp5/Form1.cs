﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        string src;
        string src2;
        private BackgroundWorker worker;
        private BackgroundWorker worker2;

        public Form1()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += new DoWorkEventHandler(worker_DoWorker);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            worker.RunWorkerAsync();

            src = "C:\\iot";
            textBox1.Text = src;

            worker2 = new BackgroundWorker();
            worker2.WorkerReportsProgress = true;
            worker2.WorkerSupportsCancellation = true;

            worker2.DoWork += new DoWorkEventHandler(worker_DoWorker2);
            worker2.ProgressChanged+= new ProgressChangedEventHandler(worker_ProgressChanged2);
            worker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted2);

            worker2.RunWorkerAsync();

            src2 = src;
            textBox2.Text = src2;
        }

        private void worker_DoWorker(object sender, DoWorkEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(src);
            FileInfo[] files = di.GetFiles();
            int totalFiles = files.Length;

            listView1.Invoke((MethodInvoker)delegate {
                listView1.BeginUpdate();
                listView1.View = View.Details;
                listView1.Items.Clear();
            });
            int i = 0;
            int pct = 0;
            foreach (var fi in files) {
                ListViewItem lvi = new ListViewItem(fi.Name);
                lvi.SubItems.Add(fi.Length.ToString());
                lvi.SubItems.Add(fi.LastWriteTime.ToString());

                listView1.Invoke((MethodInvoker) delegate
                {
                    listView1.Items.Add(lvi);
                });
                
                pct = ((++i * 100) / totalFiles);
                worker.ReportProgress(pct);
            }
            
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message,"Error");
                return;
            }

            listView1.Columns.Add("파일명", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("사이즈", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("날짜", 150, HorizontalAlignment.Left);

            listView1.Invoke((MethodInvoker)delegate {
                listView1.EndUpdate();
            });
            MessageBox.Show("end!");
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void worker_DoWorker2(object sender, DoWorkEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(src2);
            FileInfo[] files = di.GetFiles();
            int totalFiles = files.Length;

            listView2.Invoke((MethodInvoker)delegate {
                listView2.BeginUpdate();
                listView2.View = View.Details;
                listView2.Items.Clear();
            });
            int i = 0;
            int pct = 0;
            foreach (var fi in files)
            {
                ListViewItem lvi = new ListViewItem(fi.Name);
                lvi.SubItems.Add(fi.Length.ToString());
                lvi.SubItems.Add(fi.LastWriteTime.ToString());

                listView2.Invoke((MethodInvoker)delegate
                {
                    listView2.Items.Add(lvi);
                });

                pct = ((++i * 100) / totalFiles);
                worker2.ReportProgress(pct);
            }

        }

        private void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error");
                return;
            }

            listView2.Columns.Add("파일명", 200, HorizontalAlignment.Left);
            listView2.Columns.Add("사이즈", 70, HorizontalAlignment.Left);
            listView2.Columns.Add("날짜", 150, HorizontalAlignment.Left);

            listView2.Invoke((MethodInvoker)delegate {
                listView2.EndUpdate();
            });
        }

        private void worker_ProgressChanged2(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar2.Value = e.ProgressPercentage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            src = textBox1.Text;
            worker.RunWorkerAsync();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            src2 = textBox2.Text;
            worker2.RunWorkerAsync();
        }
    }
}
