using System.Net.Mail;
using System.Net.Http.Json;
using System.Text.Json;

namespace Spilton.Api.Auth;

public sealed class EmailDeliveryException(string message, System.Net.HttpStatusCode? statusCode = null, string? providerCode = null)
    : Exception(message)
{
    public System.Net.HttpStatusCode? StatusCode { get; } = statusCode;
    public string? ProviderCode { get; } = providerCode;
    public static string PublicMessage(Exception exception, string kind)
    {
        if (exception is not EmailDeliveryException delivery) return $"The email service could not send your {kind}. Please try again later.";
        return delivery.StatusCode switch
        {
            System.Net.HttpStatusCode.Unauthorized => "Brevo rejected the API key. Create a new Brevo API v3 key and update Email__BrevoApiKey in Render.",
            System.Net.HttpStatusCode.Forbidden => "Brevo has not activated sending for this account. Complete Brevo phone/account verification, then try again.",
            System.Net.HttpStatusCode.BadRequest => "Brevo rejected the sender address. Verify Email__FromAddress under Brevo → Senders, domains, IPs.",
            _ => $"The email service could not send your {kind}. Please try again later."
        };
    }
}

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
        if (!response.IsSuccessStatusCode)
        {
            string? providerCode = null;
            try
            {
                using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct));
                if (body.RootElement.TryGetProperty("code", out var value)) providerCode = value.GetString();
            }
            catch (JsonException) { }
            throw new EmailDeliveryException($"Email provider rejected the request (HTTP {(int)response.StatusCode}).", response.StatusCode, providerCode);
        }
    }
}
