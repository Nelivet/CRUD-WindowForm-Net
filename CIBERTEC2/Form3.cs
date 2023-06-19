using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CIBERTEC2
{
    public partial class Form3 : Form
    {
        string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadFormulario(cadena);
        }

        private void LoadFormulario(string cadenaConexion)
        {
            //Llamamos a los stores procedures necesarios
            SqlDataAdapter da = new SqlDataAdapter("exec usp_listar_clientes", cadenaConexion);

            //Instanciamos los datables necesarios
            DataTable tb = new DataTable();
            da.Fill(tb);

            //Asiganmos la data a los componentes
            dataGridView1.DataSource = tb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();

            if (form2 == null)
            {
                form2 = new Form2(); // Crear una instancia de Form2
                form2.Show(); // Mostrar el formulario Form2
            }
            else
            {
                form2.Focus(); // Si el formulario ya está abierto, enfocarlo en primer plano
            }
            this.Close();
        }
    }
}
