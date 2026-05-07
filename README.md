# 🚚 TransportMongoDB

<div align="center">

![TransportMongoDB Banner]()

**Modern lojistik takip ve yönetim platformu**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-Atlas-47A248?style=for-the-badge&logo=mongodb)](https://www.mongodb.com/atlas)
[![Azure](https://img.shields.io/badge/Azure-Web%20App-0078D4?style=for-the-badge&logo=microsoftazure)](https://azure.microsoft.com/)
[![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-CI%2FCD-2088FF?style=for-the-badge&logo=githubactions)](https://github.com/features/actions)
[![xUnit](https://img.shields.io/badge/xUnit-Test-512BD4?style=for-the-badge&logo=dotnet)](https://xunit.net/)
[![AdminLTE](https://img.shields.io/badge/AdminLTE-Theme-00ACD7?style=for-the-badge&logo=bootstrap)](https://adminlte.io/)
[![Live Demo](https://img.shields.io/badge/🌐%20Canlı%20Demo-Azure-0078D4?style=for-the-badge)](https://transport-logistic-fgagegajfea5a9f6.canadacentral-01.azurewebsites.net/)

</div>

---

## 📌 Proje Hakkında

**TransportMongoDB**, ASP.NET Core MVC ve **MongoDB Atlas** altyapısı üzerine inşa edilmiş modern bir lojistik takip ve yönetim platformudur. Proje, [Murat Yücedağ](https://github.com/murataydg) hocamın eğitimi kapsamında **Claude Code** kullanılarak geliştirilmiş olup **AdminLTE admin teması**, backend mimarisi ve UI bileşenlerini kapsamaktadır.

Projeye kişisel katkı olarak **Azure üzerinde deploy**, **GitHub Actions CI/CD pipeline** ve **xUnit & Mock ile test altyapısı** eklenmiştir.

---

## 🖼️ Ekran Görüntüleri

<details>
<summary>📸 <b>Proje Ekran Görüntüleri</b> &nbsp;|&nbsp; Görmek için buraya tıklayın 👆</summary>

### 🏠 Anasayfa
![]()

### 📦 Kargo Takip
![]()

### 🛠️ Admin Paneli
![]()

### 📊 Dashboard
![]()

### 🔄 CI/CD Pipeline
![]()

</details>

---

## ✨ Özellikler

### 🌐 Kullanıcı Tarafı
- 📦 Kargo takip numarasıyla gerçek zamanlı gönderi sorgulama
- 🗺️ Şehir ve ilçe bazlı konum takibi
- 📋 Kronolojik kargo hareket geçmişi
- 📱 Responsive ve modern arayüz

### 🛠️ Admin Paneli
- 📊 Özet dashboard (toplam gönderi, teslim edilen, dağıtımdaki)
- ➕ Yeni gönderi oluşturma
- ✏️ Gönderi güncelleme ve silme
- 🔍 Tracking event yönetimi

### ⚙️ Teknik Altyapı
- 🏗️ Generic Repository Pattern
- 🔄 AutoMapper ile DTO mapping
- 💉 Dependency Injection
- ☁️ MongoDB Atlas bulut veritabanı
- 🚀 Azure Web App deployment
- 🔁 GitHub Actions CI/CD pipeline
- 🧪 xUnit & Moq ile unit ve integration testler

---

## 🏗️ Mimari

```
DatabaseMastery.TransportMongoDb
├── Controllers
│   ├── HomeController
│   ├── AdminController
│   └── TrackingController
├── Services
│   └── ShipmentService (IShipmentService)
├── Repositories
│   └── GenericRepository (IGenericRepository<T>)
├── Entities
│   ├── Shipment
│   └── ShipmentTracking
├── Dtos
│   └── ShipmentDtos
├── Models (ViewModels)
│   ├── TrackingResultViewModel
│   └── TrackingEventViewModel
├── Mapping
│   └── MappingProfile (AutoMapper)
└── Settings
    └── DatabaseSettings

TransportMongoDB.Tests
├── UnitTests
│   └── ShipmentServiceTests
└── IntegrationTests
    └── ShipmentIntegrationTests
```

---

## 🚀 CI/CD Pipeline

GitHub Actions ile otomatik build, test ve deploy süreci:

```
Push / PR → GitHub Actions Tetiklenir
         ↓
    Set up job
         ↓
    Setup .NET 10
         ↓
    Create .env file
         ↓
    Restore
         ↓
    Build
         ↓
    Unit Tests (xUnit + Moq)
         ↓
    Integration Tests (xUnit + Moq)
         ↓
    Testler Başarılı mı?
     ✅ Evet → Publish → Deploy to Azure
     ❌ Hayır → Pipeline Durur
```

![CI/CD Pipeline](docs/screenshots/Pipeline_CI_CD.png)

`.github/workflows/` dizininde YAML pipeline tanımı bulunmaktadır.

---

## ☁️ Deployment

Proje **Azure Web App Service** üzerinde host edilmektedir. MongoDB bağlantısı **MongoDB Atlas** üzerinden sağlanmaktadır.

| Servis | Platform |
|--------|----------|
| Uygulama | Azure Web App |
| Veritabanı | MongoDB Atlas |
| CI/CD | GitHub Actions |

---

## 🧪 Testler

Proje **xUnit** ve **Moq** kullanılarak test edilmiştir.

```bash
dotnet test
```

- ✅ Unit Testler — Service ve Repository katmanları
- ✅ Integration Testler — End-to-end akış testleri

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|-----------|----------|
| ASP.NET Core MVC | Web framework |
| MongoDB Driver | NoSQL veritabanı bağlantısı |
| AdminLTE | Admin panel teması |
| MongoDB Atlas | Bulut veritabanı |
| AutoMapper | DTO / Entity mapping |
| xUnit & Moq | Test framework |
| GitHub Actions | CI/CD pipeline |
| Azure Web App | Cloud hosting |

---

## ⚡ Kurulum

```bash
# Repoyu klonla
git clone https://github.com/

# Projeye git
cd TransportMongoDB

# appsettings.json içinde MongoDB bağlantı bilgilerini ayarla
# DatabaseSettings → ConnectionString, DatabaseName, ShipmentCollectionName

# Projeyi çalıştır
dotnet run
```

---

## 👨‍💻 Geliştirici

<div align="center">

| | |
|---|---|
| 👨‍🏫 **Proje Eğitmeni** | [Murat Yücedağ](https://github.com/murataydg) |
| 👨‍💻 **Geliştirici** | **Halit Berk İskitoğlu** |
| ☁️ **Kişisel Katkı** | Azure Deployment, CI/CD Pipeline, Unit & Integration Testler |

</div>

---

<div align="center">

⭐ Projeyi beğendiyseniz yıldız vermeyi unutmayın!

</div>
