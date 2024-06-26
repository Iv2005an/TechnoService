﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class StatisticsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<string> _requestsTypes = new(RequestsService.GetTypes());
    [ObservableProperty]
    private string _selectedRequestType;
    [ObservableProperty]
    private int _allRequestsCount;
    [ObservableProperty]
    private int _completedRequestsCount;
    [ObservableProperty]
    private int _notCompletedRequestsCount;
    [ObservableProperty]
    private int _percentOfCompletedRequests = 0;
    [ObservableProperty]
    private TimeSpan _averageExecutionTime;

    [RelayCommand]
    private async Task ComputeStatistics()
    {
        List<RequestModel> requests = [];
        if (SelectedRequestType == "Все")
            requests = await RequestsService.GetRequests();
        else
            requests = await RequestsService.GetRequests($"type='{SelectedRequestType}';");
        AllRequestsCount = requests.Count;
        CompletedRequestsCount = requests.Where((request) => request.Status == StatusTypes.Completed).Count();
        NotCompletedRequestsCount = requests.Where((request) => request.Status == StatusTypes.NotCompleted).Count();
        if (AllRequestsCount > 0)
        {
            PercentOfCompletedRequests = (int)((double)CompletedRequestsCount / AllRequestsCount * 100);
            List<RequestModel> completedRequests = requests.Where(
                (request) =>
                request.Status == StatusTypes.Completed ||
                request.Status == StatusTypes.NotCompleted)
                .ToList();
            if (completedRequests.Count > 0)
            {
                TimeSpan allTime = new();
                foreach (var request in completedRequests)
                {
                    allTime += (TimeSpan)(request.EndDate - request.StartDate);
                }
                AverageExecutionTime = new TimeSpan(allTime.Ticks / completedRequests.Count);
            }
        }
    }
}
