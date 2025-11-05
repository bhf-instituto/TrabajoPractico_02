using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_2
{
    // Clase encargada de leer la agenda desde archivo y mostrar resultados.
    internal class Lectura
    {
        // Carga la agenda desde el archivo de texto a Declara.list_agenda.
        public static void func_cargarAgenda()
        {
            Declara.lector = new StreamReader(Declara.fileName);

            for (int linea = 1; ; linea++)
            {
                Contacto contacto = new Contacto();
                string lineaTexto = Declara.lector.ReadLine();
                if (lineaTexto == null) break;

                string[] campos = lineaTexto.Split(';');

                contacto.Apellido = campos[0];
                contacto.Nombre = campos[1];
                contacto.Telefono = campos[2];
                contacto.Email = campos[3];
                contacto.Direccion = campos[4];

                Declara.list_agenda.Add(contacto);
            }
            Declara.lector.Close();
        }

        // Muestra la agenda completa o filtra por campo/valor y muestra los resultados.
        public static void func_mostrarAgenda(string campo = "", string valor = "")
        {
            // Si los campos estan vacíos, no estamos buscando.
            bool buscar = !string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(valor);
            int[] cursorPos = { Console.CursorLeft, Console.CursorTop };


            if (!buscar)
            {
                Funciones.func_renderizarTabla(Declara.list_agenda);
            }
            else
            {
                List<Contacto> listaFiltrada = new List<Contacto>();
                bool hayResultados = false;

                foreach (Contacto contacto in Declara.list_agenda)
                {
                    bool coincidencia = false;

                    if (campo == "apellido" && contacto.Apellido.ToLower().Contains(valor.ToLower())) coincidencia = true;
                    else if (campo == "nombre" && contacto.Nombre.ToLower().Contains(valor.ToLower())) coincidencia = true;
                    else if (campo == "telefono" && contacto.Telefono.ToLower().Contains(valor.ToLower())) coincidencia = true;

                    if (coincidencia)
                    {
                        hayResultados = true;
                        listaFiltrada.Add(contacto);
                    }
                }
                if (hayResultados)
                {
                    Funciones.func_renderizarTabla(listaFiltrada);
                }
                else
                {
                    Funciones.func_mostrarMensajeError(cursorPos[0], cursorPos[1] - 1, "No hay resultados que coincidan con los criterios de busqueda.");
                }
            }
        }

        // Devuelve la agenda ordenada por el campo solicitado y la renderiza.
        public static void func_mostrarAgendaOrdenada(string campo)
        {
            int i = 0;
            List<Contacto> list_AgendaOrenada = new List<Contacto>();
            List<string> datosContacto = new List<string>();

            if (campo == "nombre")
            {
                foreach (Contacto contacto in Declara.list_agenda)
                {
                    datosContacto.Add(
                        contacto.Nombre +
                        contacto.Apellido + ";" +
                        i
                        );
                    i++;
                }
            }
            else if (campo == "apellido")
            {
                foreach (Contacto contacto in Declara.list_agenda)
                {
                    datosContacto.Add(
                        contacto.Apellido +
                        contacto.Nombre + ";" +
                        i
                        );
                    i++;
                }
            }
            else
            {
                // El parametro está hardcodeado. No deberia dar error. 
                Console.WriteLine("Error de parametro en la función mostrarAgendaOrdenada");
                Console.ReadKey();
                return;
            }

            datosContacto.Sort();

            foreach (string contactoString in datosContacto)
            {
                int indiceOriginal = Convert.ToInt32(contactoString.Split(";")[1]);
                list_AgendaOrenada.Add(Declara.list_agenda[indiceOriginal]);
            }

            Funciones.func_renderizarTabla(list_AgendaOrenada, false);
        }
    }
}
