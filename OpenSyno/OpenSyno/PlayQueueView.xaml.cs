﻿using Ninject;

namespace OpenSyno
{
    using System;
    using System.Collections.ObjectModel;

    using Microsoft.Phone.Controls;

    using OpenSyno.ViewModels;

    public partial class PlayQueueView : PhoneApplicationPage
    {
        private const string PlayQueueViewModelKey = "PlayQUeueViewModel";

        private bool _newInstance = false;

        private const string BufferedBytesCountKey = "BufferedBytesCountKey";

        private const string CurrentFileSizeKey = "CurrentFileSizeKey";

        private const string CurrentPlaybackPercentCompleteKey = "CurrentPlaybackPercentCompleteKey";

        private const string CurrentTrackPositionKey = "CurrentTrackPositionKey";

        private const string PlayQueueItemsKey = "PlayQueueItemsKey";

        private const string SelectedTrackKey = "SelectedTrackKey";

        private const string ActiveTrackKey = "ActiveTrackKey";

        private const string VolumeKey = "VolumeKey";

        private const string CurrentArtworkKey = "CurrentArtworkKey";

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayQueueView"/> class.
        /// </summary>
        public PlayQueueView()
        {
            _newInstance = true;
            InitializeComponent();
        }

        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            State[VolumeKey] = ((PlayQueueViewModel)DataContext).Volume;
            State[CurrentArtworkKey] = ((PlayQueueViewModel)DataContext).CurrentArtwork;
            State[ActiveTrackKey] = ((PlayQueueViewModel)DataContext).ActiveTrack;
            State[BufferedBytesCountKey] = ((PlayQueueViewModel)DataContext).BufferedBytesCount;
            State[CurrentFileSizeKey] = ((PlayQueueViewModel)DataContext).CurrentFileSize;
            State[CurrentPlaybackPercentCompleteKey] = ((PlayQueueViewModel)DataContext).CurrentPlaybackPercentComplete;
            State[CurrentTrackPositionKey] = ((PlayQueueViewModel)DataContext).CurrentTrackPosition;
            State[PlayQueueItemsKey] = ((PlayQueueViewModel)DataContext).PlayQueueItems;
            State[SelectedTrackKey] = ((PlayQueueViewModel)DataContext).SelectedTrack;
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var viewModelBase  = IoC.Container.Get<PlayQueueViewModel>();
            DataContext = viewModelBase;

            // Note : PlayQUeueViewModel is not serializable because there are dependencies injected in the constructor,
            // therefore those dependencies are not injected when the state object is being re-hydrated.
            // This is why we either need to make private .ctor injections public OR do the job of re-hydratation manually.
            // Here, we are going to do the latter.
            
            // We could imagine a service that would do it automatically, based on a Dictionary<PropertyInfo, object> and reflection.
            if (_newInstance)
            {
                if (this.State.ContainsKey(CurrentArtworkKey))
                {
                    viewModelBase.CurrentArtwork = (Uri)this.State[CurrentArtworkKey];
                }

                if (this.State.ContainsKey(VolumeKey))
                {
                    viewModelBase.Volume = (double)this.State[VolumeKey];
                }

                if (this.State.ContainsKey(ActiveTrackKey))
                {
                    viewModelBase.ActiveTrack = (TrackViewModel)this.State[ActiveTrackKey];
                }

                if (this.State.ContainsKey(BufferedBytesCountKey))
                {
                    viewModelBase.BufferedBytesCount = (long)this.State[BufferedBytesCountKey];
                }

                if (this.State.ContainsKey(CurrentFileSizeKey))
                {
                    viewModelBase.CurrentFileSize = (long)this.State[CurrentFileSizeKey];
                }

                if (this.State.ContainsKey(CurrentPlaybackPercentCompleteKey))
                {
                    viewModelBase.CurrentPlaybackPercentComplete = (double)this.State[CurrentPlaybackPercentCompleteKey];
                }

                if (this.State.ContainsKey(CurrentTrackPositionKey))
                {
                    viewModelBase.CurrentTrackPosition = (TimeSpan)this.State[CurrentTrackPositionKey];
                }

                if (this.State.ContainsKey(PlayQueueItemsKey))
                {
                    viewModelBase.PlayQueueItems = (ObservableCollection<TrackViewModel>)this.State[PlayQueueItemsKey];
                }

                if (this.State.ContainsKey(SelectedTrackKey))
                {
                    viewModelBase.SelectedTrack = (TrackViewModel)this.State[SelectedTrackKey];
                }
            }
                       
            base.OnNavigatedTo(e);
        }
    }
}