using System.Collections.Generic;

namespace HomeSearchAssessment.Models
{
    public class Policy
    {
        public int Id { get; set; }

        public Property Property { get; set; }

        public List<Claim> Claims { get; set; }
    }
}
