using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public string? ErrorCode { get; }

        protected Result(bool isSuccess, string? errorMessage = null, string? errorCode = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public static Result Success() => new Result(true);
        public static Result Failure(string errorMessage, string? code = null) => new Result(false, errorMessage, code);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T value) : base(true)
        {
            Value = value;
        }

        private Result(string errorMessage, string? errorCode) : base(false, errorMessage, errorCode)
        {
            Value = default;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public new static Result<T> Failure(string errorMessage, string? code = null) => new Result<T>(errorMessage, code);

        public static Result<T> NotFound(string errorMessage = "Recurso no encontrado") => new Result<T>(errorMessage, Errors.NotFound);
        public static Result<T> Conflict(string errorMessage = "Conflicto de datos") => new Result<T>(errorMessage, Errors.Conflict);
        public static Result<T> BadRequest(string errorMessage = "Solicitud incorrecta") => new Result<T>(errorMessage, Errors.BadRequest);
        public static Result<T> Validation(string errorMessage = "Error de validación") => new Result<T>(errorMessage, Errors.Validation);
    }
}
