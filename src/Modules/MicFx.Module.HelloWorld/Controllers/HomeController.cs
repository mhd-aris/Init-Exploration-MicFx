using Microsoft.AspNetCore.Mvc;

namespace MicFx.Module.HelloWorld.Controllers;

[Route("Hello")]
public class HelloWorldController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet("test")]
    public ActionResult Test()
    {
        return Json(new { message = "Hello from MicFx!" });
    }
}