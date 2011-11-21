using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iLibrary.Infrastructure {
    public class TvShow {
        public string Name { get; set; }
        public string SortName { get; set; }
        public string Genre { get; set; }
        public int Season { get; set; }
        public string PosterArtPath { get; set; }
        public List<Episode> Episodes { get; set; }

        public TvShow(string name, string sortName, int season, string posterArtPath) {
            Name = name;
            SortName = sortName;
            Season = season;
            Episodes = new List<Episode>();
            PosterArtPath = posterArtPath;
        }
    }
}