namespace BuildingBlock.Messaging.Events;    
public record IntegrationEvents
{
    /// <summary>
    /// Unique Identifier for each events
    /// </summary>
    public Guid Id = Guid.NewGuid();
    /// <summary>
    /// Timestamp for the event 
    /// </summary>
    public DateTime OccuranceOn => DateTime.Now;
   /// <summary>
   /// Event Type 
   /// </summary>
    public string EventType =>  GetType().AssemblyQualifiedName;
}
