using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace _3MLIDTS_CarlosSolar_04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // MEJORA IA: los campos se pintan de verde/rojo mientras se escribe,
            // ya no sale una ventana de error con cada tecla.
            tbNombre.TextChanged += ValidarNombre;
            tbApellido.TextChanged += ValidarApellido;
            tbEdad.TextChanged += ValidarEdad;
            tbEstatura.TextChanged += ValidarEstatura;
            tbTelefono.TextChanged += ValidarTelefono;
        }

        // ---------- Funciones de validación (retornan true/false) ----------
        private bool EsTextoValido(string valor)
        {
            // letras, acentos, ñ y espacios (y que no sea solo espacios)
            return valor.Trim() != "" && Regex.IsMatch(valor, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$");
        }

        private bool EsEnteroValido(string valor)
        {
            // solo dígitos (no acepta signos ni espacios)
            int resultado;
            return int.TryParse(valor, NumberStyles.None, CultureInfo.InvariantCulture, out resultado);
        }

        private bool EsDecimalValido(string valor)
        {
            // acepta 1.70 o 1,70
            decimal resultado;
            return decimal.TryParse(valor.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out resultado);
        }

        private bool TelefonoValido(string valor)
        {
            // exactamente 10 dígitos
            return valor.Length == 10 && valor.All(c => c >= '0' && c <= '9');
        }

        // MEJORA IA: rangos realistas
        private bool EsEdadValida(string valor)
        {
            if (!EsEnteroValido(valor)) return false;
            int edad = int.Parse(valor, CultureInfo.InvariantCulture);
            return edad >= 1 && edad <= 120;
        }

        private bool EsEstaturaValida(string valor)
        {
            if (!EsDecimalValido(valor)) return false;
            decimal estatura = decimal.Parse(valor.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            return estatura >= 0.5m && estatura <= 2.5m;
        }

        // ---------- Retroalimentación visual ----------
        private void PintarCampo(TextBox tb, bool valido)
        {
            if (tb.Text == "")
                tb.BackColor = SystemColors.Window;   // vacío: color normal
            else if (valido)
                tb.BackColor = Color.LightGreen;      // correcto
            else
                tb.BackColor = Color.LightPink;       // incorrecto
        }

        // ---------- Manejadores de eventos ----------
        private void ValidarNombre(object sender, EventArgs e)
        {
            PintarCampo(tbNombre, EsTextoValido(tbNombre.Text));
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            PintarCampo(tbApellido, EsTextoValido(tbApellido.Text));
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            PintarCampo(tbEdad, EsEdadValida(tbEdad.Text));
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            PintarCampo(tbEstatura, EsEstaturaValida(tbEstatura.Text));
        }

        private void ValidarTelefono(object sender, EventArgs e)
        {
            PintarCampo(tbTelefono, TelefonoValido(tbTelefono.Text));
        }

        // ---------- Función 1: limpiar ----------
        private void LimpiarCampos()
        {
            // Clear() deja el campo vacío y PintarCampo restablece el color normal
            tbNombre.Clear();
            tbApellido.Clear();
            tbEdad.Clear();
            tbTelefono.Clear();
            tbEstatura.Clear();
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

            // MEJORA IA: se juntan TODOS los errores en un solo mensaje
            string errores = "";

            if (Nombre == "")
                errores += "- Falta capturar el nombre.\n";
            else if (!EsTextoValido(Nombre))
                errores += "- El nombre solo puede contener letras.\n";

            if (Apellido == "")
                errores += "- Falta capturar el apellido.\n";
            else if (!EsTextoValido(Apellido))
                errores += "- El apellido solo puede contener letras.\n";

            if (Edad == "")
                errores += "- Falta capturar la edad.\n";
            else if (!EsEdadValida(Edad))
                errores += "- La edad debe ser un número entero entre 1 y 120.\n";

            if (Estatura == "")
                errores += "- Falta capturar la estatura.\n";
            else if (!EsEstaturaValida(Estatura))
                errores += "- La estatura debe estar entre 0.50 y 2.50 m.\n";

            if (Telefono == "")
                errores += "- Falta capturar el teléfono.\n";
            else if (!TelefonoValido(Telefono))
                errores += "- El teléfono debe tener exactamente 10 dígitos.\n";

            if (Genero == "")
                errores += "- Selecciona un género.\n";

            if (errores != "")
            {
                MessageBox.Show("Corrige lo siguiente:\n\n" + errores, "Errores de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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