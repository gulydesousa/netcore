using Microsoft.AspNetCore.Mvc;

public class SelectedStockController : Controller
{
    public IActionResult Trade()
    {
        // Obtén el valor del ticker del modelo
        string ticker = (string)ViewBag.Ticker;

        // Realiza cualquier lógica adicional necesaria para la acción de trading

        // Redirige a la acción "Index" del controlador "Trade" con el ticker como parámetro
        return RedirectToAction("Index", "Trade", new { ticker = ticker });
    }
}
