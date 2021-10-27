namespace Imobilizados.Domain.Entities
{
    public interface IHardware
    {
        string Id { get; set; }
        string Name { get; set; }
        string Brand { get; set; }
        string FactoryCode { get; set; }
        string Description { get; set; }

        bool IsImmobilized();
    }
}
