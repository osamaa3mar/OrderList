using Adam_s_Order_List.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adam_s_Order_List
{
    public partial class Form1 : Form
    {
        //********************Important***************************
        //change the path of the file from here
        string path = "C:\\Users\\ayhab\\source\\repos\\Adam's Order List\\Resources\\TextFile.txt";
        
        
        public Form1()
        {
            InitializeComponent();
            LoadAllFileContentToListView();
        }
        void LoadAllFileContentToListView()
        {
            listView1.Items.Clear();
            string [] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string []record = line.Split(',');
                ListViewItem item = new ListViewItem(record[0]);
                item.Group = ChosenGroup(record[1]);
                listView1.Items.Add(item);
            }
        }
        RadioButton CheckedRadio(string record="")
        {
            
            if (rbSchool.Checked || record == "rbSchool")
            {
                return rbSchool;
            }
            if (rbGames.Checked || record == "rbGames")
            {
                return rbGames;
            }
            return  rbSchool;
        }
        ListViewGroup ChosenGroup(string record = "")
        {
            if (rbSchool.Checked || record == "rbSchool")
            {
                return listView1.Groups[0];
            }
            if (rbGames.Checked || record == "rbGames")
            {
                return  listView1.Groups[1];
            }
            return listView1.Groups[0];
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("أضف طلب أولا");
                textBox1.Text = "";
                textBox1.Focus();                
                return;
            }
            if( (rbGames.Checked == false && rbSchool.Checked == false))
            {
                MessageBox.Show("أختر النوع");
                return;
            }

            ListViewItem item = new ListViewItem(new string[] { textBox1.Text });

            item.Group = ChosenGroup();
            
            listView1.Items.Add(item);
            string Line = textBox1.Text + "," + CheckedRadio().Name + Environment.NewLine;
            if (Line != "")
            {
                File.AppendAllText(path, Line);
            }
            textBox1.Text = "";
            CheckedRadio().Checked = false;
        }
    
        void DeleteSelectedFromFile(ListViewItem Item)
        {
            string newLine = "";
            string[] Lines = File.ReadAllLines(path);
            foreach (string line in Lines)
            {
                string []records = line.Split(',');
                if (records[0] != Item.SubItems[0].Text)
                {
                    newLine += line + Environment.NewLine;
                }
            }
            File.WriteAllText(path, newLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if(MessageBox.Show("هل أنت متأكد من الحذف؟","",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) == DialogResult.Cancel)
            {
                return;
            }
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                item.Remove();
                DeleteSelectedFromFile(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadAllFileContentToListView();
        }
        string CopyTheFileDataNamesOnly()
        {
            string all = "";
            string[] AllData = File.ReadAllLines(path);
            for (int i = 0; i < AllData.Length; i++)
            {
                string[] records = AllData[i].Split(',');
                records[0] += Environment.NewLine + Environment.NewLine;
                all += records[0];
            }
            return all;
        }
        string CopyTheListViewDataNamesAndGroups()
        {
            string all = "";
            string[] AllData = File.ReadAllLines(path);

            all += "***************قرطاسية***************" + Environment.NewLine+ Environment.NewLine;
            for (int i = 0; i < AllData.Length; i++)
            {
                string[] records = AllData[i].Split(',');

                if (records[1] == "rbSchool")
                {                    
                    records[0] += Environment.NewLine + Environment.NewLine;
                    all += records[0];
                }    
            }

            all += "****************العاب*********************" + Environment.NewLine + Environment.NewLine;

            for (int y = 0; y < AllData.Length; y++)
            {
                string[] records = AllData[y].Split(',');
                
                if (records[1] == "rbGames")
                {

                    records[0] += Environment.NewLine + Environment.NewLine;
                    all += records[0];
                }
                
            }
            return all;
        }
    
        private void button2_Click_1(object sender, EventArgs e)
        {

            string all = CopyTheListViewDataNamesAndGroups();

            System.Windows.Forms.Clipboard.SetText(all);
            niCopied.BalloonTipText = "تم النسخ";
            niCopied.ShowBalloonTip(10);
            MessageBox.Show("تم النسخ");
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            desktopPath += "\\Copyمكتبة ادم ليست.txt";
            File.WriteAllText(desktopPath, CopyTheListViewDataNamesAndGroups());
           
            Process.Start(desktopPath);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            desktopPath += "\\Copyمكتبة ادم ليست.txt";
            File.Delete(desktopPath);
        }
    }
}
