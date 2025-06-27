# 🛒 ASP.NET Core E-Commerce Backend API

A **production-ready e-commerce REST API** built using **ASP.NET Core Web API 8** and **Entity Framework Core**. This backend powers a full shopping experience with user authentication, product catalog, cart, orders, payments, admin controls, and more.

---

## 🚀 Live API

- **Swagger UI Link :** https://storegrid-api.azurewebsites.net/swagger/index.html

---

## 🧰 Tech Stack

- **ASP.NET Core 8 (Web API)**
- **Entity Framework Core 8**
- **SQL Server**
- **AutoMapper**
- **JWT Authentication**
- **Gmail SMTP (for email services)**
- **Swagger** – API documentation

---

## 📁 Folder Structure

- `Controllers/` – API controllers by feature
- `Models/` – Entity models
- `DTOs/` – Request and response data models
- `Services/` – Business logic for each module
- `Mappings/` – AutoMapper configuration
- `Constants/` – All the constants like order status etc goes here.
- `Data/` – ApplicationDbContext & migrations

---

## 🔐 Authentication & Authorization

- **JWT-based Auth**
- Role-based system with 3 roles:
  - `User`
  - `Admin`
  - `SuperAdmin` (seeded user)
- Secure password hashing and email verification

---

## ✅ Features

### 👤 User Module

- Signup, Login, JWT Token generation
- Change password, forgot/reset password
- Update username & phone number
- Get user profile

### 📦 Product Module

- Browse all products, categories, brands
- Product details with variants (color/size)
- Related products & new arrivals
- Add/remove favorites

### 🛍 Cart & Checkout

- Add/update/remove items from cart
- Clear cart
- Partial cart checkout
- Checkout preview endpoint
- Place order with optional proof image

### 📬 Address Management

- Add, update, delete addresses
- Set default address

### 📦 Order Management

- View all orders
- View order details
- Cancel orders

### 📝 Review System

- Leave reviews only for purchased products
- View all personal reviews
- Edit/delete reviews

### 🔧 Admin Panel

- Manage categories, products, orders, and reviews
- Toggle product active/inactive
- Filter orders by status
- Create other admins (only SuperAdmin)

---

## 🌐 API Endpoints

### 🔐 Auth

- `POST /api/Auth/Signup`
- `POST /api/Auth/Login`
- `POST /api/Auth/VerifyEmail`

### 👤 User Profile

- `GET /api/UserProfile/GetUserProfile`
- `POST /api/UserProfile/UpdateUsername`
- `POST /api/UserProfile/UpdatePhoneNumber`
- `POST /api/UserProfile/ChangePassword`
- `POST /api/UserProfile/ForgotPassword`
- `POST /api/UserProfile/ResetPassword`

### 🛍 User Cart

- `GET /api/UserCart/GetCartItems`
- `POST /api/UserCart/AddCartItem`
- `PUT /api/UserCart/UpdateCartItemQuantity`
- `DELETE /api/UserCart/RemoveCartItem/{cartItemId}`
- `DELETE /api/UserCart/ClearCart`

### 💖 User Favorite

- `GET /api/UserFavorite`
- `POST /api/UserFavorite/AddtoFavorite/{ProductId}`
- `DELETE /api/UserFavorite/RemoveFavorite/{ProductId}`

### 📬 User Address

- `GET /api/UserAddress/GetAddresses`
- `POST /api/UserAddress/AddAddress`
- `PUT /api/UserAddress/UpdateAddress/{AddressId}`
- `DELETE /api/UserAddress/DeleteAddress/{AddressId}`

### 📦 User Orders

- `GET /api/UserOrder/MyOrders`
- `GET /api/UserOrder/OrderDetails/{orderId}`
- `POST /api/UserOrder/CheckoutPreview`
- `POST /api/UserOrder/PlaceOrder`
- `PUT /api/UserOrder/cancel/{orderId}`

### 📝 User Reviews

- `GET /api/UserReview/GetMyReviews`
- `GET /api/UserReview/GetMyReviewForProduct/{productId}`
- `POST /api/UserReview/AddReview`
- `PUT /api/UserReview/UpadateReview/{reviewId}`
- `DELETE /api/UserReview/DeleteReview/{reviewId}`
- `GET /api/UserReview/GetReviewableProducts`

### 🛒 Product

- `GET /api/Product/AllProducts`
- `GET /api/Product/NewArrivals`
- `GET /api/Product/{productId}`
- `GET /api/Product/Category/{categoryName}`
- `GET /api/Product/Brand/{brandName}`
- `GET /api/Product/{productId}/Related`
- `GET /api/Product/GetCategories`

---

### 🛠 Admin Product

- `POST /api/AdminProduct/create`
- `GET /api/AdminProduct/all`
- `GET /api/AdminProduct/{id}`
- `PUT /api/AdminProduct/update/{id}`
- `PUT /api/AdminProduct/DeactiveProduct/{ProductId}`
- `PUT /api/AdminProduct/ActiveProduct/{ProductId}`

### 🗂 Admin Category

- `POST /api/AdminCategory/CreateCategory`
- `DELETE /api/AdminCategory/DeleteCategory/{id}`
- `GET /api/AdminCategory/GetAllCategories`
- `GET /api/AdminCategory/GetCategoryById/{id}`
- `PUT /api/AdminCategory/UpdateCategory/{id}`

### 🧾 Admin Order

- `GET /api/AdminOrder`
- `GET /api/AdminOrder/GetOrderByStatus`
- `GET /api/AdminOrder/{orderId}`
- `PUT /api/AdminOrder/{orderId}/UpdateStatus`
- `GET /api/AdminOrder/AllStatuses`

### 👮‍♂️ Admin Review

- `GET /api/AdminReview/GetAllReviews`
- `GET /api/AdminReview/FilterByProduct/{productId}`
- `DELETE /api/AdminReview/DeleteReview/{id}`

### 👤 Admin Registration

- `POST /api/AdminRegister/RegisterAdmin`

---


## 🔐 Security

- All endpoints use [Authorize] with policy/role-based checks
- Passwords are securely hashed
- JWT tokens expire in 1 hour
- Email-based verification for password reset
- Admin-only endpoints protected with `Role = Admin/SuperAdmin`

---

## ⚙ Setup Instructions

### 🧰 Prerequisites

- .NET 8 SDK
- SQL Server / LocalDB

### 🛠️ Setup

```bash
git clone https://github.com/Mutashim99/EcommerceAPI
cd ecommerce-backend
```

1. Add `appsettings.json` with:

```json
  {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultString": "Your Connection String"
  },
  "JwtSettings": {
    "Key": "Your Key", 
    "Issuer": "Your Issuer",
    "Audience": "Your Audience"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": "587",
    "SenderEmail": "Your sender Email",
    "SenderName": "Your Sender Name",
    "Password": "Your Gmail APP Password"
  },
  "FrontendDomainForEmailVerification": "Your Front end Domain to send in the emails with links"
}

```

2. Run the migration:

```bash
dotnet ef database update
```

3. Start the API:

```bash
dotnet run
```

4. Open Swagger:

```
https://localhost:<port>/swagger
```

---

## 🙋 Author

Built by **Mutashim Mohsin**\
Open for internship & collaboration opportunities!

🔗 GitHub: https://github.com/Mutashim99\
📧 Email: muhtashimmohsin@outlook.com

---

## 📄 License

This project is licensed under the MIT License.

