using System.Linq;
using HomeSearchAssessment.Facades;
using HomeSearchAssessment.Readers;
using Xunit;

namespace HomeSearchAssessmentTest
{
    public class HomeSearchTests
    {
        /*
         * GIVEN I am user on the home search page
         * WHEN I submit an empty string
         * THEN no properties are displayed to me
         */
        [Fact]
        public static void HomeSearch_EmptySting_NoPropertiesDisplayed()
        {
            var reader = new PropertyDatabaseReader();
            var facade = new HomeSearchFacade(reader.Read());

            var propertiesToDisplay = facade.GetPropertiesByPostcode("");
            Assert.Empty(propertiesToDisplay);
        }
        
        /*
         * GIVEN I am user on the home search page
         * WHEN I submit postcode matching one or more properties
         * THEN the matching properties are displayed to me
         */
        [Fact]
        public static void HomeSearch_ValidPostcode_MatchingPropertiesDisplayed()
        {
            var reader = new PropertyDatabaseReader();
            var facade = new HomeSearchFacade(reader.Read());

            var propertiesToDisplay = facade.GetPropertiesByPostcode("BH141PA");
            Assert.Equal(2, propertiesToDisplay.Count);
            Assert.True(propertiesToDisplay.Exists(p => p.StreetAddress == "599 Mill Road BOURNEMOUTH"));
            Assert.True(propertiesToDisplay.Exists(p => p.StreetAddress == "637 Mill Road BOURNEMOUTH"));
        }
        
        /*
         * GIVEN I am user on the home search page
         * WHEN I submit postcode matching one or more properties
         * THEN the properties that don't match are not displayed to me
         */
        [Fact]
        public static void HomeSearch_ValidPostcode_MismatchingPropertiesNotDisplayed()
        {
            var reader = new PropertyDatabaseReader();
            var facade = new HomeSearchFacade(reader.Read());

            var propertiesToDisplay = facade.GetPropertiesByPostcode("NG549BQ");
            Assert.NotEmpty(propertiesToDisplay);
            Assert.False(propertiesToDisplay.Exists(p => p.StreetAddress == "49 Park Avenue TWICKENHAM"));
        }
        
        /*
         * GIVEN I am user on the home search page
         * WHEN I submit postcode matching one or more properties
         * THEN the properties appear in order with the most expensive first
         */
        [Fact]
        public static void HomeSearch_ValidPostcode_PropertiesDisplayedByHighestToLowestPrice()
        {
            var reader = new PropertyDatabaseReader();
            var facade = new HomeSearchFacade(reader.Read());

            var propertiesToDisplay = facade.GetPropertiesByPostcode("BH141PA");
            Assert.NotEmpty(propertiesToDisplay);
            Assert.Equal(2, propertiesToDisplay.Count);
            Assert.Equal(640000, propertiesToDisplay.First().PriceInPounds);
            Assert.Equal(620000, propertiesToDisplay.Skip(1).First().PriceInPounds);
        }
        
        /*
         * GIVEN I am user on the home search page
         * WHEN I submit a wildcard postcode matching one or more properties
         * THEN the matching properties are displayed to me
         */
        [Fact]
        public static void HomeSearch_ValidWildcardPostcode_MatchingPropertiesDisplayed()
        {
            var reader = new PropertyDatabaseReader();
            var facade = new HomeSearchFacade(reader.Read());

            var propertiesToDisplay = facade.GetPropertiesByPostcode("BH9****");
            Assert.Equal(2, propertiesToDisplay.Count);
            Assert.True(propertiesToDisplay.Exists(p => p.Postcode == "BH943KV"));
            Assert.True(propertiesToDisplay.Exists(p => p.Postcode == "BH994MH"));
        }
        
        /*
         * GIVEN I am user on the home search page
         * WHEN I submit a wildcard postcode matching one or more properties
         * THEN the mismatching properties are not displayed to me
         */
        [Fact]
        public static void HomeSearch_ValidWildcardPostcode_MismatchingPropertiesNotDisplayed()
        {
            var reader = new PropertyDatabaseReader();
            var facade = new HomeSearchFacade(reader.Read());

            var propertiesToDisplay = facade.GetPropertiesByPostcode("BH9****");
            Assert.NotEmpty(propertiesToDisplay);
            Assert.False(propertiesToDisplay.Exists(p => p.Postcode == "BH141PA"));
        }
    }
}
