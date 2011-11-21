using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTunesLib;
using System.Web.Hosting;
using System.IO;
using System.Drawing;
using System.Web.Helpers;

namespace iLibrary.Infrastructure {
    public class ItunesCache {
        private static string virtualPathName = "~/Media";
        private static bool refreshTvShowPosterArt = false;
        private static bool refreshMoviePosterArt = false;

        private static List<IITFileOrCDTrack> _alliTunesTracks;
        public static List<IITFileOrCDTrack> AlliTunesTracks {
            get {
                if (_alliTunesTracks == null) {
                    try {
                        iTunesAppClass iTunes = new iTunesAppClass();
                        _alliTunesTracks = new List<IITFileOrCDTrack>();
                        foreach (IITTrack t in iTunes.LibraryPlaylist.Tracks) {
                            if (t.Kind == ITTrackKind.ITTrackKindFile) {
                                _alliTunesTracks.Add((IITFileOrCDTrack)t);
                            }
                        }
                    }
                    catch {
                        _alliTunesTracks = null;
                    }
                }               
                return _alliTunesTracks;
            }
        }

        private static List<TvShow> _tvShows;
        public static List<TvShow> TvShows {
            get {
                if (_tvShows == null) {
                    try {
                        _tvShows = new List<TvShow>();
                        var itunesTvShows = AlliTunesTracks.Where(t => t.VideoKind == ITVideoKind.ITVideoKindTVShow);
                        var shows = itunesTvShows.GroupBy(s => s.SortShow ?? s.Show);
                        foreach (var show in shows) {
                            var seasons = show.GroupBy(t => t.SeasonNumber);
                            foreach (var season in seasons) {
                                List<Episode> episodesInSeason = new List<Episode>();
                                foreach (var episode in season) {
                                    var pathToFile = GetVirtualPath(episode.Location);
                                    //Save Poster Art
                                    var posterArtFileName = String.Format("{0}.{1}.jpg", show.Key, season.Key);
                                    var posterArtRelativePath = Path.Combine("~/Content/Images/TvShows/", posterArtFileName);
                                    var posterArtAbsolutePath = HttpContext.Current.Server.MapPath(posterArtRelativePath);
                                    if (refreshTvShowPosterArt || !File.Exists(posterArtAbsolutePath)) {
                                        if (episode.Artwork.Count > 0) {
                                            episode.Artwork[1].SaveArtworkToFile(posterArtAbsolutePath);
                                            WebImage img = new WebImage(posterArtRelativePath);
                                            img.Resize(200, 200, preserveAspectRatio: true).Save();
                                        }
                                    }
                                    episodesInSeason.Add(new Episode(episode.TrackDatabaseID, pathToFile, posterArtRelativePath,
                                                                     episode.Name, episode.Time, episode.Description,
                                                                     episode.Unplayed, episode.PlayedCount, episode.EpisodeNumber, episode.BookmarkTime));
                                }

                                var tvShowSeason = new TvShow(show.First().Show, show.Key, season.Key,
                                                              episodesInSeason.Where(e => !String.IsNullOrEmpty(e.PathToPosterArt)).FirstOrDefault().PathToPosterArt);

                                tvShowSeason.Episodes = episodesInSeason.OrderBy(e => e.EpisodeNumber).ToList();
                                _tvShows.Add(tvShowSeason);
                            }
                        }
                        _tvShows = _tvShows.OrderBy(tss => tss.Name).ThenBy(tss => tss.Season).ToList();
                        if (refreshTvShowPosterArt) {
                            refreshTvShowPosterArt = false;
                        }
                    }
                    catch {
                        _tvShows = null;
                    }
                }
                return _tvShows;
            }
        }

        private static List<Movie> _movies;
        public static List<Movie> Movies {
            get {
                if (_movies == null) {
                    try {
                        _movies = new List<Movie>();
                        var itunesMovies = AlliTunesTracks.Where(t => t.VideoKind == ITVideoKind.ITVideoKindMovie);
                        foreach (var m in itunesMovies) {
                            var path = GetVirtualPath(m.Location);
                            var posterArtName = String.Format("{0}.jpg", m.SortName ?? m.Name);
                            var posertArtPath = Path.Combine("~/Content/Images/Movies/", posterArtName);
                            var posterArtFullPath = HttpContext.Current.Server.MapPath(posertArtPath);
                            if (refreshMoviePosterArt || !File.Exists(posterArtFullPath)) {
                                if (m.Artwork.Count > 0) {
                                    m.Artwork[1].SaveArtworkToFile(posterArtFullPath);
                                    WebImage img = new WebImage(posertArtPath);
                                    img.Resize(200, 200, preserveAspectRatio: true).Save();
                                }
                            }
                            _movies.Add(new Movie(m.TrackDatabaseID, path, m.Name, m.Time, m.Description, posertArtPath));
                        }
                        if (refreshMoviePosterArt) {
                            refreshMoviePosterArt = false;
                        }
                    }
                    catch {
                        _movies = null;
                    }
                }
                return _movies;
            }
        }

        public static void RefreshData() {
            _alliTunesTracks = null;
            _tvShows = null;
            _movies = null;
        }

        public static void RefreshPosterArt() {
            refreshTvShowPosterArt = true;
            refreshMoviePosterArt = true;
        }

        public static void RefreshAll() {
            _alliTunesTracks = null;
            _tvShows = null;
            _movies = null;
            refreshTvShowPosterArt = true;
            refreshMoviePosterArt = true;
        }

        public static void MarkAsWatched(int trackId) {
            var track = _alliTunesTracks.Where(e => e.TrackDatabaseID == trackId).FirstOrDefault();
            track.Unplayed = false;
            track.PlayedCount += 1;
            RefreshData();
        }

        public static string GetVirtualPath(string path) {
            string appPath = HttpContext.Current.Server.MapPath(virtualPathName);
            if (!appPath.Contains(HttpContext.Current.Server.MapPath("~/"))) {
                return string.Format("~/Media{0}", path.Replace(appPath, "").Replace("\\", "/"));
            }
            return path;
        }

    }
}