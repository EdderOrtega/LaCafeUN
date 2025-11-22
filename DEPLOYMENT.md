# 🚀 GUÍA DE DEPLOYMENT

## ⚠️ NOTA DE SEGURIDAD

Esta guía contiene información general sobre deployment. **Nunca incluyas credenciales reales en archivos públicos.**

---

## PARTE 1: PREPARAR EL CÓDIGO PARA PRODUCCIÓN

### PASO 1: Revisar Seguridad

Antes de subir código a GitHub, verifica:

```bash
# Ver archivos que se van a subir
git status

# Verificar que .gitignore está funcionando
git check-ignore -v appsettings.Development.json
git check-ignore -v appsettings.Production.json
```

### PASO 2: Subir a GitHub (si aplica)

```bash
# Inicializar repositorio
git init

# Agregar archivos (solo los que no están en .gitignore)
git add .

# Crear commit
git commit -m "Descripción de cambios"

# Conectar con GitHub
git branch -M main
git remote add origin https://github.com/TU_USUARIO/TU_REPOSITORIO.git

# Subir
git push -u origin main
```

---

## PARTE 2: DEPLOYMENT EN SERVICIOS EN LA NUBE

### Opciones Populares:

1. **Render** - Free tier disponible, fácil integración con GitHub
2. **Azure App Service** - Integración con .NET, escalable
3. **Heroku** - Simple para deployment rápido
4. **AWS Elastic Beanstalk** - Escalable, profesional
5. **DigitalOcean App Platform** - Balance precio/funcionalidad

### Requisitos Generales:

Todos los servicios necesitarán:

1. **Base de Datos PostgreSQL** (crear instancia en la nube)
2. **Variables de Entorno** (configurar credenciales de forma segura)
3. **Build Configuration** (comandos de compilación)
4. **Start Command** (comando para iniciar la app)

---

## CONFIGURACIÓN GENÉRICA DE DEPLOYMENT

### **Build Commands (típico para .NET):**
```bash
dotnet restore
dotnet publish -c Release -o out
```

### **Start Command:**
```bash
cd out && dotnet NombreDelProyecto.dll
```

### **Variables de Entorno Requeridas:**

```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<URL_DE_TU_BASE_DE_DATOS>
Cloudinary__CloudName=<TU_CONFIGURACION>
Cloudinary__ApiKey=<TU_CONFIGURACION>
Cloudinary__ApiSecret=<TU_CONFIGURACION>
ASPNETCORE_URLS=http://0.0.0.0:$PORT
```

**IMPORTANTE:** Estas credenciales deben configurarse en el panel de tu proveedor cloud, **NUNCA en el código**.

---

## PARTE 3: BASE DE DATOS EN PRODUCCIÓN

### Opciones de PostgreSQL Cloud:

- **Render** - PostgreSQL free tier
- **ElephantSQL** - Free tier disponible
- **Supabase** - PostgreSQL gratuito con extras
- **AWS RDS** - Profesional, escalable
- **Azure Database for PostgreSQL** - Integración Microsoft

### Después de crear la BD:

1. Obtén la URL de conexión (connection string)
2. Configúrala como variable de entorno (no en el código)
3. Ejecuta migraciones:

```bash
dotnet ef database update
```

---

## PARTE 4: VERIFICACIÓN POST-DEPLOYMENT

### URLs a Verificar:

```
https://tu-app.dominio.com/                    # Página principal
https://tu-app.dominio.com/api/docs            # Swagger
https://tu-app.dominio.com/api/Categorias      # Test endpoint
```

### Checklist Post-Deployment:

- [ ] La aplicación inicia correctamente
- [ ] Base de datos está conectada
- [ ] Migraciones aplicadas
- [ ] API responde correctamente
- [ ] Swagger accesible (solo si lo deseas en producción)
- [ ] HTTPS habilitado
- [ ] Variables de entorno configuradas
- [ ] Logs funcionando

---

## PARTE 5: INTEGRACIÓN CON APLICACIONES CLIENTE

### Para Apps Móviles (MAUI, Flutter, React Native):

Cambia la URL base de desarrollo a producción:

```csharp
// DESARROLLO:
public const string ApiUrl = "https://localhost:7174/api";

// PRODUCCIÓN:
public const string ApiUrl = "https://tu-app.dominio.com/api";
```

### Endpoints Principales:

```
GET  /api/Productos          # Listado de productos
POST /api/Usuarios/registro  # Registro de usuario
POST /api/Usuarios/login     # Login
POST /api/Pedidos           # Crear pedido
GET  /api/Categorias        # Categorías disponibles
GET  /api/FormasPago        # Formas de pago
```

---

## 🔄 ACTUALIZAR LA APLICACIÓN

### Workflow típico:

```bash
# 1. Hacer cambios en el código local
# 2. Probar localmente
dotnet run

# 3. Commit y push
git add .
git commit -m "Descripción de cambios"
git push

# 4. El servicio cloud redesplegará automáticamente (si está configurado)
```

---

## ⚠️ MEJORES PRÁCTICAS DE SEGURIDAD

### ✅ HACER:
- Usar variables de entorno para credenciales
- Habilitar HTTPS en producción
- Implementar autenticación y autorización robusta
- Hacer hash de contraseñas (bcrypt, SHA-256)
- Validar inputs del usuario
- Configurar CORS apropiadamente
- Usar logs para debugging
- Mantener dependencias actualizadas

### ❌ NO HACER:
- Subir credenciales a GitHub
- Dejar Swagger abierto en producción (sin autenticación)
- Usar contraseñas en texto plano
- Exponer mensajes de error detallados en producción
- Olvidar actualizar connection strings
- Dejar puertos de debug abiertos

---

## 📊 MONITOREO Y MANTENIMIENTO

### Cosas a Monitorear:

- Logs de errores
- Uso de base de datos
- Tiempo de respuesta de API
- Almacenamiento de imágenes
- Tráfico y requests

### Backups:

- Configurar backups automáticos de la base de datos
- Respaldar configuraciones importantes
- Documentar procedimientos de recuperación

---

## 🎯 CHECKLIST FINAL DE DEPLOYMENT

- [ ] Código subido a repositorio (sin credenciales)
- [ ] Base de datos en producción creada
- [ ] Variables de entorno configuradas
- [ ] Build exitoso
- [ ] Migraciones aplicadas
- [ ] Aplicación accessible vía HTTPS
- [ ] API endpoints funcionando
- [ ] Subida de imágenes funcionando
- [ ] Documentado para el equipo
- [ ] Plan de mantenimiento establecido

---

## 📚 RECURSOS ADICIONALES

- [.NET Deployment Guide](https://docs.microsoft.com/aspnet/core/host-and-deploy/)
- [PostgreSQL Cloud Providers](https://www.postgresql.org/support/professional_hosting/)
- [Environment Variables Best Practices](https://12factor.net/config)
- [HTTPS Configuration](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl)

---

## 💡 CONSEJOS PARA PROYECTOS ACADÉMICOS

- Usa free tiers de servicios cloud
- Documenta el proceso para tu equipo
- Mantén un repositorio limpio y organizado
- Implementa solo features necesarias
- Prueba exhaustivamente antes de presentar

---

**Recuerda: Un deployment exitoso requiere planificación, seguridad y buenas prácticas.** 🚀
