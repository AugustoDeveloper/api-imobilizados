using System.Collections.Generic;

namespace Imobilizados.Application.DTOs
{
    public class HardwareCollection : List<Hardware>
    {
        public static HardwareCollection Empty => new HardwareCollection();

        public HardwareCollection() : base() { }
        public HardwareCollection(IEnumerable<Hardware> collection) : base(collection) { }
    }
}
