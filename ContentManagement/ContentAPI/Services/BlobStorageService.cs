﻿using Azure.Storage.Blobs;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Services.Interfaces;

namespace ContentAPI.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageRepo _blobStorageRepo;

        public BlobStorageService(IBlobStorageRepo blobStorageRepo)
        {
            _blobStorageRepo = blobStorageRepo;
        }

        public string AddJpgImage(IFormFile image)
        {
            return _blobStorageRepo.AddJpgImage(image);
        }
    }
}
