# ElBruno.NetAgent – Troubleshooting

## App does not appear in tray

Check:

- App is still running in Task Manager.
- Windows tray overflow area.
- Logs in `%LOCALAPPDATA%\ElBruno.NetAgent\logs`.

## USB tethering does not appear

Check:

- Phone is connected by USB-C.
- USB tethering is enabled on the phone.
- Windows shows a new Ethernet-like adapter.
- Cable supports data, not only charging.

## App detects too many adapters

This is common on developer machines.

Examples:

- Hyper-V
- Docker
- WSL
- VPN
- VirtualBox
- VMware

Use config:

```json
"IgnoredAdapterNameContains": [
  "Loopback",
  "VirtualBox",
  "Hyper-V",
  "VMware"
]
```

## Auto mode does not switch

Check:

- `AutoModeEnabled` is `true`.
- `DryRunMode` is `false`.
- App has necessary permissions.
- Candidate interface has better score.
- Cooldown is not active.

## Switch fails

Possible causes:

- App is not running as admin.
- Windows policy blocks metric changes.
- Adapter is virtual or managed by VPN software.
- Interface name changed.

## Restore network metrics

Use tray menu:

```text
Restore Automatic Metrics
```

If needed, restart Windows networking or reboot.
