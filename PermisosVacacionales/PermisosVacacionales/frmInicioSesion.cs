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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace PermisosVacacionales
{
    public partial class frmInicioSesion : Form
    {
        public frmInicioSesion()
        {
            InitializeComponent();
        }

        /*
         * CADENA VIEJA PARA BASE DE DATOS INTURBIDE
        static string cadena = "Server=iturbide;Database=SISTEMA_CRIT;User Id=super;Password=super;";
        */

        //static string cadena = "Server=tutuli;Database=SISTEMA_CRIT;User Id=sa;Password=Sistemas21;";

        static string cadena = "Data Source = DESKTOP-H64H067; Initial Catalog =SISTEMA_CRIT;Persist Security Info=True;User ID = Admin; Password = admin1";

        SqlConnection conexion = new SqlConnection(cadena);

        string contrasenaEscrita;
        string contrasenaDevuelta;
        string correoDevuelto;
        string tipoUsuario;

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            //GUARDAR VALOR DE CORREO EN UNA VARIABLE GLOBAL
            VariablesGlobales.emailUsuario = tbxUsuario.Text.Trim();
            contrasenaEscrita = tbxPassword.Text.Trim();
            try
            {
                //SE HACE LA CONSULTA AL SERVIDOR SQL Y SE GUARDAN LOS DATOS DEVUELTOS EN VARIABLES 
                conexion.Open();
                string consulta = string.Format("SELECT correo, contrasena, tipo_usuario FROM COLABORADORES WHERE correo = " + "'" + VariablesGlobales.emailUsuario + "';");
                SqlCommand command = new SqlCommand(consulta, conexion);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    correoDevuelto = reader[0] as string;
                    contrasenaDevuelta = reader[1] as string;
                    tipoUsuario = reader[2] as string;
                }

                conexion.Close();
                //GUARDAR VALOR DE TIPO DE USUARIO EN UNA VARIABLE GLOBAL
                VariablesGlobales.tipoUsuario = tipoUsuario;

                //COMPROBAR SI LA CONTRASEÑA OBTENIDA COINCIDE CON LA ESCRITA
                if (contrasenaEscrita == contrasenaDevuelta)
                {

                    frmMenuPrincipal frmMenuPrincipal = new frmMenuPrincipal();
                    frmMenuPrincipal.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbxPassword.Clear();

                }
            }
            catch (Exception)
            {

                MessageBox.Show("Favor de conectarse a la red local.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            
            
        }


        private void frmInicioSesion_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

        }


        private void tbxUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            //INGRESAR AL HACER CLICK A LA TECLA ENTER
            if (e.KeyCode == Keys.Enter)
            {
                btnIngresar_Click((object) sender, (EventArgs) e);
            }
        }

        private void tbxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            //INGRESAR AL HACER CLICK A LA TECLA ENTER
            if (e.KeyCode == Keys.Enter)
            {
                btnIngresar_Click((object)sender, (EventArgs)e);
            }
        }

        private void frmInicioSesion_Load(object sender, EventArgs e)
        {

        }
    }
}
