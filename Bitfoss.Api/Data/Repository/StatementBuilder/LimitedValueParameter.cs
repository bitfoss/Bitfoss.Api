namespace Bitfoss.Api.Data.Repository.StatementBuilder
{
    public class LimitedValueParameter<T>
    {
        public T Value { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }
    }
}
