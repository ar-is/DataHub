# DataHub

**DataHub** is a scalable and efficient _Data Aggregation System_ build using **C#** & **.NET 8** that aggregates data from one or more external API endpoint sources and delivers it through a single API endpoint. 
The system provides filtering options based on specified parameters and implements authentication, authorization, caching techniques, and Swagger documentation.

## Features

- **Data Aggregation**: Aggregates data from multiple external API endpoints. Currently there are 3 external Api Sources used for the following 3 data categories:
  - **Weather**
    -  [Open-Meteo API.](https://open-meteo.com/en/docs)
  - **Books**
    -  [Open Library API](https://openlibrary.org/developers/api)
  - **News**
    -  [Spaceflight News API](https://www.spaceflightnewsapi.net/)

  More categories as well as more external APIs per category can be added efficiently just by extending the infrastructure without the need for modifications.

- **Filtering**: Provides filtering options based on specified parameters such as date range and data category.
- **Authentication and Authorization**: Secures the exposed API endpoint with authentication and authorization.
- **Caching Techniques**: Implements caching techniques to optimize performance.
- **Swagger Documentation**: Exposes the API endpoint through Swagger for easy exploration and testing.

## Technologies Used

- **.NET**: Utilizes latest .NET 8 stack for building the application using Minimal Apis.
- **Swagger**: Used for API documentation and exploration.
- **JWT Authentication**: Implements JSON Web Token-based authentication for securing the API endpoint.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download): Ensure that you have the .NET SDK installed on your machine.
- [Visual Studio](https://visualstudio.microsoft.com/): For development (optional).
- [Postman](https://www.postman.com/) or [Swagger UI](https://swagger.io/tools/swagger-ui/): For testing the API endpoints.

## Getting Started

1. **Clone the Repository**: Clone the repository to your local machine.

    ```bash
    git clone https://github.com/ar-is/DataHub.git
    ```

2. **Navigate to Project Directory**: Change your current directory to the project directory.

    ```bash
    cd .../DataHub
    ```

3. **Build the Application**: Build the application using the following command:

    ```bash
    dotnet build
    ```

4. **Run the Application**: Start the application using the following command:

    ```bash
    dotnet watch run --project src/DataHub.Api/DataHub.Api.csproj --launch-profile https
    ```

5. **Explore the API Documentation**: Navigate to the Swagger UI (`https://localhost:7208/swagger/index.html`) to explore and test the API endpoints.

## Testing API Endpoints with Swagger

1. **Authenticate User**: First, hit the `/api/authenticate` endpoint using username: _admin_ and password: _1234_ to obtain an access token.
    - Example Request:
      ```http
      GET /api/authenticate?username=admin&password=1234 HTTP/1.1
      host: localhost:<port>
      user-agent: curl/8.2.1
      accept: application/json
      ```
    - Example Response:
      ```json
      {
          "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
      }
      ```

2. **Authorize Using Access Token**: Click the "Authorize" button in Swagger and provide the obtained access token in the format `<access_token>`.

3. **Fetch Aggregated Data**: Hit the `/api/data-aggregation` endpoint and use the `datefrom`, `dateto`, and `category` filters to retrieve the desired data.
    - _Category:_ Supports only one value of the existing data categories (Book, Weather, News). If no value is submitted, all categories are returned. If more categories are added, it will be automatically supported.
    - _DateFrom_: Supports format YYYY-MM-DD.
    - _DateTo_: Supports format YYYY-MM-DD.

    - Example Request:
      ```http
      GET /api/data-aggregation HTTP/1.1
      Host: localhost:<port>
      User-Agent: curl/8.2.1
      accept: application/json
      Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
      ```
    - Example Response:
      ```json
      {
        "weather": {
            "apiProviders": [
              {
                "apiProvider": "Open-Meteo",
                "data": [
                  {
                    "latitude": 38,
                    "longitude": 23.75,
                    "date": "2024-01-01T00:00:00+02:00",
                    "temperatureUnit": "°C",
                    "temperature": 16.9
                  },
                  {
                    "latitude": 38,
                    "longitude": 23.75,
                    "date": "2024-01-02T00:00:00+02:00",
                    "temperatureUnit": "°C",
                    "temperature": 16.5
                  }
                ],
                "isSuccessful": true,
                "errorMessage": null
              }
            ]
          },
          "news": {
            "apiProviders": [
              {
                "apiProvider": "Spaceflight News",
                "data": [
                  {
                    "title": "NASA delays Artemis 2 and 3 missions",
                    "url": "https://spacenews.com/nasa-delays-artemis-2-and-3-missions/",
                    "site": "SpaceNews",
                    "summary": "NASA is postponing the next two Artemis missions, including the first crewed landing on the moon, by nearly a year to address technical issues that could affect the safety of the astronauts on board.",
                    "datePublished": "2024-01-09T23:08:25+00:00"
                  },
                  {
                    "title": "NASA Delays Next Artemis Missions to 2025 and 2026",
                    "url": "https://spacepolicyonline.com/news/nasa-delays-next-artemis-missions-to-2025-and-2026/",
                    "site": "SpacePolicyOnline.com",
                    "summary": "NASA announced today that the Artemis II mission that was to launch this year and send astronauts around the Moon for the first time in five decades is being delayed […]",
                    "datePublished": "2024-01-09T22:25:30+00:00"
                  },
                  {
                    "title": "Launch Roundup: Axiom-3 crew and Tianzhou 7 cargo space station missions this week",
                    "url": "https://www.nasaspaceflight.com/2024/01/1824-launch-roundup/",
                    "site": "NASASpaceflight",
                    "summary": "A pair of missions to space stations headline the launches for the week of Jan. 10 to Jan. 17. Axiom-3 will be flying four private astronauts to the International Space Station (ISS), becoming the first crew launch of 2024. Crew Dragon Freedom will fly from Florida, with a multinational crew, for a mission to the Station lasting around two weeks.",
                    "datePublished": "2024-01-09T21:52:09+00:00"
                  }
                ],
                "isSuccessful": true,
                "errorMessage": null
              }
            ]
          },
          "books": {
            "apiProviders": [
              {
                "apiProvider": "Open Library",
                "data": [
                  {
                    "title": "Transactions on Intelligent Welding Manufacturing",
                    "authors": [
                      "Shanben Chen",
                      "Yuming Zhang",
                      "Zhili Feng"
                    ],
                    "datePublished": "2024-01-01T00:00:00+00:00"
                  },
                  {
                    "title": "Genetic and Evolutionary Computing",
                    "authors": [
                      "Jeng-Shyang Pan",
                      "Jerry Chun-Wei Lin",
                      "Chia-Hung Wang",
                      "Xin Hua Jiang"
                    ],
                    "datePublished": "2024-01-01T00:00:00+00:00"
                  }
                ],
                "isSuccessful": true,
                "errorMessage": null
              }
            ]
          }
        }
      ```

## Running Unit Tests

1. **Navigate to Test Project Directory**: Change your current directory to a specific test project directory.

    ```bash
    cd DataHub/test/DataHub.Server.Tests
    ```

2. **Run Unit Tests**: Execute the unit tests using the following command:

    ```bash
    dotnet test
    ```
