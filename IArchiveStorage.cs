using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordImagesArchiver
{
    interface IArchiveStorage
    {
        void StoreSingleImage(string url, ITextChannel sourceChannel);
        void StoreImages(List<string> urls, ITextChannel sourceChannel);
    }
}
