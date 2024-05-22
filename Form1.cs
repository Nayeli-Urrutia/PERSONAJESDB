using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wfdb.Data.DataAccess;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Wfdb
{
    public partial class Form1 : Form
    {

        // Lista de razas
        private string[] razasDragonBall = {
            "Android",
            "Bio-Android",
            "Humana",
            "Humano",
            "Majin",
            "Namekuseijin",
            "Saiyajin",
            "Saiyajin/Humano",
            "Saiyajin/Saiyajin"
        };


        private PersonajeDB personaje;
  


        public Form1()
        {
            InitializeComponent();
            personaje = new PersonajeDB("localhost", "root", "");

            

        }




        private void buttonCargaData_Click(object sender, EventArgs e)
        {
            dataGridViewPersonajes.DataSource = personaje.LeerPersonajes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Llenar el ComboBox con las razas
            comboBoxRaza.Items.AddRange(razasDragonBall);
        }

        private void buttonInsertar_Click(object sender, EventArgs e)
        {
            string nombre = textBoxNombre.Text;
            //string raza = comboBoxRaza.Text;
            string raza = textBoxRaza.Text;
            int nivelPoder = (int)numericUpDownNivelPoder.Value;
            DateTime fecha_creacion = DateTime.Now;


            string historia = textBoxHistoria.Text;


            int respuesta = personaje.CrearPersonaje(nombre, raza, nivelPoder, fecha_creacion, historia);
            if (respuesta > 0)
            {
                MessageBox.Show("Personaje creado correctamente");
                dataGridViewPersonajes.DataSource = personaje.LeerPersonajes();
            }
            else
            {
                MessageBox.Show("Error al crear el personaje");
            }
        }


        private void buscarPorId()
        {
            int idPersonajeABuscar = int.Parse(textBoxID.Text);

            DataTable personajeEncontrado = personaje.BuscarPersonajePorId(idPersonajeABuscar);

            if (personajeEncontrado.Rows.Count > 0)
            {
                // El personaje fue encontrado
                string nombre = personajeEncontrado.Rows[0]["nombre"].ToString();
                string raza = personajeEncontrado.Rows[0]["raza"].ToString();
                int nivelPoder = int.Parse(personajeEncontrado.Rows[0]["nivel_poder"].ToString());
                textBoxNombre.Text = nombre;
                textBoxRaza.Text = raza;
                comboBoxRaza.Text = raza;
                numericUpDownNivelPoder.Value = nivelPoder;
            }
            else
            {
                // El personaje no fue encontrado
                Console.WriteLine("No se encontró el personaje con ID: " + idPersonajeABuscar);
            }
        }


        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            buscarPorId();
        }

        private void textBoxID_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxID.Text))
            {
                MessageBox.Show("Por favor, ingresa un valor en el campo de texto.");
                textBoxID.Focus(); // Devolver el foco al TextBox
            }
            else
            {
                buscarPorId();
            }
        }

        private void buttonTestCon_Click(object sender, EventArgs e)
        {
            if (personaje.ProbarConexion())
            {
                MessageBox.Show("Conexión exitosa");
            }
            else
            {
                MessageBox.Show("Error en la conexión");
            }
        }

        //ordenar fecha

        private PersonajeDB ordenar;

        
        private void buscarPorFecha()
        {
            // Obtener todos los personajes de la base de datos
            DataTable todosLosPersonajes = personaje.LeerPersonajes();

            // Obtener la fecha de creación seleccionada
            DateTime fecha_creacion = dateTimePickerfecha.Value;

            // Ordenar los personajes por fecha de creación
            DataView dv = todosLosPersonajes.DefaultView;
            dv.Sort = "fecha_creacion DESC"; // Cambia ASC por DESC si deseas ordenar en orden descendente
            DataTable personajesOrdenados = dv.ToTable();

            // Mostrar los personajes ordenados en el DataGridView
            dataGridViewPersonajes.DataSource = personajesOrdenados;


        }

       
        private void buttonfecha_creacion_Click(object sender, EventArgs e)
        {
            buscarPorFecha();
        }

        //Fecha reinte

        

        
            private void buscarPorFechareciente()
            {
                DateTime fechaFin = DateTime.Now;
                DateTime fechaInicio = fechaFin.AddDays(-5); // Cambia el número de días según tu definición de "recientemente"

                DataTable personajesRecientesOrdenados = personaje.ObtenerPersonajesRecientes(fechaInicio, fechaFin);

                dataGridViewPersonajes.DataSource = personajesRecientesOrdenados;
            }
        

        private void buttonreciente_Click(object sender, EventArgs e)
        {
            buscarPorFechareciente();
        }
    }
}