# Design Patterns/Architectures Eğitimi

Design pattern, yazılımda sık karşılaşılan problemlere karşı defalarca kanıtlanmış, tekrar kullanılabilir çözüm şablonlarıdır.

Kısaca, aynı problemi her seferinde sıfırdan düşünmemek için kullanılan hazır mimari çözüm fikirleridir.

## Patterns
- **Design Principle**	“Nasıl düşünmeliyim?”
- **Design Pattern**	“Bu problemi nasıl çözerim?”
- **Architectural Pattern**	“Uygulamanın genel iskeletini ve katmanlı yapısını tanımlayan büyük ölçekli tasarım şablonudur”

---

## Eğitim İçeriği
- [ ] AspNetCore Framework'ünü anlayalım
- [ ] Dependency Injection
- [ ] Middleware
- [ ] Design Patterns nedir?
  - [ ] **Singleton Pattern** (1994 - Book)
  - [ ] **Factory Pattern** (1994 - Book)
  - [ ] **Abstract Factory** Pattern (1994 - Book)
  - [ ] **Builder Pattern** (1994 - Book)
  - [ ] **Prototype Pattern** (199(1994 - Book)4 - Book)
  - [ ] **Facade Pattern** (1994 - Book)
  - [ ] **Proxy Pattern** (1994 - Book)
  - [ ] **Service Pattern** (Modern)
  - [ ] **Repository Pattern** (Modern)
  - [ ] **Unit Of Work Pattern** (Modern)
  - [ ] **Command Pattern**  (1994 - Book)
  - [ ] **Mediator Pattern** (1994 - Book)
  - [ ] **CQRS Pattern** (Modern)
  - [ ] **Options Pattern** (Modern)
  - [ ] **Result Pattern** (Modern)
  - [ ] **Service Discovery Pattern** (Modern)
  - [ ] **Outbox Pattern** (Modern)
  - [ ] **Observer Pattern - Queue - Channels Library** (Modern)
  - [ ] **Rate Limiting Pattern** (Modern)
  - [ ] **Circuit Breaker Pattern / Retry Pattern** (Polly Library) (Modern)
- [ ] Architectural Patterns nedir?
  - [ ] N Tier Architecture
  - [ ] Clean Architecture
    - [ ] DDD Approach

---

## Framework Nedir?
Framework, uygulamanın iskeletini ve akışını belirleyen,senin yazdığın kodu kendi kuralları içinde çağıran hazır bir yapıdır.

## Library Nedir?
Library, ihtiyacın olduğunda senin çağırdığın, belirli bir işi yapan hazır kod kütüphanesidir.

- .NET bir framework / platformdur. C# ise bu platform üzerinde kullanılan programlama dilidir.
- ASP.NET Core = .NET için web uygulama framework’ü
- Console ise bir application, .NET’in sağladığı bir application modelidir

## IoC (Inversion of Control) Nedir?
Inversion of Control, programın kontrol akışının senin kodundan çıkıp bir framework / container tarafından yönetilmesi prensibidir.
Yani:
- “Ben kimi, ne zaman, nasıl çağıracağımı kontrol etmiyorum. Framework kontrol ediyor.”
- IoC bir prensiptir. ASP.NET Core bunu uygular. Program.cs ise bunun konfigürasyon yeridir.

## Design Principles, Design Patterns, Architecture Pattern

- **Design Principles**: "Nasıl düşünmeliyim?"
- **Design Pattern**: “Bu problemi nasıl çözerim?”
- **Architectural Pattern**: “Uygulamanın genel iskeletini ve katmanlı yapısını tanımlayan büyük ölçekli tasarım şablonudur”

### Design Principles
- **SOLID**
- **DRY**
- **KISS**
- **YAGNI**
- **Separation of Concerns** 
  - "Her şey kendi işini yapsın" 
  - "Modern mimari dünyası “CQRS düşünce şeklini” öneriyor"
- **High Cohesion / Low Coupling** 
  - High Cohesion = Bir modülün / class’ın tek bir amaca odaklı olması 
  - Low Coupling = Modüllerin birbirine en az bağımlı olması

---

### Consul Docker komutu (Service Discovery)
```powershell
docker run -d --name consul -p 8500:8500 hashicorp/consul:latest
```

### Polly kütüphanesi BackoffType
```csharp
//🧩 DelayBackoffType Enum Türleri
//Constant	Her denemede sabit süre bekler.	Delay = 5s → 5s, 5s, 5s
//Linear	Her denemede gecikme lineer (doğrusal) artar.	Delay = 5s → 5s, 10s, 15s
//Exponential	Her denemede gecikme katlanarak (üstel) artar.	Delay = 5s → 5s, 10s, 20s, 40s
```

### HasiCorp Vault 
- Development Docker
```powershell
docker run -d --name vault -p 8200:8200 --cap-add=IPC_LOCK -e VAULT_DEV_ROOT_TOKEN_ID=root -e VAULT_ADDR=http://0.0.0.0:8200 hashicorp/vault:latest server -dev
```

- NuGet Package
```dash
VaultSharp
```

- C# kodları
```csharp
public class VaultService
{
    public async Task<Secret<SecretData>> GetSecrets()
    {
        var vaultToken = "root";
        var vaultUri = "http://127.0.0.1:8200";
        var vaultTokenInfo = new TokenAuthMethodInfo(vaultToken);
        var vaultClientSettings = new VaultClientSettings(vaultUri, vaultTokenInfo);
        var vaultClient = new VaultClient(vaultClientSettings);

        var secrets = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: "productapp/config",
            mountPoint: "secret");

        return secrets;
    }
}
```

- vault.hcl
```hcl
ui = true

# Bu iki satır eklendi 👇
api_addr    = "http://127.0.0.1:8200"
cluster_addr = "http://127.0.0.1:8201"

storage "raft" {
  path    = "/vault/data"
  node_id = "vault-1"
}

listener "tcp" {
  address          = "0.0.0.0:8200"
  tls_disable      = 1           # test için; prod'da kaldır
  cluster_address  = "0.0.0.0:8201"  # opsiyonel ama eklemek iyi
}

disable_mlock = true
```

- Production Docker (Bu kod vault.hcl in bulunduğu klasörde çalıştırılmalı)
```powershell
docker run -d --name vault -p 8200:8200 --cap-add=IPC_LOCK -v "${PWD}\vault-data:/vault/data" -v "${PWD}\vault.hcl:/vault/config/vault.hcl" hashicorp/vault server -config=vault.hcl
```