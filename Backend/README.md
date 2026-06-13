

# Lumini Backend

A .NET 9 backend service that integrates both **PostgreSQL** (SQL) and **MongoDB** (NoSQL) databases with JWT authentication. Configuration is managed through environment variables using a `.env` file instead of `appsettings.json`.

## Prerequisites

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **PostgreSQL 12+** - [Download](https://www.postgresql.org/download/)
- **MongoDB** - [Download](https://www.mongodb.com/try/download/community) or use [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
- **Git**

## Installation

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd Lumini/Backend
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

## Environment Configuration

The application uses a `.env` file for all configuration. Create a `.env` file in the root of the `Backend` folder:

### .env File Template

```env
# PostgreSQL Configuration
POSTGRESQL_CONNECTION=Server=localhost;Port=5432;Database=lumini_db;User Id=postgres;Password=your_password;
POSTGRESQL_NAME=lumini_db

# MongoDB Configuration
MONGODB_CONNECTION=mongodb://localhost:27017
MONGODB_NAME=lumini_mongodb

# JWT Configuration
SECRET_KEY=your_super_secret_key_with_at_least_32_characters_for_security
AUDIENCE=your_app_audience
ISSUER=your_app_issuer
JWT_EXPIRATION_HOURS=3
```

### Configuration Variables Explained

| Variable | Description | Example |
|----------|-------------|---------|
| `POSTGRESQL_CONNECTION` | PostgreSQL connection string | `Server=localhost;Port=5432;Database=lumini_db;User Id=postgres;Password=pass123;` |
| `POSTGRESQL_NAME` | Database name in PostgreSQL | `lumini_db` |
| `MONGODB_CONNECTION` | MongoDB connection URI | `mongodb://localhost:27017` |
| `MONGODB_NAME` | Database name in MongoDB | `lumini_mongodb` |
| `SECRET_KEY` | JWT signing key (min. 32 chars) | `your_super_secret_key_...` |
| `AUDIENCE` | JWT audience claim | `lumini-app` |
| `ISSUER` | JWT issuer claim | `lumini-issuer` |
| `JWT_EXPIRATION_HOURS` | Token expiration in hours | `3` |

> **Security Note:** Never commit the `.env` file to version control. Add it to `.gitignore`.

## Database Setup

### PostgreSQL Setup

1. **Create the database:**
   ```bash
   psql -U postgres -c "CREATE DATABASE lumini_db;"
   ```

2. **Apply migrations:**
   ```bash
   dotnet ef database update --context PostgreSQLContext
   ```

3. **Create a new migration (if needed):**
   ```bash
   dotnet ef migrations add YourMigrationName --context PostgreSQLContext
   ```

### MongoDB Setup

- **Local installation:** MongoDB will be available at `mongodb://localhost:27017` by default
- **MongoDB Atlas:** Update `MONGODB_CONNECTION` with your connection string from Atlas
  - Example: `mongodb+srv://username:password@cluster.mongodb.net`

## Running the Application

### Development Mode

```bash
dotnet run
```

The API will be available at `http://localhost:5000` (or the configured port in `launchSettings.json`)

### Production Mode

```bash
dotnet publish -c Release -o ./publish
cd ./publish
./Backend
```

## Project Structure

```
Backend/
├── src/
│   ├── User/              # User models and entities
│   │   └── user_model.cs
│   └── LightPoles/        # Light poles models
│       └── LightPoles_model.cs
├── DB/
│   ├── PostgreSQL/        # SQL database context
│   │   ├── SQL.cs
│   │   └── PostgreSQLContextFactory.cs
│   └── MongoDB/           # NoSQL database context
│       └── NoSQL.cs
├── JWT/
│   └── JwtService.cs      # JWT authentication service
├── Bycript/
│   └── Bycript.cs         # Password hashing and encryption
├── Middleware/
│   └── ConsoleLogs.cs     # Request/Response logging middleware
├── Error/
│   └── result.cs          # Error handling and response models
├── Migrations/            # Entity Framework migrations
├── Properties/
│   └── launchSettings.json
├── Program.cs             # Application entry point
├── Backend.csproj         # Project configuration
└── .env                   # Environment variables (DO NOT COMMIT)
```

## JWT Authentication

The backend uses JWT for authentication:

1. **User Registration:** Creates a new user and hashes password using Bcrypt
2. **User Login:** Returns a JWT token with configured expiration
3. **Token Usage:** Include the token in the `Authorization` header:
   ```
   Authorization: Bearer <your_jwt_token>
   ```

## Running Migrations

### Create a Migration
```bash
dotnet ef migrations add NameOfMigration --context PostgreSQLContext
```

### Apply Migrations
```bash
dotnet ef database update --context PostgreSQLContext
```

### Rollback Last Migration
```bash
dotnet ef migrations remove --context PostgreSQLContext
```

## API Endpoints

Base URL: `http://localhost:5000/api`

Example endpoints (customize based on your implementation):
- `GET /users` - Get all users
- `POST /auth/register` - Register new user
- `POST /auth/login` - Login user

## Testing the API

You can use the provided `Backend.http` file in VS Code with the REST Client extension to test the endpoints.

## Troubleshooting

### PostgreSQL Connection Issues
- Ensure PostgreSQL service is running
- Check credentials in `.env` match your PostgreSQL user
- Verify database exists

### MongoDB Connection Issues
- Ensure MongoDB service is running
- Check connection string format
- Verify network access (especially for MongoDB Atlas)

### JWT Token Issues
- Ensure `SECRET_KEY` has at least 32 characters
- Check token expiration hasn't elapsed
- Verify `AUDIENCE` and `ISSUER` match between token creation and validation

## Dependencies

- **Entity Framework Core** - ORM for PostgreSQL
- **MongoDB.Driver** - Official MongoDB C# driver
- **System.IdentityModel.Tokens.Jwt** - JWT handling
- **BCrypt.Net-Next** - Password hashing

## MIT

This is a personal project which also functions as a template for .NET 9, PostgreSQL, and MongoDB. If you wish to use this configuration, simply delete the SRC folder and create your own models, repositories, interfaces, and controllers, as well as add the respective scopes in the **program.cs**