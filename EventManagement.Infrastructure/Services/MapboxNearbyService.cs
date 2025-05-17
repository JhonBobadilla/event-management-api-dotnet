using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace EventManagement.Infrastructure.Services
{
    public class MapboxNearbyService
    {
        private readonly string _accessToken;
        private readonly ILogger<MapboxNearbyService> _logger;

        public MapboxNearbyService(string accessToken, ILogger<MapboxNearbyService> logger)
        {
            _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Validación básica del token
            if (string.IsNullOrWhiteSpace(_accessToken) || !_accessToken.StartsWith("pk."))
            {
                throw new ArgumentException("Token de Mapbox inválido. Debe comenzar con 'pk.'");
            }
        }

        public async Task<List<MapboxPlaceResult>> GetNearbyPlacesAsync(double latitude, double longitude, int radiusMeters)
        {
            // Validación de parámetros de entrada
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("La latitud debe estar entre -90 y 90 grados");
            
            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("La longitud debe estar entre -180 y 180 grados");
            
            if (radiusMeters <= 0 || radiusMeters > 10000) // Límite de 10km
                throw new ArgumentException("El radio debe estar entre 1 y 10000 metros");

            using var httpClient = new HttpClient();
            
            try
            {
                // Construye la URL correctamente según la documentación de Mapbox
                var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/" +
                         $"{longitude},{latitude}.json" +
                         $"?types=poi" +  // Solo puntos de interés
                         $"&limit=10" +
                         $"&access_token={_accessToken}";

                _logger.LogInformation($"Consultando Mapbox API: {url.Replace(_accessToken, "[TOKEN]")}");

                var response = await httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error en Mapbox API: {response.StatusCode} - {errorContent}");
                    throw new Exception($"Mapbox API error: {response.StatusCode} - {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();
                return ParseMapboxResponse(content, latitude, longitude, radiusMeters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lugares cercanos");
                throw;
            }
        }

        private List<MapboxPlaceResult> ParseMapboxResponse(string jsonContent, double originLat, double originLon, int radiusMeters)
        {
            var results = new List<MapboxPlaceResult>();
            
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                var features = doc.RootElement.GetProperty("features");
                foreach (var feature in features.EnumerateArray())
                {
                    var coords = feature.GetProperty("geometry").GetProperty("coordinates");
                    double lon = coords[0].GetDouble();
                    double lat = coords[1].GetDouble();
                    double distance = HaversineDistance(originLat, originLon, lat, lon);

                    // Filtra por radio
                    if (distance <= radiusMeters)
                    {
                        results.Add(new MapboxPlaceResult
                        {
                            Name = feature.GetProperty("text").GetString() ?? "Sin nombre",
                            Type = GetPlaceType(feature),
                            Latitude = lat,
                            Longitude = lon,
                            Distance = distance
                        });
                    }
                }
            }
            
            return results.OrderBy(r => r.Distance).ToList();
        }

        private string GetPlaceType(JsonElement feature)
        {
            // Intenta obtener el tipo de lugar de diferentes propiedades
            if (feature.GetProperty("properties").TryGetProperty("category", out var category))
                return category.GetString();
            
            if (feature.TryGetProperty("place_type", out var placeType) && placeType.GetArrayLength() > 0)
                return placeType[0].GetString();
            
            return "Desconocido";
        }

        // Distancia en metros entre dos puntos lat/lon usando fórmula Haversine
        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Radio de la tierra en metros
            double dLat = (lat2 - lat1) * (Math.PI / 180.0);
            double dLon = (lon2 - lon1) * (Math.PI / 180.0);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * (Math.PI / 180.0)) * Math.Cos(lat2 * (Math.PI / 180.0)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }

    public class MapboxPlaceResult
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; } // Distancia en metros desde el punto de origen
    }
}
