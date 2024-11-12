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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/Productos/ver?id={id}");

        if (response.IsSuccessStatusCode)//code 200
        {
            var content = await response.Content.ReadAsStringAsync();
            var producto = JsonConvert.DeserializeObject<ProductoViewModel>(content);

            return View(producto);
        }
        else
        {
            return RedirectToAction("Details");
        }       
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ProductoViewModel producto)
    {
        if (ModelState.IsValid)
        {
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/Productos/editar?id={id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id });
            }
            else
            {
                ModelState.AddModelError(string.Empty,"Error al actualizar producto");
            }
        }

        return View(producto);
    }

    public async Task<IActionResult> Details(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/Productos/ver?id={id}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            ProductoViewModel? producto = JsonConvert.DeserializeObject<ProductoViewModel>(content);

            return View(producto);
        }
        else
        {
            return RedirectToAction("Details");
        }

    }

    public async Task<IActionResult> Delete(int id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Productos/eliminar?id={id}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("index");
        }
        else
        {
            TempData["Error"] = "Error al eliminar producto";
            return RedirectToAction("index");
        }
    }

}
