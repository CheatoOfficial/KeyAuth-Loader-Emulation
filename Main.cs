using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyAuth;

namespace KeyAuthLoaderEmulation
{
    public partial class Main : Form
    {
        public static api KeyAuthApp;

        public Main()
        {
            InitializeComponent();
            hwidTextBox.Text = WindowsIdentity.GetCurrent().User.Value;
            pcUsernameTextBox.Text = Environment.UserName;
            custPCUsernameTextBox.Text = pcUsernameTextBox.Text;
            using (MD5 md = MD5.Create())
            {
                using (FileStream fileStream = File.OpenRead(Process.GetCurrentProcess().MainModule.FileName))
                {
                    byte[] value = md.ComputeHash(fileStream);
                    checksumTextBox.Text = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private void fillHWIDButton_Click(object sender, EventArgs e)
        {
            hwidTextBox.Text = WindowsIdentity.GetCurrent().User.Value;
        }

        private void initButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp = new api(
            name: appNameTextBox.Text.Trim(),
            ownerid: ownerIDTextBox.Text.Trim(),
            secret: appSecretTextBox.Text.Trim(),
            version: versionTextBox.Text.Trim(),
            hwid: hwidTextBox.Text.Trim(),
            apiurl: apiUrlTextBox.Text.Trim(),
            useragent: userAgentTextBox.Text.Trim(),
            pcusername: pcUsernameTextBox.Text.Trim(),
            customchecksum: checksumTextBox.Text.Trim()
        );

            KeyAuthApp.init();
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
            }
            else
            {
                MessageBox.Show("Success");
                sessionIDTextBox.Text = KeyAuthApp.sessionid;
                encKeyTextBox.Text = KeyAuthApp.enckey;
                numUsersTextBox.Text = KeyAuthApp.app_data.numUsers;
                numOnlineUsersTextBox.Text = KeyAuthApp.app_data.numOnlineUsers;
                numKeysTextBox.Text = KeyAuthApp.app_data.numKeys;
                curVerTextBox.Text = KeyAuthApp.app_data.version;
                custPanelLinkTextBox.Text = KeyAuthApp.app_data.customerPanelLink;
                downloadLinkTextBox.Text = KeyAuthApp.app_data.downloadLink;
            }
        }

        private void authLicenseButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp.license(licenseKeyTextBox.Text);
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
            }
            else
            {
                MessageBox.Show("Success");
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp.register(usernameTextBox.Text, passwordTextBox.Text, licenseKeyTextBox.Text);
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
            }
            else
            {
                MessageBox.Show("Success");
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp.login(usernameTextBox.Text, passwordTextBox.Text);
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
            }
            else
            {
                MessageBox.Show("Success");
            }
        }

        private void upgradeButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp.upgrade(usernameTextBox.Text, licenseKeyTextBox.Text);
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
            }
            else
            {
                MessageBox.Show("Success");
            }
        }

        private void fillPCUsernameButton_Click(object sender, EventArgs e)
        {
            pcUsernameTextBox.Text = Environment.UserName;
        }

        private void getVarButton_Click(object sender, EventArgs e)
        {
            string var = KeyAuthApp.var(varIDTextBox.Text);
            if (var != null)
            {
                varDataTextBox.Text = var;
                MessageBox.Show("Success");
            }
            else
            {
                varDataTextBox.Text = "NOT FOUND OR NEED TO AUTH";
                MessageBox.Show("Fail?");
            }
        }

        private void getFileButton_Click(object sender, EventArgs e)
        {
            byte[] data = KeyAuthApp.download(fileIDTextBox.Text);
            if (data == null)
            {
                MessageBox.Show("File not found or no data or need to auth");
            }
            else
            {
                File.WriteAllBytes(fileIDTextBox.Text, data);
                MessageBox.Show($"File written to file name: {fileIDTextBox.Text}");
            }
        }

        private void sendLogButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp.log(logMessageTextBox.Text);
            MessageBox.Show("OK");
        }

        private void setPCUsernameButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp.pcusername = custPCUsernameTextBox.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void getChecksumButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                //InitialDirectory = @"C:\",
                Title = "Select File to Read Checksum",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "*.*",
                Filter = "All Files|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,

                //ReadOnlyChecked = true,
                //ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (MD5 md = MD5.Create())
                {
                    using (FileStream fileStream = File.OpenRead(openFileDialog1.FileName))
                    {
                        byte[] value = md.ComputeHash(fileStream);
                        checksumTextBox.Text = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
                        return;
                    }
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void getUserVarButton_Click(object sender, EventArgs e)
        {
            string var = KeyAuthApp.getvar(userVarNameTextBox.Text);
            if (var != null)
            {
                userVarDataTextBox.Text = var;
                MessageBox.Show("Success");
            }
            else
            {
                userVarDataTextBox.Text = "NOT FOUND OR NEED TO AUTH";
                MessageBox.Show("Fail?");
            }
        }

        private void setUserVarButton_Click(object sender, EventArgs e)
        {
            KeyAuthApp.setvar(userVarNameTextBox.Text, userVarDataTextBox.Text);
        }

        private void bruteforceButton_Click(object sender, EventArgs e)
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            //List<int> numbers = Enumerable.Range(0, 999999).OrderBy(x => rnd.Next()).Take(999999).ToList();
            int[] numbers = Enumerable.Range(0, 999999).OrderBy(x => rnd.Next()).Take(999999).ToArray();
            int[] half1 = numbers.Take(numbers.Length / 2).ToArray();
            int[] half2 = numbers.Skip(numbers.Length / 2).ToArray();
            int[] quarter1 = half1.Take(half1.Length / 2).ToArray();
            int[] quarter2 = half1.Skip(half1.Length / 2).ToArray();
            int[] quarter3 = half2.Take(half2.Length / 2).ToArray();
            int[] quarter4 = half2.Skip(half2.Length / 2).ToArray();
            Task.Run(() => BruteforceFiles(quarter1.Take((quarter1.Length + 1) / 2).ToArray()));
            Task.Run(() => BruteforceFiles(quarter1.Skip((quarter1.Length + 1) / 2).ToArray()));
            Task.Run(() => BruteforceFiles(quarter2.Take((quarter2.Length + 1) / 2).ToArray()));
            Task.Run(() => BruteforceFiles(quarter2.Skip((quarter2.Length + 1) / 2).ToArray()));
            Task.Run(() => BruteforceFiles(quarter3.Take((quarter3.Length + 1) / 2).ToArray()));
            Task.Run(() => BruteforceFiles(quarter3.Skip((quarter3.Length + 1) / 2).ToArray()));
            Task.Run(() => BruteforceFiles(quarter4.Take((quarter4.Length + 1) / 2).ToArray()));
            Task.Run(() => BruteforceFiles(quarter4.Skip((quarter4.Length + 1) / 2).ToArray()));
        }

        private void BruteforceFiles(int[] numbers)
        {
            foreach (int num in numbers)
            {
                string selected = num.ToString("D6");
                byte[] data = KeyAuthApp.download(selected);
                if (!KeyAuthApp.response.success)
                {
                    AppendTextBox($"Failed file ID: {selected}: " + KeyAuthApp.response.message);
                }
                else
                {
                    if (data != null)
                    {
                        File.WriteAllBytes(selected, data);
                        AppendTextBox($"Successfully found and downloaded file ID: {selected}");
                    }
                    else
                    {
                        AppendTextBox($"Failed file ID: {selected}: NULL DATA");
                    }
                }
                //else
                //{
                //    AppendTextBox($"Failed file ID: {selected}");
                //}
            }
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            textBox1.Text += value + Environment.NewLine;
            Invoke(new MethodInvoker(delegate ()
            {
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }));
        }

        private void appNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
