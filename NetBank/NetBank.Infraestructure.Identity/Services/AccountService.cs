using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Dtos.Email;
using NetBank.Core.Application.Dtos.Rol;
using NetBank.Core.Application.Enums;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Infraestructure.Identity.Entities;

namespace NetBank.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
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
                await _userManager.AddToRoleAsync(userNew, request.Rol.ToString());
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
                response.Error = "Ocurrio un error intentando registrar este usuario";
            }


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


            var usuariosWithRoles = await Task.WhenAll(usuarios.Select(async user =>
            {
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                return new AuthenticationResponse
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    UserName = user.Name,
                    Name = user.Name,
                    Identification = user.Identification,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Rol = (Enum.TryParse<Roles>(roles.FirstOrDefault(), out Roles rolEnum)) ? (int)rolEnum : 0


                };

            }));

            return usuariosWithRoles.ToList();
            //return usuarios
            //    .Select(async user => new AuthenticationResponse
            //    {
            //        Id =user.Id,
            //        LastName=user.LastName,
            //        UserName    = user.Name,
            //        Name = user.Name,
            //        Identification = user.Identification,
            //        Email = user.Email,
            //        IsActive = user.IsActive,

            //        Rol = (Enum.TryParse<Roles>(await _userManager.GetRolesAsync(user).ConfigureAwait(false), out Roles rolEnum)). ? (int)rolEnum : 0;


            //    }).ToList();
        }
    }
}
