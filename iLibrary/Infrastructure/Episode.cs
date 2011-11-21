using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace iLibrary.Infrastructure {
    public class Episode : iTunesTrack {

        public string DisplayName { get; private set; }
        public string Runtime { get; private set; }
        public string Description { get; private set; }
        public bool Unplayed { get; private set; }
        public int EpisodeNumber { get; private set; }
        public int PlayCount { get; private set; }
        public int BookMarkTime { get; set; }


        public Episode(int id, string pathToFile, string pathToPosterArt, string displayName, string runtime, 
                       string description, bool unplayed, int playCount, int episodeNumber, int bookmarkTime)
            : base(id, pathToFile, pathToPosterArt) {
                DisplayName = displayName;
                Runtime = runtime;
                Description = description;
                Unplayed = unplayed;
                PlayCount = playCount;
                EpisodeNumber = episodeNumber;
                BookMarkTime = bookmarkTime;
                
        }
    }
}