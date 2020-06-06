using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared
{
    public class Result
    {
        public Result(bool isSuccess, string message = "")
        {
            Succeeded = isSuccess;
            Message = message;
        }
        public static Result Success()
        {
            return new Result(true, "");
        }
        public static Result Failure(string errorMessage = "")
        {
            return new Result(false, errorMessage);
        }

        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
    }

    public class Result<T>:Result 
    {
        public Result(T value,bool isSuccess, string message = "")
            :base(isSuccess,message)
        {
            Value = value;
        }
        public static Result<T> Success(T value)
        {
            return new Result<T>(value,true, "");
        }
        public static new Result<T> Failure(string errorMessage)
        {
            return new Result<T>(default(T), true, "");
        }

        public T Value { get; private set; }
    }

}
