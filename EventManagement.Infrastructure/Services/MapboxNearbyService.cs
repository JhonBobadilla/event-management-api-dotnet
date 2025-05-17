using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Globalization; // IMPORTANTE

namespace EventManagement.Infrastructure.Services
{
    public class MapboxNearbyService
    {
        private readonly string _accessToken;
        private readonly ILogger<MapboxNearbyService> _logger;
        private readonly HttpClient _httpClient;

        public MapboxNearbyService(string accessToken, ILogger<MapboxNearbyService> logger, HttpClient httpClient = null)
        {
            _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? new HttpClient();

            if (string.IsNullOrWhiteSpace(_accessToken) || !_accessToken.StartsWith("pk."))
                throw new ArgumentException("Token de Mapbox inválido");

            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public async Task<List<MapboxPlaceResult>> GetNearbyPlacesAsync(double latitude, double longitude, int radiusMeters)
        {
            _logger.LogInformation($"[LOG] RECIBIDO de base de datos: Latitude = '{latitude}', Longitude = '{longitude}'");

            string latString = latitude.ToString(CultureInfo.InvariantCulture);
            string lonString = longitude.ToString(CultureInfo.InvariantCulture);

            var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{lonString},{latString}.json" +
                      $"?types=address,poi&limit=10&access_token={_accessToken}";

            _logger.LogInformation($"[LOG] URL enviada a Mapbox: {url}");

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error en Mapbox: {response.StatusCode}");
                    return new List<MapboxPlaceResult>();
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"[LOG] Respuesta cruda de Mapbox: {content}");
                return ParseMapboxResponse(content, latitude, longitude);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar Mapbox");
                return new List<MapboxPlaceResult>();
            }
        }

        private List<MapboxPlaceResult> ParseMapboxResponse(string jsonContent, double originLat, double originLon)
        {
            var results = new List<MapboxPlaceResult>();

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    var features = doc.RootElement.GetProperty("features");
                    foreach (var feature in features.EnumerateArray())
                    {
                        try
                        {
                            var coords = feature.GetProperty("geometry").GetProperty("coordinates");
                            double lon = coords[0].GetDouble();
                            double lat = coords[1].GetDouble();
                            double distance = HaversineDistance(originLat, originLon, lat, lon);

                            _logger.LogInformation($"[LOG] Lugar: {feature.GetProperty("place_name").GetString()}, Distancia: {distance} m");

                            results.Add(new MapboxPlaceResult
                            {
                                Name = feature.GetProperty("place_name").GetString() ?? "Sin nombre",
                                Type = feature.TryGetProperty("place_type", out var pt)
                                    ? pt[0].GetString() : "address",
                                Latitude = lat,
                                Longitude = lon,
                                Distance = Math.Round(distance)
                            });
                        }
                        catch { continue; }
                    }
                }
            }
            catch { /* Silencia errores de parseo */ }

            return results.OrderBy(r => r.Distance).ToList();
        }

        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000;
            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                      Math.Cos(lat1 * (Math.PI / 180)) *
                      Math.Cos(lat2 * (Math.PI / 180)) *
                      Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return R * (2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
        }
    }

    public class MapboxPlaceResult
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }
    }
}


