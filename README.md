# [Reporting](https://huchim.com/)
[![Visual Studio Team services](https://img.shields.io/vso/build/huchim/c81ea820-fe3c-4afc-be8c-f54f70bfab24/6.svg)]() [![NuGet Pre Release](https://img.shields.io/nuget/v/Jaguar.Reporting.Html.svg?style=flat-square)][nuget] [![NuGet Pre Release](https://img.shields.io/nuget/vpre/Jaguar.Reporting.Html.svg?style=flat-square)][nuget]

[nuget]: https://www.nuget.org/packages/Jaguar.Reporting.Html

El generador de reportes es una herramienta que permite [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) para mostrar los resultados de cualquier consulta. [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) es una sintaxis para crear plantillas.

La ventaja de [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) es que usted puede publicar su aplicación web y sin necesidad de recompilar, puede modificar tanto el resultado de su reporte como también la consulta que la genera. De igual forma puede usar otros formatos como [Excel](https://github.com/huchim/reporting-excel), [Csv](https://github.com/huchim/reporting-csv), Json o cualquier otro formato que implemente [IGeneratorEngine](https://github.com/huchim/reporting/blob/master/src/IGeneratorEngine.cs).

## Ventajas

El paquete de reportes ayuda a actualizar de manera dinámica tanto los datos del reporte, como la presentación del mismo.

- Soporte para distintos frameworks
- Al soportar .NET Core puede funcionar en diferentes sistemas operativos.
- La consulta SQL puede ser actualizada dinámicamente.
- Los parámetros de la consulta no se añaden directamente al SQL si no que se usa "[sp_executesql](https://stackoverflow.com/questions/4892166/how-does-sqlparameter-prevent-sql-injection)" (o PREPARE, todo depende del gestor)
- Es compatible con cualquier conexión que implemente [IDbConnection](https://msdn.microsoft.com/en-us/library/system.data.idbconnection(v=vs.110).aspx)  como [MySqlConnection](https://dev.mysql.com/doc/connector-net/en/connector-net-ref-mysqlclient-mysqlconnection.html) o [SqlConnection](https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection(v=vs.110).aspx).



## Instalación

Este generador es compatible (por el momento con .NET Framework 4.0, .NET Core 1.1 o superior) y se instala por medio de un paquete [nuget](https://www.nuget.org/packages/Jaguar.Reporting.Html).

```bash
PM > Install-Package Jaguar.Reporting.Html -Pre
```

El paquete depende tanto de [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) (en la versión .NET Core usa una [versión modificada](https://github.com/huchim/mustache-sharp) que funciona en .NET Core) y de [Reporting](https://github.com/huchim/reporting) para generar el resultado.



## Recomendaciones

Trabajar con los reportes, mantenerlos actualizados y organizados puede ser sencillo si se usa el [administrador de reportes](ReportManager) que permite leer la información de todos los reportes contenidos en un repositorio o carpeta.

```
src/
| -- myproject.csproj
| -- Reports
     | -- Report01
     	  | -- report.json
     	  | -- template.html
     | -- Report02
     | -- Report03
     | -- Report04  
```

La estructura anterior permite organizar cada reporte de tal manera que toda la información del reporte ([archivo de configuración](ReportConfiguration), rutinas sql, etc) se mantenga separados de los demás reportes.

## Cargar la configuración del reporte

Puede crear una instancia que le ayudará a recuperar la información de los reportes.

```csharp
// Recupero la ruta al directorio donde se encuentran los reportes.
var carpetaReportes = Server.MapPath("~/Reports/");

// Creo una instancia para administrar los reportes.
var administradorReportes = new ReportRepository(carpetaReportes);
```

En el ejemplo anterior hay un reporte dentro de la carpeta `Reports/Report01.json`(leer más sobre el [archivo de configuración](ReportConfiguration)) dicho reporte tendría una configuración parecida a esta.

```json
{
  "$schema": "https://raw.githubusercontent.com/huchim/schemas/master/Reports/reports.schema.json",
  "name": "NombreReporte1",
  "label": "Ejemplo de reporte",
  "icon": "list",
  "html.template": "template.html",
  "sql": [
    {
      "name": "data",
      "script": "SELECT Nombre, Apellido FROM Alumnos WHERE Maestro = @MaestroId",
      "required": [ "MaestroId" ]
    }
  ],
  "args": [
    {
      "description": "Identificador del maestro",
      "label": "Maestro",
      "name": "MaestroId",
      "type": "number"
    }
  ]
}
```

Para poder utilizar el reporte dentro de nuestro código, debemos recuperarlo del repositorio.

```csharp
var configuracionReporte = administradorReportes.Reports.Single(x => x.Name == "NombreReporte1");
```

Toda la información del reporte ahora está disponible dentro de `configuracionReporte` y podemos trabajar con el. Nota: Utilizamos `Single` porque si no existe, generará un error, ya que el comportamiento esperado es que si exista.

## Consulta SQL

Un reporte puede tener varias consultas SQL. Cada consulta debe estar dentro de la colección `sql`del archivo de configuración.

```json
{
  "$schema": "https://raw.githubusercontent.com/huchim/schemas/master/Reports/reports.schema.json",
  "name": "NombreReporte1",
  "sql": [
    {
      "name": "data",
      "script": "SELECT Nombre, Apellido FROM Alumnos WHERE Maestro = @MaestroId",
      "required": [ "MaestroId" ]
    }
  ]
}
```

El nombre que le hemos asignado a esa consulta SQL es `data` y ella extrae una lista de alumnos, cuyo maestro es variable (`MaestroId`) .

[Reporting](https://github.com/huchim/reporting) se encargará de completar la consulta, pero antes debemos indicarle el tipo de datos de esa variable.

```json
{
  "$schema": "https://raw.githubusercontent.com/huchim/schemas/master/Reports/reports.schema.json",
  "name": "NombreReporte1",  
  "args": [
    {
      "description": "Identificador del maestro",
      "label": "Maestro",
      "name": "MaestroId",
      "type": "number"
    }
  ]
}
```

De esta manera durante la ejecución de la consulta, Reporting intentará asignar el tipo de valor correcto.

**Nota:** Si la consulta es muy larga, puede moverla a un archivo (ejemplo: scripts.sql) y utilizar la propiedad `file`en vez de `script`. El archivo es relativo al directorio del archivo `report.json`



## Ejemplos

Para usar Reporting debe primero generar un [archivo de configuración](ReportConfiguration) que incluya al menos 