using LibraryManager.Biz;
using LibraryManager.Data;
using LibraryManager.Dto;
using LibraryManager.Exceptions;
using LibraryManager.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly LibraryManagerDbContext _db;

    public BookController(LibraryManagerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _db.Books.ToListAsync(cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Book>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create(CreateBookDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var book = new Book
            {
                Title = dto.Title.Trim(),
                Author = dto.Author.Trim(),
                Genre = dto.Genre.Trim(),
                Price = dto.Price,
                Stock = dto.Stock
            };

            ThrowIfInvalid(book);
            await ThrowIfExists(book.Title, book.Author, null, cancellationToken);

            book.CreatedAt = DateTime.UtcNow;
            book.UpdatedAt = book.CreatedAt;

            await _db.Books.AddAsync(book, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Book>> Update(Guid id, UpdateBookDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = dto.Title.Trim();
            book.Author = dto.Author.Trim();
            book.Genre = dto.Genre.Trim();
            book.Price = dto.Price;
            book.Stock = dto.Stock;

            ThrowIfInvalid(book);
            await ThrowIfExists(book.Title, book.Author, id, cancellationToken);

            book.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(book);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (book == null)
        {
            return NotFound();
        }

        _db.Books.Remove(book);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static void ThrowIfInvalid(Book book)
    {
        var errors = BookRulesBiz.Validate(book);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }

    private async Task ThrowIfExists(string title, string author, Guid? exceptId, CancellationToken cancellationToken)
    {
        var exists = await _db.Books.AnyAsync(
            b => b.Title == title && b.Author == author && (exceptId == null || b.Id != exceptId),
            cancellationToken);

        if (exists)
        {
            throw new BusinessRuleException("Já existe um livro com o mesmo título e autor.");
        }
    }
}