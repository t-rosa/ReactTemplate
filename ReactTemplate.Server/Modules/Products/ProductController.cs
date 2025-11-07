using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server;
using ReactTemplate.Server.Modules.Products.Dtos;
using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Modules.Products;

/// <summary>
/// API endpoints for Product management
/// </summary>
[ApiController]
[Route("api/product")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IValidator<CreateProductRequest> _createValidator;
    private readonly IValidator<UpdateProductRequest> _updateValidator;

    public ProductController(
        ApplicationDbContext context,
        UserManager<User> userManager,
        IValidator<CreateProductRequest> createValidator,
        IValidator<UpdateProductRequest> updateValidator)
    {
        _context = context;
        _userManager = userManager;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all product for the current user
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return Unauthorized();

        // TODO: Implement pagination
        var items = await _context.Products
            .Where(x => x.UserId == user.Id)
            .Select(x => MapToResponse(x))
            .ToListAsync();

        return Ok(items);
    }

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return Unauthorized();

        var item = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

        if (item == null)
            return NotFound();

        return Ok(MapToResponse(item));
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return Unauthorized();

        // Validate
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // TODO: Create the entity with the provided data
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, MapToResponse(entity));
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return Unauthorized();

        var entity = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

        if (entity == null)
            return NotFound();

        // Validate
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // TODO: Update the entity with the provided data
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Products.Update(entity);
        await _context.SaveChangesAsync();

        return Ok(MapToResponse(entity));
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return Unauthorized();

        var entity = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

        if (entity == null)
            return NotFound();

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static GetProductResponse MapToResponse(Product entity)
    {
        return new GetProductResponse
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
