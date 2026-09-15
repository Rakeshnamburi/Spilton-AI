# Exam preparation domain

Exam → ExamStage → Subject → Topic is a normalized starter catalog. UserExamProfile belongs to a user and optional Space, with one current profile per workspace. UserTopicProgress belongs to that profile and a topic; unique profile/topic pairs and valid attempted/correct counts are enforced. Changing target stage preserves old progress but only current-stage topics enter the dashboard/context.

The seeded SSC CGL/CHSL, RRB, Banking, APPSC and UPSC labels are study organizers, **not complete or officially verified current syllabi**. Similar starter subjects are intentionally reusable. Official notification evidence must be kept separate from this taxonomy.

Additional tables: Spaces, Goals, Memories, StudyPlans, StudyPlanItems. Existing Conversations and Documents gain nullable SpaceId. Migration `20260910035814_AddPreparationSpaces` is additive. No existing user/chat/document rows are deleted. Foreign-key restrictions prevent removing a nonempty Space.

APIs: /api/spaces GET/POST and /{id} GET/PATCH/DELETE; /api/preparation/catalog and /dashboard GET; /profile, /goals, /memories, /plans POST; /goals/{id} PATCH/DELETE; /memories/{id} PATCH/DELETE; /progress/{topicId} PATCH; /plan-items/{id} PATCH. All personal endpoints require JWT ownership. General data uses omitted/null SpaceId.
