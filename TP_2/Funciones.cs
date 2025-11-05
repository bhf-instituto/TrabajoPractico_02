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
    // Clase utilitaria para funciones de interfaz y renderizado en consola.
    internal class Funciones
    {
        // Ajusta tamaño de ventana, buffer y título; carga la agenda desde archivo.
        public static void ConfigInicial()
        {
            Console.SetWindowSize(140, 40);
            Console.SetBufferSize(140, 100);
            Console.Title = "Agenda de contactos";
            Lectura.func_cargarAgenda();
        }

        // Renderiza el título de una sección usando Spectre.Console.
        public static void func_renderizarTitulo(string texto)
        {
            Console.Clear();
            var rule = new Rule("[white]" + texto + "[/]");
            rule.RuleStyle("dodgerblue2");
            rule.LeftJustified();
            AnsiConsole.Write(rule);
            Console.WriteLine();
        }

        // Muestra el menú principal y guarda la opción seleccionada en Declara.var_opcmnu.
        public static void func_menu()
        {
            Console.Clear();
            func_renderizarTitulo("Menú Principal");

            Declara.var_opcmnu = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .AddChoices(new[] {
                                            "Cargar Contacto",
                                            "Buscar Contactos",
                                            "Mostrar agenda completa",
                                            "Eliminar un contacto",
                                            "Editar un contacto",
                                            "[grey35]Salir[/]"
                                }));
        }

        // Muestra el menú de búsqueda y ejecuta la búsqueda seleccionada.
        public static void func_menuBuscarContacto()
        {
            do
            {
                Console.Clear();
                func_renderizarTitulo("Buscar Contacto");
                Declara.var_opcmnu = AnsiConsole.Prompt(
                                        new SelectionPrompt<string>()
                                            .Title("Buscar por:")
                                            .AddChoices(new[] {
                                                        "Nombre", "Apellido",
                                                        "Telefono", "[grey35]Volver[/]"
                                    }));

                switch (Declara.var_opcmnu)
                {
                    case "Nombre":
                        Console.Write("Ingrese nombre: ");
                        Declara.var_registro = Validar.func_validarString(true);
                        Lectura.func_mostrarAgenda("nombre", Declara.var_registro);
                        func_continuar();
                        break;
                    case "Apellido":
                        Console.Write("Ingrese apellido: ");
                        Declara.var_registro = Validar.func_validarString(true);
                        Lectura.func_mostrarAgenda("apellido", Declara.var_registro);
                        func_continuar();
                        break;
                    case "Telefono":
                        Console.Write("Ingrese telefono: ");
                        Declara.var_registro = Validar.func_validarTelefono(true);
                        Lectura.func_mostrarAgenda("telefono", Declara.var_registro);
                        func_continuar();
                        break;
                    case "[grey35]Volver[/]":
                        break;
                    default:
                        continue;
                }
                break;
            } while (true);
        }

        // Muestra opciones para ver la agenda (orden por creación, nombre o apellido).
        public static void func_menuMostrarAgenda()
        {
            if (Declara.list_agenda.Count == 0)
            {
                func_mostrarMensajeError(0, 1, "La lista está vacía");
                func_continuar();
                return;
            }
            do
            {
                Console.Clear();
                func_renderizarTitulo("Mostrar Agenda");
                Declara.var_opcmnu = AnsiConsole.Prompt(
                                         new SelectionPrompt<string>()
                                             .Title("Mostrar por orden de:")
                                             .AddChoices(new[] {
                                                 "Creación", "Nombre",
                                                 "Apellido", "[grey35]Volver[/]"
                                         }));

                switch (Declara.var_opcmnu)
                {
                    case "Creación":
                        Lectura.func_mostrarAgenda();
                        func_continuar();
                        break;
                    case "Nombre":
                        Lectura.func_mostrarAgendaOrdenada("nombre");
                        func_continuar();
                        break;
                    case "Apellido":
                        Lectura.func_mostrarAgendaOrdenada("apellido");
                        func_continuar();
                        break;
                    case "[grey35]Volver[/]":
                        break;
                    default:
                        continue;
                }
                break;

            } while (true);
        }

        // Renderiza un único contacto en una tabla pequeña.
        public static void func_renderizarContacto(Contacto contacto)
        {
            string apellidoFormateado = func_formatearString(contacto.Apellido);
            string nombreFormateado = func_formatearString(contacto.Nombre);
            string direccionFormateada = func_formatearString(contacto.Direccion);

            Table tabla = new Table();
            tabla.AddColumns(
                apellidoFormateado,
                nombreFormateado,
                contacto.Telefono,
                contacto.Email,
                direccionFormateada);
            tabla.BorderColor(Color.Red);
            AnsiConsole.Write(tabla);
        }

        // Renderiza una lista de contactos en forma de tabla.
        // Si linea = true añade columna índice al inicio, es decir 
        // el número de linea del contacto.
        public static void func_renderizarTabla
            (List<Contacto> listaContactos, bool linea = true)
        {
            Table tabla = new Table();

            if (linea)
            {
                tabla.AddColumn("");

            }
            foreach (string campo in Declara.arr_campos)
            {
                tabla.AddColumn("[skyblue2]" + campo + "[/]");
            }
            int i = 1;
            if (linea)
            {
                foreach (Contacto contacto in listaContactos)
                {
                    tabla.AddRow(
                        "[indianred1]" + i + "[/]",
                        "[grey62]" + func_formatearString(contacto.Apellido) + "[/]",
                        "[grey62]" + func_formatearString(contacto.Nombre) + "[/]",
                        "[grey62]" + contacto.Telefono + "[/]",
                        "[grey62]" + contacto.Email + "[/]",
                        "[grey62]" + func_formatearString(contacto.Direccion) + "[/]"
                        );
                    i++;
                }
                tabla.Columns[0].Alignment(Justify.Right);
            }
            else
            {
                foreach (Contacto contacto in listaContactos)
                {
                    tabla.AddRow(
                        "[grey62]" + contacto.Apellido + "[/]",
                        "[grey62]" + contacto.Nombre + "[/]",
                        "[grey62]" + contacto.Telefono + "[/]",
                        "[grey62]" + contacto.Email + "[/]",
                        "[grey62]" + contacto.Direccion + "[/]"
                        );
                    i++;
                }
            }
            tabla.Centered();
            tabla.Border(TableBorder.Rounded);
            tabla.BorderColor(Color.DeepSkyBlue4_1);

            AnsiConsole.Write(tabla);
        }

        // Muestra un mensaje de error en posición especificada (x,y).
        public static void func_mostrarMensajeError(int x, int y, string errorMsj)
        {
            int anchoLinea = Console.BufferWidth - x;
            Console.SetCursorPosition(0, y + 1);
            AnsiConsole.MarkupLine("[grey] → [/][red]" + errorMsj + "[/]");
            Console.SetCursorPosition(x, y);
            Console.Write(new string(' ', anchoLinea));
            Console.SetCursorPosition(x, y);
        }

        // Borra la línea y+1 en la consola.
        public static void func_borrarLinea(int y)
        {
            Console.SetCursorPosition(0, y + 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, y + 1);
        }

        // Formatea cadenas: capitaliza la primera letra de cada palabra.
        public static string func_formatearString(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            else if (texto.Contains(' '))
            {
                string resultado = "";
                string[] partes = texto.Split(' ');

                foreach (string parte in partes)
                {
                    resultado += char.ToUpper(parte[0]) + parte.Substring(1).ToLower() + " ";
                }
                return resultado;
            }
            else
            {
                texto = texto.ToLower();
                return char.ToUpper(texto[0]) + texto.Substring(1).Trim();

            }
        }

        // Pausa y espera que el usuario presione una tecla.
        public static void func_continuar()
        {
            Console.WriteLine("");
            AnsiConsole.MarkupLine("\n[grey] → [/][white]Presione una tecla para continuar...[/]");
            Console.SetCursorPosition(0, 0);
            Console.ReadKey();
        }
    }
}
