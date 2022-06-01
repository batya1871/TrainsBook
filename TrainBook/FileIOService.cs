using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace TrainBook
{
    class FileIOService
    {
        private readonly string RoutePath;
        private readonly string TrainPath;
        private readonly string EventPath;

        public FileIOService(string Rpath, string Tpath, string Epath)
        {
            RoutePath = Rpath;
            TrainPath = Tpath;
            EventPath = Epath;
        }
        public BindingList<Route> LoadAllRoots()
        {
            var fileExists = File.Exists(RoutePath);
            if (!fileExists)
            {
                File.CreateText(RoutePath).Dispose();
                return new BindingList<Route>();
            }
            using (var reader = File.OpenText(RoutePath))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Route>>(fileText);
            }

        }

        public void SaveAllRoute(object RouteList)
        {
            using (StreamWriter writer = File.CreateText(RoutePath))
            {
                string output = JsonConvert.SerializeObject(RouteList);
                writer.Write(output);
            }
        }

        public BindingList<Train> LoadAllTrains()
        {
            var fileExists = File.Exists(TrainPath);
            if (!fileExists)
            {
                File.CreateText(TrainPath).Dispose();
                return new BindingList<Train>();
            }
            using (var reader = File.OpenText(TrainPath))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Train>>(fileText);
            }

        }
        public void SaveAllTrains(object TrainList)
        {
            using (StreamWriter writer = File.CreateText(TrainPath))
            {
                string output = JsonConvert.SerializeObject(TrainList);
                writer.Write(output);
            }
        }

        public BindingList<Train> LoadEventsList()
        {
            var fileExists = File.Exists(EventPath);
            if (!fileExists)
            {
                File.CreateText(EventPath).Dispose();
                return new BindingList<Train>();
            }
            using (var reader = File.OpenText(EventPath))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Train>>(fileText);
            }

        }
        public void SaveEventsList(object EventsList)
        {
            using (StreamWriter writer = File.CreateText(EventPath))
            {
                string output = JsonConvert.SerializeObject(EventsList);
                writer.Write(output);
            }
        }
    }
}
