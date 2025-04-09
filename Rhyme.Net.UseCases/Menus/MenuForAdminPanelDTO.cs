namespace Rhyme.Net.UseCases.Menus;

public record MenuForAdminPanelDTO(Guid Id, string Title, IEnumerable<BrochureDTO> brochures);
