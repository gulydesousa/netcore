using Microsoft.AspNetCore.Mvc;

public class SelectedStockController : Controller
{
    public IActionResult Trade()
    {
        // Obt�n el valor del ticker del modelo
        string ticker = (string)ViewBag.Ticker;

        // Realiza cualquier l�gica adicional necesaria para la acci�n de trading

        // Redirige a la acci�n "Index" del controlador "Trade" con el ticker como par�metro
        return RedirectToAction("Index", "Trade", new { ticker = ticker });
    }
}
