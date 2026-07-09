using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.API_Respons
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static Response<T> Success(T data) => new Response<T> { IsSuccess = true, Data = data };

        public static Response<T> Failure(string error) => new Response<T> { IsSuccess = false, ErrorMessage = error };

    }
}
