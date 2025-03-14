using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Dtos.Email;
using NetBank.Core.Application.Dtos.Rol;
using NetBank.Core.Application.Enums;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Infraestructure.Identity.Entities;
using System.Data;

namespace NetBank.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IEmailService _emailService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AuthenticationResponse userInSession;

        private readonly IProductService _productService;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IProductService productService, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }


        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No existe una cuenta con el nombre de usuario {request.UserName}";
                return response;
            }


            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Usuario o contraseña incorrectos";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"El correo electronico de esa cuenta no ha sido confirmado";
                return response;
            }

            if(!user.IsActive)
            {
                response.HasError= true;
                response.Error = "Estas inactivo, comunicate con el administrador";
                return response;
            }

            response.Id = user.Id;
            response.UserName = user.UserName;
            response.Email = user.Email;
            response.LastName = user.LastName;
            response.Identification = user.Identification;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Rol = (Enum.TryParse<Roles>(rolesList.FirstOrDefault(), out Roles rolEnum)) ? (int)rolEnum : 0;
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            RegisterResponse response = new();
            response.HasError = false;

            var userSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Ya existe un registro con ese nombre de usuario {request.UserName}";
                return response;
            }


            var userSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Ya existe un registro con el correo {request.Email}";
                return response;
            }

            var userSameCedula = await _userManager.Users.FirstOrDefaultAsync(x => x.Identification == request.Identification);

            if (userSameCedula != null)
            {
                response.HasError = true;
                response.Error = $"Ya existe un registro con la cedula {request.Identification}";
                return response;
            }

            ApplicationUser userNew = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                Identification = request.Identification,
                Name = request.Name,
                LastName = request.LastName,
                EmailConfirmed = true,

            };

            var result = await _userManager.CreateAsync(userNew, request.Password);

            if (result.Succeeded)
            {
                var rol = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == request.RolId);
                await _userManager.AddToRoleAsync(userNew, rol?.Name);
                await _emailService.SendAsync(new EmailRequest
                {
                    Subject = "NetBank-App",
                    Body = "<h1>Acabas de registrarte en NetBank-App la aplicacion de internet banking mas moderna del siglo</h1>",
                    To = request.Email,
                });
            }
            else
            {
                response.HasError = true;
                response.Error = "La contraseña debe ser de minimo 6 caracteres, debe tener minuscula,mayuscula y al menos un caracter especial(!#$)";
            }

            response.Id = userNew.Id;
            return response;
        }

        public async Task<List<RolList>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles
                .Select(role => new RolList
                {
                    Id = role.Id,
                    Name = role?.Name,
                }).ToList();
        }

        public async Task<List<AuthenticationResponse>> GetAllUsersAsync()
        {
            var usuarios = await _userManager.Users.ToListAsync(); 

            var usuariosWithRoles = new List<AuthenticationResponse>();

            foreach (var user in usuarios) 
            {
                var roles = await _userManager.GetRolesAsync(user); 

                usuariosWithRoles.Add(new AuthenticationResponse
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Name = user.Name,
                    Identification = user.Identification,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Rol = (Enum.TryParse<Roles>(roles.FirstOrDefault(), out Roles rolEnum)) ? (int)rolEnum : 0
                });
            }

            return usuariosWithRoles.OrderByDescending(x => x.Rol).ToList();
        }


        public async Task<UserInactivate> UserInactivateAsync(string id)
        {
            UserInactivate response = new()
            {
                HasError = false
            };


            var usuario = await _userManager.FindByIdAsync(id);


            if(userInSession.Id == id)
            {
                response.HasError = true;
                response.Error = "No puedes inactivarte o activarte ya que estas en session";
                return response;
            }

            if(usuario == null)
            {
                response.HasError = true;
                response.Error = "No existe un usuario con ese id";
                return response;
            }

            usuario.IsActive = !usuario.IsActive;
            await _userManager.UpdateAsync(usuario);

            return response;
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            UpdateUserResponse response = new()
            {
                HasError = false
            };

            ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

            if(user == null)
            {
                response.HasError = true;
                response.Error = "No existe ese usuario";
                return response;
            }

            var UserWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (UserWithSameUserName != null)
            {
                if(user.Id != UserWithSameUserName.Id)
                {
                    response.HasError = true;
                    response.Error = "Ese nombre de usuario ya esta en uso";
                    return response;
                }
                
            }

            var UserWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (UserWithSameEmail != null)
            {
                if(user.Id != UserWithSameEmail.Id)
                {
                    response.HasError = true;
                    response.Error = "Ese correo ya esta en uso";
                    return response;
                }
               
            }


            user.Name = request.Name;
            user.Email = request.Email;
            user.LastName = request.LastName;
            user.Identification = request.Identification;
            user.UserName = request.UserName;
            await _userManager.UpdateAsync(user);

            if(!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user,token, request.Password);

                if (result.Succeeded)
                {
                    await _emailService.SendAsync(new EmailRequest
                    {
                        To = request.Email,
                        Subject = "Acaban de actualizar tu contraseña",
                        Body = "<h3>Tu contraseña ha sido actualizada satisfactoriamente</h3>"
                    });
                    return response;
                }
                else
                {
                    response.HasError = true;
                    response.Error = "La contraseña debe ser de minimo 6 caracteres, debe tener minuscula,mayuscula y al menos un caracter especial(!#$)";
                }
            }

            return response;
        }

        public async Task<AuthenticationResponse> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            AuthenticationResponse response = new()
            {
                Id = id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Identification = user.Identification,
                UserName = user.UserName,
                Rol = (Enum.TryParse<Roles>(roles.FirstOrDefault(), out Roles rolEnum)) ? (int)rolEnum : 0
            };

            return response;
        }

        public async Task<AuthenticationResponse> GetUserByAccountNumber(int accountNumber)
        {
            
            var account = await _productService.GetProductByAccountNumber(accountNumber);

            if (account == null)
                return null;

            var user = await _userManager.FindByIdAsync(account.UserId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            AuthenticationResponse response = new()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Identification = user.Identification,
                UserName = user.UserName,
                Rol = (Enum.TryParse<Roles>(roles.FirstOrDefault(), out Roles rolEnum)) ? (int)rolEnum : 0
            };
            return response;
        }
    }
}
