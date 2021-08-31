using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JBKClubs1.Models
{
    public partial class Artist
    {
        ClubsContext _context = new ClubsContext();
        public Artist(ClubsContext context)
        {
            _context = context;
            ArtistInstrument = new HashSet<ArtistInstrument>();
            ArtistStyle = new HashSet<ArtistStyle>();
            Contract = new HashSet<Contract>();
            GroupMemberArtistIdGroupNavigation = new HashSet<GroupMember>();
            GroupMemberArtistIdMemberNavigation = new HashSet<GroupMember>();
        }

        public int ArtistId { get; set; }
        public double MinimumHourlyRate { get; set; }
        public int NameAddressid { get; set; }

        public virtual NameAddress NameAddress { get; set; }
        public virtual ICollection<ArtistInstrument> ArtistInstrument { get; set; }
        public virtual ICollection<ArtistStyle> ArtistStyle { get; set; }
        public virtual ICollection<Contract> Contract { get; set; }
        public virtual ICollection<GroupMember> GroupMemberArtistIdGroupNavigation { get; set; }
        public virtual ICollection<GroupMember> GroupMemberArtistIdMemberNavigation { get; set; }
    }
}
