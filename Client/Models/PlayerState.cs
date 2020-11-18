using System.Collections.Generic;

namespace Client.Models
{
    public class PlayerState
    {
        public string Name { get; set; }
        public bool IsSpawnPtotected { get; set; }
        public List<Point2D> Snake { get; set; }
    }
}
