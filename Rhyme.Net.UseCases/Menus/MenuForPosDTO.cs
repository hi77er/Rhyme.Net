namespace Rhyme.Net.UseCases.Menus;

public record MenuForPosDTO(Guid Id, string Title, IEnumerable<BrochureDTO> brochures);
