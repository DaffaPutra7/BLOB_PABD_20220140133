using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BLOB_PABD_20220140133
{
    public partial class Form1 : Form
    {
        //Declare the following class level variable:
        Image curImage;
        string curFileName;
        string connectionString = "data source = LAPTOP-CM925CH2\\DAFFAPUTRA;" +
            "Initial Catalog=Blob;Integrated Security=True";
        //The savedImageName will store the path of the image file. //Initialize the variable.
        string savedImageName = " ";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                curFileName = openDlg.FileName;
                textBox1.Text = curFileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                FileStream file = new FileStream(curFileName, FileMode.OpenOrCreate, FileAccess.Read);
                byte[] rawdata = new byte[file.Length];
                file.Read(rawdata, 0, System.Convert.ToInt32(file.Length));
                file.Close();

                string sql = "SELECT * FROM Image";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
                DataSet ds = new DataSet("Image");
                adapter.Fill(ds, "Image");

                DataRow row = ds.Tables["Image"].NewRow();
                row["ID"] = 1;
                row["Nama"] = "SQL";
                row["Photo"] = rawdata;
                ds.Tables["Image"].Rows.Add(row);

                adapter.Update(ds, "Image");
                connection.Close();

                // Display the image immediately after upload
                curImage = Image.FromStream(new MemoryStream(rawdata));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // Set the SizeMode to zoom
                pictureBox1.Image = curImage;
                pictureBox1.Refresh(); // Make sure the PictureBox is updated immediately

                MessageBox.Show("Image Saved and displayed");
            }
            else
                MessageBox.Show("Click the Browse button to select an image");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear the TextBox
            textBox1.Text = "";

            // Reset current filename and image variables
            curFileName = "";
            curImage = null;

            // Optionally reset the savedImageName if needed
            savedImageName = "";

            // Display a confirmation message to the user
            MessageBox.Show("Data has been cleared.");
        }
    }
}