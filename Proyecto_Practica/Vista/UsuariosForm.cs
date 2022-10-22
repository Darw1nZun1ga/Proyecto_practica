using Datos;
using Entidades;
using Microsoft.VisualBasic.ApplicationServices;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class UsuariosForm : Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }

        UsuarioDatos userDatos = new UsuarioDatos();//Instancia de objeto de la clase Datos 
        string tipoOperacion = string.Empty;//para almacenar el boton que el usuario presiono (nuevo o mofificar)
        Usuario user;//objeto de la clase usuario almacenara datos de boton guardar

        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            LlenarDataGrid();
        }

        private async void LlenarDataGrid()
        {
            UsuariosDataGridView.DataSource = await userDatos.DevolverListaAsync();//DataSourse obtiene todos los datos del objeto userDatos 
        }

        private void NuevoButton_Click_1(object sender, EventArgs e)
        {
            HabilitarControles();//LLama la funcion habilitar controles al presionar boton nuevo
            tipoOperacion = "Nuevo";//Al dar clic en nuevo se guaradar "Nuevo" en la variable tipoOperacion
        }

        private void HabilitarControles()//Nombre del texbox .Enabled=true para habilitar campo de escritura ya que estan en false
        {
            CodigoTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            ClaveTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            RolComboBox.Enabled = true;
            EstadoActivoCheckBox.Enabled = true;
        }

        private void CancelarButton_Click(object sender, EventArgs e)//Deshabilita los campos de escritura al presionar cancelar
        {
            DesabilitarControles();
            LimpiarControles();//LLamado de la funcion limpiar campos de texto al presionar cancelar
        }
        private void DesabilitarControles()//Funcion para Deshabilitar los campos de escritura
        {
            CodigoTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            ClaveTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            RolComboBox.Enabled = false;
            EstadoActivoCheckBox.Enabled = false;
        }


        private void LimpiarControles()//Funcion para limpiar campos de texto
        {
            CodigoTextBox.Clear();
            NombreTextBox.Text = String.Empty;
            ClaveTextBox.Text = "";
            CorreoTextBox.Clear();
            RolComboBox.Text = String.Empty;
            EstadoActivoCheckBox.Checked = false;
        }


        private void ModificarButton_Click_1(object sender, EventArgs e)
        {
            tipoOperacion = "Modificar";//si presiona boton modificar guarda "modificar en la variable tipoOperacion"
            if (UsuariosDataGridView.SelectedRows.Count > 0)//Valida si el usuario registro algo en mas de un registro y pasa los datos a los campos y modificar
            {
            //CurrentRow obtiene cada celda celeccionada para cada campo y cells para indicar celda
                CodigoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString();
                NombreTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                ClaveTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Clave"].Value.ToString();
                CorreoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                RolComboBox.Text = UsuariosDataGridView.CurrentRow.Cells["Rol"].Value.ToString();
                EstadoActivoCheckBox.Checked = Convert.ToBoolean(UsuariosDataGridView.CurrentRow.Cells["EstadoActivo"].Value);
                HabilitarControles();//Habilita los controles para poder modificarlos
                CodigoTextBox.ReadOnly = true;//para que no se pueda editar codigo y solo sea lectura
            }
            else//valida que el usuario seleccione un usuario o registro
            {
                MessageBox.Show("Debe seleccionar un registro", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void GuardarButton_Click_1(object sender, EventArgs e)
        {
            user = new Usuario();//para que valide todos los campos

            if (tipoOperacion == "Nuevo")//Dara opcion a crear nuevo usuario
            {
                if (CodigoTextBox.Text == "")
                {
                    errorProvider1.SetError(CodigoTextBox, "Ingrese un código");
                    CodigoTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese un nombre");
                    NombreTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(ClaveTextBox.Text))
                {
                    errorProvider1.SetError(ClaveTextBox, "Ingrese una clave");
                    ClaveTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(RolComboBox.Text))
                {
                    errorProvider1.SetError(RolComboBox, "Seleccione un rol");
                    RolComboBox.Focus();
                    return;
                }
                // Se le pasan los campos en las cajas de texto a user
                user.Codigo = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Clave = ClaveTextBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.EstadoActivo = EstadoActivoCheckBox.Checked;//obtiene valor true o false

                bool inserto = await userDatos.InsertarAsync(user);

                if (inserto)
                {
                    LlenarDataGrid();
                    LimpiarControles();
                    DesabilitarControles();
                    MessageBox.Show("Usuario Guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (tipoOperacion == "Modificar")
            {
                if (CodigoTextBox.Text == "")
                {
                    errorProvider1.SetError(CodigoTextBox, "Ingrese un código");
                    CodigoTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese un nombre");
                    NombreTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(ClaveTextBox.Text))
                {
                    errorProvider1.SetError(ClaveTextBox, "Ingrese una clave");
                    ClaveTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(RolComboBox.Text))
                {
                    errorProvider1.SetError(RolComboBox, "Seleccione un rol");
                    RolComboBox.Focus();
                    return;
                }
                user.Codigo = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Clave = ClaveTextBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.EstadoActivo = EstadoActivoCheckBox.Checked;

                bool modifico = await userDatos.ActualizarAsync(user);

                if (modifico)
                {
                    LlenarDataGrid();
                    LimpiarControles();
                    DesabilitarControles();
                    MessageBox.Show("Usuario Guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void EliminarButton_Click(object sender, EventArgs e)
        {
            if (UsuariosDataGridView.SelectedRows.Count > 0)
            {
                bool elimino = await userDatos.EliminarAsync(UsuariosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());
                if (elimino)
                {
                    LlenarDataGrid();
                    MessageBox.Show("Usuario Eliminado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo eliminar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
