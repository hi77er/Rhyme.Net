namespace Rhyme.Net.UseCases.Menus;

public record MenuForMenuBoardDTO(Guid Id, string Title, IEnumerable<BrochureDTO> brochures);
