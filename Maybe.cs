using NullGuard;

    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException();

                return this._value;
            }
        }

        public bool HasValue => _value != null;
        public bool HasNoValue => !HasValue;

        private Maybe([AllowNull] T value)
        {
            _value = value;
        }
        
        // Maybe<string> nullableString = "abc"

        // Maybe<Customer> customer = customerRepo.GetById(1);
        // if(customer.HasNoValue) return NotFound();
        // return ViewModel(customer.Value);

        // Maybe<string> nullableString = null;
        public static implicit operator Maybe<T>([AllowNull] T value)
        {
            return new Maybe<T>(value);
        }

        public static bool operator ==(Maybe<T> maybe, T value)
        {
            if (maybe.HasNoValue)
                return false;

            return maybe.Value.Equals(value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return false;

            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Maybe<T>))
                return false;

            var other = (Maybe<T>)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return HasNoValue ? "No value" : this.Value.ToString();
        }

        [return: AllowNull]
        public T Unwrap([AllowNull] T defaultValue = default(T))
        {
            return HasValue ? Value : defaultValue;
        }
    }
