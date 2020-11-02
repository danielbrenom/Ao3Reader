using System;
using System.Threading.Tasks;
using Ao3Domain.Models.Data;
using Ao3Reader.Interfaces;
using Newtonsoft.Json;
using PCLStorage;

namespace Ao3Reader.Services
{
    public class LocalStorage: ILocalStorage
    {
        private StorageData StorageData { get; set; } = new StorageData();

        public StorageData GetStorage() => StorageData;

        public async Task SaveFile()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var folder = await rootFolder.CreateFolderAsync("Storage", CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync("cache.dat", CreationCollisionOption.ReplaceExisting);
            var userData = JsonConvert.SerializeObject(StorageData);
            await file.WriteAllTextAsync(userData);
        }
        public async Task LoadFile()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var storageFolder = await rootFolder.CreateFolderAsync("Storage", CreationCollisionOption.OpenIfExists);
            var fileExists = await storageFolder.CheckExistsAsync("cache.dat");
            if (fileExists != ExistenceCheckResult.FileExists)
            {
                return;
            }
            try
            {
                var file = await storageFolder.GetFileAsync("cache.dat");
                var data = await file.ReadAllTextAsync();
                StorageData = JsonConvert.DeserializeObject<StorageData>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task DeleteFile()
        {
            throw new System.NotImplementedException();
        }
    }
}