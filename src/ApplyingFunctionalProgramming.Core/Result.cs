﻿using System;

namespace ApplyingFunctionalProgramming.Core
{
    using NullGuard;

    public class Result
    {
        public bool IsSuccess { get; }

        public string Error { get; }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (var result in results)
            {
                if (result.IsFailure) return result;
            }

            return Ok();
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        protected internal Result([AllowNull] T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
