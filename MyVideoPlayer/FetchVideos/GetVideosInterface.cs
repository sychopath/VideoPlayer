using MyVideoPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoPlayer.FetchVideos
{
    public interface IGetVideos
    {
        public List<VideoInfo> Fetch();
    }
}
