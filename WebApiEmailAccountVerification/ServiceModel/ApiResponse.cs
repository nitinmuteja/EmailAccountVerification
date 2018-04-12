using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ServiceModel
{
    /// <summary>
    /// The response from api 
    /// </summary>
    public class ApiResponse
    {
     
        public bool VerificationResult { get; set; }

        public bool Result { get; set; }

        public string ErrorCode { get; set; }


    }
}