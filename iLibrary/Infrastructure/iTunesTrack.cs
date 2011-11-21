using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iLibrary.Infrastructure {
    public abstract class iTunesTrack {
        public int ID { get; private set; }
        public string PathToFile { get; private set; }
        public string PathToPosterArt { get; private set; }

        public iTunesTrack(int id, string pathToFile, string pathToPosterArt) {
            ID = id;
            PathToFile = pathToFile;
            PathToPosterArt = pathToPosterArt;
        }
    }
}