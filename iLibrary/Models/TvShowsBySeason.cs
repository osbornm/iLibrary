using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iLibrary.Models {
    public class TvShowsBySeason {
        public string ShowName { get; set; }
        public int SeasonNumber { get; set; }
        public int UnwatchedEpisodes { get; set; }
        public int TotalEpisodes { get; set; }
        public string PostArtPath { get; set; }
    }
}