using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class ReturnBaseMessageModel
    {

        public ReturnBaseMessageModel()
        {
            Success = false;
            Msg = "Error";
        }
        public bool Success { get; set; }
        public string Msg { get; set; }
        public int ReturnId { get; set; }
        public decimal ValueOne { get; set; }
        public string TransactionType { get; set; }
        public string Value { get; set; }
        public bool BoolValue { get; set; }
    }
}