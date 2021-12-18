using System;
namespace Core.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data) : base(data, true, "OK") { }
    }
}
