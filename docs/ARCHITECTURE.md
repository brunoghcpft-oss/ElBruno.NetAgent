# ElBruno.NetAgent – Architecture Decisions

## 1. Decision log summary

| ID | Decision | Status |
| --- | --- | --- |
| ADR-001 | Use .NET 10 | Accepted |
| ADR-002 | Use WPF for Windows UI | Accepted |
| ADR-003 | Use WinForms NotifyIcon interop for tray | Accepted |
| ADR-004 | Use generic host + DI inside WPF | Accepted |
| ADR-005 | Use latency-first decision engine | Accepted |
| ADR-006 | Start with safe route/interface metric control | Accepted |
| ADR-007 | Keep CLI as future separate project | Accepted |
| ADR-008 | Use MIT License | Accepted |
| ADR-009 | Use OIDC Trusted Publishing for NuGet | Accepted |
| ADR-010 | Generate promotional assets locally using t2i | Accepted |

## 2. Runtime and platform

Use:

- .NET 10
- Windows 11
- x64
- WPF

Do not use:

- WinUI
- MAUI
- Electron
- Avalonia for Phase 1
- Background Windows Service for Phase 1

Reasoning:

- WPF is mature and reliable for Windows desktop utilities.
- WPF works well with system tray patterns through WinForms interop.
- The app should match the style of other ElBruno Windows tray tools.

## 3. Solution structure

Recommended structure:

```text
ElBruno.NetAgent.sln
README.md
LICENSE
docs/
src/
  ElBruno.NetAgent/
    ElBruno.NetAgent.csproj
    App.xaml
    App.xaml.cs
    MainWindow.xaml
    MainWindow.xaml.cs
    appsettings.json
    Core/
      Interfaces/
      Models/
      Enums/
    Services/
      Network/
      Monitoring/
      Decision/
      Control/
      Configuration/
      Logging/
    UI/
      Tray/
      ViewModels/
      Views/
    Infrastructure/
      Windows/
      Diagnostics/
tests/
  ElBruno.NetAgent.Tests/
assets/
  images/
prompts/
.github/
  workflows/
```

## 4. Application startup

Use `Microsoft.Extensions.Hosting` inside WPF.

Expected startup flow:

1. `App.xaml.cs` creates Host.
2. Host registers services.
3. Host starts background services.
4. Tray service initializes NotifyIcon.
5. Main window remains hidden by default.
6. App shuts down cleanly from tray menu.

## 5. Dependency injection

Use:

- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Logging`
- `Microsoft.Extensions.Options`

Do not manually instantiate core services inside UI classes.

## 6. Main components

### 6.1 Network inventory service

Interface:

```csharp
public interface INetworkInventoryService
{
    Task<IReadOnlyList<NetworkInterfaceInfo>> GetInterfacesAsync(CancellationToken cancellationToken);
}
```

Responsibilities:

- Enumerate network interfaces.
- Normalize adapter data.
- Classify adapter type.
- Exclude irrelevant adapters where appropriate.

### 6.2 Network quality service

Interface:

```csharp
public interface INetworkQualityService
{
    Task<NetworkQualitySnapshot> MeasureAsync(NetworkInterfaceInfo networkInterface, CancellationToken cancellationToken);
}
```

Responsibilities:

- Ping configured endpoints.
- Calculate latency.
- Calculate packet loss.
- Return a quality score.

### 6.3 Decision engine

Interface:

```csharp
public interface INetworkDecisionEngine
{
    NetworkDecision Evaluate(NetworkDecisionContext context);
}
```

Responsibilities:

- Determine if current connection is acceptable.
- Determine if another interface is better.
- Respect cooldowns.
- Avoid flapping.
- Produce an explainable decision.

The decision engine must be pure logic and testable without Windows APIs.

### 6.4 Network controller

Interface:

```csharp
public interface INetworkController
{
    Task<NetworkSwitchResult> PreferInterfaceAsync(string interfaceId, CancellationToken cancellationToken);
    Task<NetworkSwitchResult> RestoreAutomaticMetricsAsync(CancellationToken cancellationToken);
}
```

Responsibilities:

- Apply network preference changes.
- Abstract Windows implementation details.
- Return success/failure with diagnostics.

### 6.5 Tray service

Responsibilities:

- Own the `NotifyIcon`.
- Display status.
- Provide context menu.
- Trigger manual actions.
- Exit app safely.

### 6.6 Configuration service

Responsibilities:

- Load configuration from file.
- Create default config if missing.
- Validate config.
- Support reload in future phase.

## 7. Network switching strategy

### Phase 1 safe approach

Prefer changing route/interface metrics.

Guidance:

- Do not disable adapters by default.
- Do not remove saved Wi-Fi profiles.
- Do not change DNS by default.
- Do not touch VPN adapters unless explicitly configured.
- Always log previous metric values if changed.

### Phase 2 advanced approach

Optional future capabilities:

- Enable/disable adapters.
- `netsh` fallback.
- WMI fallback.
- PowerShell fallback.
- Admin elevation flow.

## 8. Adapter classification

Classification should include:

```csharp
public enum NetworkAdapterKind
{
    Unknown,
    WiFi,
    Ethernet,
    UsbTethering,
    Virtual,
    Loopback,
    Bluetooth,
    Cellular,
    Vpn
}
```

USB tethering detection is heuristic-based. It may appear as Ethernet. Do not rely on a single string match.

Suggested heuristics:

- Interface type is Ethernet.
- Description contains words like:
  - `Remote NDIS`
  - `RNDIS`
  - `USB`
  - `Android`
  - `Pixel`
  - `Mobile`
- Gateway exists.
- Interface is up.

## 9. Quality scoring

Quality score should be deterministic.

Suggested formula for Phase 1:

```text
score = 100
score -= latencyPenalty
score -= packetLossPenalty
score -= instabilityPenalty
score += preferenceBonus
```

Latency penalty:

| Latency | Penalty |
| --- | ---: |
| <= 80ms | 0 |
| <= 150ms | 10 |
| <= 250ms | 25 |
| <= 500ms | 50 |
| > 500ms | 75 |

Packet loss penalty:

| Loss | Penalty |
| --- | ---: |
| 0% | 0 |
| <= 5% | 10 |
| <= 10% | 25 |
| <= 20% | 50 |
| > 20% | 80 |

## 10. Anti-flapping rules

The app must avoid switching repeatedly.

Rules:

- Require at least `MinimumChecksBeforeSwitch`.
- Require a better interface score difference of at least `MinimumScoreDeltaToSwitch`.
- Require cooldown after switching.
- Do not switch while a switch is already in progress.
- Do not switch if target interface has incomplete data.
- Do not switch if current interface is still healthy.

## 11. UI architecture

Use lightweight MVVM.

Phase 1 UI:

- No visible main window by default.
- Tray icon.
- Context menu.
- Optional simple status window.

Context menu items:

```text
ElBruno.NetAgent
Status: Good / Degraded / Poor
Current: Wi-Fi / Ethernet / USB Tethering
Mode: Manual / Auto
---
Open Status
Switch to...
  Wi-Fi Adapter Name
  USB Tethering Adapter Name
Auto Mode: On/Off
Refresh Now
Restore Automatic Metrics
Open Logs
Open Config
Exit
```

## 12. Logging

Use `Microsoft.Extensions.Logging`.

Minimum log events:

- App start
- App exit
- Config loaded
- Config validation warning
- Interfaces detected
- Quality measurement result
- Decision evaluation summary
- Manual switch requested
- Auto switch requested
- Switch succeeded
- Switch failed
- Restore succeeded
- Restore failed

File location:

```text
%LOCALAPPDATA%\ElBruno.NetAgent\logs\netagent-yyyyMMdd.log
```

## 13. Configuration

Default config location:

```text
%LOCALAPPDATA%\ElBruno.NetAgent\config.json
```

Template file in repo:

```text
docs/config.sample.json
```

## 14. Security and safety

- No secrets.
- No cloud telemetry.
- No admin requirement for read-only monitoring.
- Admin may be required for some network control operations.
- Ask for elevation only when needed.
- Log commands before executing them, but never log sensitive tokens.
- Avoid destructive actions by default.

## 15. Testing strategy

Use unit tests for:

- Decision engine
- Quality scoring
- Config validation
- Adapter classification heuristics
- Anti-flapping logic

Use manual testing for:

- Tray behavior
- Windows interface metric changes
- USB tethering detection
- App startup/shutdown

## 16. Package strategy

Phase 1 package options:

- Windows executable release artifact
- NuGet package for launcher/global tool if compatible with WPF constraints
- Future separate CLI package

Do not force WPF app into a global tool if it makes installation worse.
