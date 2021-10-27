namespace Imobilizados.Domain.Entities
{
    public class Hardware : IHardware
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FactoryCode { get; set; }
        public string Description { get; set; }

        public bool IsImmobilized()
            => false;
    }
}
