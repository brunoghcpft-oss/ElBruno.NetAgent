# ElBruno.NetAgent – Product Requirements Document

## 1. Product summary

ElBruno.NetAgent is a Windows-first network quality agent implemented as a .NET 10 WPF system tray application.

The application monitors available network interfaces, evaluates connection quality, and helps Windows prefer the best available connection based on explicit rules.

The first real-world use case is:

> A user is connected to hotel Wi-Fi on a Surface Laptop, also connected to a Google Pixel 8 through USB-C tethering. The user wants the machine to automatically or semi-automatically prefer the most stable connection without manually switching networks every time the hotel Wi-Fi becomes slow.

## 2. Product name

- Repository: `ElBruno.NetAgent`
- App name: `ElBruno.NetAgent`
- NuGet package: `ElBruno.NetAgent`
- Future CLI package: `ElBruno.NetAgent.CLI`
- Future CLI command: `netagent`

## 3. Positioning

ElBruno.NetAgent is not just a Wi-Fi switcher. It is a small local connectivity agent.

It should feel like:

- `ElBruno.ClockTray`: small, practical, Windows utility
- `ElBruno.AspireMonitor`: developer-focused tray utility
- `ElBruno.OllamaMonitor`: tray-first app with clear local runtime status

## 4. Target users

Primary users:

- Developers working while traveling
- Speakers doing live demos from hotels or conferences
- Remote workers on unreliable Wi-Fi
- Power users using phone tethering as a backup connection

Secondary users:

- Developers learning WPF, .NET 10, DI, hosted services, and Windows networking APIs
- People who want a simple practical sample of a system tray app in .NET

## 5. Goals

The product must:

1. Run quietly from the Windows system tray.
2. Detect active network interfaces.
3. Identify likely connection types:
   - Wi-Fi
   - Ethernet
   - USB tethering
   - VPN / virtual adapters
4. Measure network quality using latency checks.
5. Display current network status in the tray.
6. Allow manual switching from the tray menu.
7. Support automatic decision-making based on configurable rules.
8. Avoid risky actions by default.
9. Log all relevant decisions and actions.
10. Be easy to implement incrementally with GitHub Copilot and local models.

## 6. Non-goals

Phase 1 explicitly does not include:

- VPN management
- Firewall management
- Full traffic routing engine
- Deep packet inspection
- Bandwidth stress testing
- Cloud telemetry
- Paid services
- Kernel drivers
- Network packet capture
- Proxy/VPN tunneling
- Mobile app companion
- macOS or Linux support

## 7. User stories

### 7.1 System tray status

As a user, I want to see the current network quality from the tray icon so that I can quickly know whether my connection is healthy.

Acceptance criteria:

- Tray icon exists.
- Tooltip shows current active interface and quality summary.
- Context menu shows current interface, latency, and mode.
- The icon state changes based on quality.

### 7.2 Detect interfaces

As a user, I want the app to detect my Wi-Fi and USB tethering connection automatically so I do not have to configure interface names manually.

Acceptance criteria:

- Active network interfaces are listed.
- Wi-Fi adapters are detected.
- Ethernet adapters are detected.
- USB tethering adapters are grouped as Ethernet-like adapters.
- Virtual/VPN adapters are identified and not preferred by default unless explicitly configured.

### 7.3 Manual switch

As a user, I want to manually switch to another available interface from the tray menu.

Acceptance criteria:

- Tray menu lists candidate interfaces.
- Selecting an interface attempts to make it preferred.
- The app logs the action.
- The UI reports success or failure.

### 7.4 Automatic mode

As a user, I want the app to switch automatically when the current connection becomes unstable.

Acceptance criteria:

- Auto mode can be enabled/disabled.
- Auto mode uses configured thresholds.
- Auto mode has cooldowns to avoid switching back and forth.
- Auto mode never switches if there is no clearly better interface.
- Auto mode logs the reason for each switch.

### 7.5 Safe mode

As a user, I want a safe default behavior that does not break my connection.

Acceptance criteria:

- The first release should prefer changing route/interface metrics rather than disabling adapters.
- Destructive operations must require explicit configuration.
- The app should never disable all active network interfaces.
- The app should provide a panic/restore option.

## 8. MVP scope

MVP should include:

- .NET 10 WPF app
- System tray icon
- Context menu
- Interface detection
- Latency monitoring
- Rolling average latency
- Simple decision engine
- Manual switch
- Config file
- File logging
- Basic packaging
- README
- MIT License

## 9. Post-MVP scope

Post-MVP may include:

- CLI wrapper
- Floating status panel
- Better adapter classification
- Historical latency charts
- Profiles for travel/home/conference
- Export diagnostics bundle
- AI-generated recommendations
- Windows startup registration
- NuGet global tool launcher or installer package
- Better UX for tethering detection
- Toast notifications

## 10. Quality thresholds

Default values:

```json
{
  "LatencyThresholdMs": 180,
  "PacketLossThresholdPercent": 20,
  "CheckIntervalSeconds": 5,
  "FailoverDurationSeconds": 20,
  "FailbackCooldownSeconds": 120,
  "MinimumChecksBeforeSwitch": 3
}
```

Quality levels:

| Level | Latency | Packet loss | State |
| --- | ---: | ---: | --- |
| Excellent | <= 80ms | 0% | Green |
| Good | <= 150ms | <= 5% | Green |
| Degraded | <= 250ms | <= 10% | Yellow |
| Poor | > 250ms | > 10% | Red |
| Offline | No success | 100% | Black/Gray |

## 11. Success criteria

The project is successful when:

- The app starts minimized to tray.
- The app can detect Wi-Fi and USB tethering.
- The app can show current quality.
- The app can manually prefer an interface.
- The app can automatically prefer a better interface when configured.
- The solution builds from a clean clone.
- The implementation is documented and easy to continue with Copilot.

## 12. Important constraints for AI implementation

The implementation must be optimized for execution by GitHub Copilot and local models.

Rules:

- Implement one phase at a time.
- Keep files small and focused.
- Prefer explicit interfaces and simple classes.
- Avoid clever reflection or hidden magic.
- Document every public service.
- Add TODO comments only when linked to an issue or a phase.
- Do not introduce large dependencies without approval.
- Do not use preview packages unless explicitly required.
