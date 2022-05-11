namespace FoxLauncher
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    public partial class ChangeDirectX : Form
    {
        public ChangeDirectX()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText("selectedDirectX.txt", "DirectX9");
            this.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText("selectedDirectX.txt", "DirectX10");
            this.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            File.WriteAllText("selectedDirectX.txt", "DirectX11");
            this.Exit();
        }

        private void Exit()
        {
            this.Hide();
        }
    }
}