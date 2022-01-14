using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PermisosVacacionales
{
    public partial class frmMenuSuper : Form
    {
        public frmMenuSuper()
        {
            InitializeComponent();
        }

        private void frmMenuSuper_Load(object sender, EventArgs e)
        {
           
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            frmConsulta frmConsulta = new frmConsulta();
            frmConsulta.Show();
            this.Dispose();
        }

        private void btnListaPedidos_Click(object sender, EventArgs e)
        {
            frmAceptarDenegar frmAceptarDenegar = new frmAceptarDenegar();
            frmAceptarDenegar.Show();
            this.Dispose();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            
;        }

        private void frmMenuSuper_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pbxRegresar_Click(object sender, EventArgs e)
        {
            Form MenuPrincipal = new frmMenuPrincipal();
            MenuPrincipal.Show();
            this.Dispose();
        }
    }
}
