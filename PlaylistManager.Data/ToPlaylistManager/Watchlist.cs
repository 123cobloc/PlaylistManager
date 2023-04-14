namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Watchlist
    {
        public string UserId { get; set; }
        public string ItemId { get; set; }
        public ItemType ItemType { get; set; }

        public Watchlist(string userId, string itemId, ItemType itemType)
        {
            UserId = userId;
            ItemId = itemId;
            ItemType = itemType;
        }
    }

    public enum ItemType
    {
        Album,
        Artist,
        Playlist
    }
}
