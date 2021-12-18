﻿using System;
namespace Core.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult() : base(false) { }
        public ErrorResult(string message) : base(false, message) { }
    }
}
