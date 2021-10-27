namespace Imobilizados.Application.DTOs
{
    public class Hardware
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FactoryCode { get; set; }
        public string Description { get; set; }
        public Floor Floor { get; set; }
    }
}
