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
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace AutoMailer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        string folder = "";

        private void button2_Click(object sender, EventArgs e)
        {
            //Choose Folder Path
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folder = dlg.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (folder == "")
            {
                MessageBox.Show("Please select a folder to continue");
            }

                List<string> DeniedFileTypes = new List<string> { };
                    
                //Loop through checked items to determine whether or not to attach
                foreach(Control c in groupBox1.Controls)
                {
                    if((c is CheckBox) && ((CheckBox)c).Checked)
                    {
                        DeniedFileTypes.Add(c.Text);
                    }
                }

                List<string> files = new List<string> { };

                foreach(string f in Directory.GetFiles(folder))
                {
                    if (!DeniedFileTypes.Contains(Path.GetExtension(f)))
                    {
                        FileInfo mb = new FileInfo(f);
                        if (Convert.ToInt32(mb.Length) < 25000000)
                        {
                            files.Add(f);
                        }
                    }
                    else { files.Remove(f); }
                }

            int count = 0;
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(textBox4.Text + "@gmail.com");
            mail.To.Add(textBox1.Text);
            mail.Subject = (textBox2.Text);
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential(textBox4.Text, textBox3.Text);
            SmtpServer.EnableSsl = true;
            int messageSize = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                while (count < files.Count())
                    {
                        FileInfo mb = new FileInfo(files[count]);
                        if ((messageSize + Convert.ToInt32(mb.Length)) < 25000000)
                        {
                            MemoryStream ms = new MemoryStream(File.ReadAllBytes(files[count]));
                            mail.Attachments.Add(new Attachment(ms, files[count]));
                            messageSize += Convert.ToInt32(mb.Length);
                            count += 1;
                            if(count == files.Count())
                            {
                                SmtpServer.Send(mail);
                            }
                        }
                        else
                        {
                            SmtpServer.Send(mail);
                            mail.Attachments.Clear();
                            messageSize = 0;
                        }
                    }
                });
            t.Start();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
