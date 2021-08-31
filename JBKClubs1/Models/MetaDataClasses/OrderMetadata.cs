using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JBKClubs1.Models
{
    [ModelMetadataType(typeof(OrderMetadata))]
    public partial class GroupMember
    {

    }
    public class OrderMetadata
    {

        public int ArtistIdGroup { get; set; }
        public int ArtistIdMember { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]

        public DateTime? DateJoined { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]

        public DateTime? DateLeft { get; set; }

        public virtual Artist ArtistIdGroupNavigation { get; set; }
        public virtual Artist ArtistIdMemberNavigation { get; set; }
    }
}
