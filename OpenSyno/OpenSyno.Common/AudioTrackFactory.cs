﻿using System;
using System.Net;
using Microsoft.Phone.BackgroundAudio;
using OpenSyno.Services;

namespace OpenSyno.Common
{
    using System.Linq;
    using System.Threading;

    using Newtonsoft.Json.Linq;

    using Synology.AudioStationApi;

    public class AudioTrackFactory : IAudioTrackFactory
    {
        private IVersionDependentResourcesProvider versionDependentResourcesProvider;

        public AudioTrackFactory(IVersionDependentResourcesProvider versionDependentResourcesProvider)
        {
            this.versionDependentResourcesProvider = versionDependentResourcesProvider;
        }

        #region Implementation of IAudioTrackFactory

        public AudioTrack Create(SynoTrack baseSynoTrack, Guid guid, string host, int port, string token)
        {
            if (baseSynoTrack == null)
            {
                throw new ArgumentNullException("baseSynoTrack");
            }
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            // hack : Synology's webserver doesn't accept the + character as a space : it needs a %20, and it needs to have special characters such as '&' to be encoded with %20 as well, so an HtmlEncode is not an option, since even if a space would be encoded properly, an ampersand (&) would be translated into &amp;
            var relativePathToAudioStreamService = this.versionDependentResourcesProvider.GetAudioStreamWebserviceRelativePath(DsmVersions.V4_0);
            string url =
                string.Format(
                    "http://{0}:{1}{2}/0.mp3?sid={3}&action=streaming&songpath={4}",
                    host,
                    port,
                    relativePathToAudioStreamService,
                    token.Split('=')[1],
                    HttpUtility.UrlEncode(baseSynoTrack.Res).Replace("+", "%20"));

            return Create(baseSynoTrack, guid,host, port, token, url);
            // ugly fix for backgroundAudioPlayer that does not support & > 7bits characters
            //if (baseSynoTrack.Res.Contains("&") || baseSynoTrack.Res.Any(o => o > 127))
            //{
            //    // Use url-shortening service.
            //    // http://t0.tv/api/shorten?u=<url>
            //    WebClient webClient = new WebClient();
            //    string result = null;
            //    var mre = new ManualResetEvent(false);
            //    webClient.DownloadStringCompleted += (s, e) =>
            //        {
            //            result = e.Result;

            //            mre.Set();
            //        };
            //    webClient.DownloadStringAsync(new Uri("http://t0.tv/api/shorten?u=" + url ));
            //    mre.WaitOne();
            //    JObject jo = JObject.Parse(result);
            //    url = jo["short_url"].Value<string>();
            //}


        }

        public AudioTrack Create(SynoTrack baseSynoTrack, Guid guid, string host, int port, string token, string urlOverride)
        {
            if (baseSynoTrack == null)
            {
                throw new ArgumentNullException("baseSynoTrack");
            }
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            if (urlOverride == null)
            {
                throw new ArgumentNullException("urlOverride");
            }
            return new AudioTrack(
                new Uri(urlOverride),
                baseSynoTrack.Title,
                baseSynoTrack.Artist,
                baseSynoTrack.Album,
                new Uri(baseSynoTrack.AlbumArtUrl),
                guid.ToString(),
                EnabledPlayerControls.All);
        }

        #endregion
    }
}