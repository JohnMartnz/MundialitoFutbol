using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string message) => new Result(false, message);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, string message, T? value) : base(isSuccess, message)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, string.Empty, value);
        public new static Result<T> Failure(string message) => new Result<T>(false, message, default);
    }
}
