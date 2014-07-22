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
            return phoenixValue.Name;
        }

        public int DomainId { get; set; }
        
        public long Id { get { return _id; } }

        public string Name
        {
            get { return _name; }
        }
    }
}