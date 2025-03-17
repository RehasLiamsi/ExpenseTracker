# Expense Tracker API

## Overview

The **Expense Tracker API** is a RESTful service built using **C# (.NET 8.0)** and **PostgreSQL**. It allows users to **register, log in, manage expenses, and track their financial data** securely.

## Features

- **User Authentication** (JWT-based login & registration)

- **Expense Management** (Create, Read, Update, Delete expenses)

- **Secure API Endpoints** (Authorization using JWT tokens)

- **Patch Requests** (Partial updates for user profiles)

- **Automated Data Mapping** (Using AutoMapper for DTOs)

- **Error Handling** (Handles invalid requests, authentication failures, etc.)


## Technologies Used

- **.NET 8.0** (ASP.NET Core Web API)

- **Entity Framework Core** (Database ORM)

- **PostgreSQL** (Database storage)

- **AutoMapper** (Model to DTO mapping)

- **JWT Authentication** (Secure user authentication)

- **Newtonsoft.Json** (JSON Patch handling)

<br />

## Setup & Installation

### Prerequisites

Ensure you have the following installed:

- **.NET SDK 8.0**

- **Visual Studio 2022** (or VS Code with C# extension)

- **PostgreSQL Database**

- **Postman** (For API testing)


### Step 1: Clone the Repository
```sh
git clone https://github.com/your-username/expense-tracker.git
cd expense-tracker
```

### Step 2: Configure PostgreSQL

Create a PostgreSQL database and update appsettings.json:
```sh
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ExpenseTracker;Username=postgres;Password=yourpassword"
}
```

### Step 3: Install Dependencies
```sh
dotnet restore
```

### Step 4: Apply Migrations & Run Database Setup
```sh
dotnet ef database update
```

### Step 5: Run the Application
```sh
dotnet run
```

The API will be available at:

- **HTTPS:** ``` https://localhost:7002 ```

- **HTTP:** ``` http://localhost:5282 ```

<br />

Contribution Guide
------------------

Feel free to contribute to this project by submitting issues and pull requests.

### Steps to Contribute:

1.  Fork the repository
    
2.  Create a feature branch (git checkout -b feature-new-feature)
    
3.  Commit your changes (git commit -m "Added a new feature")
    
4.  Push to the branch (git push origin feature-new-feature)
    
5.  Open a Pull Request

<br />
    
License
-------

This project is licensed under the **MIT License**.

<br />

Contact & Support
-----------------

For any issues, please open a GitHub issue or reach out via email.

🚀 **Happy Coding!**
