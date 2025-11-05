using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_2
{
    // Clase responsable de escribir cambios en archivos y gestionar alta/baja/edición.
    internal class Escritura
    {
        // Reescribe el archivo principal con el contenido actual de Declara.list_agenda.
        public static void func_actualizarArchivo()
        {
            File.WriteAllText(Declara.fileName, "");
            Declara.escritor = File.AppendText(Declara.fileName);

            foreach (Contacto contacto in Declara.list_agenda)
            {
                Declara.escritor.WriteLine(contacto.ToString());
            }

            Declara.escritor.Close();
            Declara.bool_cambiosSesion = false;
        }

        // Crea un nuevo contacto pidiendo datos por consola y lo añade a la lista.
        public static void func_cargarContacto()
        {

            Funciones.func_renderizarTitulo("Cargar Contacto");
            Contacto contacto_temp = new Contacto();
            Console.Write("Ingrese Apellido: ");
            contacto_temp.Apellido = Validar.func_validarString();
            Console.Write("Ingrese Nombre: ");
            contacto_temp.Nombre = Validar.func_validarString();
            Console.Write("Ingrese Teléfono: ");
            contacto_temp.Telefono = Validar.func_validarTelefono();
            Console.Write("Ingrese Email: ");
            contacto_temp.Email = Validar.func_validarEmail();
            Console.Write("Ingrese Dirección: ");
            contacto_temp.Direccion = Validar.func_validarDireccion();

            func_registrarLog("cargar", contacto_temp);
            Declara.list_agenda.Add(contacto_temp);
            Declara.bool_cambiosSesion = true;
        }

        // Elimina un contacto seleccionado por número de línea.
        public static void func_eliminarContacto()
        {

            Console.Clear();
            Funciones.func_renderizarTitulo("Eliminar Contacto");
            AnsiConsole.Markup("Ingrese el número de linea del contacto (o [blue]Escape[/] para volver): ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Declara.var_registro = Validar.func_validarNumeroDeLinea();

            Console.WriteLine();
            Console.ResetColor();

            if (Declara.var_registro.Trim() == "volver") return;

            Contacto contacto_temp = Declara.list_agenda[Convert.ToInt32((Declara.var_registro)) - 1];
            AnsiConsole.MarkupLine("[dodgerblue1]\nContacto: [/]");
            Funciones.func_renderizarContacto(contacto_temp);

            Declara.var_opcmnu = AnsiConsole.Prompt(
                                     new SelectionPrompt<string>()
                                         .Title("\n [red]→ [/]¿Seguro que desea eliminar el contacto?")
                                         .AddChoices(new[] {
                                                 "Si", "No"
                                     }));
            if (Declara.var_opcmnu == "Si")
            {
                Declara.list_agenda.RemoveAt(Convert.ToInt32(Declara.var_registro) - 1);
                func_registrarLog("eliminar", contacto_temp);
                Declara.bool_cambiosSesion = true;
                AnsiConsole.MarkupLine("\n[blue]→ [/]Contacto eliminado");
                Console.ReadKey();
            }


        }

        // Permite editar campos de un contacto existente; registra cambios en log.
        public static void func_editarContacto()
        {
            bool salir = false;
            Console.Clear();
            Funciones.func_renderizarTitulo("Editar Contacto");
            AnsiConsole.Markup("Ingrese el número de linea del contacto (o [blue]Escape[/] para volver): ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Declara.var_registro = Validar.func_validarNumeroDeLinea();
            Console.ResetColor();

            if (Declara.var_registro == "volver") return;

            // creo una referencia al contacto de la lista, y una copia para el log.
            Contacto contacto_actual = Declara.list_agenda[Convert.ToInt32((Declara.var_registro)) - 1];
            Contacto contacto_copia = Declara.list_agenda[Convert.ToInt32((Declara.var_registro)) - 1].Clonar();

            do
            {
                Console.Clear();
                Funciones.func_renderizarTitulo("Editar Contacto");
                AnsiConsole.MarkupLine("[dodgerblue1]Contacto: [/]");
                Funciones.func_renderizarContacto(contacto_actual);
                Console.WriteLine();
                Declara.var_opcmnu = AnsiConsole.Prompt(
                         new SelectionPrompt<string>()
                             .Title("Seleccione el campo que desea editar:")
                             .AddChoices(new[] {
                                                 "Apellido", "Nombre",
                                                 "Telefono", "Email",
                                                 "Dirección", "[grey35]Volver[/]"
                         }));

                switch (Declara.var_opcmnu)
                {
                    case "Apellido":
                        Console.Write("Ingrese nuevo Apellido: ");
                        contacto_actual.Apellido = Validar.func_validarString();
                        func_registrarLog("editar", contacto_actual, "apellido", contacto_copia);
                        break;
                    case "Nombre":
                        Console.Write("Ingrese nuevo Nombre: ");
                        contacto_actual.Nombre = Validar.func_validarString();
                        func_registrarLog("editar", contacto_actual, "nombre", contacto_copia);
                        break;
                    case "Telefono":
                        Console.Write("Ingrese nuevo Telefono: ");
                        contacto_actual.Telefono = Validar.func_validarTelefono();
                        func_registrarLog("editar", contacto_actual, "telefono", contacto_copia);
                        break;
                    case "Email":
                        Console.Write("Ingrese nuevo Email: ");
                        contacto_actual.Email = Validar.func_validarEmail();
                        func_registrarLog("editar", contacto_actual, "email", contacto_copia);
                        break;
                    case "Dirección":
                        Console.Write("Ingrese nueva Dirección: ");
                        contacto_actual.Direccion = Validar.func_validarDireccion();
                        func_registrarLog("editar", contacto_actual, "direccion", contacto_copia);
                        break;
                    case "[grey35]Volver[/]":
                        salir = true;
                        break;
                    default:
                        continue;
                }
                // actualizo el archivo acá porque sino no se actualiza hasta salir al menú princial.
                // Además permite ver a tiempo real los cambios en los campos del contacto. 
                func_actualizarArchivo();
            } while (!salir);
        }

        // Registra en el archivo de log la acción realizada sobre un contacto.
        public static void func_registrarLog(
            string accion, Contacto contacto,
            string campo = "", Contacto contacto_copia = null)
        {
            Declara.escritor = File.AppendText(Declara.fileName_log);

            string fecha = "[ " + DateTime.Now + " ]";
            string nombreFormateado = Funciones.func_formatearString(contacto.Nombre);
            string apellidoFormateado = Funciones.func_formatearString(contacto.Apellido);
            string campoFormateado = Funciones.func_formatearString(campo);

            if (accion == "cargar")
            {
                Declara.escritor.WriteLine(fecha + " - Contacto Agregado: " + nombreFormateado + " " + apellidoFormateado);
                Declara.escritor.WriteLine("\t→ " + contacto.ToString());
            }
            else if (accion == "eliminar")
            {
                Declara.escritor.WriteLine(fecha + " - Contacto Eliminado: " + nombreFormateado + " " + apellidoFormateado);
                Declara.escritor.WriteLine("\t← " + contacto.ToString());
            }
            else if (accion == "editar")
            {
                string valorAntiguo = "";
                string valorNuevo = "";

                switch (campo)
                {
                    case "nombre":
                        valorAntiguo = contacto_copia.Nombre;
                        valorNuevo = contacto.Nombre;
                        break;
                    case "apellido":
                        valorAntiguo = contacto_copia.Apellido;
                        valorNuevo = contacto.Apellido;
                        break;
                    case "telefono":
                        valorAntiguo = contacto_copia.Telefono;
                        valorNuevo = contacto.Telefono;
                        break;
                    case "email":
                        valorAntiguo = contacto_copia.Email;
                        valorNuevo = contacto.Email;
                        break;
                    case "direccion":
                        valorAntiguo = contacto_copia.Direccion;
                        valorNuevo = contacto.Direccion;
                        break;
                        // los valores de campo estan harcodeados, no deberia entrar al default. 
                    default:
                        valorAntiguo = "Desconocido";
                        valorNuevo = "Desconocido";
                        break;
                }


                // Escribo los cambios en el log, con los datos formateados. 
                Declara.escritor.WriteLine(fecha + " - Contacto Editado: " +
                    Funciones.func_formatearString(contacto_copia.Nombre) + " " +
                    Funciones.func_formatearString(contacto_copia.Apellido));
                Declara.escritor.WriteLine("- " + campoFormateado + " '" +
                    Funciones.func_formatearString(valorAntiguo) + "' cambia a '" +
                    Funciones.func_formatearString(valorNuevo) + "'");
                Declara.escritor.WriteLine("\t← " + contacto_copia.ToString());
                Declara.escritor.WriteLine("\t→ " + contacto.ToString());
            }

            // Dejo un espacio entre cambio y cambio.
            Declara.escritor.WriteLine();
            Declara.escritor.Close();
        }
    }
}
