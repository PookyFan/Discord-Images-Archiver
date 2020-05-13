using Discord;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DiscordImagesArchiver
{
    class LocalArchiveStorage : IArchiveStorage
    {
        private string rootDirectory;
        private WebClient downloader;

        public LocalArchiveStorage(string storageRootDirectory)
        {
            rootDirectory = storageRootDirectory + (storageRootDirectory.EndsWith('\\') ? "" : "\\"); ;
            downloader = new WebClient();
        }

        public void StoreSingleImage(string url, ITextChannel sourceChannel)
        {
            string fileName = url.Split('/')[^1];
            string path = $"{rootDirectory}\\{sourceChannel.Guild.Name}\\{sourceChannel.Name}\\";
            try
            {
                System.IO.Directory.CreateDirectory(path);
                downloader.DownloadFile(url, path + fileName);
                App.Log(LogLevel.Debug, $"File {fileName} saved in {path}");
            }
            catch(Exception e)
            {
                App.Log(LogLevel.Error, $"[{e.GetType().Name}] Couldn't download image from {url} - the error was: {e.Message}");
            }
        }

        public void StoreImages(List<string> urls, ITextChannel sourceChannel)
        {
            foreach(string url in urls)
                StoreSingleImage(url, sourceChannel);
        }
    }
}
