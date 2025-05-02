using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.BaseResponse
{
    public class BaseCommandResponse<T> where T : class
    {
        public bool Success => string.IsNullOrWhiteSpace(Error);
        public string Message { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}
