﻿namespace ClaimsMicroservice.Models
{
    internal class Claim
    {
        public string? ClaimNumber { get; set; }

        public int PolicyId { get; set; }

        public string? Status { get; set; }

        public decimal AmountInPounds { get; set; }

        public DateTime CreationDate { get; set; }
    }
}