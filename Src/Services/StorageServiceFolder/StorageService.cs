using System.Net;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Postgrest;
using Storage.Interfaces;

namespace MaterialeShop.Admin.Src.Services.StorageServiceFolder;

public class StorageService
{
    private readonly Supabase.Client client;
    private readonly ILogger<StorageService> logger;
    private readonly IDialogService dialogService;
    private readonly IStorageClient<Supabase.Storage.Bucket, Supabase.Storage.FileObject> Storage;

    public StorageService(
        Supabase.Client client,
        ILogger<StorageService> logger,
        IDialogService dialogService
    ) : base()
    {
        logger.LogInformation("------------------- CONSTRUCTOR -------------------");
        this.client = client;
        this.logger = logger;
        this.dialogService = dialogService;

        Storage = client.Storage;
    }

    static long maxFileSizeInMB = 15;
    long maxFileSize = 1024 * 1024 * maxFileSizeInMB;

    public async Task<string> UploadFile(IBrowserFile file, String bucketName, String folderName, String fileName)
    {
        Stream streamData = file.OpenReadStream(maxFileSize);

        // TODO: verify if there is a better way to do it
        // Maybe this isn't a good way to do it
        byte[] bytesData = await StreamToBytesAsync(streamData);

        string fileExtesion = file.Name.Split(".").Last();

        String saveName = fileName +"_" + DateTime.Now;

        saveName = saveName.Replace("/", "_").Replace(" ", "_").Replace(":", "_");
        saveName = saveName + "." + fileExtesion;
        
        var bucket = Storage.From(bucketName);

        return await bucket.Upload(bytesData, folderName +"/"+ saveName);
    }

    public async Task<byte[]> StreamToBytesAsync(Stream streamData)
    {
        byte[] bytes;

        using (MemoryStream memoryStream = new MemoryStream())
        {
            await streamData.CopyToAsync(memoryStream);
            bytes = memoryStream.ToArray();
        }

        return bytes;
    }

    public async Task<List<Supabase.Storage.FileObject?>?> GetFilesFromBucket(String bucketName, String folderName)
    {
        return await Storage.From(bucketName).List(folderName);
    }

    public async Task<byte[]> DownloadFile(String bucketName, String folderName, String fileName)
    {
        var bucket = Storage.From(bucketName);
        return await bucket.Download(folderName +"/"+ fileName);
    }
}