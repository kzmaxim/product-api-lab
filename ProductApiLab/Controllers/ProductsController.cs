using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApiLab.Dtos;
using ProductApiLab.Models;
using ProductApiLab.Repositories;

namespace ProductApiLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<IEnumerable<ProductDto>>>> GetAll()
    {
        var products = await _repository.GetAllAsync();
        var dto = _mapper.Map<IEnumerable<ProductDto>>(products);

        return Ok(new ReturnResult<IEnumerable<ProductDto>>
        {
            IsSuccess = true,
            Result = dto
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<ProductDto>>> Get(int id)
    {
        var product = await _repository.GetAsync(id);
        if (product is null)
        {
            return NotFound(new ReturnResult<ProductDto>
            {
                IsSuccess = false,
                ErrorMessage = [$"Product with id {id} was not found."]
            });
        }

        return Ok(new ReturnResult<ProductDto>
        {
            IsSuccess = true,
            Result = _mapper.Map<ProductDto>(product)
        });
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<ProductDto>>> Create([FromBody] ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _repository.CreateAsync(product);

        var result = new ReturnResult<ProductDto>
        {
            IsSuccess = true,
            Result = _mapper.Map<ProductDto>(product)
        };

        return CreatedAtAction(nameof(Get), new { id = product.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReturnResult<ProductDto>>> Update(int id, [FromBody] ProductDto productDto)
    {
        var existing = await _repository.GetAsync(id);
        if (existing is null)
        {
            return NotFound(new ReturnResult<ProductDto>
            {
                IsSuccess = false,
                ErrorMessage = [$"Product with id {id} was not found."]
            });
        }

        existing.Name = productDto.Name;
        existing.Price = productDto.Price;
        await _repository.UpdateAsync(existing);

        return Ok(new ReturnResult<ProductDto>
        {
            IsSuccess = true,
            Result = _mapper.Map<ProductDto>(existing)
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ReturnResult<ProductDto>>> Delete(int id)
    {
        var existing = await _repository.GetAsync(id);
        if (existing is null)
        {
            return NotFound(new ReturnResult<ProductDto>
            {
                IsSuccess = false,
                ErrorMessage = [$"Product with id {id} was not found."]
            });
        }

        await _repository.DeleteAsync(id);

        return Ok(new ReturnResult<ProductDto>
        {
            IsSuccess = true,
            Result = _mapper.Map<ProductDto>(existing)
        });
    }
}
