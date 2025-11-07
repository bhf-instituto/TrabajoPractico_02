# Trabajo Práctico N° 2

**Autor:** Bauhoffer Mariano  
**Licencia:** MIT
**Materia:** Programación 1
**Profesor:** Pini Leandro
**Curso:** 1° C
**Año:** 2025

## Resumen
Aplicación de consola para gestionar una agenda de contactos. Interfaz enriquecida con Spectre.Console para menús y tablas. Los contactos se guardan en un archivo de texto y cada acción importante se registra en un log.

## Requisitos
- .NET 8
- C# 12
- Paquete NuGet: Spectre.Console (en el proyecto se usa la versión 0.53.0)

## Locación original de la solución
- **C:\Prog1\TP_2**

## Repositorio de GitHub

## Estructura principal
- **TP_2/Program.cs** – Punto de entrada y loop principal.  
- **TP_2/Declara.cs** – Variables y configuración global (rutas de archivos, arrays de dominios, lista de contactos).  
- **TP_2/Contacto.cs** – Modelo del contacto y utilidades (`ToString`, `Clonar`).  
- **TP_2/Validar.cs** – Validaciones interactivas (nombre, teléfono, email, dirección, número de línea).  
- **TP_2/Lectura.cs** – Carga y visualización de la agenda desde archivo.  
- **TP_2/Escritura.cs** – Alta, baja, edición y registro en log.  
- **TP_2/Funciones.cs** – Utilidades de UI (renderizado de título, tablas, menús, mensajes).  

## Cómo funciona (uso para usuarios finales)
1. Al iniciar, la aplicación carga la agenda desde el archivo especificado en `Declara.fileName`.  
2. El menú principal presenta las opciones:  
   - **Cargar Contacto:** solicita datos validados y agrega a la agenda.  
   - **Buscar Contactos:** buscar por Nombre, Apellido o Teléfono; muestra resultados en tabla.  
   - **Mostrar agenda completa:** ver lista completa (orden por creación, nombre o apellido).  
   - **Eliminar un contacto:** seleccionar por número de línea y confirmar eliminación.  
   - **Editar un contacto:** seleccionar por número de línea y editar campo por campo; los cambios se guardan en el archivo en tiempo real.  
   - **Salir:** cierra la aplicación.  
3. Datos persistentes:  
   - **Agenda:** archivo de texto (ruta por defecto en `Declara.fileName`). Cada línea: `Apellido;Nombre;Telefono;Email;Direccion`.  
   - **Log:** fichero (ruta en `Declara.fileName_log`) donde se registran acciones (alta, baja, edición) con fecha y detalle comparativo.  

## Validaciones relevantes
- **Nombre/Apellido:** mínimo 3 caracteres en alta; en búsqueda permite entradas más cortas. Máximo visible por campo ~15 caracteres.  
- **Teléfono:** debe comenzar con prefijo válido (11 o 2224 en la implementación) y tener 10 dígitos en total.  
- **Email:** formato `usuario@dominio`, la parte `usuario` debe tener al menos 6 caracteres y al menos una letra. Dominios permitidos: los listados en `Declara.arr_dominiosEmail`.  
- **Dirección:** debe contener calle + número; normaliza a minúsculas al guardar.  

## Spectre.Console: cómo y por qué se usa
La aplicación usa Spectre.Console para una experiencia de consola más rica:  
- **Menús interactivos:** `SelectionPrompt<T>` para seleccionar opciones.  
- **Tablas:** `Table` y `AnsiConsole.Write` para mostrar contactos con columnas y colores.  
- **Regla/encabezados:** `Rule` para títulos estilizados.  
- **Marcas y colores:** `AnsiConsole.MarkupLine` permite texto con color y estilos.  

**Beneficio:** interfaz más clara y navegación por teclado.  

> Si deseas modificar estilos, headers o colores, revisa `Funciones.func_renderizarTitulo`, `Funciones.func_renderizarTabla` y las llamadas a `AnsiConsole.Markup*` en el proyecto.

## Personalización rápida
- Cambiar ruta de archivos: editar `Declara.fileName` y `Declara.fileName_log`.  
- Añadir/quitar dominios de email: editar `Declara.arr_dominiosEmail`.  
- Cambiar campos mostrados: `Declara.arr_campos` y la lógica de renderizado en `Funciones`.  

## Errores comunes y soluciones
- **Archivos no encontrados:** asegúrate que la ruta en `Declara.fileName` exista y sea accesible por la cuenta que ejecuta la app.  
- **Encodings/formatos:** la app espera que cada línea tenga 5 campos separados por `;`. Corrupción dará excepciones en carga.  
- **Permisos:** si no se pueden crear/abrir `Log.txt` o `Agenda.txt`, prueba permisos de carpeta o usar una ruta dentro de tu usuario.  


## Soporte y contacto
**Autor:** Bauhoffer Mariano  

---

**Licencia MIT** – ver fichero LICENSE si se agrega al repositorio.
