using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxLauncher
{
    internal static class Program
    {
       
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!File.Exists("RustClient.exe"))
            {
                MessageBox.Show("Поместите лаунчер в папку с игрой!");
                Environment.Exit(0);
            }
            Application.Run(new Form1());

        }
        
    }
}
