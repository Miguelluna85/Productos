using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productos.Server.Models;

namespace Productos.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly ProductosContext _context;
    public ProductosController(ProductosContext context)
    {
        try
        {
            _context = context;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost]
    [Route("Crear")]
    public async Task<IActionResult> CrearProducto(Producto producto)
    {
        try
        {
            if (producto == null)
                return BadRequest();

            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("Lista")]
    public async Task<ActionResult<IEnumerable<Producto>>> ListaProductos()
    {
        try
        {
            var productos = await _context.Productos.ToListAsync();

            return Ok(productos);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }

    }

    [HttpGet]
    [Route("ver")]
    public async Task<IActionResult> VerProducto(int id)
    {
        try
        {
            if (id <= 0)
                return NotFound();

            Producto producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);

        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("editar")]
    public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
    {
        try
        {
            if (id <= 0 || producto == null)
                return BadRequest();

            var productoExistente = await _context.Productos.FindAsync(id);

            if (productoExistente == null)
                return NotFound();


            productoExistente!.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;

            await _context.SaveChangesAsync();

            return Ok(productoExistente);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    [Route("Eliminar")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest();

            var productoBorrado = await _context.Productos.FindAsync(id);

            if (productoBorrado == null)
                return NotFound();

            _context.Remove(productoBorrado);
            await _context.SaveChangesAsync();

            return Ok(productoBorrado);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
