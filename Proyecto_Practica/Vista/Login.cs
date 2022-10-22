using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Datos;

namespace Vista
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Aceptarbutton_Click(object sender, EventArgs e)
        {
            if (UsuarioTextBox.Text == String.Empty)
            {
                errorProvider1.SetError(UsuarioTextBox, "Ingrese un código de usuario");
                UsuarioTextBox.Focus();
                return;
            }
            errorProvider1.Clear();
            if (ClaveTextBox.Text == String.Empty)
            {
                errorProvider1.SetError(ClaveTextBox, "Ingrese una clave");
                ClaveTextBox.Focus();
                return;
            }
            errorProvider1.Clear();

            UsuarioDatos userDatos = new UsuarioDatos();//Lamar de la clase datos un usuario

            bool valido = await userDatos.LoginAsync(UsuarioTextBox.Text, ClaveTextBox.Text);

            if (valido)
            {
               Menu formulario = new Menu();
               Hide();
               formulario.Show();
            }
            else
            {
                MessageBox.Show("Datos de usuario incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cancelarbutton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
