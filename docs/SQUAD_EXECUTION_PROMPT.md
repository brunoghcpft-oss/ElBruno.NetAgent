# SQUAD Execution Prompt – ElBruno.NetAgent

Use this prompt when starting the repository with SQUAD or GitHub Copilot.

```text
You are implementing ElBruno.NetAgent.

Read these files first:
- docs/PRD.md
- docs/ARCHITECTURE.md
- docs/IMPLEMENTATION.md
- docs/REPO_RULES.md
- docs/CONFIGURATION.md

Important rules:
- Use .NET 10.
- Use WPF.
- Use latest stable library versions available.
- Implement only one phase at a time.
- Do not skip phases.
- Do not implement speculative features.
- Do not disable network adapters by default.
- Start with DryRunMode enabled.
- Keep AutoModeEnabled disabled by default.
- Add tests for pure logic.
- Run dotnet build and dotnet test after each phase.
- Commit after each phase.

Start with Phase 0 from docs/IMPLEMENTATION.md.

After Phase 0:
- summarize files created
- summarize commands run
- summarize any issues
- stop for review
```

## Follow-up prompt for each phase

```text
Continue with the next phase from docs/IMPLEMENTATION.md.

Before coding:
1. Restate the phase objective.
2. List the files you expect to change.
3. Implement the smallest working version.
4. Run build/tests.
5. Update docs if needed.
6. Stop after the phase is complete.
```

## Prompt for local model constraints

```text
Use simple code.
Avoid large context changes.
Prefer small files.
Do not rewrite unrelated files.
Do not introduce unnecessary dependencies.
If uncertain about Windows networking commands, add an abstraction and implement dry-run behavior first.
```
