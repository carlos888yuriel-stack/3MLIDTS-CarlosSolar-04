using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3MLIDTS_CarlosSolar_04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            string Nombre = tbNombre.Text;
            string Apellido = tbApellido.Text;
            string Edad = tbEdad.Text;
            string Telefono = tbTelefono.Text;
            string Estatura = tbEstatura.Text;
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

            string Mensaje =


            $"Nombre: {Nombre}\nApellido: {Apellido}\nEdad: {Edad}\nTelefono: {Telefono}\nEstatura: {Estatura}\nGenero: {Genero}";
            MessageBox.Show(Mensaje, "Datos Guardados", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);


            //ruta de almacenamiento de archivos TXT
            string rutaFile = "C:\\Users\\carlo\\CarlosxCarlos\\datos.txt";
            //valor de verificacion de archivo existen
            bool ArchivoExiste = File.Exists(rutaFile);
            ///instancia de la clase objeto StreamWriter para escribir en el archivo txt
            using (StreamWriter Escritor = new StreamWriter(rutaFile, true)) {
                if (ArchivoExiste) { 
                    Escritor.WriteLine();
                }
                Escritor.WriteLine(Mensaje);
                
        }
                MessageBox.Show(Mensaje, "registros de usuarios", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

     }
  }

}



