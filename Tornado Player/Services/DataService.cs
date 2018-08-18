﻿namespace Tornado.Player.Services
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    using Tornado.Player.Services.Interfaces;

    internal class DataService : IDataService
    {
        private readonly IAppDataService _appDataService;

        public DataService(IAppDataService appDataService)
        {
            _appDataService = appDataService;
        }

        public T Load<T>(string dataName, string emptyData = "")
        {
            return Load<T>(dataName, () => emptyData);
        }

        public T Load<T>(string dataName, Func<string> emptyData)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(_appDataService.GetFile($"Data/{dataName}.json", emptyData)));
        }

        public void Save<T>(string dataName, T data)
        {
            File.WriteAllText(_appDataService.GetFile($"Data/{dataName}.json"), JsonConvert.SerializeObject(data));
        }
    }
}