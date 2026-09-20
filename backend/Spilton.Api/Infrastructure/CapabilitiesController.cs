using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spilton.Api.Chat;
using Spilton.Api.Coding;
using Spilton.Api.Security;
using Spilton.Api.Auth;
using Spilton.Api.Documents;

namespace Spilton.Api.Infrastructure;

[ApiController,Authorize(Roles="User"),Route("api/capabilities"),ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
public sealed class CapabilitiesController(IModelProviderResolver models,IMultimodalProvider multimodal,ICodeExecutionService execution,IUploadScanner scanner,IDistributedRateLimitStore rateLimits,IAccountEmailSender email,IFileStorage storage):ControllerBase
{
    [HttpGet]
    public IActionResult Get()=>Ok(new
    {
        models=models.Available,
        vision=multimodal.Capability,
        codingExecution=execution.Capability,
        production=new{malwareScanning=scanner.Status,distributedRateLimiting=rateLimits.Status,passwordRecoveryEmail=email.Available?"CONFIGURED":"CONFIGURATION_REQUIRED",documentStorage=storage.Provider},
        guarantees=new[]{"Only configured models are advertised","Web and document content are untrusted data","Host shell execution is unavailable"}
    });
}
