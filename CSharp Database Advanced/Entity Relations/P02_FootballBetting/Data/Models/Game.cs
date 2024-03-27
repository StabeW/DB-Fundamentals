using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        public Game()
        {
            PlayersStatistics = new HashSet<PlayerStatistic>();
            Bets = new HashSet<Bet>();
            Result = $"{HomeTeamGoals}:{AwayTeamGoals}";
        }

        public int GameId { get; set; }

        public int HomeTeamId { get; set; }
        [NotMapped]
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        [NotMapped]
        public Team AwayTeam { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public DateTime DateTime { get; set; }

        public double HomeTeamBetRate { get; set; }

        public double AwayTeamBetRate { get; set; }

        public double DrawBetRate { get; set; }

        public string Result { get; set; }

        public ICollection<PlayerStatistic> PlayersStatistics { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }
}