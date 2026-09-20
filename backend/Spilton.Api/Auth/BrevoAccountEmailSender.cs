using System.Net.Mail;
using System.Net.Http.Json;

namespace Spilton.Api.Auth;

public sealed class BrevoAccountEmailSender(EmailSettings settings, IHttpClientFactory clients) : IAccountEmailSender
{
    public bool Available => settings.Provider.Equals("Brevo", StringComparison.OrdinalIgnoreCase)
        && !string.IsNullOrWhiteSpace(settings.BrevoApiKey)
        && MailAddress.TryCreate(settings.FromAddress, out _);

    public Task SendPasswordResetCode(string email, string name, string code, CancellationToken ct) =>
        Send(email, name, "Your Spilton password reset code", $"Your password reset code is: {code}", ct);

    public Task SendEmailVerificationCode(string email, string name, string code, CancellationToken ct) =>
        Send(email, name, "Verify your Spilton email", $"Your email verification code is: {code}", ct);

    private async Task Send(string email, string name, string subject, string codeText, CancellationToken ct)
    {
        if (!Available) throw new InvalidOperationException("Brevo email is not configured.");
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
        request.Headers.Add("api-key", settings.BrevoApiKey);
        request.Content = JsonContent.Create(new
        {
            sender = new { email = settings.FromAddress, name = settings.FromName },
            to = new[] { new { email, name } },
            subject,
            textContent = $"Hello {name},\n\n{codeText}\n\nThis code expires in 10 minutes and can be used once. If you did not request it, ignore this email."
        });
        using var client = clients.CreateClient("brevo-email");
        using var response = await client.SendAsync(request, ct);
        // Never include provider response bodies, recipient details, or codes in errors.
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Email provider rejected the request (HTTP {(int)response.StatusCode}).", null, response.StatusCode);
    }
}
