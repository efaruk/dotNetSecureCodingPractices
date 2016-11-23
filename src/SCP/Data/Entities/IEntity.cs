namespace SCP.Data.Entities
{
    public interface IEntity<TId> where TId : struct
    {
        TId Id { get; set; }
    }
}