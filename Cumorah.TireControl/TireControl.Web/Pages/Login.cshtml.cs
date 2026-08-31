using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TireControl.Web;

public class LoginModel(IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger) : PageModel
{
    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var client = httpClientFactory.CreateClient("TireControl.Api");
            var response = await client.PostAsJsonAsync("api/auth/login", new
            {
                Input.Email,
                Input.Password
            }, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return Page();
            }

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível concluir o acesso. Tente novamente em instantes.");
                return Page();
            }

            var login = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
            if (string.IsNullOrWhiteSpace(login?.AccessToken))
            {
                ModelState.AddModelError(string.Empty, "A resposta de acesso é inválida. Tente novamente.");
                return Page();
            }

            Response.Cookies.Append("tirecontrol_access_token", login.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps,
                Expires = Input.RememberMe ? login.ExpiresAtUtc : null
            });

            return RedirectToPage("/Index");
        }
        catch (HttpRequestException exception)
        {
            logger.LogWarning(exception, "Não foi possível comunicar com a API durante o login.");
            ModelState.AddModelError(string.Empty, "O serviço de acesso está indisponível. Tente novamente em instantes.");
            return Page();
        }
    }

    public class LoginInput
    {
        [Required(ErrorMessage = "Informe seu e-mail.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe sua senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Manter-me conectado")]
        public bool RememberMe { get; set; }
    }

    private sealed record LoginResponse(string AccessToken, string TokenType, DateTime ExpiresAtUtc);
}
