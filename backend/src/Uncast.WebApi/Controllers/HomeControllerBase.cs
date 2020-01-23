namespace Uncast.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/[controller]")]
    public abstract class HomeControllerBase : ControllerBase { }
}