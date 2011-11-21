using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iLibrary.Infrastructure;
using iLibrary.Models;
using System.IO;

namespace iLibrary.Controllers
{
    public class TvShowsController : Controller
    {
        public ActionResult Index(ShowFilter iLibraryFilter = ShowFilter.all) {
            List<TvShowsBySeason> ShowsBySeason = new List<TvShowsBySeason>();

            foreach (var s in ItunesCache.TvShows) {
                ShowsBySeason.Add(new TvShowsBySeason() {
                    ShowName = s.Name,
                    SeasonNumber = s.Season,
                    TotalEpisodes = s.Episodes.Count,
                    UnwatchedEpisodes = s.Episodes.Where(e => e.PlayCount < 1).Count(),
                    PostArtPath = s.PosterArtPath
                });
            }

            if (iLibraryFilter == ShowFilter.unwatched) {
                return View(ShowsBySeason.Where(s => s.UnwatchedEpisodes > 0).ToList());
            }

            return View(ShowsBySeason);

        }

        [HttpPost]
        public ActionResult Index(bool unwatched = false, bool all = false) {
            if (unwatched) {
                Response.Cookies.Add(new HttpCookie("iLibraryFilter", "unwatched"));
            }
            if (all) {
                Response.Cookies.Add(new HttpCookie("iLibraryFilter", "all"));
            }
            
            return RedirectToAction("index");
        }

        public ActionResult Show(string ShowName) {
            List<TvShowsBySeason> TvShowBySeason = new List<TvShowsBySeason>();

            foreach (var s in ItunesCache.TvShows.Where(t => t.Name == ShowName)) {
                TvShowBySeason.Add(new TvShowsBySeason() {
                    ShowName = s.Name,
                    SeasonNumber = s.Season,
                    TotalEpisodes = s.Episodes.Count,
                    UnwatchedEpisodes = s.Episodes.Where(e => e.Unplayed).Count(),
                    PostArtPath = s.PosterArtPath
                });
            }

            return View(TvShowBySeason);
        }

        public ActionResult Season(string ShowName, int Season) {
            TvShow tvShow = ItunesCache.TvShows.Where(s => s.Name == ShowName && s.Season == Season).Single();
            return View(tvShow);
        }

        public ActionResult Episode(string ShowName, int Season, int Episode) {
            TvShow tvShow = ItunesCache.TvShows.Where(s => s.Name == ShowName && s.Season == Season).Single();
            var episodes = tvShow.Episodes.Where(e => e.EpisodeNumber == Episode);
            return View(episodes);
        }

        [HttpPost]
        public ActionResult Episode(string ShowName, int Season, int Episode, int ID) {
            ItunesCache.MarkAsWatched(ID);
            TvShow tvShow = ItunesCache.TvShows.Where(s => s.Name == ShowName && s.Season == Season).Single();
            var episodes = tvShow.Episodes.Where(e => e.EpisodeNumber == Episode);
            return View(episodes);
        }
    }
}
