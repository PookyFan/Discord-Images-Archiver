﻿using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordImagesArchiver
{
    public class DiscordImagesScanner
    {
        public DiscordImagesScanner()
        {
        }

        public List<string> GetAllImagesUrlsFromChannelMessages(ITextChannel channel)
        {
            List<string> res = null;
            Thread t = new Thread(() => res = GetAllImagesUrlsFromChannelMessagesAsync(channel).GetAwaiter().GetResult());
            t.Start();
            t.Join();
            return res;
        }

        private async Task<List<string>> GetAllImagesUrlsFromChannelMessagesAsync(ITextChannel channel)
        {
            List<string> urls = new List<string>();
            try
            {
                await foreach(var messagesPack in channel.GetMessagesAsync(int.MaxValue, CacheMode.AllowDownload))
                {
                    var messages = messagesPack.GetEnumerator();
                    while(messages.MoveNext())
                    {
                        var msg = messages.Current;
                        var enumerator = msg.Attachments.GetEnumerator();

                        //There should be up to one attachment per message - let's get it right away if it's there
                        if(enumerator.MoveNext())
                        {
                            var attachment = enumerator.Current;
                            if(attachment.Width.HasValue) //Attachment has dimensions -> it's an image
                                urls.Add(attachment.Url);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                App.Log(LogLevel.Error, e.Message);
            }

            return urls;
        }
    }
}
