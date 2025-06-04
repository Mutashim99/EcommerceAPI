# 🛒 ASP.NET Core eCommerce Backend API

A complete, eCommerce backend built with **ASP.NET Core Web API** and **Entity Framework Core**, supporting all essential operations of a modern eCommerce platform.

---

## 🚀 Features

### 👤 User Management
- JWT Authentication & Authorization
- Role-based access (Admin, Customer)
- Register, Login, Profile Update
- Password Change & Forgot/Reset Password (via Email)
- Soft Delete / Deactivate Account

### 🛍️ Product Management
- CRUD for Products
- Product Variants (Size, Color, Price, Stock)
- Product Categories
- Product Reviews
- Product Images (coming soon)

### 📦 Order & Cart
- Add to Cart
- Update & Remove Cart Items
- Place Orders from Cart
- Track Orders
- Order History for Users
- Admin View of All Orders

### 💖 Wishlist / Favorites
- Add or Remove Products from Wishlist

### 📧 Email Services
- Send verification, reset password, and notification emails

### 🛠 Admin Features
- View all users
- Assign roles
- Manage product catalog
- View all orders

---

## 🧰 Tech Stack

- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core**
- **SQL Server**
- **JWT** for Authentication
- **MailKit** for Email Services
- **Swagger** for API testing
- **AutoMapper** (optional)
- **FluentValidation** (optional)

---

## 🏗️ Project Structure
EcommerceAPI/
│
├── Controllers/
├── Services/
├── Models/
├── Data/ # DbContext & Migrations
├── DTOs/ # (if used)
├── Helpers/ # Email, JWT generation, etc.
├── Program.cs
└── appsettings.json

---

## ⚙️ Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/Mutashim99/EcommerceAPI.git
cd ecommerce-api
```
### 2. Configure your DB and Email
- Update appsettings.json:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Your SQL Server connection string"
},
"Jwt": {
  "Key": "Your_Secret_Key",
  "Issuer": "your-app",
  "Audience: "your audience"
},
"EmailSettings": {
  "SmtpServer": "smtp.yourdomain.com",
  "Port": 587,
  "SenderEmail": "your-email@domain.com",
  "SenderName" : "Your Name",
  "Password": "your-email-password"
}
```

### 3. Run Migrations & Database Update
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the API

- Run the application using the .NET CLI:

```bash
dotnet run
```


---

- You can replace `5000`/`5001` with your actual ports if you've changed them. Let me know if you'd like me to regenerate the full README with this included.


### 🤝 Contributing
- Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
