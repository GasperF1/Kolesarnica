using System.Text.Json.Serialization;

namespace MonkeyFinder.Model;

public class Monkey
{
    public string znamka { get; set; }
    //public string Location { get; set; }
    public string lastnik { get; set; }
    public string slika { get; set; }
    //public int Population { get; set; }
    public double trentnaLokacijaLatitude { get; set; }
    public double trentnaLokacijaLongitude { get; set; }
}
public class MonkeyVM
{
    public string znamka { get; set; }
    //public string Location { get; set; }
    public string lastnik { get; set; }
    public string slika { get; set; }
    //public int Population { get; set; }
    public double trentnaLokacijaLatitude { get; set; }
    public double trentnaLokacijaLongitude { get; set; }
    public string naslov {get; set; }   
}
[JsonSerializable(typeof(List<Monkey>))]
internal sealed partial class MonkeyContext : JsonSerializerContext
{

}