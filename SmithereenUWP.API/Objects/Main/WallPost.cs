using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SmithereenUWP.API.Objects.Main
{
    public enum WallPostPrivacy
    {
        [EnumMember(Value = "followers")]
        Followers,

        [EnumMember(Value = "followers_and_mentioned")]
        FollowersAndMentioned,

        [EnumMember(Value = "friends")]
        Friends
    }

    public sealed class WallPostLikesInfo
    {
        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("can_like")]
        public bool CanLike { get; private set; }

        [JsonProperty("user_likes")]
        public bool UserLikes { get; private set; }
    }

    public sealed class WallPostCommentsInfo
    {
        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("can_post")]
        public bool CanPost { get; private set; }
    }

    public sealed class WallPostRepostsInfo
    {
        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("can_repost")]
        public bool CanRepost { get; private set; }

        [JsonProperty("user_reposted")]
        public bool UserReposted { get; private set; }
    }

    public sealed class WallPostThread
    {
        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("reply_count")]
        public int ReplyCount { get; private set; }

        [JsonProperty("items")]
        public List<WallPost> Items { get; private set; }
    }

    public sealed class WallPost
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("ap_id")]
        public string ActivityPubId { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("from_id")]
        public int FromId { get; private set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }

        [JsonProperty("text")]
        public string Text { get; private set; }

        [JsonProperty("date")]
        public long Date { get; private set; }

        [JsonProperty("privacy")]
        public WallPostPrivacy Privacy { get; private set; }

        [JsonProperty("likes")]
        public WallPostLikesInfo Likes { get; private set; }

        [JsonProperty("reposts")]
        public WallPostRepostsInfo Reposts { get; private set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; private set; }

        [JsonProperty("content_warning")]
        public string ContentWarning { get; private set; }

        [JsonProperty("can_delete")]
        public bool CanDelete { get; private set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; private set; }

        [JsonProperty("mentioned_users")]
        public List<int> MentionedUsers { get; private set; }

        [JsonProperty("comments")]
        public WallPostCommentsInfo Comments { get; private set; }

        [JsonProperty("repost_history")]
        public List<WallPost> RepostHistory { get; private set; }

        [JsonProperty("is_mastodon_style_repost")]
        public bool IsMastodonStyleRepost { get; private set; }

        [JsonProperty("can_pin")]
        public bool CanPin { get; private set; }

        [JsonProperty("is_pinned")]
        public bool IsPinned { get; private set; }

        [JsonProperty("parents_stack")]
        public List<int> ParentsStack { get; private set; }

        [JsonProperty("reply_to_comment")]
        public int ReplyToComment { get; private set; }

        [JsonProperty("reply_to_user")]
        public int ReplyToUser { get; private set; }

        [JsonProperty("thread")]
        public WallPostThread Thread { get; private set; }
    }
}
