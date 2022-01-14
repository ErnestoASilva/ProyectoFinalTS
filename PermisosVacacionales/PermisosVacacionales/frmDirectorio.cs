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

namespace PermisosVacacionales
{
    public partial class frmDirectorio : Form
    {
        public frmDirectorio()
        {
            InitializeComponent();
        }

        //static string cadena = "Server=tutuli;Database=SISTEMA_CRIT;User Id=sa;Password=Sistemas21;";
        static string cadena = "Data Source = DESKTOP-H64H067; Initial Catalog =SISTEMA_CRIT;Persist Security Info=True;User ID = Admin; Password = admin1";

        SqlConnection conexion = new SqlConnection(cadena);

        string tipoUsuario;

        private void frmDirectorio_Load(object sender, EventArgs e)
        {
            //SE INICIA CON EL ELEMENTO CERO DEL COMBOBOX (LA PRIMERA PERSONA EN ORDEN ALFABETICO)
            cbxTipoBusqueda.SelectedIndex = 0;

            try
            {
                //CREACIÓN Y EJECUCIÓN DE COMANDO SELECT PARA OBTENER TODOS LOS REGISTROS DE LA TABLA COLABORADORES
                conexion.Open();

                string consulta = "SELECT nombre AS 'Nombre', puesto AS 'Puesto', extension AS 'Extensión', correo AS 'Correo', oficina AS 'Oficina', area AS 'Área' FROM COLABORADORES ORDER BY nombre";
                var dataAdapter = new SqlDataAdapter(consulta, conexion);
                var commandBuilder = new SqlCommandBuilder(dataAdapter);

                var ds = new DataSet();
                dataAdapter.Fill(ds);
                dgvColaboradores.ReadOnly = true;
                dgvColaboradores.DataSource = ds.Tables[0];

                dgvColaboradores.BackgroundColor = Color.White;

                DataGridViewColumn column = dgvColaboradores.Columns[0];
                column.Width = 166;

                conexion.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo establecer la conexión con el servidor, favor de conectarse a la red local.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            tipoUsuario = VariablesGlobales.tipoUsuario;
            switch (tipoUsuario)
            {
                //ADMIN PUEDE ACCESAR TODOS LOS BOTONES
                case "admin":
                    break;
                //JEFE SOLO VA A PODER ACCESAR A LAS SOLICITUDES VACACIONALES, POR LO QUE SE DESHABILITA CAUS Y CHECADOR
                case "jefe":
                    inventarioToolStripMenuItem.Enabled = false;
                    agregarColaboradoresToolStripMenuItem.Enabled = false;
                    break;
                case "jefe_almacen":
                    inventarioToolStripMenuItem.Enabled = false;
                    agregarColaboradoresToolStripMenuItem.Enabled = false;
                    break;
                //LOS USUARIOS CON EL DERECHO DE VACACIONES SON LOS QUE PUEDEN ACCESAR A LA ACTUALIZACIÓN DE PERMISOS VACACIONALES
                // Y QUE PROCESAN LAS SOLICITUDES DE TODOS LOS COLABORADORES.
                case "vacaciones":
                    inventarioToolStripMenuItem.Enabled = false;
                    agregarColaboradoresToolStripMenuItem.Enabled = true;
                    agregarColaboradoresToolStripMenuItem.Text = "Administrar Vacaciones Colaboradores";
                    break;
                //ORIGINALMENTE ALMACEN SON LOS USUARIOS QUE PUEDEN ACCESAR A LA SECCIÓN DE 'ALMACEN', PERO YA NO EXISTE ESA SECCIÓN
                case "almacen":
                    inventarioToolStripMenuItem.Enabled = false;
                    agregarColaboradoresToolStripMenuItem.Enabled = false;
                    break;
                default:
                    //CUALQUIER OTRO USUARIO DETECTADO NO PUEDE ACCESAR A LAS SECCIONES MENCIONADAS ANTERIORMENTE
                    inventarioToolStripMenuItem.Enabled = false;
                    agregarColaboradoresToolStripMenuItem.Enabled = false;
                    aceptarDenegarSolicitudesToolStripMenuItem.Enabled = false;
                    //btnCaus.Enabled = false;
                    break;
            }


        }

        //SE USA EL EVENTO TextChanged PARA QUE SE BUSQUE AL COLABORADOR MIENTRAS ESCRIBIMOS
        private void tbxBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //CREACIÓN Y EJECUCIÓN DE LA CONSULTA PARA CONSEGUIR A LAS PERSONAS QUE SATISFACEN NUESTRO FILTRO
                conexion.Open();
                string consulta = "SELECT nombre, puesto, extension, correo, oficina, area FROM COLABORADORES WHERE " + cbxTipoBusqueda.Text.Trim() + " LIKE '%" + tbxBuscar.Text.Trim() + "%' ORDER BY nombre";
                var dataAdapter = new SqlDataAdapter(consulta, conexion);
                var commandBuilder = new SqlCommandBuilder(dataAdapter);

                var ds = new DataSet();
                dataAdapter.Fill(ds);
                dgvColaboradores.ReadOnly = true;
                dgvColaboradores.DataSource = ds.Tables[0];
                conexion.Close();
            }
            catch (Exception)
            {

            }
        }

        //ESTE EVENTO SE LLAMA CADA VEZ QUE LE DAMOS CLICK A CUALQUIER CELDA DEL DIAGRAMVIEW
        private void dgvColaboradores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //SE TOMA LA CELDA SELECCIONADA Y EXTRAEMOS TODOS LOS DATOS DE SU RENGLÓN PARA MOSTRARLOS EN LABELS
            tbxNombre.Text = dgvColaboradores.CurrentRow.Cells[0].Value.ToString();
            tbxPuesto.Text = dgvColaboradores.CurrentRow.Cells[1].Value.ToString();
            tbxExtension.Text = dgvColaboradores.CurrentRow.Cells[2].Value.ToString();
            tbxCorreo.Text = dgvColaboradores.CurrentRow.Cells[3].Value.ToString();
            tbxOficina.Text = dgvColaboradores.CurrentRow.Cells[4].Value.ToString();
            tbxArea.Text = dgvColaboradores.CurrentRow.Cells[5].Value.ToString();

            conexion.Open();
            try
            {
                //CREACIÓN Y EJECUCIÓN DE LA CONSULTA PARA OBTENER LA IMAGEN DEL COLABORADOR (EN CASO DE QUE TENGA UNA)
                SqlCommand comandoFoto = new SqlCommand("SELECT imagen FROM COLABORADORES WHERE nombre = '" + tbxNombre.Text + "'", conexion);
                byte[] img = (byte[])comandoFoto.ExecuteScalar();
                MemoryStream ms = new MemoryStream(img);
                pbxImagen.Image = Image.FromStream(ms);

            }
            catch (Exception)
            {
                //SI EL COLABORADOR NO TIENE IMAGEN SIMPLEMENTE NO SE MUESTRA NADA
                pbxImagen.Image = null;
            }
            conexion.Close();
        }

        private void pbxRegresar_Click(object sender, EventArgs e)
        {
            Form MenuPrincipal = new frmMenuPrincipal();
            MenuPrincipal.Show();
            this.Dispose();
        }

        private void frmDirectorio_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form MenuPrincipal = new frmMenuPrincipal();
            MenuPrincipal.Show();
            this.Dispose();
        }

        private void solicitarVacacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form MenuSolicitarVacaciones = new frmConsulta();
            MenuSolicitarVacaciones.Show();
            this.Dispose();
        }

        private void aceptarDenegarSolicitudesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form MenuAceptarDenegar = new frmAceptarDenegar();
            MenuAceptarDenegar.Show();
            this.Dispose();
        }

        private void agregarColaboradoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoUsuario == "vacaciones")
            {
                Form AltasyBajas = new frmActualizarVacaciones();
                AltasyBajas.Show();
                this.Dispose();

            }
            else if (tipoUsuario == "admin")
            {
                //PARA LOS ADMINISTRADORES, ESTE BOTON LLEVA AL FORMULARIO PARA ACTUALIZAR TODOS LOS DATOS DEL USUARIO, ASÍ COMO
                // AGREGAR NUEVOS O BORRAR EXISTENTES.
                Form AltasyBajas = new frmAgregarColaborador();
                AltasyBajas.Show();
                this.Dispose();
            }
        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form MenuInventario = new frmProveedores();
            MenuInventario.Show();
            this.Dispose();
        }
    }
}
