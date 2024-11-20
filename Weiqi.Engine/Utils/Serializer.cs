using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Utilities;

public static class Serializer
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true, 
        Converters = { new JsonStringEnumConverter() } 
    };

    public static void SaveBoard(Board board, string filePath)
    {
        string json = JsonSerializer.Serialize(board, JsonOptions);
        File.WriteAllText(filePath, json);
    }

    public static Board LoadBoard(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Board>(json, JsonOptions);
    }
}