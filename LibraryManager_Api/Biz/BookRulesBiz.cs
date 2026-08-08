using LibraryManager.Model;

namespace LibraryManager.Biz;

public static class BookRulesBiz
{
    public static readonly string[] ValidGenres =
    {
        "Ficção", "Romance", "Poesia", "Mistério", "Suspense", "Terror",
        "Ficção Científica", "Fantasia", "Aventura", "História", "Biografia",
        "Infantil", "Tecnologia", "Software Engineering"
    };

    public static List<string> Validate(Book book)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(book.Title))
        {
            errors.Add("O título é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(book.Author))
        {
            errors.Add("O autor é obrigatório.");
        }

        if (book.Price < 0)
        {
            errors.Add("O preço não pode ser negativo.");
        }

        if (book.Stock < 0)
        {
            errors.Add("O estoque não pode ser negativo.");
        }

        if (!IsValidGenre(book.Genre))
        {
            errors.Add($"O gênero '{book.Genre}' não é válido. Gêneros válidos: {string.Join(", ", ValidGenres)}.");
        }

        return errors;
    }

    private static bool IsValidGenre(string? genre)
    {
        return ValidGenres.Any(g => string.Equals(g, genre?.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}