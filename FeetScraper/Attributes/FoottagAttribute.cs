using System;

namespace FeetScraper.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FoottagAttribute : Attribute
    {

        public char Id { get; init; }
        public string Description { get; init; }
        public FoottagAttribute(char id, string description)
        {
            this.Id = id;
            this.Description = description;
        }
    }
}
