-- ===============================================
-- Travel App Database Setup Script
-- PostgreSQL
-- ===============================================

-- Step 1: Create the database (run this as postgres superuser)
CREATE DATABASE travelapp_db
    WITH 
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.UTF-8'
    LC_CTYPE = 'en_US.UTF-8'
    TEMPLATE = template0;

-- Step 2: Connect to travelapp_db and enable extensions
\c travelapp_db

-- Enable UUID generation
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Step 3: After running this script, apply EF Core migrations using:
-- dotnet ef database update --project TravelApp.Infrastructure --startup-project TravelApp.API
