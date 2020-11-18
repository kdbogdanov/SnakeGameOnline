using System.Collections.Generic;

namespace Client.Models
{
    public class GameStateResponse
    {
        public bool IsStarted { get; set; }
        public bool IsPaused { get; set; }
        public int RoundNumber { get; set; }
        public int TurnNumber { get; set; }
        public int TurnTimeMilleseconds { get; set; }
        public int TimeUntilNextTurnMilleseconds { get; set; }
        public Size2D GameBoardSize { get; set; }
        public int MaxFood { get; set; }
        public List<PlayerState> Players { get; set; }
        public List<Point2D> Food { get; set; }
        public List<Rectangle2D> Walls { get; set; }
        public List<Point2D> Snake { get; set; }
    }
}
