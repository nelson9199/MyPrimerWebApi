namespace MyPrimerWebApi.Models
{
    public class Enlace
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Metodo { get; private set; }
        public Enlace(string href, string rel, string metodo)
        {
            Href = href;
            Rel = rel;
            Metodo = metodo;
        }
    }
}