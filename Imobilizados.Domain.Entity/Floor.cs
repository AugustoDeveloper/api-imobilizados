namespace Imobilizados.Domain.Entity
{
    public class Floor : IEntity
    {
        public dynamic Id { get; set; }
        public int Level { get; set; } = -1;
        public string Departament { get; set; }
    }    
}