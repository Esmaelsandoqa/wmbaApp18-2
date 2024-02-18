using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace wmbaApp.Models
{
    public class PlayersLineUp
    {
        public int Id { get; set; }
        
        [ForeignKey("Game")]
        public int GameId { get; set; }
        public Game Game { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public Team Team  { get; set; }
        [ForeignKey("CompetitorTeam")]
        public int CompetitorTeamId { get; set; }
        public Team CompetitorTeam { get; set; }
        public ICollection<Player> Players { get; set; }=new HashSet<Player>();
       
        public ICollection<Player> CompetatorPlayers { get; set; } = new HashSet<Player>();
    }
}
