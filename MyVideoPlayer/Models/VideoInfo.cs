namespace MyVideoPlayer.Models
{
    public class VideoInfo
    { 
        public string Title { get; set; }
        public byte[] Thumbnail { get; set; }
        public string FilePath { get; set; }
        public string UserName { get; set; }
        public int Version { get; set; }
    }
}
