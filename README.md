# [Reporting](https://huchim.com/)
[![Visual Studio Team services](https://img.shields.io/vso/build/huchim/c81ea820-fe3c-4afc-be8c-f54f70bfab24/6.svg)][aa] [![NuGet Pre Release](https://img.shields.io/nuget/v/Jaguar.Reporting.Html.svg?style=flat-square)][(https://www.nuget.org/packages/Jaguar.Reporting.Html)][nuget] [![NuGet Pre Release](https://img.shields.io/nuget/vpre/Jaguar.Reporting.Html.svg?style=flat-square)][(https://www.nuget.org/packages/Jaguar.Reporting.Html)]nuget]

[nuget]: https://www.nuget.org/packages/Jaguar.Reporting.Html

# Generador de reportes

El generador de reportes es una herramienta que permite [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) para mostrar los resultados de cualquier consulta. [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) es una sintaxis para crear plantillas.

La ventaja de [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) es que usted puede publicar su aplicación web y sin necesidad de recompilar puede modificar tanto el resultado de su reporta como también la consulta que la genera. De igual forma puede usar otros formatos como [Excel](https://github.com/huchim/reporting-excel), [Csv](https://github.com/huchim/reporting-csv), Json o cualquier otro formato que implemente [IGeneratorEngine](https://github.com/huchim/reporting/blob/master/src/IGeneratorEngine.cs).

## Instalación

Este generador es compatible (por el momento con .NET Framework 4.0, .NET Core 1.1 o superior) y se instala por medio de un paquete [nuget](https://www.nuget.org/packages/Jaguar.Reporting.Html).

```bash
PM > Install-Package Jaguar.Reporting.Html -Pre
```

El paquete depende tanto de [#Mustache](https://github.com/jehugaleahsa/mustache-sharp) (en la versión .NET Core usa una [versión modificada](https://github.com/huchim/mustache-sharp) que funciona en .NET Core) y de [Reporting](https://github.com/huchim/reporting) para generar el resultado.

