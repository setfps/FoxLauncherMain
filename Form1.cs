namespace FoxLauncher
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    using Microsoft.Win32;

    public partial class Form1 : Form
    {
        private static Uri URLBrowser;

        public bool isStarted;

        private Point mPoint;

        private Process[] process;

        private readonly string steamLabel = Process.GetProcessesByName("Steam").Any() ? ": Online" : ": Offline";


        /// <summary>
        ///     The web.
        /// </summary>
        private readonly WebClient web = new WebClient();

        public Form1()
        {
            SetBrowserEmulation(11000);
            this.InitializeComponent();
            this.CheckDirectX();
            this.label2.Show();
            var thread = new Thread(this.DownloadFile);
            thread.Start();
            CheckisHohol();
            this.process = Process.GetProcessesByName("Steam");
            SetAnotherUserAgent(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36");
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Url = URLBrowser;
            this.label1.Text = this.steamLabel;

            this.panel1.MouseDown += this.panel1_MouseDown;
            this.panel1.MouseMove += this.panel1_MouseMove;
            this.panel5.MouseDown += this.panel1_MouseDown;
            this.panel5.MouseMove += this.panel1_MouseMove;
        }

        public static void CheckisHohol()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://darkmoon1.gamestores.ru/#/app/store/"))
                {
                    URLBrowser = new Uri("https://darkmoon1.gamestores.ru/#/app/store/");
                }
            }
            catch (WebException)
            {
                URLBrowser = new Uri("https://darkmoon1.gamestores.app/#/app/store/");
            }
        }

        public static void SetAnotherUserAgent(string ua)
        {
            const int urlmonOptionUseragent = 0x10000001;
            const int urlmonOptionUseragentRefresh = 0x10000002;
            var UserAgent = ua;
            UrlMkSetSessionOption(urlmonOptionUseragentRefresh, null, 0, 0);
            UrlMkSetSessionOption(urlmonOptionUseragent, UserAgent, UserAgent.Length, 0);
        }

        public static void SetBrowserEmulation(int version)
        {
            var program = Application.ExecutablePath.Split('\\').Reverse().ToList()[0];
            var key = "SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";

            var hkcu = Registry.CurrentUser.CreateSubKey(key);
            hkcu.SetValue(program, version, RegistryValueKind.DWord);
            hkcu.Close();
        }

        public void CheckDirectX()
        {
            if (!File.Exists("selectedDirectX.txt")) File.Create("selectedDirectX.txt");
        }

        public void CompletedDownload(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                Environment.Exit(0);
            }
            else
            {
                this.label2.Text = "Готово к работе!";
                Thread.Sleep(3000);
                this.isStarted = true;
                this.label2.Hide();
            }
        }

        public void DownloadFile()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                this.web.DownloadFileAsync(
                    new Uri("https://github.com/setfps/FoxLauncherFiles/raw/main/Assembly-CSharp.dll"),
                    Environment.CurrentDirectory + @"\RustClient_Data\Managed\Assembly-CSharp.dll");
                this.web.DownloadFileCompleted += this.CompletedDownload;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                Environment.Exit(0);
            }
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(
            int dwOption,
            string pBuffer,
            int dwBufferLength,
            int dwReserved);

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            this.mPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Location = new Point(this.Location.X + e.X - this.mPoint.X, this.Location.Y + e.Y - this.mPoint.Y);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            var changeDirectX = new ChangeDirectX();
            changeDirectX.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            if (this.isStarted)
            {
                if (File.Exists("RustClient.exe"))
                {
                    var startInfo = new ProcessStartInfo();
                    var CurrentDirectX = File.ReadAllText("selectedDirectX.txt");
                    string GameDirectX = null;
                    if (CurrentDirectX.Length <= 1 || !CurrentDirectX.Contains("DirectX"))
                    {
                        MessageBox.Show("Выберите DirectX!");
                        return;
                    }

                    if (CurrentDirectX == "DirectX9") GameDirectX = "-show-screen-selector -force-d3d9-ref";
                    else if (CurrentDirectX == "DirectX10") GameDirectX = "-show-screen-selector -force-feature-level-10-0";
                    else if (CurrentDirectX == "DirectX11") GameDirectX = "-show-screen-selector -force-feature-level-11-0";
                    startInfo.Arguments = GameDirectX;
                    startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                    startInfo.FileName = "RustClient.exe";
                    var process = Process.Start(startInfo);
                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("В папке не обнаружена игра!");
                }
            }
            else
            {
                MessageBox.Show("Лаунчер не готов к работе, ожидайте!");
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}