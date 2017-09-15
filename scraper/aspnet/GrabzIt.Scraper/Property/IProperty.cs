namespace GrabzIt.Scraper.Property
{
    public interface IProperty
    {
        string Type
        {
            get;
        }

        string ToXML();
    }
}
