# Citas-api

## Antes de iniciar el proyecto

El proyecto requiere tener instalado .NET 9 SDK. Puedes descargarlo desde el sitio oficial de [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

Adicionalmente se necesitan ejecutar los siguientes comandos.

El proyecto utiliza JWT Bearer para la autenticación, por lo que es necesario ejecutar el siguiente script para generar una clave secreta para firmar los tokens:

```bash
dotnet run --project Genkey
```

El proyecto utiliza https para las comunicaciones, por lo que es necesario generar un certificado autofirmado para el entorno de desarrollo. Puedes hacerlo ejecutando el siguiente comando:
```bash
dotnet dev-certs https --trust
```

## Arquitectura

Monolito con Arquitectura Limpia con .NET 9 y C# 13

- **Domain**
- **Application**
- **Infrastructure**
- **API**