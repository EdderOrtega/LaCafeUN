using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;
using ProyectoFinalPOO2.Models;
using ProyectoFinalPOO2.ViewModels;
using ProyectoFinalPOO2.Entities;
using ProyectoFinalPOO2.Services;

namespace ProyectoFinalPOO2.Controllers
{
    public class AccountController : Controller
    {
        private readonly CafeteriaContext _context;
        private readonly IImagenService _imagenService;
        
        public AccountController(CafeteriaContext context, IImagenService imagenService)
        {
            _context = context;
            _imagenService = imagenService;
        }
        
        // GET: Account/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") != null || HttpContext.Session.GetInt32("AdminId") != null)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        
        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TipoCuenta == "Administrador")
                {
                    // Buscar en tabla Administradores
                    var admin = await _context.Administradores
                        .FirstOrDefaultAsync(a => a.Email == model.Email && a.Password == model.Password && a.Activo);
                    
                    if (admin != null)
                    {
                        HttpContext.Session.SetInt32("AdminId", admin.Id);
                        HttpContext.Session.SetString("AdminNombre", admin.Nombre);
                        HttpContext.Session.SetString("AdminEmail", admin.Email);
                        HttpContext.Session.SetString("TipoCuenta", "Administrador");
                        if (!string.IsNullOrEmpty(admin.FotoPerfil))
                        {
                            HttpContext.Session.SetString("UsuarioFoto", admin.FotoPerfil);
                        }
                        
                        TempData["Mensaje"] = $"¡Bienvenido Administrador {admin.Nombre}!";
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                else
                {
                    // Buscar en tabla Usuarios
                    var usuario = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password && u.Activo);
                    
                    if (usuario != null)
                    {
                        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                        HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                        HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                        HttpContext.Session.SetString("TipoCuenta", "Usuario");
                        if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                        {
                            HttpContext.Session.SetString("UsuarioFoto", usuario.FotoPerfil);
                        }
                        
                        TempData["Mensaje"] = $"¡Bienvenido {usuario.Nombre}!";
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                
                ModelState.AddModelError("", "Email, contraseña o tipo de cuenta incorrectos");
            }
            
            return View(model);
        }
        
        // GET: Account/Registro
        public IActionResult Registro()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") != null || HttpContext.Session.GetInt32("AdminId") != null)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        
        // POST: Account/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model, IFormFile? archivoFotoPerfil)
        {
            if (ModelState.IsValid)
            {
                var emailExisteUsuario = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
                var emailExisteAdmin = await _context.Administradores.AnyAsync(a => a.Email == model.Email);
                
                if (emailExisteUsuario || emailExisteAdmin)
                {
                    ModelState.AddModelError("Email", "Este email ya está registrado");
                    return View(model);
                }
                
                string urlFotoPerfil;

                if (archivoFotoPerfil != null && archivoFotoPerfil.Length > 0)
                {
                    urlFotoPerfil = await _imagenService.SubirImagenAsync(archivoFotoPerfil, "usuarios");
                    
                    if (string.IsNullOrEmpty(urlFotoPerfil))
                    {
                        ModelState.AddModelError("", "Error al subir la imagen. Intenta de nuevo.");
                        return View(model);
                    }
                }
                else
                {
                    urlFotoPerfil = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(model.Nombre)}&background=831D81&color=fff&size=200&rounded=true&bold=true";
                }
                
                if (model.TipoCuenta == "Administrador")
                {
                    var admin = new AdministradorEntity
                    {
                        Nombre = model.Nombre,
                        Email = model.Email,
                        Password = model.Password,
                        FotoPerfil = urlFotoPerfil,
                        FechaCreacion = DateTime.UtcNow,
                        Activo = true
                    };
                    
                    _context.Administradores.Add(admin);
                    await _context.SaveChangesAsync();
                    
                    HttpContext.Session.SetInt32("AdminId", admin.Id);
                    HttpContext.Session.SetString("AdminNombre", admin.Nombre);
                    HttpContext.Session.SetString("AdminEmail", admin.Email);
                    HttpContext.Session.SetString("TipoCuenta", "Administrador");
                    HttpContext.Session.SetString("UsuarioFoto", admin.FotoPerfil);
                }
                else
                {
                    var usuario = new UsuarioEntity
                    {
                        Nombre = model.Nombre,
                        Email = model.Email,
                        Password = model.Password,
                        FotoPerfil = urlFotoPerfil,
                        Activo = true
                    };
                    
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                    
                    HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                    HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                    HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                    HttpContext.Session.SetString("TipoCuenta", "Usuario");
                    HttpContext.Session.SetString("UsuarioFoto", usuario.FotoPerfil);
                }
                
                TempData["Mensaje"] = "¡Cuenta creada exitosamente!";
                return RedirectToAction("Dashboard", "Home");
            }
            
            return View(model);
        }
        
        // GET: Account/Perfil
        public async Task<IActionResult> Perfil()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var adminId = HttpContext.Session.GetInt32("AdminId");
            
            if (usuarioId == null && adminId == null)
            {
                return RedirectToAction("Login");
            }
            
            if (adminId != null)
            {
                var admin = await _context.Administradores.FindAsync(adminId);
                if (admin == null)
                {
                    return RedirectToAction("Logout");
                }
                
                var modelAdmin = new Usuario
                {
                    Id = admin.Id,
                    Nombre = admin.Nombre,
                    Email = admin.Email,
                    FotoPerfil = admin.FotoPerfil
                };
                
                ViewBag.EsAdmin = true;
                return View(modelAdmin);
            }
            else
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                {
                    return RedirectToAction("Logout");
                }
                
                var model = new Usuario
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    FotoPerfil = usuario.FotoPerfil
                };
                
                ViewBag.EsAdmin = false;
                return View(model);
            }
        }
        
        // POST: Account/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Mensaje"] = "Sesión cerrada exitosamente";
            return RedirectToAction("Index", "Home");
        }
    }
}
