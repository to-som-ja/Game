namespace Game.Web.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string PlayerId { get; set; }
        public int Seed { get; set; }
        public int Dificulty { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
    }
}
