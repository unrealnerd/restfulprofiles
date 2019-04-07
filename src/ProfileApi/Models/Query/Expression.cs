namespace ProfileApi.Models.Query
{
    public class Expression
    {
        public string Operand { get; set; }
        public string Operator { get; set; }
        public dynamic Value { get; set; }
        public string OptionalValue { get; set; }
        public string LogicalCondition { get; set; }
    }
}