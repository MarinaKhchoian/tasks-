using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class CountryDataGenerator
{
    private static readonly HttpClient client = new HttpClient();

    public static async void GenerateCountryDataFiles()
    {
        try
        {
            await GenerateCountryDataFilesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"შეცდომა: {ex.Message}");
        }
    }

    private static async Task GenerateCountryDataFilesAsync()
    {
        string apiUrl = "https://restcountries.com/v3.1/all?fields=name,region,subregion,latlng,area,population";

        Console.WriteLine("მიმდინარეობს მონაცემების ჩამოტვირთვა...");

      
        HttpResponseMessage response = await client.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();

        
        List<CountryDto> countries = JsonSerializer.Deserialize<List<CountryDto>>(jsonResponse);

        if (countries == null || countries.Count == 0)
        {
            Console.WriteLine("ქვეყნების მონაცემები ვერ მოიძებნა");
            return;
        }

        int fileCount = 0;

     
        foreach (var country in countries)
        {
            try
            {
             
                string countryName = country.Name?.Common ?? "Unknown";

               
                string fileName = SanitizeFileName(countryName) + ".txt";

             
                string latlng = country.Latlng != null && country.Latlng.Length >= 2
                    ? $"[{country.Latlng[0]}, {country.Latlng[1]}]"
                    : "N/A";

                
                string content = $"ქვეყანა: {countryName}\n" +
                               $"Region: {country.Region ?? "N/A"}\n" +
                               $"Subregion: {country.Subregion ?? "N/A"}\n" +
                               $"LatLng: {latlng}\n" +
                               $"Area: {(country.Area.HasValue ? country.Area.Value.ToString("N0") : "N/A")}\n" +
                               $"Population: {(country.Population.HasValue ? country.Population.Value.ToString("N0") : "N/A")}\n";

              
                File.WriteAllText(fileName, content);
                fileCount++;

                Console.WriteLine($"შეიქმნა: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"შეცდომა ქვეყნის დამუშავებისას: {ex.Message}");
            }
        }

        Console.WriteLine($"\n✓ სულ შეიქმნა {fileCount} ფაილი");
    }

    private static string SanitizeFileName(string fileName)
    {
       
        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            fileName = fileName.Replace(c, '_');
        }
        return fileName;
    }
}

public class CountryDto
{
    [JsonPropertyName("name")]
    public NameDto Name { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("subregion")]
    public string Subregion { get; set; }

    [JsonPropertyName("latlng")]
    public double[] Latlng { get; set; }

    [JsonPropertyName("area")]
    public double? Area { get; set; }

    [JsonPropertyName("population")]
    public int? Population { get; set; }
}

public class NameDto
{
    [JsonPropertyName("common")]
    public string Common { get; set; }

    [JsonPropertyName("official")]
    public string Official { get; set; }

    [JsonPropertyName("nativeName")]
    public JsonElement? NativeName { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("  ქვეყნების ფაილების გენერატორი");
        Console.WriteLine("═══════════════════════════════════════\n");

        CountryDataGenerator.GenerateCountryDataFiles();

       
        Console.WriteLine("\nდააჭირეთ Enter-ს გასასვლელად...");
        Console.ReadLine();
    }
}
