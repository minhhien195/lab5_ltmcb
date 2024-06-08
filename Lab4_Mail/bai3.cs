using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4_Mail
{
    public partial class bai3 : Form
    {
        private List<string> selectedFilePaths = new List<string>();
        public bai3()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(txtFrom.Text.ToString().Trim());
                mail.To.Add(txtTo.Text.ToString().Trim());
                mail.Subject = txtSubject.Text;
                mail.IsBodyHtml = true;
                // Đính kèm tập tin
                foreach (var filePath in selectedFilePaths)
                {
                    if (filePath != null)
                    {
                        Attachment attachment = new Attachment(filePath);
                        mail.Attachments.Add(attachment);
                    }
                }

                mail.Body = rtbBody.Text.Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>");

                mail.Priority = MailPriority.High;
                
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(txtFrom.Text.ToString().Trim(), txtPassword.Text.ToString().Trim());
                SmtpServer.EnableSsl = true;
/*                string userState = "test message1";
                SmtpServer.SendAsync(mail,userState);*/
                SmtpServer.Send(mail);
                DialogResult dialogResult =  MessageBox.Show("Gửi thành công!", "Thành công");
                if (dialogResult == DialogResult.OK)
                {
                    rtbBody.Text = "";
                    selectedFilePaths.Clear();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    selectedFilePaths.Add(fileName);
                    rtbBody.Text +=  Path.GetFileName(fileName) + "\r\n";
                }
/*                selectedFilePaths.Add(openFileDialog.FileName);
                rtbBody.Text +=  Path.GetFileName(openFileDialog.FileName) + "\r\n";*/
            }
        }
    }
}
