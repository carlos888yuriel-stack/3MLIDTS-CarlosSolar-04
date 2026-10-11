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

namespace _3MLIDTS_CarlosSolar_04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Manejadores de eventos para validar mientras se escribe
            tbNombre.TextChanged += ValidarNombre;
            tbApellido.TextChanged += ValidarApellido;
            tbEdad.TextChanged += ValidarEdad;
            tbEstatura.TextChanged += ValidarEstatura;
            tbTelefono.Leave += ValidarTelefono;
        }

        // ---------- Funciones de validación (retornan true/false) ----------
        private bool EsTextoValido(string valor)
        {
            // letras, acentos, ñ y espacios
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

        // ---------- Manejadores de eventos ----------
        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textBoxLocal = (TextBox)sender;
            if (textBoxLocal.Text == "") return; // campo vacío: no validar aquí

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

        // ---------- Función 1: limpiar ----------
        private void LimpiarCampos()
        {
            tbNombre.Clear();
            tbApellido.Clear();
            tbEdad.Clear();
            tbTelefono.Clear();
            tbEstatura.Clear();
            tbTelefono.BackColor = SystemColors.Window; // quita el verde/rojo
            rbMasculino.Checked = false;
            rbFemenino.Checked = false;
            rbOtro.Checked = false;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // ---------- Función 2: guardar ----------
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string Nombre = tbNombre.Text.Trim();
            string Apellido = tbApellido.Text.Trim();
            string Edad = tbEdad.Text.Trim();
            string Telefono = tbTelefono.Text.Trim();
            string Estatura = tbEstatura.Text.Trim();

            // Campos vacíos
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Apellido) ||
                string.IsNullOrWhiteSpace(Edad) || string.IsNullOrWhiteSpace(Telefono) ||
                string.IsNullOrWhiteSpace(Estatura))
            {
                MessageBox.Show("Por favor completa todos los campos.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nombre y apellido
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

            // Edad
            if (!int.TryParse(Edad, out int edadNum) || edadNum <= 0)
            {
                MessageBox.Show("La edad debe ser un número entero mayor a 0.", "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Estatura
            if (!decimal.TryParse(Estatura, out decimal estaturaNum) || estaturaNum <= 0)
            {
                MessageBox.Show("La estatura debe ser un número mayor a 0.", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Teléfono
            if (!TelefonoValido(Telefono))
            {
                MessageBox.Show("El teléfono debe tener exactamente 10 dígitos.", "Error Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Género
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

            string FechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string Mensaje =
                $"Fecha: {FechaHora}\nNombre: {Nombre}\nApellido: {Apellido}\nEdad: {Edad}\nTelefono: {Telefono}\nEstatura: {Estatura}\nGenero: {Genero}";

            // Ruta de almacenamiento del TXT
            string rutaFile = "C:\\Users\\carlo\\CarlosxCarlos\\datos.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(rutaFile)); // crea la carpeta si no existe
            bool ArchivoExiste = File.Exists(rutaFile);

            using (StreamWriter Escritor = new StreamWriter(rutaFile, true))
            {
                if (ArchivoExiste)
                {
                    Escritor.WriteLine();
                }
                Escritor.WriteLine(Mensaje);
            }

            MessageBox.Show(Mensaje, "Registro guardado exitosamente", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarCampos();
        }
    }
}