using ContactManager.Core.Domain.IdentityEntites;
using ContactManager.Core.DTO;
using ContactManager.Core.Enums;
using CRUDexample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    /// <summary>
    /// Controller for managing user accounts.
    /// </summary>
    [Route("[controller]/[action]")]
    //[AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign-in manager.</param>
        /// <param name="roleManager">The role manager.</param>
        public AccountController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// GET action for the register page.
        /// </summary>
        /// <returns>The register view.</returns>
        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST action for registering a new user.
        /// </summary>
        /// <param name="registerDTO">The registration data.</param>
        /// <returns>The result of the registration.</returns>
        [HttpPost]
        [Authorize("NotAuthenticated")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            //Check for validation errors
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(ModelState => ModelState.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };

            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);

            if (result.Succeeded)
            {
                //Asignar el rol al usuario
                await AddRole(applicationUser, registerDTO.UserType);

                //SignIn: Crea la cookie y la envia al browser como parte del response
                //IsPersistent: La cookie se queda en el navegador incluso cuando se cierra el navegador.
                //Cuando abre el mismo browser el usuario sigue autenticado.
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return View(registerDTO);
            }

        }

        /// <summary>
        /// Adds a role to the specified user.
        /// </summary>
        /// <param name="applicationUser">The user to add the role to.</param>
        /// <param name="role">The role to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task AddRole(ApplicationUser applicationUser, UserTypeOptions role)
        {
            if (await _roleManager.FindByNameAsync(role.ToString()) is null)
            {
                //Create role if doesn't exist
                ApplicationRole applicationRole = new ApplicationRole()
                {
                    Name = role.ToString()
                };

                await _roleManager.CreateAsync(applicationRole);
            }

            await _userManager.AddToRoleAsync(applicationUser, role.ToString());
        }

        /// <summary>
        /// GET action for logging out the user.
        /// </summary>
        /// <returns>The result of the logout.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");

        }

        /// <summary>
        /// GET action for the login page.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// POST action for logging in the user.
        /// </summary>
        /// <param name="loginDTO">The login data.</param>
        /// <param name="ReturnUrl">The return URL.</param>
        /// <returns>The result of the login.</returns>
        [HttpPost]
        [Authorize("NotAuthenticated")]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(ModelState => ModelState.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

            if (result.Succeeded)
            {
                //Admin
                ApplicationUser applicationUser = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (applicationUser != null)
                {
                    if (await _userManager.IsInRoleAsync(applicationUser, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    //Redirect to the url that the user was trying to access before being redirected to the login page
                    //This is to avoid the user to be redirected to the home page after login
                    //This is a security measure to avoid open redirect attacks
                    //https://docs.microsoft.com/en-us/aspnet/core/security/preventing-open-redirects?view=aspnetcore-5.0
                    return LocalRedirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(PersonsController.Index), "Persons");
                }
            }
            else
            {
                ModelState.AddModelError("Login", "Invalid login attempt.");
                return View(loginDTO);
            }
        }

        /// <summary>
        /// Checks if an email is already registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>A JSON result indicating if the email is already registered.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser applicationUser =
            await _userManager.FindByEmailAsync(email);

            if (applicationUser == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
