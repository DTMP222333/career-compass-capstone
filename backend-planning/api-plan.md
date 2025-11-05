# Career Compass Backend API – Initial Plan (PJ)

## Overview
The backend will be built using ASP.NET Core (.NET 8).  
It will be deployed on Azure and connected to Azure SQL Database.

## Data We Store
- User Name
- Birthdate
- Numerology Calculations (Life Path & Expression)

## API Endpoints (Initial List)
POST /api/user/register  
GET /api/user/{id}  
POST /api/numerology/calculate

## Tech Stack
- ASP.NET Core Web API
- Entity Framework Core
- Azure SQL Database

## How API Talks to Database
EF Core models → DbContext → Azure SQL connection string

## Next Steps
- Design ERD diagram
- Create sample EF models (User, NumerologyResult)
