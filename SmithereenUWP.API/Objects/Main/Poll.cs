using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Main
{
    public sealed class PollAnswer
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("text")]
        public string Votes { get; private set; }

        [JsonProperty("rate")]
        public float Rate { get; private set; }
    }

    public sealed class Poll
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }

        [JsonProperty("author_id")]
        public int AuthorId { get; private set; }

        [JsonProperty("created")]
        public long Created { get; private set; }

        [JsonProperty("question")]
        public string Question { get; private set; }

        [JsonProperty("votes")]
        public int Votes { get; private set; }

        [JsonProperty("answers")]
        public List<PollAnswer> ANswers { get; private set; }

        [JsonProperty("anonymous")]
        public bool Anonymous { get; private set; }

        [JsonProperty("multiple")]
        public bool Multiple { get; private set; }

        [JsonProperty("answer_ids")]
        public List<int> AnswerIds { get; private set; }

        [JsonProperty("end_date")]
        public long EndDate { get; private set; }

        [JsonProperty("closed")]
        public bool Closed { get; private set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; private set; }

        [JsonProperty("can_vote")]
        public bool CanVote { get; private set; }
    }
}
