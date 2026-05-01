# ElBruno.NetAgent – Image Prompts

## 1. Tool

Use local CLI tool:

```text
t2i
```

Expected tool:

```text
ElBruno.Text2Image
```

If `t2i` is not available, do not block implementation. Save the prompts and continue.

## 2. Output folder

Save generated images in:

```text
assets/images/
```

## 3. NuGet logo prompt

Filename:

```text
assets/images/elbruno-netagent-logo.png
```

Prompt:

```text
Square app icon for a Windows developer tool named ElBruno.NetAgent. A modern minimal network agent symbol with connected nodes, Wi-Fi arcs, USB tethering hint, and a small protective shield. Clean flat vector style, high contrast, blue and purple gradient, subtle glow, no text, centered composition, professional NuGet package icon, 1024x1024.
```

Suggested command:

```powershell
t2i --prompt "Square app icon for a Windows developer tool named ElBruno.NetAgent. A modern minimal network agent symbol with connected nodes, Wi-Fi arcs, USB tethering hint, and a small protective shield. Clean flat vector style, high contrast, blue and purple gradient, subtle glow, no text, centered composition, professional NuGet package icon, 1024x1024." --output assets/images/elbruno-netagent-logo.png --size 1024x1024
```

## 4. Twitter/X promo image prompt

Filename:

```text
assets/images/elbruno-netagent-twitter.png
```

Prompt:

```text
A clean futuristic illustration of a laptop automatically switching between hotel Wi-Fi and phone USB tethering. Show two network paths, one unstable red/yellow Wi-Fi signal and one stable blue USB-C phone connection. Developer productivity vibe, minimal UI overlays, Windows tray icon concept, modern tech style, no logos, no readable text, 16:9.
```

Suggested command:

```powershell
t2i --prompt "A clean futuristic illustration of a laptop automatically switching between hotel Wi-Fi and phone USB tethering. Show two network paths, one unstable red/yellow Wi-Fi signal and one stable blue USB-C phone connection. Developer productivity vibe, minimal UI overlays, Windows tray icon concept, modern tech style, no logos, no readable text, 16:9." --output assets/images/elbruno-netagent-twitter.png --size 1792x1024
```

## 5. LinkedIn promo image prompt

Filename:

```text
assets/images/elbruno-netagent-linkedin.png
```

Prompt:

```text
Professional developer desk scene with a Windows laptop, smartphone connected with USB-C, and abstract network quality indicators floating above. The visual should communicate reliable connectivity, smart failover, and developer tooling. Clean modern design, soft lighting, blue accent color, no readable text, 16:9.
```

Suggested command:

```powershell
t2i --prompt "Professional developer desk scene with a Windows laptop, smartphone connected with USB-C, and abstract network quality indicators floating above. The visual should communicate reliable connectivity, smart failover, and developer tooling. Clean modern design, soft lighting, blue accent color, no readable text, 16:9." --output assets/images/elbruno-netagent-linkedin.png --size 1792x1024
```

## 6. Blog header prompt

Filename:

```text
assets/images/elbruno-netagent-blog-header.png
```

Prompt:

```text
Wide blog header image showing a developer traveling with a laptop in a hotel room, phone connected by USB-C, and an elegant visualization of network failover from weak hotel Wi-Fi to stable mobile tethering. Modern technical illustration, clean composition, subtle humor, no readable text, 21:9.
```

Suggested command:

```powershell
t2i --prompt "Wide blog header image showing a developer traveling with a laptop in a hotel room, phone connected by USB-C, and an elegant visualization of network failover from weak hotel Wi-Fi to stable mobile tethering. Modern technical illustration, clean composition, subtle humor, no readable text, 21:9." --output assets/images/elbruno-netagent-blog-header.png --size 1792x768
```

## 7. Alt text

### Logo

Minimal app icon showing connected network nodes, Wi-Fi arcs, and a shield representing smart and safe network switching.

### Twitter/X image

Illustration of a laptop switching between unstable hotel Wi-Fi and a stable phone tethering connection.

### LinkedIn image

Developer desk scene with a laptop and phone connected by USB-C, surrounded by abstract network quality indicators.

### Blog header

Developer traveling with laptop and phone tethering, with visual network failover from weak Wi-Fi to stable mobile connection.
