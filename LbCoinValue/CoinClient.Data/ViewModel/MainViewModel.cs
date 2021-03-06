﻿using CoinClient.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace CoinClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private double _currentCoinValue;
        private bool _isBusy;
        private bool _isDownTrendVsible;
        private bool _isFlatTrendVsible = true;
        private bool _isUpTrendVsible;
        private string _errorMessage;
        private RelayCommand _refreshCommand;
        private ICoinService _service;

        public double CurrentCoinValue
        {
            get
            {
                return _currentCoinValue;
            }
            set
            {
                Set(ref _currentCoinValue, value);
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (Set(ref _isBusy, value))
                {
                    RefreshCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsDownTrendVisible
        {
            get
            {
                return _isDownTrendVsible;
            }
            set
            {
                Set(ref _isDownTrendVsible, value);
            }
        }

        public bool IsFlatTrendVisible
        {
            get
            {
                return _isFlatTrendVsible;
            }
            set
            {
                Set(ref _isFlatTrendVsible, value);
            }
        }

        public bool IsUpTrendVisible
        {
            get
            {
                return _isUpTrendVsible;
            }
            set
            {
                Set(ref _isUpTrendVsible, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                Set(ref _errorMessage, value);
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                    ?? (_refreshCommand = new RelayCommand(
                    async () =>
                    {
                        IsBusy = true;

                        try
                        {
                            var trend = await _service.GetTrend();

                            CurrentCoinValue = trend.CurrentValue;

                            IsUpTrendVisible = trend.Trend > 0;
                            IsFlatTrendVisible = trend.Trend == 0;
                            IsDownTrendVisible = trend.Trend < 0;
                            ErrorMessage = string.Empty;
                        }
                        catch (Exception ex)
                        {
                            CurrentCoinValue = 0;
                            IsUpTrendVisible = false;
                            IsFlatTrendVisible = true;
                            IsDownTrendVisible = false;
                            ErrorMessage = ex.Message;
                        }

                        IsBusy = false;
                    },
                    () => !IsBusy));
            }
        }

        public MainViewModel(ICoinService service)
        {
            _service = service;
        }
    }
}