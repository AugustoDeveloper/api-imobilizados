namespace Imobilizados.Domain.Entity
{
    public class Floor : IEntity
    {
        public object Id { get; set; }
        public int Level { get; set; } = -1;
        public string LevelName { get; set; }
    }    
}