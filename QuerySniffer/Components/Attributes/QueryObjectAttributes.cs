using Grasshopper.Kernel.Attributes;

namespace QuerySniffer.Components.Attributes
{
    internal class QueryObjectAttributes : GH_FloatingParamAttributes
    {
        public QueryObjectAttributes(QueryObject owner) : base(owner)
        {
        }
    }
}
