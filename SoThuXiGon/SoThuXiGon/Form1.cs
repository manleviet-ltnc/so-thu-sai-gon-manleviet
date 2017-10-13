using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SoThuXiGon
{
    public partial class Form1 : Form
    {
        private bool isChanged = false;
        private bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void ListBox_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = lb.IndexFromPoint(e.X, e.Y);

            if (index != -1)
                lb.DoDragDrop(lb.Items[index].ToString(),
                              DragDropEffects.Copy);
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
        }

        private void lstDanhSach_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                ListBox lb = (ListBox)sender;
                
                string item = e.Data.GetData(DataFormats.Text).ToString();

                if (!Contains(lb, item))
                {
                    int index = lb.IndexFromPoint(lb.PointToClient(new Point(e.X, e.Y)));

                    if (index == -1)
                        lb.Items.Add(item);
                    else
                        lb.Items.Insert(index, item);

                    IsChanged = true;
                }
            }
        }

        private bool Contains(ListBox lb, string item)
        {
            foreach (string it in lb.Items)
                if (it == item)
                    return true;
            return false;
        }

        private void Save(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            // Mo tap tin
            StreamWriter write = new StreamWriter("danhsachthu.txt");

            if (write == null) return;

            foreach (var item in lstDanhSach.Items)
                write.WriteLine(item.ToString());

            write.Close();
            IsChanged = false;
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            SaveAndClose();
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveAndClose();
            base.OnFormClosing(e);
        }

        private void SaveAndClose()
        {
            if (IsChanged)
                Save();
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("thumoi.txt");

            if (reader == null) return;

            string input = null;
            while ((input = reader.ReadLine()) != null)
            {
                lstThuMoi.Items.Add(input);
            }
            reader.Close();

            using (StreamReader rs = new StreamReader("danhsachthu.txt"))
            {
                input = null;
                while ((input = rs.ReadLine()) != null)
                {
                    lstDanhSach.Items.Add(input);
                }
            }

            IsChanged = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = string.Format("Bây giờ là {0}:{1}:{2} ngày {3} tháng {4} năm {5}",
                                         DateTime.Now.Hour,
                                         DateTime.Now.Minute,
                                         DateTime.Now.Second,
                                         DateTime.Now.Day,
                                         DateTime.Now.Month,
                                         DateTime.Now.Year);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void Delete(object sender, EventArgs e)
        {
            int index = lstDanhSach.SelectedIndex;
            if (index >= 0 && index < lstDanhSach.Items.Count)
                // out of range
                lstDanhSach.Items.RemoveAt(index);

            IsChanged = true;
        }
    }
}
