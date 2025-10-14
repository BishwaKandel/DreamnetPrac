using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ApiResponse<T>
    {
        public bool success { get; set; } //True false

        public string message { get; set; } // Employee retrieved successfully
        public T? Data { get; set; } // Id : Name : 
    }
}
