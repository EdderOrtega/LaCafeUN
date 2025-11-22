# ☕ LA CAFE - SISTEMA DE CAFETERÍA UNIVERSITARIA

Sistema web completo de gestión de cafetería con API REST integrada.

## 🚀 INICIO RÁPIDO

### **1. Crear Base de Datos**
```powershell
.\CrearBDFinal.ps1
```

Este script:
- ✅ Elimina BD anterior (si existe)
- ✅ Crea nueva BD `lacafe_db`
- ✅ Crea carpetas de uploads
- ✅ Aplica migraciones
- ✅ Inserta datos iniciales
- ✅ Inicia la aplicación

### **2. Si Ya Tienes la BD Creada**
```powershell
dotnet run
```

---

## 🔧 REQUISITOS

- ✅ .NET 9.0 SDK
- ✅ PostgreSQL 16+ (puerto 5433)
- ✅ Password PostgreSQL: `root#12345`

---

## 🌐 URLS PRINCIPALES

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

## 👤 CREDENCIALES DE PRUEBA

### **Administrador:**
```
Email:    admin@lacafe.com
Password: admin123
Tipo:     Administrador
```

---

## 📊 ESTRUCTURA DE LA BASE DE DATOS

```
lacafe_db (PostgreSQL)
├─ Administradores
│  └─ 1 registro inicial (admin@lacafe.com)
├─ Usuarios
│  └─ Se llenan con registros
├─ CategoriasProducto
│  ├─ Bebidas Calientes ☕
│  ├─ Bebidas Frías 🥤
│  ├─ Comida 🍔
│  ├─ Postres 🍰
│  └─ Snacks 🍿
├─ Productos
│  └─ Se llenan desde /AgregarProducto
├─ FormasDePago
│  ├─ Efectivo
│  ├─ Tarjeta
│  └─ Transferencia
├─ Pedidos
│  └─ Se crean desde la API
└─ DetallesPedidos
   └─ Detalles de cada pedido
```

---

## 🎯 FUNCIONALIDADES

### **Web (MVC):**
- ✅ Landing page con productos
- ✅ Registro de usuarios/administradores
- ✅ Login con tipo de cuenta (Usuario/Admin)
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
```
wwwroot/
└─ uploads/
   ├─ usuarios/     (Fotos de perfil)
   └─ productos/    (Fotos de productos)
```

### **Producción (Render):**
- Se suben automáticamente a **Cloudinary**
- Configurar variables de entorno en Render

---

## 🧪 PROBAR LA APLICACIÓN

### **1. Registrar Usuario:**
1. Ve a: `https://localhost:7174/Account/Registro`
2. Selecciona tipo: **Usuario** o **Administrador**
3. Llena el formulario
4. Sube foto de perfil (opcional)
5. Clic en "Crear Cuenta"

### **2. Login:**
1. Ve a: `https://localhost:7174/Account/Login`
2. Selecciona tipo de cuenta
3. Ingresa credenciales
4. Accede al Dashboard

### **3. Agregar Producto:**
1. Inicia sesión como Admin
2. Ve a: `https://localhost:7174/AgregarProducto`
3. Llena formulario del producto
4. Selecciona categoría
5. Sube imagen del producto
6. Clic en "Agregar Producto"

### **4. Crear Pedido (API):**
```bash
POST https://localhost:7174/api/Pedidos
Content-Type: application/json

{
  "usuarioId": 1,
  "formaDePagoId": 1,
  "numeroMesa": "5",
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "notas": "Sin azúcar"
    }
  ]
}
```

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### **Error: Demasiadas redirecciones**
```
Solución: Borra las cookies del navegador
```

### **Error: No se puede conectar a PostgreSQL**
```
1. Verifica que PostgreSQL esté corriendo
2. Puerto correcto: 5433
3. Usuario: postgres
4. Password: root#12345
```

### **Error: Base de datos no existe**
```powershell
.\CrearBDFinal.ps1
```

### **Error al compilar**
```powershell
dotnet clean
dotnet build
```

---

## 📱 INTEGRACIÓN CON APP MAUI

### **URL Base (Desarrollo):**
```csharp
public const string ApiUrl = "https://localhost:7174/api";
```

### **URL Base (Producción):**
```csharp
public const string ApiUrl = "https://lacafe-api.onrender.com/api";
```

---

## 🚢 DEPLOY A RENDER

### **1. Subir a GitHub:**
```bash
git init
git add .
git commit -m "Initial commit - La Cafe"
git branch -M main
git remote add origin https://github.com/TU_USUARIO/lacafe-backend.git
git push -u origin main
```

### **2. Crear Web Service en Render:**
- **Build Command:** `dotnet publish -c Release -o out`
- **Start Command:** `cd out && ./ProyectoFinalPOO2`

### **3. Variables de Entorno:**
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=TU_CONNECTION_STRING_POSTGRES
Cloudinary__CloudName=TU_CLOUD_NAME
Cloudinary__ApiKey=TU_API_KEY
Cloudinary__ApiSecret=TU_API_SECRET
```

---

## 📝 NOTAS IMPORTANTES

- ⚠️ Las contraseñas están en texto plano (solo para desarrollo)
- ⚠️ En producción, implementar hash de contraseñas
- ⚠️ Configurar CORS según necesidades
- ⚠️ Revisar `appsettings.Production.json` antes de deploy

---

## 🎓 DESARROLLADO PARA

Universidad del Norte - Proyecto Final POO2

---

## 📧 SOPORTE

Para problemas o dudas, revisa la documentación en:
- `REVISION_COMPLETA.md` - Checklist completo
- `CLOUDINARY_SETUP.md` - Configuración de Cloudinary
- Swagger: `https://localhost:7174/api/docs`

---

**¡Disfruta La Cafe! ☕**
