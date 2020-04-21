namespace Kodo.Robots.Api.ViewModels
{
    public class HateoasLinkViewModel
    {
        public HateoasLinkViewModel(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        public string Href { get; }
        public string Rel { get; }
        public string Method { get; }
    }
}