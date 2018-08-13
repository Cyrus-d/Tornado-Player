﻿namespace Tornado.Player.ViewModels
{
    using System.Linq;

    using Caliburn.Micro;

    using Tornado.Player.Factories.Interfaces;
    using Tornado.Player.Services.Interfaces;
    using Tornado.Player.ViewModels.Interfaces;

    internal class PlaylistViewModel : ViewModelBase, IPlaylistViewModel
    {
        private readonly IMusicPlayerService _musicPlayerService;

        public PlaylistViewModel(ITrackFactory trackFactory, IMusicPlayerService musicPlayerService)
        {
            _musicPlayerService = musicPlayerService;

            Tracks = new BindableCollection<ITrackViewModel>(_musicPlayerService.Tracks.Select(trackFactory.MakeTrackViewModel));

            _musicPlayerService.PlaylistLoaded += (sender, e) =>
            {
                Tracks.Clear();
                Tracks.AddRange(e.Tracks.Select(trackFactory.MakeTrackViewModel));
            };

            _musicPlayerService.TrackChanged += (sender, e) => SelectedIndex = e.TrackIndex;
        }

        public IObservableCollection<ITrackViewModel> Tracks { get; }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;

            set
            {
                if (_selectedIndex == value) return;

                _selectedIndex = value;
                NotifyOfPropertyChange(() => SelectedIndex);

                _musicPlayerService.SelectTrack(_selectedIndex);
            }
        }
    }
}