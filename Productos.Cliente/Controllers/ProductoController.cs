using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Productos.Cliente.Models;
using Productos.Server.Models;
using System.Text.Json.Serialization;
using System.Text;

namespace Productos.Cliente.Controllers;
public class ProductoController : Controller
{
    private readonly HttpClient _httpClient;
    public ProductoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7095");
    }
    public async Task<IActionResult> Index()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/Productos/Lista");

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            IEnumerable<ProductoViewModel>? Productos = JsonConvert.DeserializeObject<IEnumerable<ProductoViewModel>>(content);
            
            return View(Productos);
        }
        return View(new List<ProductoViewModel>());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            string json = JsonConvert.SerializeObject(producto);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, 
                                                            "application/json");
            HttpResponseMessage response = await _httpClient
                                .PostAsync("api/Productos/crear", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            else
                ModelState.AddModelError(string.Empty, "Error al Crear producto");

        }

        return View(producto);
    }




}
