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
#if NETCOREAPP
#else
            // Thanks to https://blogs.msdn.microsoft.com/shawnfa/2009/06/08/more-implicit-uses-of-cas-policy-loadfromremotesources/
            System.Security.PermissionSet trustedLoadFromRemoteSourceGrantSet = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            AppDomainSetup trustedLoadFromRemoteSourcesSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
            };
            AppDomain trustedRemoteLoadDomain = AppDomain.CreateDomain("Trusted LoadFromRemoteSources Domain",
                                                                       null,
                                                                       trustedLoadFromRemoteSourcesSetup,
                                                                       trustedLoadFromRemoteSourceGrantSet);
#endif


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow frm = new MainWindow(args);
            Application.Run(frm);
        }
    }
}
