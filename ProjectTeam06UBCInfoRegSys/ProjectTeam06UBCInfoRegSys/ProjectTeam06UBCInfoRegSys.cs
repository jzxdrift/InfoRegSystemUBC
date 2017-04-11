using System;
using System.Windows.Forms;

namespace ProjectTeam06UBCInfoRegSys
{
    static class ProjectTeam06UBCInfoRegSys
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UBCMainForm());
        }
    }
}