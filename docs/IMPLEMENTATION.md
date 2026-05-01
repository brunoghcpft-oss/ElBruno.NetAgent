# ElBruno.NetAgent – Implementation Plan for Copilot and Local Models

## 1. Purpose

This document is the primary execution plan for GitHub Copilot, SQUAD, and local coding models.

The implementation must be incremental. Do not build everything in a single pass.

## 2. Global implementation rules

1. Use .NET 10.
2. Use WPF.
3. Use latest stable package versions available at implementation time.
4. Keep code simple.
5. Keep classes small.
6. Add interfaces for core services.
7. Avoid large dependencies.
8. Use async APIs where appropriate.
9. Do not block UI thread.
10. Do not implement future phases early.
11. Do not silently ignore errors.
12. Log important decisions.
13. Commit after each phase.
14. Build and test after each phase.

## 3. Phase 0 – Repository scaffold

### Objective

Create the repository structure and empty solution.

### Tasks

- Create solution `ElBruno.NetAgent.sln`.
- Create WPF project:
  - `src/ElBruno.NetAgent/ElBruno.NetAgent.csproj`
- Create test project:
  - `tests/ElBruno.NetAgent.Tests/ElBruno.NetAgent.Tests.csproj`
- Add README.
- Add MIT License.
- Add `.gitignore`.
- Add `docs/` folder.
- Add `assets/images/` folder.
- Add `.github/workflows/ci.yml`.
- Add `.github/workflows/publish.yml`.

### Acceptance criteria

- `dotnet build` succeeds.
- `dotnet test` succeeds.
- App starts and exits.

### Commit message

```text
chore: scaffold ElBruno.NetAgent solution
```

## 4. Phase 1 – WPF host and tray shell

### Objective

Create a tray-first WPF application.

### Tasks

- Configure WPF app startup.
- Integrate `Microsoft.Extensions.Hosting`.
- Register DI services.
- Add `TrayIconService`.
- Use `System.Windows.Forms.NotifyIcon`.
- App should start minimized to tray.
- Add tray menu:
  - Open Status
  - Refresh Now
  - Auto Mode
  - Open Logs
  - Open Config
  - Exit
- Add placeholder icons.
- Add clean shutdown.

### Acceptance criteria

- App appears in tray.
- App does not show main window by default.
- Exit works from tray.
- Logs app startup and shutdown.

### Commit message

```text
feat: add WPF tray shell
```

## 5. Phase 2 – Configuration system

### Objective

Add strongly typed configuration.

### Tasks

- Create `NetAgentOptions`.
- Load config from:
  - `%LOCALAPPDATA%\ElBruno.NetAgent\config.json`
- Create default config if missing.
- Add validation.
- Add sample config in `docs/config.sample.json`.
- Add menu item to open config file.
- Add menu item to open app data folder.

### Acceptance criteria

- Missing config is created.
- Invalid config logs warning.
- Default values are used safely.
- Config path is documented.

### Commit message

```text
feat: add configuration system
```

## 6. Phase 3 – Network inventory

### Objective

Detect and classify network interfaces.

### Tasks

- Add models:
  - `NetworkInterfaceInfo`
  - `NetworkAdapterKind`
  - `NetworkOperationalState`
- Add service:
  - `INetworkInventoryService`
  - `NetworkInventoryService`
- Use `NetworkInterface.GetAllNetworkInterfaces()`.
- Detect:
  - Wi-Fi
  - Ethernet
  - USB tethering heuristics
  - Virtual/VPN
  - Loopback
- Add tests for classification logic.
- Show detected interfaces in logs.
- Show detected interfaces in tray submenu.

### Acceptance criteria

- Active interfaces are listed.
- Wi-Fi is identified.
- USB tethering appears as candidate if connected.
- Virtual adapters are not preferred by default.

### Commit message

```text
feat: detect network interfaces
```

## 7. Phase 4 – Network quality monitor

### Objective

Measure connection quality.

### Tasks

- Add models:
  - `NetworkQualitySnapshot`
  - `EndpointPingResult`
  - `NetworkQualityLevel`
- Add service:
  - `INetworkQualityService`
  - `PingNetworkQualityService`
- Ping configured endpoints.
- Calculate:
  - average latency
  - min latency
  - max latency
  - packet loss
  - quality level
  - quality score
- Add rolling history per interface.
- Log summary every cycle.
- Avoid noisy logs by using debug-level details.

### Acceptance criteria

- App measures latency.
- App handles ping failures.
- App does not crash offline.
- Tray tooltip shows current quality.

### Commit message

```text
feat: monitor network quality
```

## 8. Phase 5 – Decision engine

### Objective

Create deterministic network decision logic.

### Tasks

- Add models:
  - `NetworkDecisionContext`
  - `NetworkDecision`
  - `NetworkDecisionReason`
- Add service:
  - `INetworkDecisionEngine`
  - `NetworkDecisionEngine`
- Implement:
  - healthy current network = stay
  - unhealthy current network + better candidate = switch
  - cooldown = stay
  - insufficient samples = wait
  - score delta too small = stay
- Add unit tests for:
  - no switch when current healthy
  - switch when current poor and candidate good
  - no switch during cooldown
  - no switch with insufficient samples
  - no switch when delta too small

### Acceptance criteria

- Decision engine is pure and testable.
- All decision paths return human-readable reason.
- Tests pass.

### Commit message

```text
feat: add network decision engine
```

## 9. Phase 6 – Network controller

### Objective

Allow Windows to prefer a selected interface.

### Tasks

- Add service:
  - `INetworkController`
  - `WindowsNetworkController`
- Start with safe metric-based approach.
- Implement:
  - Get current metrics
  - Prefer selected interface
  - Restore automatic metrics
- Add dry-run mode.
- Add admin detection.
- Add failure diagnostics.
- Log all attempted changes.
- Do not disable adapters in Phase 6.

### Acceptance criteria

- Manual switch attempts to prefer selected interface.
- Restore option is available.
- Errors are visible in logs and tray status.
- No adapter is disabled.

### Commit message

```text
feat: add Windows network controller
```

## 10. Phase 7 – Manual switching UI

### Objective

Expose manual network switching from tray.

### Tasks

- Update tray menu with interface list.
- Add click handler per candidate interface.
- Show result notification or status update.
- Log manual action.
- Refresh quality after manual switch.

### Acceptance criteria

- User can manually select target interface.
- App reports success/failure.
- App remains running after failure.

### Commit message

```text
feat: add manual network switching from tray
```

## 11. Phase 8 – Auto mode

### Objective

Enable automatic switching.

### Tasks

- Add background monitor loop using hosted service.
- Respect config:
  - `AutoModeEnabled`
  - thresholds
  - cooldowns
- Evaluate decision engine on interval.
- Call network controller only when decision says switch.
- Update tray status.
- Add toggle in tray menu.
- Persist auto mode setting.

### Acceptance criteria

- Auto mode can be enabled/disabled.
- Auto mode switches only when rules are met.
- Cooldown prevents flapping.
- All switches are logged.

### Commit message

```text
feat: add automatic network failover
```

## 12. Phase 9 – Diagnostics and status window

### Objective

Provide a small status UI.

### Tasks

- Add simple WPF status window.
- Show:
  - current interface
  - detected interfaces
  - latency
  - quality score
  - auto mode
  - last decision
  - last switch
- Add buttons:
  - Refresh
  - Open Logs
  - Open Config
  - Restore Metrics
- Keep UI simple.

### Acceptance criteria

- Status window opens from tray.
- Status is readable.
- UI does not block monitoring.

### Commit message

```text
feat: add status window
```

## 13. Phase 10 – Packaging

### Objective

Prepare release-ready package artifacts.

### Tasks

- Add version metadata to `.csproj`.
- Add icon.
- Add publish profile or documented command.
- Add GitHub Actions CI.
- Add GitHub Actions publish workflow.
- Add docs/PUBLISHING.md.
- Add release artifact upload.

### Acceptance criteria

- CI builds on push.
- Release workflow packs/publishes.
- Artifact is downloadable from GitHub Actions.

### Commit message

```text
chore: add packaging and release workflow
```

## 14. Phase 11 – Promotional materials

### Objective

Generate launch materials.

### Tasks

- Add Twitter/X post.
- Add LinkedIn post.
- Add blog post.
- Add image prompts.
- Add t2i usage commands.
- Generate images into `assets/images/`.
- Include NuGet logo image.

### Acceptance criteria

- `docs/PROMOTION.md` is complete.
- `docs/IMAGE_PROMPTS.md` is complete.
- Assets exist or commands are documented.

### Commit message

```text
docs: add promotional materials
```

## 15. Stop conditions

Stop and ask for human review when:

- A dependency requires admin install.
- Network switching requires disabling adapters.
- Tests cannot validate decision logic.
- NuGet packaging conflicts with WPF app constraints.
- A Windows command would make destructive changes.

## 16. Recommended SQUAD execution pattern

Use one agent for each area:

- Repo/setup agent
- WPF/tray agent
- Network APIs agent
- Decision engine agent
- Testing agent
- Docs/promotion agent

Each agent should produce small PRs or commits.
