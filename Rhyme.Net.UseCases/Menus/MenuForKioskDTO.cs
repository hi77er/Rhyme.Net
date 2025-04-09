namespace Rhyme.Net.UseCases.Menus;

public record MenuForKioskDTO(Guid Id, string Title, IEnumerable<BrochureDTO> brochures);
