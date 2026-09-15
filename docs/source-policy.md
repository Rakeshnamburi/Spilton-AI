# Government-source policy

Source labels are separate from truth or freshness of a claim:

| Label | Rank | Meaning |
|---|---:|---|
| OFFICIAL | 100 | Exact uploaded bytes and source URL match a locally reviewed official download |
| TRUSTED_SECONDARY | 70 | Exact bytes/URL match an administrator-reviewed institutional source entry |
| USER_UPLOADED | 40 | Owned uploaded document; origin not verified |
| UNVERIFIED | 10 | Registered URL metadata without verified document evidence |

The browser cannot set/promote SourceType, verification, hash or verification date. SourcePolicy computes them. A document's filename or a government URL typed beside unrelated bytes does not establish official provenance. Manual titles, dates, classifications and summaries remain manual metadata even when source bytes are verified.

The app does **not** fetch user-supplied URLs. HTTPS, public DNS-style hostnames, standard port and no URL credentials are required; IP/localhost/internal host forms and executable schemes are rejected. Because no server request is made, registration cannot trigger SSRF or a crawler. Links open as labelled sources, not guessed Apply buttons. Never infer deadlines, eligibility, vacancies or application URLs from the starter exam catalog.

## Development official-source workflow

1. Open the official organization's portal and verify the exact document link. Respect its terms; download one permitted public document, not an entire site.
2. Run the project-local administrative script with that exact HTTPS hostname:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
.\scripts\register-official-source.ps1 `
  -Url 'https://www.upsc.gov.in/sites/default/files/AdvtNo-52-2026-Special-Engl-210826.pdf' `
  -Organization 'Union Public Service Commission' `
  -ReviewedOfficialHost 'www.upsc.gov.in'
```

This **example is a dated test snapshot, not a recommendation to apply**. The PDF was linked from [UPSC's recruitment advertisements page](https://www.upsc.gov.in/recruitment/recruitment-advertisement) when checked on 10 September 2026. The script rejects redirects, limits download to 5 MB, verifies the PDF signature, calculates SHA-256 and writes ignored `.local/verified-sources.json` plus `.local/official-sources/{hash}.pdf`.

3. Upload the returned file through Spilton. Register the exact source URL alongside that Ready document. Matching bytes receive `SOURCE_BYTES_VERIFIED` and the review timestamp. Changed/unmatched bytes remain USER_UPLOADED.
4. Check subsequent corrections and the portal again before relying on high-impact facts. A verified snapshot is not proof that information remains current.

The local registry is an administrator trust boundary, not an end-user edit feature. It may contain other reviewed organizations; the architecture is not SSC-specific. TRUSTED_SECONDARY entries use the same exact URL/hash structure and require deliberate local administrative review. No public administration endpoint or paid search integration exists.

No provider credentials are involved in registration. The script does not purchase anything. Do not paste API keys into source forms, scripts, chat or documentation.
