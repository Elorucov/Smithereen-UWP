using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Main
{
    public sealed class BoardTopic
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("group_id")]
        public int GroupId { get; private set; }

        [JsonProperty("ap_id")]
        public string ActivityPubId { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("created")]
        public long Created { get; private set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; private set; }

        [JsonProperty("updated")]
        public long Updated { get; private set; }

        [JsonProperty("updated_by")]
        public int UpdatedBy { get; private set; }

        [JsonProperty("is_closed")]
        public bool IsClosed { get; private set; }

        [JsonProperty("is_pinned")]
        public bool IsPinned { get; private set; }

        [JsonProperty("comments")]
        public int Comments { get; private set; }

        [JsonProperty("comment_preview")]
        public string CommentPreview { get; private set; }
    }
}
