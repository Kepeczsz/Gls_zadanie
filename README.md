# GLS Labels Management System / System Zarządzania Etykietami GLS

## 📋 Project Overview / Przegląd Projektu

**English:**
This is an Azure Functions-based application that integrates with the GLS (General Logistics Systems) API to automatically fetch, manage, and print shipping labels. The system periodically retrieves labels from the GLS API, stores them in a SQL Server database, and provides functionality to generate PDF documents for printing.

**Polski:**
Aplikacja oparta na Azure Functions, która integruje się z API GLS (General Logistics Systems) w celu automatycznego pobierania, zarządzania i drukowania etykiet wysyłkowych. System okresowo pobiera etykiety z API GLS, przechowuje je w bazie danych SQL Server i zapewnia funkcjonalność generowania dokumentów PDF do druku.

## 🚀 Key Features / Główne Funkcje

- **Automated Label Retrieval**: Timer-triggered function runs every 10 minutes to fetch new labels from GLS API
- **Database Storage**: Efficient storage and management of labels using Entity Framework Core with SQL Server
- **PDF Generation**: Creates PDF documents with labels using iText7 library
- **Printer Integration**: Sends generated PDFs to moja-drukarka.pl API for printing
- **Multi-User Support**: Handles labels for multiple users with separate authentication

---

- **Automatyczne Pobieranie Etykiet**: Funkcja uruchamiana co 10 minut pobiera nowe etykiety z API GLS
- **Przechowywanie w Bazie Danych**: Efektywne przechowywanie i zarządzanie etykietami przy użyciu Entity Framework Core z SQL Server
- **Generowanie PDF**: Tworzy dokumenty PDF z etykietami przy użyciu biblioteki iText7
- **Integracja z Drukarką**: Wysyła wygenerowane pliki PDF do API moja-drukarka.pl w celu wydruku
- **Obsługa Wielu Użytkowników**: Obsługuje etykiety dla wielu użytkowników z osobnymi danymi uwierzytelniającymi

## 🛠️ Technology Stack / Stack Technologiczny

- **.NET 8.0**: Latest .NET framework for modern application development
- **Azure Functions v4**: Serverless compute service for event-driven applications
- **Entity Framework Core 8.0**: Object-relational mapping (ORM) for database operations
- **SQL Server**: Relational database for storing labels and user information
- **iText7**: PDF generation library
- **RestSharp**: HTTP client library for API communication
- **NUnit**: Testing framework
- **Application Insights**: Monitoring and diagnostics

## 📁 Project Structure / Struktura Projektu

```
Gls_zadanie/
├── Gls-Etykiety/                    # Main application project
│   ├── Configuration/               # Application configuration
│   ├── Domain/                      # Database context and domain logic
│   ├── Exceptions/                  # Custom exception classes
│   ├── Extensions/                  # Extension methods and utilities
│   ├── FunctionApp.Labels/          # Azure Functions implementations
│   │   ├── GetLabels.cs            # Timer-triggered function for fetching labels
│   │   └── PostLabels.cs           # HTTP-triggered function for printing labels
│   ├── Migrations/                  # EF Core database migrations
│   ├── Models/                      # Data models and DTOs
│   ├── Program.cs                   # Application entry point
│   └── host.json                    # Azure Functions host configuration
└── LabelsTests/                     # Test project
    └── PostLabelsTests.cs           # Unit tests
```

## ⚙️ Setup Instructions / Instrukcje Instalacji

### Prerequisites / Wymagania Wstępne

- .NET 8.0 SDK
- SQL Server (local or remote)
- Azure Functions Core Tools (optional, for local testing)
- Visual Studio 2022 or Visual Studio Code with C# extension

### Installation Steps / Kroki Instalacji

**English:**

1. **Clone the repository**
   ```bash
   git clone https://github.com/Kepeczsz/Gls_zadanie.git
   cd Gls_zadanie
   ```

2. **Configure the database connection**
   - Update the connection string in `local.settings.json` file
   - Ensure your SQL Server instance is running and accessible

3. **Run database migrations**
   ```bash
   cd Gls-Etykiety
   dotnet ef database update
   ```
   Or in Visual Studio Package Manager Console:
   ```
   Update-Database
   ```

4. **Configure GLS API credentials**
   - Add user credentials in the database `Users` table
   - Update API endpoints if necessary in `GetLabels.cs`

5. **Build and run the application**
   ```bash
   dotnet build
   dotnet run
   ```

**Polski:**

1. **Sklonuj repozytorium**
   ```bash
   git clone https://github.com/Kepeczsz/Gls_zadanie.git
   cd Gls_zadanie
   ```

2. **Skonfiguruj połączenie z bazą danych**
   - Zaktualizuj connection string w pliku `local.settings.json`
   - Upewnij się, że instancja SQL Server jest uruchomiona i dostępna

3. **Wykonaj migracje bazy danych**
   ```bash
   cd Gls-Etykiety
   dotnet ef database update
   ```
   Lub w konsoli Package Manager w Visual Studio:
   ```
   Update-Database
   ```

4. **Skonfiguruj dane dostępowe do API GLS**
   - Dodaj dane uwierzytelniające użytkowników do tabeli `Users` w bazie danych
   - W razie potrzeby zaktualizuj endpointy API w pliku `GetLabels.cs`

5. **Zbuduj i uruchom aplikację**
   ```bash
   dotnet build
   dotnet run
   ```

## 📖 Usage / Użytkowanie

### Timer Function (Automatic Label Fetching)

The `SaveLabelsToDatabase` function runs automatically every 10 minutes and:
- Authenticates with GLS API for each user
- Retrieves package IDs
- Fetches labels for each package
- Converts Base64 encoded data to strings
- Stores labels in the database

### HTTP Function (Print Labels)

Send a POST request to the `PostLabels` endpoint with user ID:
```json
{
  "id": "user-guid-here"
}
```

The function will:
- Retrieve all labels for the specified user
- Generate PDF documents (10 labels per document)
- Send PDFs to the printer API

## 🧪 Testing / Testowanie

```bash
cd LabelsTests
dotnet test
```

**Note**: The test suite is currently under development and requires implementation of repository pattern for proper database mocking.

**Uwaga**: Zestaw testów jest obecnie w fazie rozwoju i wymaga implementacji wzorca repozytorium do prawidłowego mockowania bazy danych.

## 📝 Configuration / Konfiguracja

Key configuration settings in `local.settings.json`:

```json
{
  "ConnectionStrings": {
    "Db": "your-sql-server-connection-string"
  },
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```

## 🤝 Contributing / Współpraca

Contributions are welcome! Please feel free to submit issues and pull requests.

Wkład w projekt jest mile widziany! Prosimy o zgłaszanie problemów i pull requestów.

## 📄 License / Licencja

This project is for educational/demonstration purposes.

Ten projekt ma charakter edukacyjny/demonstracyjny.

## 🔗 Related Links / Powiązane Linki

- [GLS API Documentation](https://gls-group.eu)
- [Azure Functions Documentation](https://docs.microsoft.com/azure/azure-functions/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core/)
