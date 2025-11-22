# 🔒 CONFIGURACIÓN DE SEGURIDAD

## ⚠️ IMPORTANTE ANTES DE EJECUTAR

Este proyecto requiere configuración local que **NO está incluida** en el repositorio por razones de seguridad.

---

## 📋 PASOS DE CONFIGURACIÓN

### 1️⃣ **Crear archivo de configuración local**

El proyecto incluye un archivo de ejemplo. Cópialo y personalízalo:

```bash
cp appsettings.Development.json.EXAMPLE appsettings.Development.json
```

### 2️⃣ **Editar `appsettings.Development.json`**

Abre el archivo y configura con TUS propias credenciales:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=lacafe_db;Username=TU_USUARIO;Password=TU_PASSWORD;Port=TU_PUERTO"
  },
  "Cloudinary": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  }
}
```

**Configura:**
- `TU_USUARIO` → Tu usuario de PostgreSQL (generalmente `postgres`)
- `TU_PASSWORD` → Tu contraseña de PostgreSQL
- `TU_PUERTO` → Tu puerto de PostgreSQL (usualmente `5432` o `5433`)

---

## 🗄️ CONFIGURAR POSTGRESQL

### **Instalación de PostgreSQL**

1. Descarga e instala PostgreSQL desde: https://www.postgresql.org/download/
2. Durante la instalación, establece una contraseña para el usuario `postgres`
3. Anota el puerto configurado (por defecto: 5432)

### **Crear la Base de Datos**

Abre el terminal de PostgreSQL (psql):

```sql
-- Conectarse a PostgreSQL
psql -U postgres

-- Crear la base de datos
CREATE DATABASE lacafe_db;

-- Salir
\q
```

### **Ejecutar Migraciones**

Desde el directorio del proyecto:

```bash
dotnet ef database update
```

---

## ☁️ CLOUDINARY (SOLO PARA PRODUCCIÓN)

**En desarrollo local NO es necesario configurar Cloudinary.** Las imágenes se guardan en `wwwroot/uploads/`

Para producción:
1. Crea cuenta gratuita en: https://cloudinary.com
2. Obtén tus credenciales desde el Dashboard
3. Configura las variables de entorno en tu servidor de producción

---

## 🚀 EJECUTAR LA APLICACIÓN

```bash
# 1. Restaurar dependencias
dotnet restore

# 2. Compilar el proyecto
dotnet build

# 3. Aplicar migraciones (crear tablas)
dotnet ef database update

# 4. Ejecutar la aplicación
dotnet run
```

Abre tu navegador en: **https://localhost:7174**

---

## 🔐 ARCHIVOS QUE **NUNCA** DEBES SUBIR A GITHUB

El archivo `.gitignore` ya está configurado para proteger:

- ❌ `appsettings.Development.json` (contiene TUS contraseñas)
- ❌ `appsettings.Production.json` (contiene credenciales de producción)
- ❌ Scripts con contraseñas (*.ps1)
- ❌ Base de datos local (*.db)
- ❌ Carpeta de uploads con imágenes de usuarios
- ❌ Archivos de configuración del IDE

---

## ✅ VERIFICAR ANTES DE HACER COMMIT

Antes de subir cambios a GitHub, verifica que no incluyas información sensible:

```bash
git status
```

**NO deberías ver archivos como:**
- `appsettings.Development.json`
- `appsettings.Production.json`
- Archivos `.ps1` con contraseñas
- Carpeta `wwwroot/uploads/` con contenido

Si ves alguno de estos archivos, **NO los subas**. Están en `.gitignore` por seguridad.

---

## 🚢 VARIABLES DE ENTORNO PARA PRODUCCIÓN

Para deployment en servidores (Render, Azure, AWS, etc.), configura estas variables de entorno:

```
ConnectionStrings__DefaultConnection=postgresql://usuario:password@host:puerto/database
Cloudinary__CloudName=tu_cloud_name
Cloudinary__ApiKey=tu_api_key
Cloudinary__ApiSecret=tu_api_secret
ASPNETCORE_ENVIRONMENT=Production
```

**Nunca incluyas estas credenciales en el código fuente.**

---

## 🔑 DATOS DE PRUEBA

Después de ejecutar las migraciones, puedes crear un usuario administrador de prueba manualmente a través de la interfaz de registro.

**Para ambiente de desarrollo:**
- Crea tus propios usuarios de prueba a través de `/Account/Registro`
- Define tus propias contraseñas seguras
- No uses contraseñas reales de producción

---

## 📞 PROBLEMAS COMUNES

### **No puedo conectarme a PostgreSQL**
1. Verifica que el servicio PostgreSQL esté corriendo
2. Confirma usuario, contraseña y puerto en `appsettings.Development.json`
3. Revisa el firewall

### **Error: Database does not exist**
```bash
# Crear la base de datos manualmente
psql -U postgres -c "CREATE DATABASE lacafe_db;"
```

### **Error en migraciones**
```bash
# Eliminar migraciones y recrear
dotnet ef database drop
dotnet ef database update
```

---

## 🎯 CHECKLIST DE SEGURIDAD

Antes de compartir tu código:

- [ ] Verificar que `.gitignore` excluye archivos sensibles
- [ ] No incluir contraseñas en el código
- [ ] No incluir credenciales de API en el repositorio
- [ ] No subir archivos de configuración con datos reales
- [ ] Usar variables de entorno en producción
- [ ] Revisar con `git status` antes de cada commit

---

## 📚 RECURSOS ADICIONALES

- [Documentación de .NET Core](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Buenas prácticas de seguridad](https://owasp.org/www-project-top-ten/)

---

**Recuerda: La seguridad comienza con buenas prácticas desde el desarrollo.** 🔒
