using ImageContent.Common.Interfaces.IService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.BL.Service
{
    public class TokenBlackListService : ITokenBlackListService
    {
        private readonly ConcurrentDictionary<string, bool> blacklistedTokens = new();
        private readonly ConcurrentDictionary<string, List<string>> userTokens = new();

        public void BlackListPreviousToken(string userId, string token)
        {
            if (!userTokens.TryGetValue(userId, out var tokens))
            {
                return;
            }

            lock (tokens)
            {
                foreach (var item in tokens)
                {
                    if (item != token)
                    {
                        BlacklistToken(item);
                    }
                }
            }

            tokens.Clear();
            tokens.Add(token);
        }

        public void BlacklistToken(string token)
        {
            blacklistedTokens[token] = true;
        }

        public bool IsTokenBlacklisted(string token)
        {
            return blacklistedTokens.ContainsKey(token);
        }

        public void StoreUserTokens(string userId, string token)
        {
            var tokens = userTokens.GetOrAdd(userId, _ => new List<string>());

            lock (tokens) 
            {
                tokens.Add(token);
            }
        }
    }
}
