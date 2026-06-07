# CLAUDE.md — EnergyHistory Projektdokumentation

Diese Datei gibt Claude einen vollständigen Überblick über das Projekt, sodass der Code
effizient und ohne wiederholtes Einlesen aller Quelldateien verstanden werden kann.
Bitte diese Datei beim Start jeder Konversation als erstes lesen.

---

## 1. Projektziel und Kontext

EnergyHistory ist eine private Anwendung zur Erfassung von Zählerständen eines Stromzählers
(Verbrauch und/oder Einspeisung) sowie einer Kleinsolaranlage. Der Benutzer trägt manuell
Ablesungen und Monatswerte ein. Die Anwendung berechnet daraus Differenzwerte, Tagesdurchschnitte
und Ertragswerte (kWh × Preis).

Alle Daten werden als JSON-Dateien abgelegt — entweder lokal auf dem Gerät oder über den
WebApp-Server, der als zentraler Speicher im Heimnetzwerk fungiert.

---

## 2. Solution-Struktur

```
CS4N.EnergyHistory/                   ← Solution-Wurzel
│
├── Components/
│   ├── Contracts/                    ← Gemeinsame Schnittstellen & Modelle (net10.0)
│   ├── Core/                         ← Hilfsdienste: PathHelper, EnvironmentHelper, FileLogger
│   ├── DataStore.LiteDB/             ← JSON-Dateispeicher (Name ist irreführend: kein LiteDB!)
│   └── DataImport.FritzBox/          ← Datenimport aus FritzBox-Router (experimentell)
│
├── WebApp/                           ← ASP.NET-Backend + SAP UI5-Frontend (net10.0)
│
└── CS4N.EnergyHistory.MobileApp/     ← .NET MAUI-App (net10.0-android/ios/maccatalyst/windows)
```

---

## 3. Komponenten im Detail

### 3.1 Contracts (gemeinsame Modelle)

Der `Contracts`-Namespace ist das Herzstück: hier liegen alle Datenmodelle und Schnittstellen,
die von WebApp und MobileApp geteilt werden.

**`IDataStore`** — die zentrale Datenzugriffs-Schnittstelle mit synchronen Methoden für CRUD
auf Solar-Definitionen, Solar-Daten, Stromzähler-Definitionen und Stromzähler-Daten.

**Modell-Hierarchie:**

```
DataObjectBase { Guid }
  ├── ElectricMeter.Data.DataObject          ← Alle Ablesungen eines Zählers
  │     Records: List<DataRecord>
  │     LastRecord, LastRecordDate, LastRecordValue (berechnete Properties)
  │     AverageAmountPerDay, AverageValuePerDay (berechnet aus Records[1..])
  │
  └── SolarStation.Data.DataSummary          ← Alle Ertragsdaten einer Station
        Years: List<DataOfYear>
        GeneratedElectricityAmount (Summe aller Jahre)
        GeneratedElectricityValue (Summe aller Jahre, decimal)
        FedInElectricityValue (Summe aller Jahre, decimal)
```

**`DataRecord`** (eine Stromzähler-Ablesung):

```csharp
int Id
string ReadingDate          // Format: "yyyy-MM-dd"
double ReadingValue         // Zählerstand in kWh
double DifferenceValue      // Differenz zur Vorablesung
int DifferenceDays          // Tage seit Vorablesung
decimal KilowattHourPrice
string CurrencyUnit
// Berechnete Properties:
double DifferenceAmountPerDay  => DifferenceValue / DifferenceDays
decimal DifferenceValuePerDay  => DifferenceAmountPerDay * KilowattHourPrice
```

**`DataOfYear`** (Solar, ein Jahr):

```csharp
int Number                          // Jahreszahl
bool AutomaticSummation = true      // Wenn true: Summe aus Monaten
double GeneratedElectricityAmount   // Manuell oder aus Monaten summiert
bool GeneratedElectricityEnabled
decimal GeneratedElectricityKilowattHourPrice
// GeneratedElectricityValue: wenn AutomaticSummation → Summe der Monate; sonst Menge × Preis
// FedInElectricityValue: Summe über Monate von (Menge × monatlichem FedIn-Preis)
List<DataOfMonth> Months            // immer 12 Einträge
string Comments
```

**`DataOfMonth`** (Solar, ein Monat):

```csharp
int Number                          // Monatszahl 1–12
bool AutomaticSummation = false     // Tages-Summierung (aktuell nicht genutzt)
double GeneratedElectricityAmount
bool GeneratedElectricityEnabled
decimal GeneratedElectricityKilowattHourPrice
decimal GeneratedElectricityValue   => Amount × Price
bool FedInEnabled
decimal FedInElectricityKilowattHourPrice
decimal FedInElectricityValue       => Amount × FedInPrice
string Comments
```

**`ElectricMeter.Definition`**:

```csharp
string Guid, Name, IconUrl, Number, UnitCode ("2.8.0"), CapacityUnit ("kWh")
string InstalledAt              // Format "yyyy-MM-dd"
bool IsConsumptionMeter         // true = Verbrauch, false = Einspeisung
decimal KilowattHourPrice
string CurrencyUnit ("€")
DateTime CreatedAt, UpdatedAt?
```

**`SolarStation.Definition`**:

```csharp
string Guid, Name, IconUrl
double PowerPeak
string PowerUnit ("W"), CapacityUnit ("kWh"), CurrencyUnit ("€")
string InstalledAt, CommonComments
decimal PurchaseCosts
bool GeneratedElectricityEnabled
decimal GeneratedElectricityKilowattHourPrice
bool FedInEnabled
decimal FedInElectricityKilowattHourPrice
string FinanceComments
ApiDefinition ApiDefinition         // FritzBox-Import-Konfiguration
DateTime CreatedAt, UpdatedAt?
```

**`Constants`** (wichtige Strings):

```csharp
PowerUnit_Watt = "W"
CapacityUnit_KilowattHour = "kWh"
SolarStationDefinitionDefaultIconUrl = "sap-icon://photo-voltaic"
ElectricMeterDefinitionDefaultIconUrl = "sap-icon://time-account"
```

---

### 3.2 DataStore.LiteDB (FileStore)

Trotz des Namens verwendet diese Komponente **kein LiteDB**, sondern speichert Daten als
einfache JSON-Dateien im Dateisystem. Der `FileStore` implementiert `IDataStore` als
`partial class` über drei Dateien:

- `FileStore.cs` — Basismethoden: `WriteDefinitionsFile<T>`, `LoadDefinitionsFile<T>`,
  `WriteDataFile<T>`, `LoadDataFile<T>`, `DeleteDataFile<T>`
- `FileStore.ElectricMeter.cs` — CRUD für Stromzähler
- `FileStore.SolarStation.cs` — CRUD für Solar-Stationen

**Dateistruktur auf Disk:**

```
{WorkPath}/FileStore/
  ElectricMeters.json            ← Liste aller ElectricMeter.Definition
  SolarStations.json             ← Liste aller SolarStation.Definition
  {guid}.json                    ← Datenobjekt je Zähler/Station (DataObject / DataSummary)
```

`PathHelper.GetWorkPath()` liefert den Basispfad — im Debug-Betrieb das Projektverzeichnis,
in Production das App-Verzeichnis. Der `FileStore` cached Definitionen und Daten in statischen
Listen; nach jedem Schreibvorgang wird der Cache geleert.

---

### 3.3 WebApp (ASP.NET + SAP UI5)

Das Backend ist ein eigenständiger Kestrel-Webserver, der gleichzeitig die statischen UI5-Dateien
und eine REST-API ausliefert. Port-Standard: **5678** (konfigurierbar in `appsettings.json`
unter `ServerSettings:PortNumber`).

**ServiceApp** konfiguriert:
- `IDataStore` als Singleton → `FileStore`
- `ILogger` als Singleton → `FileLogger`
- Kestrel lauscht auf allen IPs (`ListenAnyIP`)
- Static Files aus `wwwroot/` (bzw. `PathHelper.GetRootPath()`)

**REST-API-Endpunkte** (Präfix: `/api/`):

| Controller | Route | Methoden |
|---|---|---|
| `CockpitController` | `api/cockpit` | GET → Liste aller Tiles |
| `SolarStationDefinitionController` | `api/SolarStationDefinition` | GET (alle), GET/{guid}, POST, PATCH, DELETE |
| `SolarStationDataController` | `api/SolarStationData` | GET/init, POST/{guid} (mit Filter) |
| `ElectricMeterDefinitionController` | `api/ElectricMeterDefinition` | GET (alle), GET/{guid}, GET/new, POST, PATCH, DELETE |
| `ElectricMeterDataController` | `api/ElectricMeterData/{guid}` | GET, POST (neue Ablesung), POST/.../compare, DELETE/.../record |

**Frontend: SAP UI5 SPA**

Die UI5-Runtime wird zur Laufzeit von CDN geladen (im ZIP-Archiv entfernt, daher nicht im
Repo vorhanden). Die App-Konfiguration liegt in `wwwroot/manifest.json`.

Routing-Schema (UI5-Router):
```
Cockpit (/)  →  SolarStationDefinitionOverview  →  SolarStationDefinition/:guid
             →  SolarStationData/:guid  →  SolarStationDataEdit/:guid  →  SolarStationDataEditYear/:guid
             →  ElectricMeterDefinitionOverview  →  ElectricMeterDefinition/:guid
             →  ElectricMeterData/:guid
```

**Controller-Muster (JavaScript):**
Alle Controller erben von `BaseController`, der `navigateTo()`, `handleApiError()`,
`showResponseError()`, `isNullOrEmpty()` und `setFocus()` bereitstellt. Der `ApiConnector`
kapselt alle fetch()-Aufrufe gegen `window.location.href + "api/" + urlPath`.

Das Modell jedes Controllers wird mit `resetModel()` beim Route-Match initialisiert und
via `sap.ui.model.json.JSONModel` an die View gebunden. Datenbindung erfolgt per
`{/pfad/zum/wert}` in den XML-Views.

---

### 3.4 MobileApp (.NET MAUI)

**Zielplattformen:** Android, iOS, macOS Catalyst, Windows  
**Framework:** net10.0  
**Wichtige NuGet-Pakete:**
- `CommunityToolkit.Maui 14.2.0` — Expander, Popup, diverse Helpers
- `Syncfusion.Maui.Toolkit 1.0.10` — Charts (noch nicht aktiv in Verwendung)
- `Microsoft.Maui.Controls 10.0.70`

**Projektreferenzen:** `Contracts` und `DataStore.LiteDB` (beide direkt referenziert)

#### Architektur: MVVM mit abstraktem Speicherdienst

Die MobileApp wurde im Juni 2026 von einer einfachen Code-Behind-Implementierung auf
vollständiges MVVM mit abstrahiertem Storage-Layer umgebaut. Die folgende Struktur wurde
hinzugefügt (Dateien liegen im ZIP `MobileApp-MAUI.zip`; noch einzupflegen):

```
MobileApp/
├── Services/
│   ├── IStorageService.cs          ← Async-Schnittstelle (entspricht IDataStore)
│   ├── LocalStorageService.cs      ← Wrapper um FileStore (async Task.FromResult)
│   ├── RemoteStorageService.cs     ← HTTP-Client gegen WebApp-REST-API
│   └── StorageServiceFactory.cs    ← Liest Preferences → gibt richtige Impl. zurück
│
├── ViewModels/
│   ├── BaseViewModel.cs            ← INotifyPropertyChanged + RunWithBusyAsync
│   ├── MainViewModel.cs            ← Cockpit: TileGroup / TileModel, LoadAsync()
│   ├── ElectricMeterViewModel.cs   ← Ablesungen: CRUD, Validierung, Diff-Berechnung
│   ├── SolarStationViewModel.cs    ← Solar: Zusammenfassung + Jahres-/Monatserfassung
│   └── SettingsViewModel.cs        ← Speicherort-Konfiguration via Preferences
│
├── Views/
│   ├── ElectricMeterDetailPage.xaml(.cs)   ← Tab: Erfassung + Historie (SwipeView zum Löschen)
│   ├── SolarStationDetailPage.xaml(.cs)    ← Tab: Zusammenfassung + Jahreserfassung (Expander)
│   └── SettingsPage.xaml(.cs)              ← RadioButton: Lokal vs. Remote + URL-Eingabe
│
├── Converters/
│   └── Converters.cs               ← InvertBoolConverter, StringNotEmptyConverter, NotNullConverter
│
├── App.xaml                        ← Globale Ressourcen inkl. Converter-Instanzen
├── AppShell.xaml(.cs)              ← Routen: SolarStation.Detail, ElectricMeter.Detail, Settings
├── MainPage.xaml(.cs)              ← Cockpit: gruppiete CollectionView, OnAppearing lädt neu
└── MauiProgram.cs                  ← UseMauiCommunityToolkit()
```

**Navigations-Schema (MAUI Shell):**
```
MainPage  →  SolarStation.Detail?guid={guid}
          →  ElectricMeter.Detail?guid={guid}
          →  Settings
```
Parameter-Übergabe per `[QueryProperty(nameof(Guid), "guid")]` in den Page-Klassen.

**Speicherdienst-Logik:**
`StorageServiceFactory.Create()` liest `Preferences.Get("storage_mode")`:
- `"local"` (Default) → `LocalStorageService` → `FileStore` → lokale JSON-Dateien
- `"remote"` → `RemoteStorageService(url)` → HTTP gegen `{url}/api/`

Die Factory wird bei **jedem** `LoadAsync()`-Aufruf neu aufgerufen, damit
Einstellungsänderungen sofort wirksam werden ohne App-Neustart.

**MVVM-Muster:**
`BaseViewModel.RunWithBusyAsync(Func<Task>)` setzt `IsBusy = true`, führt die Aktion aus
und setzt `IsBusy = false` im finally-Block — entspricht dem busy/unbusy-Muster der
UI5-Controller. `SetProperty<T>` mit `[CallerMemberName]` vermeidet Magic Strings.

**Compiled Bindings:** Alle XAML-Views verwenden `x:DataType="vm:XyzViewModel"` für
Compile-Zeit-Überprüfung der Bindings (Performance-Gewinn gegenüber dynamischem Binding).

---

## 4. Datenfluss

### Lokaler Modus (Standard)

```
MobileApp  →  LocalStorageService  →  FileStore  →  JSON-Dateien im App-Datenverzeichnis
```

Das Datenverzeichnis wird von `PathHelper.GetWorkPath()` bestimmt. Auf Android ist das
das interne App-Verzeichnis (nicht auf SD-Karte sichtbar), auf Windows der AppData-Pfad.

### Remote-Modus (WebApp-Server im Heimnetzwerk)

```
MobileApp  →  RemoteStorageService  →  HTTP/REST  →  WebApp  →  FileStore  →  JSON-Dateien
```

Die WebApp muss laufen und im selben Netzwerk erreichbar sein. Die URL wird in den
App-Einstellungen gespeichert (z.B. `http://192.168.1.10:5678`). API-Präfix: `/api/`.

---

## 5. Wichtige Konventionen

**Datum-Format:** Immer `"yyyy-MM-dd"` als String in den Modellen. Anzeige per
`DateTime.Parse(d).ToString("dd.MM.yyyy")` bzw. `.ToString("dddd, dd. MMMM yyyy")`.

**Geld-Werte:** `decimal` für Preise und Erträge, `double` für physikalische Mengen (kWh).

**GUIDs:** `System.Guid.NewGuid().ToString()` — werden beim ersten `Upsert` vergeben wenn
`Guid` leer ist. In Dateinamen direkt als `{guid}.json` verwendet.

**Icons in der WebApp:** SAP UI5 Icon-URLs (z.B. `sap-icon://photo-voltaic`) — in der
MobileApp nicht verwendbar, dort wird ein Emoji oder ein Bild-Fallback genutzt.

**Caching im FileStore:** Statische Listen (`cachedSolarStationDefinitions` etc.) werden
nach jedem Schreibvorgang mit `.Clear()` geleert. Thread-Safety ist nicht implementiert
(Single-User-App, kein Problem in der Praxis).

**Namespacing:** `CS4N.EnergyHistory.*` durchgehend. `CS4N` = CSharp4Net (GitHub-Handle
des Autors).

---

## 6. Bekannte Lücken / TODO

Diese Punkte sind im Code angelegt aber noch nicht vollständig implementiert:

- **`DataOfMonth.Days`** — Tages-granulare Erfassung innerhalb eines Monats ist auskommentiert
  (komplette Logik in `DataOfMonth`-Konstruktor als Kommentar vorhanden).
- **`DataOfYear.AutomaticSummation`** — die Logik für manuelle Jahreseingabe (ohne Monats-
  Summierung) ist im Modell vorhanden, aber in der UI noch nicht vollständig exponiert.
- **`DataImport.FritzBox`** — liest Verbrauchsdaten aus der FritzBox-API. Implementiert,
  aber noch nicht in die UI integriert.
- **`SolarStation.ApiDefinition`** — Struktur für externe API-Anbindung existiert, ist
  aber noch nicht genutzt.
- **`Location`, `Inverter`, `Module`** — Modelle existieren, sind in `Definition` auskommentiert.
- **Syncfusion Charts in MobileApp** — `Syncfusion.Maui.Toolkit` ist referenziert und
  `LineChartDrawable.cs` (custom IDrawable) existiert, aber noch nicht in Views eingebaut.
- **Definition-CRUD in MobileApp** — Stammdaten (Solar-Station anlegen/bearbeiten,
  Stromzähler anlegen/bearbeiten) sind in der MobileApp noch nicht implementiert.
  Stammdaten werden aktuell über die WebApp gepflegt.
- **`ElectricMeterData/compare`** — Vergleich zweier ausgewählter Ablesungen (WebApp-Feature,
  noch nicht in MobileApp).

---

## 7. Entwicklungsumgebung

- **IDE:** Visual Studio 2022 (`.vs/`-Verzeichnis vorhanden)
- **Target Framework:** net10.0 (alle Projekte)
- **Solution-Datei:** `CS4N.EnergyHistory/CS4N.EnergyHistory.sln` (nicht im ZIP enthalten,
  muss im Repository vorhanden sein)
- **Build:** Standard MSBuild; MAUI benötigt Android SDK / Xcode für mobile Targets

---

## 8. Wie Claude dieses Projekt am effizientesten liest

Beim nächsten Einlesen genügt es, folgende Dateien zu lesen (in dieser Reihenfolge):

1. `CLAUDE.md` — diese Datei (liefert 90% des Kontexts)
2. Nur bei spezifischen Änderungsaufgaben: die betroffene Quelldatei direkt lesen
3. Bei neuen Features: `IDataStore.cs` für den Datenvertrages und das relevante Modell

Nicht nötig beim Einlesen (sofern kein Bug in diesen Bereichen):
- `FileStore.*.cs` (Verhalten vollständig in Abschnitt 3.2 beschrieben)
- `ServiceApp.cs` / `Program.cs` (Setup vollständig in Abschnitt 3.3 beschrieben)
- `BaseController.js` / `ApiConnector.js` (Verhalten in Abschnitt 3.3 beschrieben)
- Alle `.vs/`-, `obj/`- und `bin/`-Verzeichnisse
