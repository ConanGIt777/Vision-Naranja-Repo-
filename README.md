# 🟠 Visión Naranja

Catálogo web de emprendimientos locales - conecta a emprendedores con productos y servicios excepcionales con la comunidad. Proyecto desarrollado para la **Universidad Internacional SEK (UISEK)**.
---

## 📋 Descripción

Visión Naranja es una plataforma que permite a emprendedores locales publicar y gestionar sus productos y servicios, mientras los usuarios pueden explorar el catálogo, filtrar por categoría/ciudad/tipo de oferta, y contactar directamente por WhatsApp.

El proyecto está construido con el patrón **Modelo-Vista-Controlador (MVC)** usando **ASP.NET Core Razor Pages**, con un panel de administración protegido para la gestión completa de los datos.

## ✨ Funcionalidades

### Sitio público
- **Inicio** — hero con acceso rápido a productos/servicios y carrusel de "Noticias Destacadas"
- **Catálogo** — grilla de emprendimientos con filtros por categoría, ciudad y tipo de oferta (producto/servicio), más buscador de texto libre
- **Detalle del emprendimiento** — logo, descripción, galería de productos, información del emprendedor, credenciales/certificaciones, redes sociales y botón directo de contacto por WhatsApp

### Panel de administración (`/Admin`)
- Login protegido con contraseña hasheada (cookie de sesión)
- **CRUD de Emprendedores** — con subida de foto de perfil
- **CRUD de Emprendimientos** — con logo, datos de contacto/logística y gestión de redes sociales
- **CRUD de Productos y Servicios** — con subida múltiple de fotos, selección de portada y galería
- Cambio de contraseña de administrador

## 🛠️ Tecnologías

- **Backend:** ASP.NET Core (Razor Pages) — .NET 10
- **ORM:** Entity Framework Core
- **Base de datos:** SQL Server
- **Frontend:** HTML5, CSS3, Bootstrap 5, Bootstrap Icons
- **Autenticación:** Cookie Authentication + `PasswordHasher` de ASP.NET Core Identity

## 📁 Estructura del proyecto

```
VisionNaranja/
├── Data/                      # DbContext de Entity Framework
├── Models/                    # Entidades (Emprendedor, Emprendimiento, ProductosServicio, etc.)
│   └── ViewModels/             # ViewModels para vistas de detalle
├── Pages/
│   ├── Index.cshtml            # Home
│   ├── Catalogo/                # Catálogo y Detalle públicos
│   ├── Media/                   # Endpoints que sirven imágenes desde la BD
│   ├── Admin/                   # Panel administrativo (Login, CRUDs)
│   └── Shared/                  # Layouts compartidos
├── Services/
│   └── CatalogoService.cs      # Lógica de negocio del catálogo
└── wwwroot/                    # CSS, JS, íconos
```

## 🗄️ Modelo de datos

Entidades principales y sus relaciones:

- **Emprendedores** → tienen uno o más **Emprendimientos**
- **Emprendimientos** → pertenecen a una **Ciudad**, tienen **RedesSociales** y **ProductosServicios**
- **ProductosServicios** → pertenecen a una **Categoria** y tienen **MediaProducto** (imágenes)
- **AdminUsuarios** → credenciales del panel administrativo

Las fotos (perfil de emprendedor, logo de emprendimiento, fotos de producto) se almacenan como `varbinary` en la base de datos y se sirven mediante endpoints dedicados en `Pages/Media/`.

## 🚀 Cómo ejecutar el proyecto

### Requisitos previos
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server (local o Express)
- Visual Studio / VS Code

### Pasos

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/ConanGIt777/Vision-Naranja-Repo-.git
   cd Vision-Naranja-Repo-
   ```

2. **Configura la cadena de conexión**

   Crea (o edita) `appsettings.Development.json` con tu conexión local:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=TU_SERVIDOR;Database=VisionNaranja;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
     }
   }
   ```
   > Este archivo está en `.gitignore` y no se sube al repositorio — cada colaborador usa su propia configuración local.

3. **Restaura los paquetes y compila**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Ejecuta el proyecto**
   ```bash
   dotnet run
   ```
   La aplicación estará disponible en `http://localhost:5145` (o el puerto que indique la consola).

5. **Configura el administrador**

   La primera vez, visita `/Admin/Setup` para crear el usuario y contraseña del panel administrativo. Esta página se deshabilita automáticamente una vez creado el primer admin.

## 🔐 Panel de administración

- **Acceso:** `/Admin/Login`
- **Gestión disponible:** Emprendedores, Emprendimientos (con logo y redes sociales), Productos/Servicios (con galería de fotos)
- **Cambio de contraseña:** disponible dentro del panel, en el menú lateral

Si olvidas la contraseña de administrador, elimina el registro en la tabla `AdminUsuarios` desde SQL Server y vuelve a `/Admin/Setup` para crear uno nuevo.

## 🌿 Flujo de trabajo con Git

- `main` — rama estable
- Cambios en curso se desarrollan en ramas `feature/...` y se integran mediante Pull Request

```bash
git checkout -b feature/nombre-de-tu-cambio
git add .
git commit -m "Descripción del cambio"
git push -u origin feature/nombre-de-tu-cambio
```

## 👥 Créditos

Proyecto desarrollado por estudiantes Jaime Jiménez y Caleb Molina de la **Universidad Internacional SEK (UISEK)**.

## 📄 Licencia

Proyecto académico - Edición 2026.
