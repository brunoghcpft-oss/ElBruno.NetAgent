# ElBruno.NetAgent – Security and Safety

## 1. Security posture

ElBruno.NetAgent is local-first.

The app should not send telemetry, logs, adapter names, IP data, or network data to any cloud service.

## 2. Data handled

The app may read:

- Network adapter names
- Adapter descriptions
- Interface operational status
- Gateway information
- Ping results
- Latency
- Packet loss
- Local config file

The app should not collect:

- Browsing history
- DNS query history
- Packet contents
- Credentials
- Wi-Fi passwords
- VPN credentials

## 3. Logs

Logs must avoid sensitive data.

Allowed:

- Adapter display name
- Adapter kind
- Quality score
- Latency summary
- Decision reason
- Command result codes

Avoid:

- Full IP configuration unless diagnostic mode is enabled
- Secrets
- Tokens
- Wi-Fi passwords

## 4. Admin operations

Monitoring should not require admin privileges.

Some switching operations may require admin privileges.

Rules:

- Detect admin status.
- Explain when elevation is required.
- Prefer dry-run if not elevated.
- Never repeatedly prompt for elevation.
- Never disable all adapters.
- Always provide restore option.

## 5. Safe defaults

Default config:

```json
{
  "AutoModeEnabled": false,
  "DryRunMode": true
}
```

## 6. Destructive actions

Do not implement in Phase 1:

- Disable adapters
- Delete network profiles
- Change DNS
- Reset TCP/IP
- Flush routing tables globally
- Modify VPN settings

## 7. Reporting issues

When publishing the repository, include a GitHub issue template for bugs and diagnostics.

Users should include:

- Windows version
- App version
- Adapter types
- Whether USB tethering is used
- Redacted logs
