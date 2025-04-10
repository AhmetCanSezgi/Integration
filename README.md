<h1 align="center">📦 Dehasoft Entegrasyon Uygulaması</h1>

<p align="center">
  REST API + WinForms + Dapper + AutoMapper ile <br/>
  sipariş yönetimi ve çift taraflı stok/fiyat senkronizasyonu 🚀
</p>

---

## ✨ Genel Bakış

Bu uygulama, Dehasoft servisinden gelen siparişleri çekip veritabanına kaydeden ve ürünlerin stok ile fiyat bilgilerinin hem lokalde hem de servis tarafında güncel kalmasını sağlayan bir **çift taraflı entegrasyon çözümüdür**.

## ⚙️ Kullanılan Teknolojiler

- 🖥 **C# WinForms** (.NET 8)
- 💾 **SQL Server**
- 🔄 **Dapper ORM**
- 🧭 **AutoMapper**
- 📡 **RESTful API (JSON)**
- 💡 **Dependency Injection**
- 📋 **Newtonsoft.Json**

---

## 🛠 Özellikler

| İşlem                         | Açıklama                                                                 |
|------------------------------|--------------------------------------------------------------------------|
| 🛍 Siparişleri Getir         | API'den paginasyonlu sipariş çekimi                                     |
| 🗃 Sipariş Kaydet            | Siparişi ve ürünlerini veritabanına kaydeder                            |
| 🧮 Stok Azaltma              | Sipariş adedine göre stok düşürülür                                     |
| 💸 Fiyat/Stok Güncelleme     | Güncellenen bilgiler API'ye geri gönderilir                             |
| 🔁 IsSynced Kontrolü         | Ürün güncellemeleri için senkronizasyon kontrolü yapılır                |
| 🧠 ProductHistory Kaydı       | Ürün stok veya fiyat değişimlerinde geçmiş kayıt alınır (trigger ile)   |
| 🧾 Loglama                   | Tüm işlemler `Logs` tablosunda kaydedilir                               |

---

## 🧩 Proje Yapısı

Dehasoft/ ├── WinForms/ → Kullanıcı Arayüzü 
├── Business/ → Servisler, DTO’lar, AutoMapper 
├── DataAccess/ → Veritabanı işlemleri (Dapper) │ └── Models/ │ └── Repositories/ 
├── appsettings.json → Bağlantı & API ayarları └── README.md


## 🖼 Ekran Görüntüsü
![Ekran görüntüsü 2025-04-10 080212](https://github.com/user-attachments/assets/1adc16db-4833-47cc-b8ab-2d719f8c14aa)


📥 Kurulum ve Kullanım
🔗 1. Projeyi Klonlayın

git clone https://github.com/AhmetCanSezgi/DehaSoft.git


⚙️ 2. appsettings.json İçeriği

![Ekran görüntüsü 2025-04-10 081508](https://github.com/user-attachments/assets/923fb68e-24dc-4ad8-b97d-dba3eda1f29b)


📦 3. Gerekli NuGet Paketleri
Dapper

AutoMapper.Extensions.Microsoft.DependencyInjection

Microsoft.Extensions.DependencyInjection

Newtonsoft.Json


🧱 4. SQL Tabloları

```sql
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL UNIQUE,
    Barcode VARCHAR(100) NOT NULL UNIQUE,
    StockCode VARCHAR(100) NOT NULL UNIQUE,
    Name VARCHAR(255) NOT NULL,
    IsSynced BIT NOT NULL DEFAULT(1),
    Price DECIMAL(18,2) NOT NULL,
    Stock DECIMAL(18,2) NOT NULL
);

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EntryId INT NOT NULL UNIQUE,
    Oid VARCHAR(100) NOT NULL,
    UserId INT NOT NULL,
    Total DECIMAL(18,2) NOT NULL,
    OrderDate DATETIME NOT NULL
);

CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    SalePrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Logs (
    Id INT IDENTITY PRIMARY KEY,
    LogTime DATETIME,
    Type VARCHAR(10),
    Message NVARCHAR(MAX)
);

CREATE TABLE ProductHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    OldPrice DECIMAL(18, 2),
    NewPrice DECIMAL(18, 2),
    OldStock DECIMAL(18, 2),
    NewStock DECIMAL(18, 2),
    ChangedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE
);
```
Aşağıdaki SQL Trigger’ı oluştur (stok/fiyat değişimi izlenir):

```sql

CREATE TRIGGER trg_ProductChangeLog
ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ProductHistory (ProductId, OldPrice, NewPrice, OldStock, NewStock)
    SELECT
        i.Id,
        d.Price, i.Price,
        d.Stock, i.Stock
    FROM
        inserted i
        INNER JOIN deleted d ON i.Id = d.Id
    WHERE
        i.Price != d.Price OR i.Stock != d.Stock;

 
    UPDATE p
    SET p.IsSynced = 0
    FROM Products p
    INNER JOIN inserted i ON p.Id = i.Id
    INNER JOIN deleted d ON p.Id = d.Id
    WHERE
        i.Price != d.Price OR i.Stock != d.Stock;
END

```
📇 İletişim
LinkedIn - Ahmet Can Sezgi

GitHub - AhmetCanSezgi
