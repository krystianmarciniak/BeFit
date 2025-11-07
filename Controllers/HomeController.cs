using Microsoft.AspNetCore.Mvc;

namespace BeFit.Controllers
{
  public class HomeController : Controller
  {
    // Akcja startowa – / lub /Home/Index
    public IActionResult Index()
    {
      return View();
    }

    // Akcja obsługi błędów
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View();
    }
  }
}
