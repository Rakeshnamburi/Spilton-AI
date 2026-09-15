# Controlled coding workspace

Coding responses remain working, including multi-turn generation, complete files, commands and debugging guidance. Real project editing/execution needs a separate per-user isolated workspace.

The Phase 2 boundary provides workspace file, patch-preview and execution contracts. `CodingWorkspacePolicy` accepts relative paths under a provisioned root, canonicalizes them, blocks traversal/absolute paths and blocks common secret files. The application repository is never offered as a user workspace. `UnavailableCodeExecutionService` explicitly reports `ISOLATED_EXECUTION_NOT_CONFIGURED`; no host shell endpoint exists.

Before enabling execution, add an ephemeral container or equivalent sandbox with CPU, memory, process, time and network limits, immutable base images, per-run storage, allowlisted commands, output caps and cleanup. Patch application should require a preview and preserve a rollback snapshot.

