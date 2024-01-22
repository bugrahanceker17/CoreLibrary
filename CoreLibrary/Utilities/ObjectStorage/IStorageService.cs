using CoreLibrary.Utilities.ObjectStorage.Base;

namespace CoreLibrary.Utilities.ObjectStorage
{
    public interface IStorageService : IStorage
    {
        public string StorageName { get; }
    }
}

