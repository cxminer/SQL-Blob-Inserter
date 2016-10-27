using System;
using System.Windows.Forms;

namespace BlobInserter
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Arguments passed to FormMain constructor
            string host = string.Empty;
            string username = string.Empty;
            string password = string.Empty;
            string database = string.Empty;
            string query = string.Empty;
            string paramName = string.Empty;
            string fileName = string.Empty;

            // Allocate values if possible
            if (args.Length > 0)
                host = args[0];
            if (args.Length > 1)
                username = args[1];
            if (args.Length > 2)
                password = args[2];
            if (args.Length > 3)
                database = args[3];
            if (args.Length > 4)
                query = args[4];
            if (args.Length > 5)
                paramName = args[5];
            if (args.Length > 6)
                fileName = args[6];

            // Start application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(host, username, password, database, query, paramName, fileName));
        }
    }
}
