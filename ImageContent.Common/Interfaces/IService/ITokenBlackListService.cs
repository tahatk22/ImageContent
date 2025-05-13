using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.Interfaces.IService
{
    public interface ITokenBlackListService
    {
        void BlacklistToken(string token);
        bool IsTokenBlacklisted(string token);
        void StoreUserTokens(string userId , string token);
        void BlackListPreviousToken(string userId , string token);
    }
}
