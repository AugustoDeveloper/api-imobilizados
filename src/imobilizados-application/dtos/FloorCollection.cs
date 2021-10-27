using System.Collections.Generic;

namespace Imobilizados.Application.DTOs
{
    public class FloorCollection : List<Floor>
    {
        public static FloorCollection Empty => new FloorCollection();

        public FloorCollection() : base() { }
        public FloorCollection(IEnumerable<Floor> collection) : base(collection) { }
    }
}
