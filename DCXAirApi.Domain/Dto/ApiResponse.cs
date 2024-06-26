﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCXAirApi.Domain.Dto
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }

        public ApiResponse(T data, string message = "")
        {
            Data = data;
            Message = message;
        }
    }
}
