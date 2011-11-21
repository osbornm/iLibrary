using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iLibrary.Infrastructure {
    public class Movie {
        public int TrackDatabaseId { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Runtime { get; set; }
        public string Description { get; set; }
        public string PosterArtPath { get; set; }

        public Movie(int trackId, string fileName, string name, string RunTime, string description, string posterArtPath) {
            TrackDatabaseId = trackId;
            FileName = fileName;
            Name = name;
            Runtime = RunTime;
            Description = description;
            PosterArtPath = posterArtPath;
        }
    }
}