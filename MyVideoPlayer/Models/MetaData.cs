using System;

namespace MyVideoPlayer.Models
{
    public class MetaData
    {
        public string title;
        public Uri uri;
        public string thumbnailFilePath;

        public MetaData(string title, Uri uri, string thumbnailFilePath)
        {
            this.title = title;
            this.uri = uri;
            this.thumbnailFilePath = thumbnailFilePath;
        }
    }
}
