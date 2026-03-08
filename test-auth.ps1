# Test Authentication Endpoints

Write-Host "=== Testing Travel App Authentication ===" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5268"

# Test 1: Register new user
Write-Host "1. Testing Register..." -ForegroundColor Yellow
$registerData = @{
    fullName = "John Doe"
    email = "john.doe@example.com"
    password = "Password123!"
    phone = "+1234567890"
} | ConvertTo-Json

try {
    $registerResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/register" `
        -Method Post `
        -Body $registerData `
        -ContentType "application/json"
    
    Write-Host "✓ Register successful!" -ForegroundColor Green
    Write-Host "User ID: $($registerResponse.userId)" -ForegroundColor Gray
    Write-Host "Role: $($registerResponse.role)" -ForegroundColor Gray
    Write-Host "Token: $($registerResponse.token.Substring(0, 50))..." -ForegroundColor Gray
    Write-Host ""
    
    $token = $registerResponse.token
} catch {
    Write-Host "✗ Register failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
}

# Test 2: Login with registered user
Write-Host "2. Testing Login..." -ForegroundColor Yellow
$loginData = @{
    email = "john.doe@example.com"
    password = "Password123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" `
        -Method Post `
        -Body $loginData `
        -ContentType "application/json"
    
    Write-Host "✓ Login successful!" -ForegroundColor Green
    Write-Host "User: $($loginResponse.fullName)" -ForegroundColor Gray
    Write-Host "Email: $($loginResponse.email)" -ForegroundColor Gray
    Write-Host "Role: $($loginResponse.role)" -ForegroundColor Gray
    Write-Host "Token: $($loginResponse.token.Substring(0, 50))..." -ForegroundColor Gray
    Write-Host ""
} catch {
    Write-Host "✗ Login failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
}

# Test 3: Login with wrong password
Write-Host "3. Testing Login with wrong password..." -ForegroundColor Yellow
$wrongLoginData = @{
    email = "john.doe@example.com"
    password = "WrongPassword"
} | ConvertTo-Json

try {
    $wrongLoginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" `
        -Method Post `
        -Body $wrongLoginData `
        -ContentType "application/json"
    
    Write-Host "✗ Should have failed but didn't!" -ForegroundColor Red
} catch {
    Write-Host "✓ Correctly rejected wrong password" -ForegroundColor Green
    Write-Host ""
}

# Test 4: Register with existing email
Write-Host "4. Testing Register with existing email..." -ForegroundColor Yellow
try {
    $duplicateResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/register" `
        -Method Post `
        -Body $registerData `
        -ContentType "application/json"
    
    Write-Host "✗ Should have failed but didn't!" -ForegroundColor Red
} catch {
    Write-Host "✓ Correctly rejected duplicate email" -ForegroundColor Green
    Write-Host ""
}

Write-Host "=== Testing Complete ===" -ForegroundColor Cyan
