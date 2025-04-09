namespace Rhyme.Net.UseCases.Menus;

public record MenuForMobileDTO(Guid Id, string Title, IEnumerable<BrochureDTO> brochures);
