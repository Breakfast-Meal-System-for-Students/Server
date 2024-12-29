using AutoMapper;
using Azure.Core;
using Azure.Storage.Blobs;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Map;
using BMS.BLL.Models.Responses.Map;
using BMS.BLL.Models.Responses.Roles;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class GoogleMapService : BaseService,IGoogleMapService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GoogleMapService(IUnitOfWork unitOfWork, IMapper mapper,IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
  
   
  
        public async Task<ServiceActionResult> ComputeRoutes(RouteRequest request)
        {
            // Replace with your actual GoMaps API key
            var apiKey = _configuration["GoogleMapSettings:ApiKey"];
            var url = _configuration["GoogleMapSettings:UrlMatrix"];
           // var url = "https://routes.gomaps.pro/directions/v2:computeRoutes";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Goog-Api-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "routes.distanceMeters,routes.duration");

            // Convert the request object to JSON
            //var jsonRequest = JsonSerializer.Serialize(request);
            // Chuyển đổi yêu cầu thành JSON
            var jsonRequest = JsonSerializer.Serialize(new
            {
                origins = request.Origins,
                destinations = request.Destinations
            });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send the POST request to the GoMaps API
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var routeResponse = JsonSerializer.Deserialize<RouteResponse>(jsonResponse);

                return new ServiceActionResult()
                {
                    Data = routeResponse
                };
            }
            return new ServiceActionResult()
            {
            };
        }
        public async Task<Models.Responses.Map.Location> GetCoordinatesFromAddress(string address)
        {
            // Lấy API Key từ cấu hình
            var apiKey = _configuration["GoogleMapSettings:ApiKey"];
            var url = $"https://maps.gomaps.pro/maps/api/geocode/json?key={apiKey}&address={address}";

            // Tạo client Http
            var client = _httpClientFactory.CreateClient();

            // Gửi yêu cầu GET đến API
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Đọc nội dung phản hồi từ API
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON string into your C# object model
                var geocodeResponse = JsonSerializer.Deserialize<GeocodeResponse>(jsonResponse);

                // Check if the response is valid
                if (geocodeResponse != null && geocodeResponse.Status == "OK")
                {
                    if (geocodeResponse.Results != null && geocodeResponse.Results.Any())
                    {
                        var location = geocodeResponse.Results.First().Geometry.Location;

                        return new Models.Responses.Map.Location()
                        {
                            Lat = location.Lat,
                            Lng = location.Lng
                        };
                    }
                    else
                    {
                        Console.WriteLine("No results found.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {geocodeResponse?.Status}");
                }

            }

            // Return null or log an error if the request failed
            return null;
        }



        public async Task<IQueryable<Shop>> GetShopWithGeo(double latA, double lngA, double latB, double lngB)
        {
            double minLat = Math.Min(latA, latB);      
            double minLng = Math.Min(lngA, lngB);
            double maxLat = Math.Max(latA, latB);    
            double maxLng = Math.Max(lngA, lngB);
            IQueryable<Shop> applicationQuery = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable()).Where(a=> (a.lat>minLat&&a.lat<maxLat)&&(a.lng>minLng&&a.lng<maxLng));

          
            return applicationQuery;
        }

        public async Task<ServiceActionResult> GetShopsByShortestTravelTime(string add1, string add2)
        {
            var apiKey = _configuration["GoogleMapSettings:ApiKey"];
            var url = _configuration["GoogleMapSettings:distancematrix"];
            var client = _httpClientFactory.CreateClient();
            var local1 = await GetCoordinatesFromAddress(add1);
            var local2 = await GetCoordinatesFromAddress(add2);
            var shops = await GetShopWithGeo(local1.Lat,local1.Lng,local2.Lat,local2.Lng);

            // Tạo chuỗi destinations từ tất cả các cửa hàng
            var destinations = string.Join("|", shops.Select(s => $"{s.lat},{s.lng}"));

            // Tạo chuỗi origins là hai điểm A và B
            var origins = $"{local1.Lat},{local1.Lng}|{local2.Lat},{local2.Lng}";

            // Tạo URL cho request
            var requestUrl = $"{url}?origins={origins}&destinations={destinations}&key={apiKey}";

            // Gửi request đến Google Maps API
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var distanceMatrixResponse = JsonSerializer.Deserialize<DistanceMatrixResponse>(jsonResponse);

                // Tạo danh sách lưu thời gian di chuyển từ A -> Shop -> B
                var shopTimes = new List<ShopTimeResult>();
                foreach(var o in shops)
                {
                    int i =0;
                    var timeFromAtoShop = distanceMatrixResponse.rows[0].elements[i].duration.value;
                    var timeFromShoptoB = distanceMatrixResponse.rows[1].elements[i].duration.value;
                    var totalTime = timeFromAtoShop + timeFromShoptoB;

                    // Thêm vào danh sách
                    shopTimes.Add(new ShopTimeResult
                    {
                        Store = o,
                        TotalTime = totalTime,
                        TimeText = $"{TimeSpan.FromSeconds(totalTime)}"  // Chuyển đổi thời gian thành định dạng HH:mm:ss
                    });
                    i++;
                }
           

                // Sắp xếp danh sách cửa hàng theo tổng thời gian tăng dần
                var sortedShopTimes = shopTimes.OrderBy(s => s.TotalTime).ToList();

                return new ServiceActionResult()
                {
                    Data = sortedShopTimes
                };
            }
            else
            {
                return new ServiceActionResult()
                {
                    Data = "Failed to fetch data from Google Maps API."
                };
            }
        }

        // Lớp chứa kết quả cho mỗi shop dựa trên thời gian
        public class ShopTimeResult
        {
            public Shop Store { get; set; }
            public double TotalTime { get; set; }  // Tổng thời gian di chuyển (giây)
            public string TimeText { get; set; }   // Chuỗi mô tả thời gian
            public double TotalDistance { get; set; }  // Tổng khoảng cách (mét)
            public string DistanceText { get; set; }   // Chuỗi mô tả khoảng cách
        }

        public async Task<ServiceActionResult> GetShopsByShortestTravelTime2(string add1, string add2)
        {
            var apiKey = _configuration["GoogleMapSettings:ApiKey"];
            var url = _configuration["GoogleMapSettings:distancematrix"];
            var client = _httpClientFactory.CreateClient();
            IQueryable<Shop> shops = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable());

           // var shops = await GetShopWithGeo(local1.Lat, local1.Lng, local2.Lat, local2.Lng);

            // Tạo chuỗi destinations từ tất cả các cửa hàng
            var destinations = string.Join("|", shops.Select(s => $"{s.lat},{s.lng}"));

            // Tạo chuỗi origins là hai điểm A và B
            var origins = add1 + "|" + add2; 

            // Tạo URL cho request
            var requestUrl = $"{url}?origins={origins}&destinations={destinations}&key={apiKey}";

            // Gửi request đến Google Maps API
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var distanceMatrixResponse = JsonSerializer.Deserialize<DistanceMatrixResponse>(jsonResponse, options);


                // Tạo danh sách lưu thời gian di chuyển từ A -> Shop -> B
                var shopTimes = new List<ShopTimeResult>();
                int i = 0;
                foreach (var o in shops)
                {
                    // Thời gian từ A -> Shop
                    var timeFromAtoShop = distanceMatrixResponse.rows[0].elements[i].duration.value;
                    // Thời gian từ Shop -> B
                    var timeFromShoptoB = distanceMatrixResponse.rows[1].elements[i].duration.value;
                    // Tổng thời gian
                    var totalTime = timeFromAtoShop + timeFromShoptoB;

                    // Khoảng cách từ A -> Shop
                    var distanceFromAtoShop = distanceMatrixResponse.rows[0].elements[i].distance.value;
                    // Khoảng cách từ Shop -> B
                    var distanceFromShoptoB = distanceMatrixResponse.rows[1].elements[i].distance.value;
                    // Tổng khoảng cách
                    var totalDistance = distanceFromAtoShop + distanceFromShoptoB;

                    // Thêm vào danh sách
                    shopTimes.Add(new ShopTimeResult
                    {
                        Store = o,
                        TotalTime = totalTime,
                        TimeText = $"{TimeSpan.FromSeconds(totalTime)}",  // Chuyển đổi thời gian thành định dạng HH:mm:ss
                        TotalDistance = totalDistance,
                        DistanceText = $"{totalDistance / 1000} km"  // Chuyển đổi khoảng cách thành km
                    });
                    i++;
                }


                // Sắp xếp danh sách cửa hàng theo tổng thời gian tăng dần
                var sortedShopTimes = shopTimes.OrderBy(s => s.TotalDistance).ToList();

                return new ServiceActionResult()
                {
                    Data = sortedShopTimes
                };
            }
            else
            {
                return new ServiceActionResult()
                {
                    Data = "Failed to fetch data from Google Maps API."
                };
            }
        }

        public async Task<ServiceActionResult> GetShopsByShortestTravelTime3(string add1, string add2,string search)
        {
            var apiKey = _configuration["GoogleMapSettings:ApiKey"];
            var url = _configuration["GoogleMapSettings:distancematrix"];
            var client = _httpClientFactory.CreateClient();
            var local1 = await GetCoordinatesFromAddress(add1);
            var local2 = await GetCoordinatesFromAddress(add2);
            // Di chuyển 5km về phía Nam
            double minLat = Math.Min(local1.Lat, local2.Lat) - 0.045;
            double minLng = Math.Min(local1.Lng, local2.Lng) -0.045;
            double maxLat = Math.Max(local1.Lat, local2.Lat) + 0.045;
            double maxLng = Math.Max(local1.Lng, local2.Lng)+ 0.045;

            IQueryable<Shop> shops = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable())
              .Include(a => a.Package_Shop)
                  .ThenInclude(b => b.Package).Where(x =>
                  x.Status == ShopStatus.ACCEPTED &&
                  (x.Package_Shop.Any()
                      ? x.Package_Shop.Max(p => p.Package != null
                          ? p.CreateDate.AddDays(p.Package.Duration)
                          : DateTime.MinValue)
                      : DateTime.MinValue) > DateTimeHelper.GetCurrentTime() &&
                  (x.lat > minLat && x.lat < maxLat) &&
                  (x.lng > minLng && x.lng < maxLng)); 


            if (!string.IsNullOrEmpty(search))
            {
                shops = shops.Where(x => x.Name.Contains(search));
            }

            // var shops = await GetShopWithGeo(local1.Lat, local1.Lng, local2.Lat, local2.Lng);

            // Tạo chuỗi destinations từ tất cả các cửa hàng
            var destinations = string.Join("|", shops.Select(s => $"{s.lat},{s.lng}"));

            // Tạo chuỗi origins là hai điểm A và B
            var origins = add1 + "|" + add2;

            // Tạo URL cho request
            var requestUrl = $"{url}?origins={origins}&destinations={destinations}&key={apiKey}";

            // Gửi request đến Google Maps API
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var distanceMatrixResponse = JsonSerializer.Deserialize<DistanceMatrixResponse>(jsonResponse, options);


                // Tạo danh sách lưu thời gian di chuyển từ A -> Shop -> B
                var shopTimes = new List<ShopTimeResult>();
                int i = 0;
                foreach (var o in shops)
                {
                    // Thời gian từ A -> Shop
                    var timeFromAtoShop = distanceMatrixResponse.rows[0].elements[i].duration.value;
                    // Thời gian từ Shop -> B
                    var timeFromShoptoB = distanceMatrixResponse.rows[1].elements[i].duration.value;
                    // Tổng thời gian
                    var totalTime = timeFromAtoShop + timeFromShoptoB;

                    // Khoảng cách từ A -> Shop
                    var distanceFromAtoShop = distanceMatrixResponse.rows[0].elements[i].distance.value;
                    // Khoảng cách từ Shop -> B
                    var distanceFromShoptoB = distanceMatrixResponse.rows[1].elements[i].distance.value;
                    // Tổng khoảng cách
                    var totalDistance = distanceFromAtoShop + distanceFromShoptoB;

                    // Thêm vào danh sách
                    shopTimes.Add(new ShopTimeResult
                    {
                        Store = o,
                        TotalTime = totalTime,
                        TimeText = $"{TimeSpan.FromSeconds(totalTime)}",  // Chuyển đổi thời gian thành định dạng HH:mm:ss
                        TotalDistance = totalDistance,
                        DistanceText = $"{totalDistance / 1000} km"  // Chuyển đổi khoảng cách thành km
                    });
                    i++;
                }


                // Sắp xếp danh sách cửa hàng theo tổng thời gian tăng dần
                var sortedShopTimes = shopTimes.OrderBy(s => s.TotalDistance).ToList();

                return new ServiceActionResult()
                {
                    Data = sortedShopTimes
                };
            }
            else
            {
                return new ServiceActionResult()
                {
                    Data = "Failed to fetch data from Google Maps API."
                };
            }
        }
    }
}