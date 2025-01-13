using System;
using System.Linq;
using System.Collections.Generic;

namespace OSK
{
    public class Result<T, E>
    {
        public T Value { get; }
        public E Error { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        private Result(T value, E error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        // Factory methods
        public static Result<T, E> Success(T value) => new Result<T, E>(value, default(E), true);
        public static Result<T, E> Failure(E error) => new Result<T, E>(default(T), error, false);
        public static Result<T, string> Failure(string error) => new Result<T, string>(default(T), error, false);

        public static Result<T, E> Try(Func<T> func, E error)
        {
            try
            {
                return Success(func());
            }
            catch
            {
                return Failure(error);
            }
        }

        public void Match(Action<T> onSuccess, Action<E> onFailure)
        {
            if (IsSuccess)
                onSuccess(Value);
            else
                onFailure(Error);
        }

        // Combine multiple results into a single result
        public static Result<T, E> Combine(IEnumerable<Result<T, E>> results)
        {
            var errors = new List<E>();
            var value = default(T);

            foreach (var result in results)
            {
                if (result.IsFailure)
                {
                    errors.Add(result.Error);
                }
                else
                {
                    value = result.Value;
                }
            }

            if (errors.Any())
            {
                return Result<T, E>.Failure(errors.First());
            }

            return Result<T, E>.Success(value);
        }

        // Combine two results into a tuple
        public Result<Tuple<T, U>, E> CombineWith<U>(Result<U, E> other)
        {
            if (IsFailure) return Result<Tuple<T, U>, E>.Failure(Error);
            if (other.IsFailure) return Result<Tuple<T, U>, E>.Failure(other.Error);

            return Result<Tuple<T, U>, E>.Success(Tuple.Create(Value, other.Value));
        }

        // Chaining: if success, do something and return the result of the action
        public Result<U, E> Bind<U>(Func<T, Result<U, E>> func)
        {
            return IsSuccess ? func(Value) : Result<U, E>.Failure(Error);
        }

        // Chaining: if failure, do something and return the current result
        public Result<T, E> OnFailure(Action<E> action)
        {
            if (IsFailure) action(Error);
            return this;
        }

        // Chaining: if success, do something and return the current result
        public Result<T, E> OnSuccess(Action<T> action)
        {
            if (IsSuccess) action(Value);
            return this;
        }

        // change the error type
        public Result<T, ENew> MapError<ENew>(Func<E, ENew> map)
        {
            return IsFailure ? Result<T, ENew>.Failure(map(Error)) : Result<T, ENew>.Success(Value);
        }
        
        public override string ToString()
        {
            return IsSuccess ? $"Success: {Value}" : $"Failure: {Error}";
        }
    }
}