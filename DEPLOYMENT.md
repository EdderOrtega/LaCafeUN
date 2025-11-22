# 🚀 GUÍA DE DEPLOYMENT - GITHUB Y RENDER

## PARTE 1: SUBIR A GITHUB

### PASO 1: Crear repositorio en GitHub

1. Ve a: https://github.com
2. Clic en **"New repository"** (botón verde)
3. Configura:
   - **Repository name:** `LaCafePOO2-API`
   - **Description:** `API REST para sistema de cafetería`
   - **Visibilidad:** Public
   - ❌ NO marcar "Add a README file"
   - ❌ NO marcar "Add .gitignore"
4. Clic en **"Create repository"**

### PASO 2: Inicializar Git en tu proyecto

Abre PowerShell en la carpeta del proyecto:

```powershell
cd "C:\Users\rede9\Downloads\LaCafePOO2NET\LaCafePOO2-main"

# Inicializar Git
git init

# Agregar todos los archivos
git add .

# Hacer el primer commit
git commit -m "Initial commit - La Cafe API"

# Conectar con GitHub (reemplaza TU_USUARIO con tu usuario de GitHub)
git branch -M main
git remote add origin https://github.com/TU_USUARIO/LaCafePOO2-API.git

# Subir a GitHub
git push -u origin main
```

### PASO 3: Verificar en GitHub

Recarga la página de tu repositorio en GitHub y deberías ver todos tus archivos.

---

## PARTE 2: DEPLOY EN RENDER

### PASO 1: Crear cuenta en Render

1. Ve a: https://render.com
2. Clic en **"Get Started"**
3. Registrate con tu cuenta de GitHub (recomendado)

### PASO 2: Crear PostgreSQL Database

1. En el dashboard de Render, clic en **"New +"**
2. Selecciona **"PostgreSQL"**
3. Configura:
   - **Name:** `lacafe-db`
   - **Database:** `lacafe_db`
   - **User:** `lacafe_user`
   - **Region:** Oregon (US West)
   - **Plan:** Free
4. Clic en **"Create Database"**
5. **IMPORTANTE:** Copia la **Internal Database URL** (la usaremos después)

Ejemplo de URL:
```
postgresql://lacafe_user:PASSWORD@dpg-xxx.oregon-postgres.render.com/lacafe_db
```

### PASO 3: Crear Web Service

1. En el dashboard, clic en **"New +"**
2. Selecciona **"Web Service"**
3. Conecta tu repositorio de GitHub
4. Selecciona el repositorio **"LaCafePOO2-API"**
5. Configura:

```
Name:              lacafe-api
Region:            Oregon (US West)
Branch:            main
Root Directory:    (dejar vacío)
Runtime:           .NET
Build Command:     dotnet publish -c Release -o out
Start Command:     cd out && dotnet ProyectoFinalPOO2.dll
Plan:              Free
```

### PASO 4: Configurar Variables de Entorno

En la sección **"Environment Variables"**, agrega:

**Variable 1:**
```
Key:   ConnectionStrings__DefaultConnection
Value: [PEGA AQUÍ LA INTERNAL DATABASE URL DE TU POSTGRESQL]
```

**Variable 2:**
```
Key:   ASPNETCORE_ENVIRONMENT
Value: Production
```

**Variable 3:**
```
Key:   ASPNETCORE_URLS
Value: http://0.0.0.0:$PORT
```

### PASO 5: Deploy

1. Clic en **"Create Web Service"**
2. Render comenzará a buildear tu aplicación (tarda 5-10 minutos)
3. Espera a que el status sea **"Live"** (verde)

### PASO 6: Aplicar Migraciones

Una vez que el servicio esté "Live":

1. En Render, ve a tu web service
2. Clic en **"Shell"** (en el menú lateral)
3. Ejecuta:

```bash
dotnet ef database update
```

Esto creará las tablas en PostgreSQL de Render.

---

## PARTE 3: VERIFICAR QUE FUNCIONE

### URL de tu API:
```
https://lacafe-api.onrender.com
```

### Swagger (Documentación):
```
https://lacafe-api.onrender.com/api/docs
```

### Probar endpoint de categorías:
```
https://lacafe-api.onrender.com/api/Categorias
```

Deberías ver las 5 categorías en formato JSON.

---

## PARTE 4: USAR EN APP MAUI

En tu app MAUI, cambia la URL base:

```csharp
// ANTES (desarrollo local):
public const string ApiUrl = "https://localhost:7174/api";

// AHORA (producción en Render):
public const string ApiUrl = "https://lacafe-api.onrender.com/api";
```

### Ejemplo de uso:

```csharp
// Obtener productos
var response = await httpClient.GetAsync($"{ApiUrl}/Productos");
var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();

// Registrar usuario
var nuevoUsuario = new {
    nombre = "Juan Pérez",
    email = "juan@example.com",
    password = "123456",
    telefono = "8123456789"
};
var response = await httpClient.PostAsJsonAsync($"{ApiUrl}/Usuarios/registro", nuevoUsuario);

// Crear pedido
var pedido = new {
    usuarioId = 1,
    formaDePagoId = 1,
    detalles = new[] {
        new { productoId = 1, cantidad = 2 },
        new { productoId = 3, cantidad = 1 }
    }
};
await httpClient.PostAsJsonAsync($"{ApiUrl}/Pedidos", pedido);
```

---

## 🔄 ACTUALIZAR LA API (Después de hacer cambios)

Cuando hagas cambios en tu código:

```powershell
# 1. Agregar cambios
git add .

# 2. Hacer commit
git commit -m "Descripción de tus cambios"

# 3. Subir a GitHub
git push

# 4. Render detectará los cambios y redesplegará automáticamente
```

---

## ⚠️ NOTAS IMPORTANTES

### Free Tier de Render:
- ✅ Gratis permanentemente
- ⚠️ Se duerme después de 15 minutos de inactividad
- ⚠️ Primera petición tarda 30-60 segundos en despertar
- ✅ Perfecto para proyectos estudiantiles

### Base de datos PostgreSQL Free:
- ✅ 256 MB de almacenamiento
- ✅ 97 horas de runtime al mes
- ✅ Suficiente para el proyecto

### Solución al "sleep":
En tu app MAUI, puedes agregar un loading mientras despierta:

```csharp
// Mostrar "Conectando con el servidor..."
// La primera petición tardará más
```

---

## 🎯 CHECKLIST FINAL

- [ ] Crear repositorio en GitHub
- [ ] Subir código con `git push`
- [ ] Crear PostgreSQL en Render
- [ ] Crear Web Service en Render
- [ ] Configurar variables de entorno
- [ ] Esperar a que buildee (5-10 min)
- [ ] Aplicar migraciones en Shell
- [ ] Probar `/api/docs`
- [ ] Probar endpoints
- [ ] Actualizar URL en app MAUI
- [ ] Compartir URL con compañeros

---

## 📱 COMPARTIR CON TUS COMPAÑEROS

Envíales:

**URL de la API:**
```
https://lacafe-api.onrender.com
```

**Swagger (para probar):**
```
https://lacafe-api.onrender.com/api/docs
```

**URL para código de MAUI:**
```csharp
public const string ApiUrl = "https://lacafe-api.onrender.com/api";
```

---

¡Listo! Ahora tu equipo puede trabajar con la API en internet. 🚀
