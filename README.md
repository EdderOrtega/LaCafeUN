# ☕ LA CAFE - SISTEMA DE CAFETERÍA UNIVERSITARIA

Sistema web completo de gestión de cafetería con API REST integrada.

## 🚀 INICIO RÁPIDO

### **1. Configuración Inicial**

1. Clonar el repositorio
2. Configurar base de datos PostgreSQL local
3. Configurar archivo `appsettings.Development.json` con tus credenciales locales
4. Ejecutar migraciones
5. Iniciar la aplicación

```powershell
dotnet restore
dotnet ef database update
dotnet run
```

---

## 🔧 REQUISITOS

- ✅ .NET 9.0 SDK
- ✅ PostgreSQL 16+
- ✅ Archivo de configuración local (no incluido en el repositorio)

---

## 🌐 URLS PRINCIPALES (Desarrollo Local)

| Servicio | URL |
|----------|-----|
| 🏠 Página Principal | https://localhost:7174 |
| 📝 Registro | https://localhost:7174/Account/Registro |
| 🔐 Login | https://localhost:7174/Account/Login |
| 🏢 Dashboard | https://localhost:7174/Home/Dashboard |
| 📋 Menú | https://localhost:7174/Menu |
| ➕ Agregar Producto | https://localhost:7174/AgregarProducto |
| 🛒 Pedidos | https://localhost:7174/Pedidos |
| 📚 Swagger API | https://localhost:7174/api/docs |

---

## 📊 ESTRUCTURA DE LA BASE DE DATOS

```
lacafe_db (PostgreSQL)
├─ Administradores
├─ Usuarios
├─ CategoriasProducto
│  ├─ Bebidas Calientes ☕
│  ├─ Bebidas Frías 🥤
│  ├─ Comida 🍔
│  ├─ Postres 🍰
│  └─ Snacks 🍿
├─ Productos
├─ FormasDePago
│  ├─ Efectivo
│  ├─ Tarjeta
│  └─ Transferencia
├─ Pedidos
└─ DetallesPedidos
```

---

## 🎯 FUNCIONALIDADES

### **Web (MVC):**
- ✅ Landing page con productos
- ✅ Registro de usuarios/administradores
- ✅ Login con autenticación
- ✅ Dashboard personalizado
- ✅ Ver menú completo por categorías
- ✅ Agregar productos con imágenes
- ✅ Ver mis pedidos
- ✅ Gestión de perfil

### **API REST:**
- ✅ `/api/Usuarios` - Gestión de usuarios
- ✅ `/api/Productos` - CRUD de productos
- ✅ `/api/Categorias` - Categorías de productos
- ✅ `/api/Pedidos` - Crear y gestionar pedidos
- ✅ `/api/FormasPago` - Formas de pago disponibles
- ✅ Documentación Swagger integrada

---

## 📸 GESTIÓN DE IMÁGENES

### **Desarrollo (Local):**
Las imágenes se guardan localmente en:
```
wwwroot/uploads/usuarios/     (Fotos de perfil)
wwwroot/uploads/productos/    (Fotos de productos)
```

### **Producción:**
Se integra con Cloudinary para almacenamiento en la nube.

---

## 🧪 FLUJO DE USO

1. **Registrar Usuario:** Crear cuenta seleccionando tipo de usuario
2. **Login:** Acceder con credenciales
3. **Dashboard:** Ver panel personalizado según tipo de usuario
4. **Menú:** Explorar productos por categorías
5. **Pedidos:** Crear y gestionar pedidos vía API

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### **Error: No se puede conectar a PostgreSQL**
- Verifica que PostgreSQL esté ejecutándose
- Revisa la configuración en tu archivo local de settings
- Verifica el puerto y credenciales

### **Error: Base de datos no existe**
```powershell
dotnet ef database update
```

### **Error al compilar**
```powershell
dotnet clean
dotnet build
```

---

## 📱 INTEGRACIÓN CON APP MÓVIL

La API REST está diseñada para integrarse con aplicaciones móviles (MAUI, Flutter, React Native, etc.)

### **Endpoints principales:**
- `GET /api/Productos` - Obtener listado de productos
- `POST /api/Usuarios/registro` - Registrar nuevo usuario
- `POST /api/Pedidos` - Crear nuevo pedido
- `GET /api/Categorias` - Obtener categorías

Ver documentación completa en Swagger: `/api/docs`

---

## 🚀 DEPLOYMENT EN PRODUCCIÓN

### Deployment en Render

Este proyecto incluye archivos de configuración para deployment en Render:

- **Dockerfile** - Configuración de contenedor Docker
- **.dockerignore** - Archivos excluidos del build
- **render.yaml** - Configuración de servicio (opcional)

**Instrucciones detalladas**: Ver archivo `RENDER_DEPLOYMENT.md`

### Archivos necesarios para deployment:
```
✅ Dockerfile                  (Incluido)
✅ .dockerignore              (Incluido)
✅ render.yaml                (Incluido - opcional)
✅ RENDER_DEPLOYMENT.md       (Guía paso a paso)
```

---

## 🔒 SEGURIDAD

- ⚠️ Este es un proyecto académico/educativo
- ⚠️ Configura todas las credenciales localmente (no incluidas en el repo)
- ⚠️ Usa variables de entorno para credenciales en producción
- ⚠️ Nunca subas credenciales o información sensible al repositorio público

---

## 🎓 PROYECTO ACADÉMICO

Desarrollado para: Universidad del Norte - Proyecto Final POO2

---

## 📧 DOCUMENTACIÓN

- **Local**: Swagger API en `https://localhost:7174/api/docs`
- **Deployment**: Ver `RENDER_DEPLOYMENT.md` para instrucciones de producción

---

**¡Disfruta La Cafe! ☕**
