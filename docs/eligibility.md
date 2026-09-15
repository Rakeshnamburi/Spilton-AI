# Explainable eligibility screening

The UI deliberately collects only age at the notification's stated cutoff, a coarse education level, whether relaxation is needed and confirmation that the complete notification was reviewed. No date of birth, Aadhaar, PAN, registration ID, password or provider credential is requested. Request inputs and results are **not persisted**; request bodies are not logged.

Notification extraction classifies exact text excerpts as organization, qualification, age, relaxation, dates, fee, selection process, exam pattern, vacancies, application instructions, syllabus, citizenship or experience. Each field stores its source chunk, actual page/section and `EXCERPT_REVIEW_REQUIRED` confidence. Heading-only lines include nearby text where available. This is bounded heuristic section detection, not a complete parser or legal interpretation. Missing fields are unknown.

The base-rule engine currently recognizes one explicit `Age: 21 to 30 years`-style range and one unambiguous general bachelor's-degree requirement. Discipline, alternative qualifications, percentage/marks conditions, multiple age ranges, relaxation, citizenship and experience wording require manual review. The exam preparation profile has no education/age fields, so none are inferred from it.

| Result | Meaning |
|---|---|
| INSUFFICIENT_INFORMATION | Required age/education input is missing |
| NEEDS_MANUAL_REVIEW | Unsupported/ambiguous rules, relaxation, additional explicit conditions or incomplete source review |
| LIKELY_NOT_ELIGIBLE | Supported reviewed base rules include a failed comparison |
| LIKELY_ELIGIBLE | Supported reviewed base checks passed, conditional on complete-source review |

Each comparison returns rule, input used, PASS/FAIL/MISSING/REVIEW, reason and source quotation/page. A “likely” result is not an official determination; changing posts, cutoff dates or corrections can change eligibility. Complex real notifications generally return manual review rather than overconfident automation. The UI shows the source's verification label.

Two notifications can be compared side by side with exact quotes for qualification, age/relaxation, dates, fees, selection, exam pattern and syllabus. Missing excerpts are not treated as proven changes. Broader structured rule authoring, discipline/experience matching and category relaxation interpretation remain future work.
