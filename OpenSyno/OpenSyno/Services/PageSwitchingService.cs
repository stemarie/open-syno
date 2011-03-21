﻿using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Practices.Prism.Events;
using OpenSyno.ViewModels;
using Synology.AudioStationApi;

namespace OpenSyno.Services
{
    public class PageSwitchingService : IPageSwitchingService
    {
        private readonly NavigationService _navigationService;

        private const string AboutBoxUri = "/AboutBoxView.xaml";

        private const string PlayQueueResultsUri = "/PlayQueueView.xaml";

        private const string ArtistPanoramaUri = "/ArtistPanoramaView.xaml";
        private const string SearchResultsUri = "/SearchResultsView.xaml";

        public PageSwitchingService(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void NavigateToSearchResults()
        {
            _navigationService.Navigate(new Uri(SearchResultsUri, UriKind.RelativeOrAbsolute));                        
        }

        public void NavigateToArtistPanorama()
        {
            _navigationService.Navigate((new Uri(ArtistPanoramaUri, UriKind.RelativeOrAbsolute)));
        }

        public void NavigateToPreviousPage()
        {
            _navigationService.GoBack();
        }

        public void NavigateToAboutBox()
        {
            _navigationService.Navigate(new Uri(AboutBoxUri, UriKind.RelativeOrAbsolute));                        
        }

        public void NavigateToPlayQueue()
        {
            _navigationService.Navigate(new Uri(PlayQueueResultsUri, UriKind.RelativeOrAbsolute));                        

        }
    }
}