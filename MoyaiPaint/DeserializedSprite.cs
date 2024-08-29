using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoyaiPaint
{
    class DeserializedSprite
    {
        public int[] size { get; set; }
        public DeserializedLayer[] layers { get; set; }
    }

    class DeserializedSymbol
    {
        public string character { get; set; }
        public int[] color { get; set; }
    }
    class DeserializedLayer
    {
        public string name { get; set; }
        public bool hidden { get; set; }
        public DeserializedSymbol[][] data { get; set; }
        public DeserializedLayer[] children { get; set; }
    }
}
