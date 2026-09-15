# Multimodal boundary

`IMultimodalProvider`, `MediaPart`, model capability metadata and `MultimodalPolicy` form the reusable vision boundary. Supported image metadata is limited to PNG, JPEG, WebP and GIF with an 8 MB ceiling and an owned storage reference. The policy rejects traversal-like references.

No configured provider currently supplies verified vision support. The API reports `VISION_PROVIDER_NOT_CONFIGURED`; SPILTON must not describe an image or claim visual inspection. Audio is planned and is not exposed as working.

