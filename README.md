# \# CafeApp Backend

# 

# This is the backend service for \*\*CafeApp\*\*, built with \*\*.NET 8\*\*, \*\*Entity Framework Core (EF Core)\*\*, and \*\*MySQL\*\*.  

# It provides APIs to manage cafes and employees with CQRS using \*\*MediatR\*\*.

# 

# ---

# 

# \## 📦 Tech Stack

# \- \*\*.NET 8 Web API\*\*

# \- \*\*Entity Framework Core\*\* with Pomelo MySQL Provider

# \- \*\*MediatR\*\* for CQRS pattern

# \- \*\*XUnit\*\* for unit testing

# \- \*\*MySQL 8\*\* for database

# 

# ---

# 

# \## 🚀 Getting Started

# 

# \### 1. Clone the repository

# ```bash

# git clone https://github.com/danielntda/CafeApp.git

# cd CafeApp





# \### 2. Run the SQL file 

# CafeSQLCreationSeedData.sql





# \### 3. Update Configuration

Edit appsettings.json with your MySQL connection string:
"ConnectionStrings": {
===

# &nbsp; "DefaultConnection": "server=\*.\*.\*.\*;port=\*\*\*\*;database=cafedatabase;user=\*yourusername\*;password=\*yourpassword\*;"

# }





# \### 4. Run the Application

# dotnet build

# dotnet run --project CafeApp.Api





# \### 5. Run tests

# dotnet test







