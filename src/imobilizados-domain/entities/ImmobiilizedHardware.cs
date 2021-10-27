namespace Imobilizados.Domain.Entities
{
    public class ImmobilizedHardware : IHardware
    {
        public Floor Floor { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FactoryCode { get; set; }
        public string Description { get; set; }

        public ImmobilizedHardware() { }

        public ImmobilizedHardware(Floor floor) 
        {
            this.Floor = floor;
        } 

        public ImmobilizedHardware(IHardware hardware, Floor floor) : this(floor)
        { 
            this.Id = hardware.Id;
            this.Brand = hardware.Brand;
            this.Description = hardware.Description;
            this.FactoryCode = hardware.FactoryCode;
            this.Name = hardware.Name;
        }

        public bool IsImmobilized()
            => true;
    }
}
