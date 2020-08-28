namespace Order.Core.Entities.Base
{
   public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}
