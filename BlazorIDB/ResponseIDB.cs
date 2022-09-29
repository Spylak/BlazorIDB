using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorIDB
{
    public class ResponseIDB
    {
        public ResponseIDB(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public bool IsSuccess { get; init; }
        public string? Message { get; init; }
    }
    public class ResponseIDB<T> : ResponseIDB where T : class?
    {
        public ResponseIDB(T? data, bool isSuccess, string message = null) : base(isSuccess, message)
        {
            Data = data;
        }
        public T? Data { get; set; }
    }
}
