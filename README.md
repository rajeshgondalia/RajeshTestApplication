# ReqresApi Integration Library

This project demonstrates a robust .NET 5+ class library for interacting with the public API [reqres.in](https://reqres.in/). It includes API client logic, error handling, and unit tests.

## 🔧 Features

- Fetch a single user or all paginated users
- Resilient error handling (404s, deserialization issues)
- Uses `HttpClientFactory`
- Fully async
- Ready for DI in a real-world app
- Cache responses in-memory for repeated calls
- Exception Handling with Logerror

## 🚀 How to Run

1. Clone the repo
2. Open the solution
3. Run `dotnet test` to run unit tests

## 🧪 Test Coverage

- `ExternalUserServiceTests` covers typical scenarios using mocking
- Extend to test network failures or add retry logic with Polly

