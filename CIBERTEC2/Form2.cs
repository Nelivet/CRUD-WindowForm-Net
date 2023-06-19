using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIBERTEC2
{
    public partial class Form2 : Form
    {
        string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadFormulario(cadena);
            LoadCombobox(cadena);
        }
        private void LoadFormulario(string cadenaConexion)
        {
            //Llamamos a los stores procedures necesarios
            SqlDataAdapter da = new SqlDataAdapter("exec sp_GetProductos", cadenaConexion);

            //Instanciamos los datables necesarios
            DataTable tb = new DataTable();
            da.Fill(tb);

            //Asiganmos la data a los componentes
            dataGridView1.DataSource = tb;
        }
        private int? GetId()
        {
            return int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
        }
        private void LoadCombobox(string cadenaConexion)
        {
            //Llamamos a los stores procedures necesarios
            SqlDataAdapter categorias = new SqlDataAdapter("exec sp_GetCategorias", cadenaConexion);

            //Instanciamos los datables necesarios
            DataTable dtCategoria = new DataTable();
            categorias.Fill(dtCategoria);

            //Asiganmos la data a los componentes
            cboCategoria.DataSource = dtCategoria;
            cboCategoria.ValueMember = "idCategoria";
            cboCategoria.DisplayMember = "nombreCategoria";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(cadena);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", GetId());
                cn.Open();
                cmd.ExecuteNonQuery();
                LoadFormulario(cadena);
                MessageBox.Show("Se eliminado al producto correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow f = dataGridView1.CurrentRow;
            nombreProducto.Text = f.Cells[1].Value.ToString();
            textBox3.Text = f.Cells[3].Value.ToString();
            textBox4.Text = f.Cells[4].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nombreProducto.Text) && !string.IsNullOrWhiteSpace(textBox3.Text)
          && !string.IsNullOrWhiteSpace(textBox4.Text))
            {
                SqlConnection cn = new SqlConnection(cadena);
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateProducto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", GetId());
                    cmd.Parameters.AddWithValue("@nombreProducto", nombreProducto.Text);
                    cmd.Parameters.AddWithValue("@precioUnitario", textBox3.Text);
                    cmd.Parameters.AddWithValue("@stock", textBox4.Text);
                    cmd.Parameters.AddWithValue("@idCategoria", cboCategoria.SelectedValue);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    LoadFormulario(cadena);
                    MessageBox.Show("Se actualizó al producto correctamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Debe completar todos campos.");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nombreProducto.Text) && !string.IsNullOrWhiteSpace(textBox3.Text)
                && !string.IsNullOrWhiteSpace(textBox4.Text))
            {
                SqlConnection cn = new SqlConnection(cadena);
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertProducto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombreProducto", nombreProducto.Text);
                    cmd.Parameters.AddWithValue("@precioUnitario", textBox3.Text);
                    cmd.Parameters.AddWithValue("@stock", textBox4.Text);
                    cmd.Parameters.AddWithValue("@idCategoria", cboCategoria.SelectedValue);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    LoadFormulario(cadena);
                    MessageBox.Show("Se registró al producto correctamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Debe completar todos campos.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.Show();
        }
    }
}
