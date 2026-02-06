# <h1 align="center">🐄 Cattle Management System</h1>


<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge&logo=nuget&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/Tailwind%20CSS-3.4-06B6D4?style=for-the-badge&logo=tailwindcss&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)

**Aplikasi web modern untuk manajemen peternakan sapi perah yang komprehensif**

[Demo](#demo) • [Fitur](#fitur) • [Instalasi](#instalasi) • [Dokumentasi](#dokumentasi)

</div>

---

## 📋 Deskripsi

**Cattle Management System** adalah sistem informasi manajemen peternakan sapi perah berbasis web yang dibangun dengan ASP.NET Core 8.0 dan MySQL. Aplikasi ini dirancang untuk membantu peternak dan administrator dalam mengelola data sapi, produksi susu, produksi olahan, kesehatan sapi, dan jadwal kegiatan dengan antarmuka yang modern dan responsif.

### 🎯 Tujuan Proyek

- Digitalisasi pencatatan data peternakan
- Monitoring kesehatan dan produksi sapi secara real-time
- Mempermudah pembuatan laporan dan analisis data
- Meningkatkan efisiensi manajemen peternakan
- Menyediakan sistem role-based access untuk keamanan data

---

## ✨ Fitur

### 👤 Manajemen User & Akses

- ✅ **Multi-role Authentication** - Admin dan User dengan hak akses berbeda
- ✅ **Data Isolation** - User hanya melihat data miliknya sendiri
- ✅ **User Management** - Admin dapat membuat dan mengelola akun user
- ✅ **Profile Management** - Kelola data peternak dan link ke user

### 🐄 Manajemen Sapi

- ✅ **CRUD Sapi** - Tambah, edit, hapus data sapi
- ✅ **Tracking Status** - Aktif, Sakit, Dijual, Mati
- ✅ **Filter & Search** - Pencarian berdasarkan kode, nama, jenis kelamin
- ✅ **Data Lengkap** - Kode sapi, nama, tanggal lahir, jenis kelamin

### 🥛 Produksi Susu

- ✅ **Input Produksi Harian** - Catat produksi pagi dan sore
- ✅ **Statistik Real-time** - Total produksi per hari/minggu/bulan
- ✅ **Chart Visualization** - Grafik tren produksi 7 hari terakhir
- ✅ **Export Report** - Generate laporan PDF

### 🧀 Produksi Olahan

- ✅ **Manajemen Produk** - Yogurt, Keju, Mentega, Es Krim, dll
- ✅ **Tracking Kadaluarsa** - Monitor tanggal expired produk
- ✅ **Perhitungan Biaya** - Input biaya produksi dan harga jual
- ✅ **Status Produksi** - Tersedia, Terjual, Kadaluarsa

### 🏥 Kesehatan Sapi

- ✅ **Riwayat Pemeriksaan** - Vaksinasi, pengobatan, pemeriksaan rutin
- ✅ **Diagnosa & Gejala** - Catat diagnosis dan gejala penyakit
- ✅ **Data Vital** - Berat badan, suhu tubuh
- ✅ **Auto-update Status** - Status sapi otomatis berubah berdasarkan kesehatan
- ✅ **Tracking Biaya** - Monitor biaya pengobatan

### 📅 Jadwal Kegiatan

- ✅ **Scheduling System** - Buat dan kelola jadwal kegiatan
- ✅ **Pendaftaran Peserta** - User dapat mendaftar ke kegiatan
- ✅ **Absensi** - Admin kelola kehadiran peserta
- ✅ **Notifikasi** - Reminder kegiatan yang akan datang
- ✅ **Multi-jenis** - Vaksinasi, Pelatihan, Rapat, dll

### 📊 Dashboard & Laporan

- ✅ **Dashboard Interaktif** - Statistik real-time dengan visualisasi
- ✅ **Quick Actions** - Akses cepat ke fitur utama
- ✅ **Activity Log** - Histori aktivitas pengguna
- ✅ **PDF Report** - Generate laporan profesional dengan QuestPDF

### 🔐 Keamanan & Audit

- ✅ **Auto Audit Trail** - Semua perubahan data tercatat otomatis
- ✅ **Password Hashing** - Keamanan password dengan ASP.NET Identity
- ✅ **Role-based Access** - Kontrol akses berdasarkan role
- ✅ **Session Management** - Manajemen sesi pengguna yang aman

---

## 🛠️ Tech Stack

### Backend

| Technology                           | Version  | Purpose                         |
| ------------------------------------ | -------- | ------------------------------- |
| **ASP.NET Core MVC**                 | 8.0      | Web Framework                   |
| **C#**                               | 12.0     | Programming Language            |
| **Entity Framework Core**            | 8.0      | ORM (Object-Relational Mapping) |
| **Pomelo.EntityFrameworkCore.MySql** | 8.0.0    | MySQL Database Provider         |
| **ASP.NET Core Identity**            | 8.0      | Authentication & Authorization  |
| **QuestPDF**                         | 2024.3.0 | PDF Generation                  |
| **Newtonsoft.Json**                  | 13.0.3   | JSON Serialization              |

### Frontend

| Technology       | Version     | Purpose               |
| ---------------- | ----------- | --------------------- |
| **Razor Pages**  | -           | Server-side Rendering |
| **TailwindCSS**  | 3.4 (CDN)   | CSS Framework         |
| **Font Awesome** | 6.4.0 (CDN) | Icon Library          |
| **Chart.js**     | 4.x (CDN)   | Data Visualization    |
| **SweetAlert2**  | 11.x (CDN)  | Beautiful Alerts      |

### Database

| Technology | Version | Purpose                  |
| ---------- | ------- | ------------------------ |
| **MySQL**  | 8.0+    | Relational Database      |
| **MAMP**   | -       | Local Development Server |

### Development Tools

| Tool                   | Purpose                |
| ---------------------- | ---------------------- |
| **Visual Studio Code** | Code Editor            |
| **dotnet CLI**         | .NET Development Tools |
| **Git**                | Version Control        |
| **phpMyAdmin**         | Database Management    |

---

## 📦 Instalasi

### Prerequisites

Pastikan Anda telah menginstal:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.0+](https://www.mysql.com/) atau [MAMP](https://www.mamp.info/)
- [Git](https://git-scm.com/)
- [Visual Studio Code](https://code.visualstudio.com/) (Recommended)

### Langkah Instalasi

#### 1. Clone Repository

```bash
git clone https://github.com/username/sim-sapi.git
cd sim-sapi
```

#### 2. Restore Dependencies

```bash
dotnet restore
```

#### 3. Konfigurasi Database

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=db_simsapi;User=root;Password=yourpassword;CharSet=utf8mb4;"
  }
}
```

**Catatan untuk MAMP:**

- Default Port MySQL MAMP: `8889`
- Default User: `root`
- Default Password: `root`

#### 4. Buat Database

Buka phpMyAdmin dan buat database baru:

```sql
CREATE DATABASE db_simsapi CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

#### 5. Jalankan Migration

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### 6. Jalankan Aplikasi

```bash
dotnet run
```

Aplikasi akan berjalan di:

- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

---

## 👤 Default Login

Setelah instalasi, gunakan akun berikut untuk login:

### Admin Account

```
Email: admin@simsapi.com
Password: Admin123
```

### User Account (Peternak)

```
Email: user@simsapi.com
Password: User123
```

---

## 📖 Dokumentasi

### Struktur Project

```
SimSapi/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── DashboardController.cs
│   ├── SapiController.cs
│   ├── ProduksiSusuController.cs
│   ├── ProduksiOlahanController.cs
│   ├── KesehatanSapiController.cs
│   ├── JadwalKegiatanController.cs
│   ├── PeternakController.cs
│   ├── UserManagementController.cs
│   └── ReportController.cs
├── Models/               # Data Models
│   ├── Sapi.cs
│   ├── ProduksiSusu.cs
│   ├── ProduksiOlahan.cs
│   ├── KesehatanSapi.cs
│   ├── JadwalKegiatan.cs
│   ├── Peternak.cs
│   ├── ApplicationUser.cs
│   └── ActivityLog.cs
├── ViewModels/           # View Models
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   ├── DashboardViewModel.cs
│   └── CreateUserViewModel.cs
├── Views/                # Razor Views
│   ├── Account/
│   ├── Dashboard/
│   ├── Sapi/
│   ├── ProduksiSusu/
│   ├── ProduksiOlahan/
│   ├── KesehatanSapi/
│   ├── JadwalKegiatan/
│   ├── Peternak/
│   ├── UserManagement/
│   ├── Report/
│   └── Shared/
├── Data/                 # Database Context
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Services/             # Business Logic
│   └── ReportService.cs
├── Migrations/           # EF Core Migrations
├── wwwroot/              # Static Files
├── appsettings.json      # Configuration
└── Program.cs            # Application Entry Point
```

### Database Schema

```
┌────────────────┐       ┌────────────────┐
│  AspNetUsers   │◄──────│   Peternak     │
└────────┬───────┘       └────────────────┘
         │
         │ UserId (FK)
         │
    ┌────┴────────────────────┐
    │                         │
    ▼                         ▼
┌─────────┐            ┌─────────────┐
│  Sapi   │            │ProdOlahan   │
└────┬────┘            └─────────────┘
     │
     │ SapiId (FK)
     │
┌────┴──────────────┬──────────────┐
│                   │              │
▼                   ▼              ▼
┌──────────────┐ ┌──────────────┐  │
│ProduksiSusu  │ │KesehatanSapi │  │
└──────────────┘ └──────────────┘  │
                                   │
┌─────────────────┐                │
│JadwalKegiatan   │                │
└────────┬────────┘                │
         │                         │
         ▼                         ▼
┌──────────────────┐     ┌──────────────┐
│PesertaKegiatan   │     │ActivityLogs  │
│(Junction Table)  │     │(Audit Trail) │
└──────────────────┘     └──────────────┘
```

### API Endpoints (Routes)

#### Authentication

- `GET /Account/Login` - Halaman login
- `POST /Account/Login` - Proses login
- `GET /Account/Register` - Halaman registrasi
- `POST /Account/Register` - Proses registrasi
- `POST /Account/Logout` - Logout

#### Dashboard

- `GET /Dashboard/Index` - Dashboard utama

#### Sapi Management

- `GET /Sapi/Index` - List semua sapi
- `GET /Sapi/Create` - Form tambah sapi
- `POST /Sapi/Create` - Proses tambah sapi
- `GET /Sapi/Edit/{id}` - Form edit sapi
- `POST /Sapi/Edit/{id}` - Proses edit sapi
- `POST /Sapi/Delete/{id}` - Hapus sapi

#### Produksi Susu

- `GET /ProduksiSusu/Index` - List produksi susu
- `GET /ProduksiSusu/Create` - Form input produksi
- `POST /ProduksiSusu/Create` - Proses input produksi
- `GET /ProduksiSusu/Edit/{id}` - Form edit produksi
- `POST /ProduksiSusu/Edit/{id}` - Proses edit produksi
- `POST /ProduksiSusu/Delete/{id}` - Hapus produksi

#### Produksi Olahan

- `GET /ProduksiOlahan/Index` - List produksi olahan
- `GET /ProduksiOlahan/Create` - Form tambah produksi olahan
- `POST /ProduksiOlahan/Create` - Proses tambah produksi olahan
- `GET /ProduksiOlahan/Edit/{id}` - Form edit produksi olahan
- `POST /ProduksiOlahan/Edit/{id}` - Proses edit produksi olahan
- `POST /ProduksiOlahan/Delete/{id}` - Hapus produksi olahan

#### Kesehatan Sapi

- `GET /KesehatanSapi/Index` - List riwayat kesehatan
- `GET /KesehatanSapi/Create` - Form input riwayat kesehatan
- `POST /KesehatanSapi/Create` - Proses input riwayat kesehatan
- `GET /KesehatanSapi/Edit/{id}` - Form edit riwayat kesehatan
- `POST /KesehatanSapi/Edit/{id}` - Proses edit riwayat kesehatan
- `POST /KesehatanSapi/Delete/{id}` - Hapus riwayat kesehatan

#### Jadwal Kegiatan

- `GET /JadwalKegiatan/Index` - List jadwal kegiatan
- `GET /JadwalKegiatan/Details/{id}` - Detail jadwal kegiatan
- `GET /JadwalKegiatan/Create` - Form tambah jadwal (Admin)
- `POST /JadwalKegiatan/Create` - Proses tambah jadwal (Admin)
- `GET /JadwalKegiatan/Edit/{id}` - Form edit jadwal (Admin)
- `POST /JadwalKegiatan/Edit/{id}` - Proses edit jadwal (Admin)
- `POST /JadwalKegiatan/Delete/{id}` - Hapus jadwal (Admin)
- `POST /JadwalKegiatan/Daftar/{id}` - Daftar ke kegiatan (User)
- `POST /JadwalKegiatan/BatalDaftar/{id}` - Batal daftar (User)
- `POST /JadwalKegiatan/Absen` - Kelola absensi (Admin)

#### User Management (Admin Only)

- `GET /UserManagement/Index` - List semua user
- `GET /UserManagement/Create` - Form tambah user
- `POST /UserManagement/Create` - Proses tambah user
- `GET /UserManagement/Edit/{id}` - Form edit user
- `POST /UserManagement/Edit/{id}` - Proses edit user
- `POST /UserManagement/Delete/{id}` - Hapus user

#### Peternak

- `GET /Peternak/Index` - List semua peternak
- `GET /Peternak/Create` - Form tambah peternak
- `POST /Peternak/Create` - Proses tambah peternak
- `GET /Peternak/Edit/{id}` - Form edit peternak
- `POST /Peternak/Edit/{id}` - Proses edit peternak
- `POST /Peternak/Delete/{id}` - Hapus peternak (Admin)

#### Report

- `GET /Report/Index` - Halaman generate laporan
- `POST /Report/GeneratePDF` - Generate PDF report

---

## 🔐 Role & Permissions

### Admin Role

- ✅ Full access ke semua fitur
- ✅ Lihat data semua user
- ✅ Create, edit, delete data siapa saja
- ✅ User management (create user, assign role)
- ✅ Create dan manage jadwal kegiatan
- ✅ Kelola absensi kegiatan
- ✅ Akses semua laporan

### User Role (Peternak)

- ✅ Dashboard dengan data milik sendiri
- ✅ CRUD sapi milik sendiri
- ✅ CRUD produksi susu milik sendiri
- ✅ CRUD produksi olahan milik sendiri
- ✅ CRUD kesehatan sapi milik sendiri
- ✅ View dan daftar jadwal kegiatan
- ✅ Generate laporan data milik sendiri
- ❌ Tidak bisa lihat/edit data user lain
- ❌ Tidak bisa create/edit jadwal kegiatan

---

## 📸 Screenshots

### Dashboard

![Dashboard](docs/screenshots/dashboard.png)

### Kelola Sapi

![Kelola Sapi](docs/screenshots/kelola-sapi.png)

### Produksi Susu

![Produksi Susu](docs/screenshots/produksi-susu.png)

### Kesehatan Sapi

![Kesehatan Sapi](docs/screenshots/kesehatan-sapi.png)

### Jadwal Kegiatan

![Jadwal Kegiatan](docs/screenshots/jadwal-kegiatan.png)

---

## 🚀 Deployment

### Prerequisites untuk Production

- Server dengan .NET 8.0 Runtime
- MySQL Server 8.0+
- Web Server (IIS, Nginx, atau Apache)
- SSL Certificate (Recommended)

### Deployment Steps

#### 1. Build untuk Production

```bash
dotnet publish -c Release -o ./publish
```

#### 2. Setup Database

```bash
# Update connection string di appsettings.Production.json
dotnet ef database update --environment Production
```

#### 3. Deploy ke Server

Upload folder `publish` ke server dan konfigurasi web server Anda.

### Environment Variables

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection="Server=prod-server;Database=db_simsapi;User=prod_user;Password=prod_password;"
```

---

## 🤝 Contributing

Kontribusi sangat diterima! Silakan ikuti langkah berikut:

1. Fork repository ini
2. Buat branch baru (`git checkout -b feature/AmazingFeature`)
3. Commit perubahan Anda (`git commit -m 'Add some AmazingFeature'`)
4. Push ke branch (`git push origin feature/AmazingFeature`)
5. Buat Pull Request

### Code Style Guide

- Gunakan PascalCase untuk class dan method names
- Gunakan camelCase untuk variable names
- Tambahkan XML comments untuk public methods
- Ikuti [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

## 📝 Changelog

### Version 1.0.0 (2026-01-12)

- ✅ Initial release
- ✅ CRUD Sapi
- ✅ CRUD Produksi Susu
- ✅ CRUD Produksi Olahan
- ✅ CRUD Kesehatan Sapi
- ✅ Jadwal Kegiatan & Absensi
- ✅ User Management
- ✅ Dashboard Interaktif
- ✅ PDF Report Generation
- ✅ Auto Audit Trail
- ✅ Role-based Access Control

---

## 👥 Authors

**Your Name**

- GitHub: [@fachriramdhan](https://github.com/fachriramdhan)
- LinkedIn: [fachriramdhan](https://linkedin.com/in/fachriramdhan)

---

## 🙏 Acknowledgments

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [TailwindCSS](https://tailwindcss.com)
- [QuestPDF](https://www.questpdf.com)
- [Chart.js](https://www.chartjs.org)
- [Font Awesome](https://fontawesome.com)
- [SweetAlert2](https://sweetalert2.github.io)

---

<div align="center">

**Dibuat dengan ❤️ untuk meningkatkan efisiensi peternakan sapi perah di Indonesia**

[⬆ Kembali ke atas](#-sim-sapi---sistem-informasi-manajemen-sapi-perah)

</div>
