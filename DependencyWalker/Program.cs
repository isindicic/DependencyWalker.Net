using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SindaSoft.DependencyWalker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow frm = new MainWindow(args);
            Application.Run(frm);
        }
    }
}
