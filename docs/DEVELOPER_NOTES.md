# ElBruno.NetAgent – Developer Notes

## Why not just use Wi-Fi priority?

Windows Wi-Fi priority only helps between Wi-Fi profiles. This app targets multiple interface types such as Wi-Fi and USB tethering.

## Why not disable the bad adapter?

Disabling adapters is risky. It can break remote sessions, VPNs, or leave the user offline. Phase 1 should use safer preference mechanisms.

## Why WPF?

WPF is reliable for Windows desktop utilities and fits the existing ElBruno tray-tool style.

## Why a decision engine?

The switching decision should be testable and explainable. It should not be mixed with UI code or Windows command execution.

## Why dry-run first?

Network automation can easily surprise users. Dry-run lets the app collect data and explain what it would do.

## Future CLI

The future CLI should be a separate project:

```text
src/ElBruno.NetAgent.CLI/
```

Possible commands:

```text
netagent status
netagent interfaces
netagent switch --interface "<name>"
netagent auto on
netagent auto off
netagent config open
netagent logs open
```
