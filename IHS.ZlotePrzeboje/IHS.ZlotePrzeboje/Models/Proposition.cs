using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHS.ZlotePrzeboje.Models
{
    public class Proposition : IComparable<Proposition>
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public int VotesUP { get; set; }

        public int CompareTo(Proposition other)
        {
            var votesUpCompare = other.VotesUP.CompareTo(this.VotesUP);
            return votesUpCompare != 0 ? votesUpCompare : this.Id.CompareTo(other.Id);
        }
    }
}