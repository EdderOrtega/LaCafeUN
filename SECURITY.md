# 🔒 CONFIGURACIÓN DE SEGURIDAD

## ⚠️ IMPORTANTE ANTES DE EJECUTAR

Este proyecto requiere configuración local que **NO está incluida** en el repositorio por razones de seguridad.

---

## 📋 PASOS DE CONFIGURACIÓN

### 1️⃣ **Crear archivo de configuración local**

Copia el archivo de ejemplo:

```bash
cp appsettings.Development.json.EXAMPLE appsettings.Development.json
```

### 2️⃣ **Editar `appsettings.Development.json`**

Abre el archivo y configura tus credenciales:

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
    "DefaultConnection": "Host=localhost;Database=lacafe_db;Username=postgres;Password=TU_PASSWORD;Port=5432"
  },
  "Cloudinary": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  }
}
```

**Reemplaza:**
- `TU_PASSWORD` → Tu contraseña de PostgreSQL
- Puerto `5432` → Tu puerto de PostgreSQL (usualmente 5432 o 5433)

---

## 🗄️ CONFIGURAR POSTGRESQL

### **Opción 1: PostgreSQL local**

```bash
# 1. Instalar PostgreSQL desde: https://www.postgresql.org/download/

# 2. Crear base de datos
psql -U postgres
CREATE DATABASE lacafe_db;
\q

# 3. Ejecutar migraciones
dotnet ef database update
```

### **Opción 2: Docker**

```bash
docker run --name lacafe-postgres -e POSTGRES_PASSWORD=mipassword -p 5432:5432 -d postgres
docker exec -it lacafe-postgres psql -U postgres -c "CREATE DATABASE lacafe_db;"
```

---

## ☁️ CLOUDINARY (OPCIONAL PARA DESARROLLO)

Cloudinary solo es necesario para producción. En desarrollo local, las imágenes se guardan en `wwwroot/uploads/`

Si quieres configurar Cloudinary:

1. Crear cuenta gratuita: https://cloudinary.com
2. Obtener credenciales del Dashboard
3. Agregarlas en `appsettings.Development.json`

---

## 🚀 EJECUTAR LA APLICACIÓN

```bash
# 1. Restaurar paquetes
dotnet restore

# 2. Compilar
dotnet build

# 3. Aplicar migraciones
dotnet ef database update

# 4. Ejecutar
dotnet run
```

Abre: https://localhost:7174

---

## 🔐 ARCHIVOS QUE **NUNCA** DEBES SUBIR A GITHUB

- ❌ `appsettings.Development.json` (con contraseñas)
- ❌ `appsettings.Production.json` (con credenciales de producción)
- ❌ Cualquier archivo `.ps1` con contraseñas
- ❌ Carpeta `wwwroot/uploads/` (imágenes de usuarios)
- ❌ Carpeta `Migrations/` (específica de cada ambiente)

---

## ✅ VERIFICAR CONFIGURACIÓN

El archivo `.gitignore` ya está configurado para excluir estos archivos automáticamente.

Para verificar qué archivos se subirían:

```bash
git status
```

**NO deberías ver:**
- `appsettings.Development.json`
- `appsettings.Production.json`
- Archivos `.ps1`
- Carpeta `wwwroot/uploads/`

---

## 🚢 DEPLOY EN RENDER

Para producción, configura las variables de entorno en Render (ver `DEPLOYMENT.md`):

```
ConnectionStrings__DefaultConnection=postgresql://...
Cloudinary__CloudName=tu_cloud_name
Cloudinary__ApiKey=tu_api_key
Cloudinary__ApiSecret=tu_api_secret
ASPNETCORE_ENVIRONMENT=Production
```

---

## 📞 AYUDA

Si tienes problemas de configuración, revisa:
- `README.md` - Guía general
- `DEPLOYMENT.md` - Guía de deploy
- `CONFIGURATION.md` - Configuración detallada
