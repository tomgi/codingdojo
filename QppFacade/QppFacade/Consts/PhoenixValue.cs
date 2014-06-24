namespace IHS.Phoenix.QPP
{
    public class PhoenixValue
    {
        private readonly long _id;
        private readonly string _name;

        public PhoenixValue(long id, string name)
        {
            _id = id;
            _name = name;
        }

        public static implicit operator long(PhoenixValue phoenixValue)
        {
            return phoenixValue._id;
        }

        public static implicit operator string(PhoenixValue phoenixValue)
        {
            return phoenixValue._name;
        }

        public int DomainId { get; set; }
    }
}