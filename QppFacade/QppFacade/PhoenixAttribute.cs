using IHS.Phoenix.QPP;

namespace QppFacade
{
    public class PhoenixAttribute<T> : PhoenixValue
    {
        public PhoenixAttribute(long id, string name) : base(id, name)
        {
        }

        public int Type { get; set; }
    }
}