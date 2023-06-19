using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml;

namespace CIBERTEC2
{
    public partial class Form1 : Form
    {
        string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        DataTable tb = new DataTable();

        public Form1()
        {
            InitializeComponent();
            tb.Columns.Add("idOrdenCompra");
            tb.Columns.Add("codigo");
            tb.Columns.Add("fechaEmision");
            tb.Columns.Add("idProveedor");
            tb.Columns.Add("razonSocial");
            tb.Columns.Add("total");
            LoadCombobox(cadena);
            LoadFormulario(cadena);
        }
        private void LoadFormulario(string cadenaConexion)
        {
            SqlConnection cn = new SqlConnection(cadenaConexion);

            SqlCommand cmd = new SqlCommand("exec sp_GetComprasByFechasProveedor @prmdatFechaInicio, @prmdatFechaFin, @prmintIdProveedor", cn);
            cmd.Parameters.AddWithValue("@prmdatFechaInicio", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@prmdatFechaFin", dateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@prmintIdProveedor", cboProveedor.SelectedValue);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            tb.Rows.Clear();

            while (dr.Read())
            {
                DataRow fila = tb.NewRow();
                fila[0] = dr.GetInt32(0);
                fila[1] = dr.GetString(1);
                fila[2] = dr.GetDateTime(2);
                fila[3] = dr.GetInt32(3);
                fila[4] = dr.GetString(4);
                fila[5] = dr.GetDecimal(5);
                tb.Rows.Add(fila);
            }

            dataGridView1.DataSource = tb;


        }

        private void LoadCombobox(string cadenaConexion)
        {
            //Llamamos a los stores procedures necesarios
            SqlDataAdapter proveedores = new SqlDataAdapter("exec sp_GetProveedores", cadenaConexion);

            //Instanciamos los datables necesarios
            DataTable dtProveedor = new DataTable();
            proveedores.Fill(dtProveedor);

            //Asiganmos la data a los componentes
            cboProveedor.DataSource = dtProveedor;
            cboProveedor.ValueMember = "idProveedor";
            cboProveedor.DisplayMember = "razonSocial";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadFormulario(cadena);
        }

        private void button2_Click(object sender, EventArgs e)
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
            //    Form2 form2 = new Form2();
            //  form2.Show();
            //  this.Close();
        }
    }
}
