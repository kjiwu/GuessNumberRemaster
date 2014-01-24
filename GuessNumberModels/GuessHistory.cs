using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GuessNumberModels
{
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    };

    public enum GameState
    {
        Win,
        Loss,
        Giveup
    }

    [Table(Name = "GuessHistory")]
    public class GuessHistory
    {
        public GuessHistory()
        {
            Id = Guid.NewGuid().ToString("n");
        }

        [Column(IsPrimaryKey = true)]
        public string Id
        {
            get;
            set;
        }

        [Column]
        public long TimeSpan { get; set; }

        public string DisplayTime
        {
            get
            {
                System.TimeSpan span = System.TimeSpan.FromTicks(TimeSpan);
                return String.Format("{0}'{1}\"{2}", span.Minutes, span.Seconds, span.Milliseconds);
            }
        }

        [Column]
        public GameState State { get; set; }

        [Column]
        public GameDifficulty Difficulty { get; set; }
    }

    public class GuessHistoryDataContext : DataContext
    {
        public const string connectionString = "Data Source=isostore:/history.sdf";

        public GuessHistoryDataContext()
            : base(connectionString)
        {
        }

        public Table<GuessHistory> Historys;
    }
}
