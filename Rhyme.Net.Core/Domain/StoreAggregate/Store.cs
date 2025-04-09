using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.ProductAggregate;
using Rhyme.Net.Core.Domain.StoreAggregate.Events;

namespace Rhyme.Net.Core.Domain.StoreAggregate;

public class Store : EntityBase<Guid>, IAggregateRoot
{
  public string Name { get; private set; }
  public string? WorkingHours { get; private set; }
  public Guid? MenuId { get; private set; }
  private readonly List<Facility> _facilities = new();
  public IEnumerable<Facility> Facilities => _facilities.AsReadOnly();
  private readonly List<Channel> _channels = new();
  public IEnumerable<Channel> Channels => _channels.AsReadOnly();

  public Store(string name)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
  }

  public Store(string name, string workingHours, Guid menuId)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
    WorkingHours = Guard.Against.NullOrWhiteSpace(workingHours, "workingHours");
    MenuId = Guard.Against.Expression(x => x == Guid.Empty, menuId, "menuId");
  }

  public void UpdateName(string newName)
  {
    Guard.Against.NullOrWhiteSpace(newName, "newName");
    var oldName = Name;
    Name = newName;

    var domainEvent = new StoreNameUpdatedEvent(this, oldName);
    RegisterDomainEvent(domainEvent);
  }

  public void SetWorkingHours(string newWorkingHours)
  {
    WorkingHours = Guard.Against.NullOrWhiteSpace(newWorkingHours, "newWorkingHours");

    var domainEvent = new StoreWorkingHoursUpdatedEvent(this, newWorkingHours);
    RegisterDomainEvent(domainEvent);
  }

  public void AddFacility(Facility facility)
  {
    Guard.Against.Null(facility, "facility");
    _facilities.Add(facility);
  }

  public void SetFacilities(IEnumerable<Facility> facilities)
  {
    Guard.Against.NullOrEmpty(facilities, "facilities");

    _facilities.Clear();
    foreach (var facility in facilities)
      _facilities.Add(facility);
  }

  public void AddChannel(Channel channel)
  {
    Guard.Against.Null(channel, "channel");
    _channels.Add(channel);
  }

  public void SetChannels(IEnumerable<Channel> channels)
  {
    Guard.Against.NullOrEmpty(channels, "channels");

    _channels.Clear();
    foreach (var channel in channels)
      _channels.Add(channel);
  }

  public void UpdateMenuId(Guid newMenuId)
  {
    MenuId = Guard.Against.Expression(x => x == Guid.Empty, newMenuId, "newMenuId");

    var domainEvent = new StoreMenuUpdatedEvent(this);
    RegisterDomainEvent(domainEvent);
  }

  public override string ToString()
  {
    return $"Store Id: {Id}; Name: {Name}; Working hours: {WorkingHours}";
  }
}