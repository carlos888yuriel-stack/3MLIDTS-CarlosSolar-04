using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace _3MLIDTS_CarlosSolar_04
{
    public partial class Form1 : Form
    {
        // CAMBIO: la base de datos se llama formulario04 (sin guion)
        string conexionSQL = "Server=localhost;Port=3306;Database=formulario04;Uid=root;Pwd=46178661;";

        public Form1()
        {
            InitializeComponent();

            tbNombre.TextChanged += ValidarNombre;
            tbApellido.TextChanged += ValidarApellido;
            tbEdad.TextChanged += ValidarEdad;
            tbEstatura.TextChanged += ValidarEstatura;
            tbTelefono.Leave += ValidarTelefono;
        }

        private void InsertarRegistros(string Nombre, string Apellido, int Edad, decimal Estatura, string Telefono, string Genero)
        {
            using (MySqlConnection conection = new MySqlConnection(conexionSQL))
            {
                conection.Open();

                // CAMBIO: espacio antes de VALUES
                string insertQuery = "INSERT INTO registros (Nombre, Apellido, Edad, Estatura, Telefono, Genero) " +
                                     "VALUES (@Nombre, @Apellido, @Edad, @Estatura, @Telefono, @Genero)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@Nombre", Nombre);
                    command.Parameters.AddWithValue("@Apellido", Apellido);
                    command.Parameters.AddWithValue("@Edad", Edad);
                    command.Parameters.AddWithValue("@Estatura", Estatura);
                    command.Parameters.AddWithValue("@Telefono", Telefono);
                    command.Parameters.AddWithValue("@Genero", Genero);

                    command.ExecuteNonQuery();
                }
                conection.Close();
            }
        }

        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
        }

        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }

        private bool TelefonoValido(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textBoxLocal = (TextBox)sender;
            if (textBoxLocal.Text == "") return;

            if (!EsTextoValido(textBoxLocal.Text))
            {
                MessageBox.Show("El nombre solo puede contener letras.", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLocal.Clear();
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            TextBox textBoxLocal = (TextBox)sender;
            if (textBoxLocal.Text == "") return;

            if (!EsTextoValido(textBoxLocal.Text))
            {
                MessageBox.Show("El apellido solo puede contener letras.", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLocal.Clear();
            }
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textBoxLocal = (TextBox)sender;
            if (textBoxLocal.Text == "") return;

            if (!EsEnteroValido(textBoxLocal.Text))
            {
                MessageBox.Show("Escriba una edad válida (número entero).", "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLocal.Clear();
            }
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textBoxLocal = (TextBox)sender;
            if (textBoxLocal.Text == "") return;

            if (!EsDecimalValido(textBoxLocal.Text))
            {
                MessageBox.Show("Escriba una estatura válida.", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLocal.Clear();
            }
        }

        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textBoxLocal = (TextBox)sender;
            if (textBoxLocal.Text == "") return;

            if (!TelefonoValido(textBoxLocal.Text))
            {
                MessageBox.Show("El teléfono debe tener exactamente 10 dígitos.", "Error Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxLocal.BackColor = System.Drawing.Color.IndianRed;
                textBoxLocal.Clear();
            }
            else
            {
                textBoxLocal.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        private void LimpiarCampos()
        {
            tbNombre.Clear();
            tbApellido.Clear();
            tbEdad.Clear();
            tbTelefono.Clear();
            tbEstatura.Clear();
            tbTelefono.BackColor = SystemColors.Window;
            rbMasculino.Checked = false;
            rbFemenino.Checked = false;
            rbOtro.Checked = false;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string Nombre = tbNombre.Text.Trim();
            string Apellido = tbApellido.Text.Trim();
            string Edad = tbEdad.Text.Trim();
            string Telefono = tbTelefono.Text.Trim();
            string Estatura = tbEstatura.Text.Trim();

            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Apellido) ||
                string.IsNullOrWhiteSpace(Edad) || string.IsNullOrWhiteSpace(Telefono) ||
                string.IsNullOrWhiteSpace(Estatura))
            {
                MessageBox.Show("Por favor completa todos los campos.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!EsTextoValido(Nombre))
            {
                MessageBox.Show("El nombre solo puede contener letras.", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!EsTextoValido(Apellido))
            {
                MessageBox.Show("El apellido solo puede contener letras.", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(Edad, out int edadNum) || edadNum <= 0)
            {
                MessageBox.Show("La edad debe ser un número entero mayor a 0.", "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(Estatura, out decimal estaturaNum) || estaturaNum <= 0)
            {
                MessageBox.Show("La estatura debe ser un número mayor a 0.", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TelefonoValido(Telefono))
            {
                MessageBox.Show("El teléfono debe tener exactamente 10 dígitos.", "Error Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string Genero = "";
            if (rbMasculino.Checked)
            {
                Genero = "Masculino";
            }
            else if (rbFemenino.Checked)
            {
                Genero = "Femenino";
            }
            else if (rbOtro.Checked)
            {
                Genero = "Otro";
            }
            else
            {
                MessageBox.Show("Selecciona un género.", "Dato faltante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // NUEVO: guardar en la base de datos (si falla, avisa y no sigue)
            try
            {
                InsertarRegistros(Nombre, Apellido, edadNum, estaturaNum, Telefono, Genero);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar en la base de datos:\n" + ex.Message,
                                "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string FechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string Mensaje =
                $"Fecha: {FechaHora}\nNombre: {Nombre}\nApellido: {Apellido}\nEdad: {Edad}\nTelefono: {Telefono}\nEstatura: {Estatura}\nGenero: {Genero}";

            // Ruta de almacenamiento del TXT
            string rutaFile = "C:\\Users\\carlo\\CarlosxCarlos\\datos.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(rutaFile));
            bool ArchivoExiste = File.Exists(rutaFile);

            using (StreamWriter Escritor = new StreamWriter(rutaFile, true))
            {
                if (ArchivoExiste)
                {
                    Escritor.WriteLine();
                }
                Escritor.WriteLine(Mensaje);
            }

            // CAMBIO: el título ahora indica que también se guardó en la base de datos
            MessageBox.Show(Mensaje, "Registro guardado en la base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarCampos();
        }
    }
}