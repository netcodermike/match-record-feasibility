using System.Text.Json;

try
{
    if (args.Length != 1) throw new ArgumentException("Pass one JSON input file");
    using var data = JsonDocument.Parse(File.ReadAllText(args[0]));
    Console.WriteLine(JsonSerializer.Serialize(Analyze(data.RootElement)));
    return 0;
}
catch (Exception error) when (error is IOException or JsonException or FormatException
                               or ArgumentException or InvalidOperationException
                               or KeyNotFoundException or OverflowException)
{
    Console.Error.WriteLine(error.Message);
    return 2;
}

static object Analyze(JsonElement input) {
 var games=input.GetProperty("games").GetInt32();var points=input.GetProperty("points").GetInt32();var win=input.GetProperty("winPoints").GetInt32();var draw=input.GetProperty("drawPoints").GetInt32();
 if(games<0||games>1000||points<0||win<=0||draw<0||draw>=win)throw new ArgumentException("Invalid games or scoring system");
 var records=new List<object>();
 for(int wins=0;wins<=games;wins++)for(int draws=0;draws<=games-wins;draws++)if((long)wins*win+(long)draws*draw==points)records.Add(new{wins,draws,losses=games-wins-draws});
 return new{records,count=records.Count};
}
