# TestCase-TetaCode

Bir not yönetim sistemi. Backend .NET 10 ile, frontend React + TypeScript + Vite ile geliştirilmiştir.

## 📋 İçindekiler

- [Gereksinimler](#gereksinimler)
- [Kurulum](#kurulum)
- [Projeyi Çalıştırma](#projeyi-çalıştırma)
- [Proje Yapısı](#proje-yapısı)
- [API Endpoints](#api-endpoints)

## 🔧 Gereksinimler

Projeyi çalıştırmak için aşağıdaki yazılımların sisteminizde kurulu olması gerekmektedir:

### Backend Gereksinimleri
- **.NET 10 SDK** - [İndir](https://dotnet.microsoft.com/download/dotnet/10.0)
- **SQL Server 2019+** veya **SQL Server Express** - [İndir](https://www.microsoft.com/sql-server/sql-server-downloads)

### Frontend Gereksinimleri
- **Node.js 18+** - [İndir](https://nodejs.org/)
- **npm** (Node.js ile birlikte gelir)

## 📦 Kurulum

### 1. Projeyi Klonlayın

```bash
git clone https://github.com/MEnesAydin/TestCase-TetaCode.git
cd TestCase-TetaCode
```

### 2. Backend Kurulumu

#### a. Bağımlılıkları Yükleyin

```bash
cd backend
dotnet restore
```

#### b. Veritabanı Bağlantı Ayarları

`backend/src/TestCase.WebAPI/appsettings.json` dosyasını oluşturun veya düzenleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TestCaseDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "TestCase.WebAPI",
    "Audience": "TestCase.Clients",
    "SecretKey": "SuperSecretKeySuperSecretKeySuperSecretKeySuperSecretKeySuperSecretKeySuperSecretKey"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### c. Entity Framework Core Tools Kurulumu

```bash
dotnet tool install --global dotnet-ef
```

Eğer zaten kuruluysa, güncelleyin:

```bash
dotnet tool update --global dotnet-ef
```

#### d. Veritabanı ve Tabloları Oluşturun (Migration)


```bash
cd src/TestCase.Infrastructure
dotnet ef database update --startup-project ../TestCase.WebAPI
```

**Bu komut:**
- Eğer veritabanı yoksa, otomatik olarak `TestCaseDb` veritabanını oluşturacaktır
- Tüm tabloları (Users, Grades) ve ilişkileri oluşturacaktır
- Migration geçmişini veritabanına kaydedecektir
### 3. Frontend Kurulumu

```bash
cd ../../frontend
npm install
```

## 🚀 Projeyi Çalıştırma

### Backend'i Çalıştırma

Backend klasöründen:

```bash
cd backend/src/TestCase.WebAPI
dotnet run
```

veya watch mode ile (kod değişikliklerinde otomatik yeniden başlatma):

```bash
dotnet watch run
```

Backend varsayılan olarak şu adreslerde çalışacaktır:
- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001
- **API Dokümantasyonu (Scalar):** http://localhost:5000/scalar/v1

### Frontend'i Çalıştırma

Yeni bir terminal penceresi açın ve frontend klasöründen:

```bash
cd frontend
npm run dev
```

Frontend varsayılan olarak şu adreste çalışacaktır:
- **URL:** http://localhost:5173

### Tüm Projeyi Çalıştırma (İki Terminal)

**Terminal 1 - Backend:**
```bash
cd backend/src/TestCase.WebAPI
dotnet watch run
```

**Terminal 2 - Frontend:**
```bash
cd frontend
npm run dev
```

## 📁 Proje Yapısı

```
TestCase-TetaCode/
├── backend/
│   ├── src/
│   │   ├── TestCase.Application/    # Uygulama katmanı (CQRS, MediatR)
│   │   │   ├── Auth/                # Authentication komutları
│   │   │   ├── Grades/              # Not yönetimi komutları
│   │   │   ├── Users/               # Kullanıcı komutları
│   │   │   └── Behaviors/           # MediatR pipeline behaviors
│   │   ├── TestCase.Domain/         # Domain katmanı (Entities, Interfaces)
│   │   │   ├── Grades/              # Not entity ve repository
│   │   │   ├── Users/               # Kullanıcı entity ve repository
│   │   │   └── Features/            # Generic repository pattern
│   │   ├── TestCase.Infrastructure/ # Altyapı katmanı (EF Core, Repositories)
│   │   │   ├── Context/             # DbContext
│   │   │   ├── Configurations/      # Entity configurations
│   │   │   ├── Repositories/        # Repository implementasyonları
│   │   │   └── Services/            # JWT, Claims servisleri
│   │   └── TestCase.WebAPI/         # API katmanı (Minimal API)
│   │       ├── Modules/             # API endpoint modülleri
│   │       └── wwwroot/             # Statik dosyalar (yüklenen resimler)
│   └── TestCase-TetaCode.slnx       # Solution dosyası
├── frontend/
│   ├── src/
│   │   ├── components/              # React bileşenleri
│   │   │   ├── GradeCard.tsx        # Not kartı
│   │   │   ├── GradeModal.tsx       # Not ekleme/düzenleme modal
│   │   │   ├── Layout.tsx           # Ana layout
│   │   │   └── ProtectedRoute.tsx   # Route koruma
│   │   ├── context/                 # Context API
│   │   │   └── AuthContext.tsx      # Authentication context
│   │   ├── pages/                   # Sayfa bileşenleri
│   │   │   ├── Dashboard.tsx        # Ana sayfa
│   │   │   ├── Login.tsx            # Giriş sayfası
│   │   │   └── Register.tsx         # Kayıt sayfası
│   │   ├── services/                # API servisleri
│   │   │   └── api.ts               # Axios instance ve API çağrıları
│   │   └── types/                   # TypeScript tipleri
│   │       └── index.ts             # Type tanımlamaları
│   └── package.json
└── README.md
```

## 🔑 API Endpoints

### Authentication
- `POST /api/auth/login` - Kullanıcı girişi
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```
- `POST /api/auth/register` - Yeni kullanıcı kaydı
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```

### Grades (Notlar)
- `GET /api/grades` - Tüm notları listele (sayfalama destekli)
- `POST /api/grades` - Yeni not oluştur (multipart/form-data)
- `PUT /api/grades/{id}` - Not güncelle (multipart/form-data)
- `DELETE /api/grades/{id}` - Not sil

### Query Parameters (Grades)
- `pageNumber` - Sayfa numarası (varsayılan: 1)
- `pageSize` - Sayfa başına kayıt (varsayılan: 10)
- `searchTerm` - Arama terimi (ders adı veya açıklama)
- `sortBy` - Sıralama alanı
- `sortDescending` - Azalan sıralama (true/false)



## 📝 Teknolojiler ve Özellikler

### Backend
- **.NET 10** - En son .NET framework
- **Clean Architecture** - Katmanlı mimari
- **CQRS Pattern** - Command Query Responsibility Segregation
- **MediatR** - Mediator pattern implementasyonu
- **Entity Framework Core 10** - ORM
- **SQL Server** - Veritabanı
- **JWT Authentication** - Token tabanlı kimlik doğrulama
- **FluentValidation** - Validasyon
- **Minimal API** - Endpoint tanımlamaları
- **Scalar** - API dokümantasyonu

### Frontend
- **React 18** - UI framework
- **TypeScript** - Type safety
- **Vite** - Build tool
- **React Router** - Routing
- **Axios** - HTTP client
- **Tailwind CSS** - Styling
- **Lucide React** - Icons
- **Context API** - State management

## 🐛 Sorun Giderme

