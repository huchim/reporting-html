# [Reporting](https://huchim.com/)
[![Visual Studio Team services](https://img.shields.io/vso/build/huchim/c81ea820-fe3c-4afc-be8c-f54f70bfab24/6.svg)]() [![NuGet Pre Release](https://img.shields.io/nuget/v/Jaguar.Reporting.Html.svg?style=flat-square)][nuget] [![NuGet Pre Release](https://img.shields.io/nuget/vpre/Jaguar.Reporting.Html.svg?style=flat-square)][nuget]

[nuget]: https://www.nuget.org/packages/Jaguar.Reporting.Html

El generador de reportes es una herramienta que permite [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) para mostrar los resultados de cualquier consulta. [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) es una sintaxis para crear plantillas.

La ventaja de [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) es que usted puede publicar su aplicación web y sin necesidad de recompilar puede modificar tanto el resultado de su reporta como también la consulta que la genera. De igual forma puede usar otros formatos como [Excel](https://github.com/huchim/reporting-excel), [Csv](https://github.com/huchim/reporting-csv), Json o cualquier otro formato que implemente [IGeneratorEngine](https://github.com/huchim/reporting/blob/master/src/IGeneratorEngine.cs).

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







## Ejemplos

Para usar Reporting debe primero generar un [archivo de configuración](ReportConfiguration) que incluya al menos 