Project Documentation: Grocery and Money Donation System

Introduction

This project is a Grocery and Money Donation Management System built using ASP.NET Core MVC. It allows users to donate food and money, track available stock, deduct food usage, withdraw money, and maintain a history of transactions.

Features Implemented

User Authentication & Roles

Users can register, log in, and authenticate via ASP.NET Identity.

Admins have additional privileges for managing donations and withdrawals.

Grocery Donations

Users can donate different types of food items.

Donations are recorded and accumulated in the database.

Each donation includes a quantity field.

Tracking Total Available Food Stock

The system calculates the total quantity of each food type donated.

When new donations are added, the system updates the total available stock.

Food Usage and Deduction

Admins can specify the quantity of each food item used for meals (Breakfast, Lunch, or Dinner).

The system deducts the used quantity from the oldest available stock first (FIFO method).

A history of food usage transactions is maintained.

Money Donations

Users can donate money through the system.

Donations are recorded along with donor details.

Money Withdrawal Feature

Admins can withdraw money for specific purposes.

The system tracks all withdrawals with timestamps and reasons.

Available balance updates after each withdrawal.

Transaction History

Users can view a history of both food usage and money withdrawals.

Admins can track how donations are utilized over time.


![Screenshot 2025-01-17 150440](https://github.com/user-attachments/assets/9058bde7-27eb-46d7-a59d-7f5d73cc3957)
![Screenshot 2025-02-11 181634](https://github.com/user-attachments/assets/0a525079-f639-439c-b07d-15cf97709050)
![Screenshot 2025-02-11 182056](https://github.com/user-attachments/assets/a3f58d24-40fb-4d52-9799-43f5236db135)
![Screenshot 2025-02-11 182038](https://github.com/user-attachments/assets/d45b4cc1-72bd-456e-84e2-18e83f97cae4)
![Screenshot 2025-02-11 182024](https://github.com/user-attachments/assets/68c98084-1674-4351-99a6-6194fce105ad)
![Screenshot 2025-02-11 181959](https://github.com/user-attachments/assets/be3570b5-ec37-4692-8172-fa033dc52d02)
![Screenshot 2025-02-11 181941](https://github.com/user-attachments/assets/9ee89327-5328-498a-b0b8-2b2adb1c7757)
![Screenshot 2025-02-11 181928](https://github.com/user-attachments/assets/1712fb3e-10f0-48d1-a196-861c3874e09e)
![Screenshot 2025-02-11 181905](https://github.com/user-attachments/assets/6933f456-8d40-4d38-a8ad-2e204b646f0f)
![Screenshot 2025-02-11 181852](https://github.com/user-attachments/assets/33978e1d-8956-4a1f-8fac-2e866954a54e)
![Screenshot 2025-02-11 181820](https://github.com/user-attachments/assets/4d32d74b-e5d8-4a97-9642-a4b1eeb87c75)
![Screenshot 2025-02-11 181810](https://github.com/user-attachments/assets/8197c439-09b0-4781-926c-e1f7947f7e15)
![Screenshot 2025-02-11 181739](https://github.com/user-attachments/assets/a664fee4-f74a-4ec0-b3ff-f05ad6d33071)
![Screenshot 2025-02-11 181730](https://github.com/user-attachments/assets/7c3cc6f9-c5ca-46cb-a861-0e67721ceb33)
![Screenshot 2025-02-11 181717](https://github.com/user-attachments/assets/35d1e4f4-24a5-432d-84f1-cd422764bf94)
![Screenshot 2025-02-11 181659](https://github.com/user-attachments/assets/aab1eafb-9149-46c8-af5c-c403585c1917)
![Screenshot 2025-02-11 181651](https://github.com/user-attachments/assets/2823d77f-86d4-4978-95fd-cb9a2c8f972a)
