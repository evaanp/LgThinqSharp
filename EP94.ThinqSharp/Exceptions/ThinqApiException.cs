﻿using EP94.ThinqSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Exceptions
{
    public class ThinqApiException : Exception
    {
        public ThinqResponseCode ErrorCode { get; set; }
        public ThinqApiException()
        {
        }

        public ThinqApiException(string? message) : base(message)
        {
        }

        public ThinqApiException(string? message, ThinqResponseCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ThinqApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ThinqApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
