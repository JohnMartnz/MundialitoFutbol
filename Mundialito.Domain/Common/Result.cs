using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, string error, T? value) : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, string.Empty, value);
        public new static Result<T> Failure(string error) => new Result<T>(false, error, default);
    }
}
