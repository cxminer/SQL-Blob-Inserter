using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace BlobInserter
{
    public partial class FormMain : Form
    {
        // Constants
        private const string conString = "Server={0};Database={1};User Id={2};Password={3};";

        // Data bound variables
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Query { get; set; }
        public string ParamName { get; set; }
        public string FileName { get; set; }

        // SQL connection
        private SqlConnection connection;

        public FormMain() : this("", "", "", "", "", "", "") { }

        public FormMain(string host,
            string username,
            string password,
            string database,
            string query,
            string paramName,
            string fileName)
        {
            InitializeComponent();

            // Save variables
            Host = host;
            Username = username;
            Password = password;
            Database = database;
            Query = query;
            ParamName = paramName;
            FileName = fileName;
        }

        private void ToggleUIState(bool enabled)
        {
            groupBoxConnection.Enabled = !enabled;
            buttonConnect.Enabled = !enabled;
            buttonDisconnect.Enabled = enabled;
            groupBoxSQL.Enabled = enabled;
        }

        private void DisplayException(Exception ex)
        {
            MessageBox.Show(
                string.Format("An exception occured!{0}{1}{0}{2}",
                    Environment.NewLine,
                    ex.Message,
                    ex.StackTrace),
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Establish bindings
            textBoxHost.DataBindings.Add("Text", this, "Host");
            textBoxUsername.DataBindings.Add("Text", this, "Username");
            textBoxPassword.DataBindings.Add("Text", this, "Password");
            textBoxDatabase.DataBindings.Add("Text", this, "Database");
            textBoxQuery.DataBindings.Add("Text", this, "Query");
            textBoxParamName.DataBindings.Add("Text", this, "ParamName");
            textBoxFileName.DataBindings.Add("Text", this, "FileName");
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // Create connection
            var connection = new SqlConnection(
                string.Format(conString,
                    textBoxHost.Text,
                    textBoxDatabase.Text,
                    textBoxUsername.Text,
                    textBoxPassword.Text));

            try
            {
                // Try to connect
                connection.Open();

                // If invalid state, restore form defaults
                if (connection.State != ConnectionState.Open)
                    buttonDisconnect_Click(sender, e);

                // Enable further interaction
                ToggleUIState(true);
            }
            catch (Exception ex)
            {
                // Restore form defaults
                buttonDisconnect_Click(sender, e);
                // Inform user about error
                DisplayException(ex);
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            // Cleanup SQL connection
            if (connection != null)
                connection.Dispose();
            connection = null;

            // Disable further interaction
            ToggleUIState(false);
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            // Prompt user to choose file
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            // Remember chosen file
            FileName = openFileDialog.FileName;
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            // Read file into byte array
            byte[] data = File.ReadAllBytes(FileName);

            // Create SQL command
            using (var command = connection.CreateCommand())
            {
                // Set query and parameter
                command.CommandText = Query;
                command.Parameters.Add(ParamName, SqlDbType.VarBinary).Value = data;

                try
                {
                    // Try to execute query
                    int rowsAffected = command.ExecuteNonQuery();

                    // Prompt user about success
                    MessageBox.Show(
                        string.Format("Query execution successful!{0}Rows affected: {1}",
                            Environment.NewLine,
                            rowsAffected),
                        "OK",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Prompt user about error
                    DisplayException(ex);
                }
            }
        }
    }
}
