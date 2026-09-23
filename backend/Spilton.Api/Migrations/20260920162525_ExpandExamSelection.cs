using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Spilton.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandExamSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exams",
                columns: new[] { "Id", "Family", "Name" },
                values: new object[,]
                {
                    { new Guid("01ed612f-d0e1-b799-58fe-04272c052166"), "State PSC", "MPPSC" },
                    { new Guid("0d2e5bed-b211-5add-f025-3ae3d5f1a50e"), "State PSC", "MPSC" },
                    { new Guid("0f8155b3-856e-b417-a59c-469ffb8110f4"), "State PSC", "Punjab PSC" },
                    { new Guid("0fae0d7d-dd59-4346-3bf3-f682d2de291b"), "SSC Other", "SSC JE" },
                    { new Guid("115d0ce3-91ec-9925-32d0-e67eb97f8c1a"), "Banking", "RBI Grade B" },
                    { new Guid("14d36c47-2010-4872-8481-64301f48d37b"), "Teaching", "CTET" },
                    { new Guid("17b2cbae-241d-31a2-915e-620bf7ad08c0"), "Defence", "AFCAT" },
                    { new Guid("1c6e410e-80e8-20b3-b612-ad966de94f48"), "UPSC", "UPSC Civil Services" },
                    { new Guid("1e7bd3d6-2c6b-d230-cf4d-329d5a046bdc"), "Defence", "UPSC CAPF" },
                    { new Guid("22ea299e-04da-0a03-4cc8-d138b621b9d1"), "State PSC", "Telangana Group 2" },
                    { new Guid("25ef7a86-ce44-fd78-4f81-0688a9163dbd"), "Banking", "SBI PO" },
                    { new Guid("28cfeb5b-0dbc-836e-224d-3545a0691cd0"), "State PSC", "APPSC Group 2" },
                    { new Guid("2f6c21bb-9f0d-d631-4b38-948b63d5ac96"), "State PSC", "APPSC Group 1" },
                    { new Guid("3ad9ae3e-8f52-79e6-d8ce-b3e21dce6576"), "Banking", "SBI Clerk" },
                    { new Guid("4061d256-6119-a453-dbc5-d5a0f20b3914"), "Banking", "IBPS RRB" },
                    { new Guid("4210a105-d2c7-523e-4aa8-5007da4903c7"), "SSC Other", "SSC CPO" },
                    { new Guid("44c9aadb-ad67-1fcc-9b26-2c683fc5405c"), "State PSC", "Haryana PSC" },
                    { new Guid("4a3c4aff-a8a0-15e0-2475-bcc12de9d71b"), "State PSC", "GPSC" },
                    { new Guid("5173fd10-a78d-0ee0-1d05-1dcbe924c5ec"), "Defence", "UPSC CDS" },
                    { new Guid("52112b11-75c4-8710-6598-7d5d740b3118"), "RRB", "RRB JE" },
                    { new Guid("5a36f5c8-4696-4467-4c72-fb8e04fdce5d"), "SSC Other", "SSC MTS" },
                    { new Guid("6446a1ca-308d-799f-3d14-19e396b0501e"), "State PSC", "BPSC" },
                    { new Guid("831881f3-0c28-8ca0-f835-9deb2c83b2cd"), "State PSC", "RPSC" },
                    { new Guid("90f766dc-b2a5-d960-5d49-bb218e703ad6"), "Teaching", "UGC NET" },
                    { new Guid("af08d38b-cbf5-fa88-812f-8f843932c902"), "State PSC", "UPPSC" },
                    { new Guid("b6fcc5c7-6540-dbe3-3cab-9fc71e72a0ed"), "State PSC", "OPSC" },
                    { new Guid("bd68ec8d-6664-8323-4263-deb347b82226"), "Teaching", "CSIR NET" },
                    { new Guid("ceadd86e-83ff-96ca-3fab-129c16982e63"), "Defence", "UPSC NDA" },
                    { new Guid("cee1596f-d5c8-6eb8-e2a7-214ea5e3e7f0"), "RRB", "RRB Group D" },
                    { new Guid("cf651407-313d-497d-b71b-a47fcb56f32d"), "State PSC", "TNPSC" },
                    { new Guid("d4298507-6165-c070-fdff-b9532bb9ff81"), "Banking", "IBPS Clerk" },
                    { new Guid("d6c5dd45-1a1f-9337-c1ad-603bfe687b03"), "Banking", "IBPS PO" },
                    { new Guid("d79f3259-1cf2-646d-15f9-e8f314a9d9d1"), "State PSC", "Telangana Group 1" },
                    { new Guid("d951e176-1ad3-6ae4-7547-31cb15aa0501"), "SSC Other", "SSC GD" },
                    { new Guid("e52c40c3-ffda-ecef-2f71-9461cca27782"), "State PSC", "WBPSC" },
                    { new Guid("e9dab7c0-bbf4-8798-a1ca-62959c568082"), "State PSC", "Assam PSC" },
                    { new Guid("eef5343d-befb-84e2-8d32-f0eb54c0d00f"), "State PSC", "Kerala PSC" },
                    { new Guid("f01c3e41-20ab-a1ba-beaf-d411c1fb3892"), "Teaching", "State TET" },
                    { new Guid("f1e36386-fdd0-eb23-5904-6abd9742bb93"), "RRB", "RRB ALP" },
                    { new Guid("fe61ec0a-b7d5-eb45-1e2c-9a8d40cfce21"), "State PSC", "Karnataka PSC" }
                });

            migrationBuilder.InsertData(
                table: "ExamStages",
                columns: new[] { "Id", "ExamId", "Name" },
                values: new object[,]
                {
                    { new Guid("0109ec90-48c8-fe15-3bbd-b0d0aea6002a"), new Guid("4a3c4aff-a8a0-15e0-2475-bcc12de9d71b"), "General preparation" },
                    { new Guid("02649287-aaf7-1427-3199-e9c77d3d995c"), new Guid("2f6c21bb-9f0d-d631-4b38-948b63d5ac96"), "General preparation" },
                    { new Guid("0716af56-8176-1a84-63ff-9c6a32622a47"), new Guid("fe61ec0a-b7d5-eb45-1e2c-9a8d40cfce21"), "General preparation" },
                    { new Guid("140935c2-6291-87af-2235-b79257104223"), new Guid("af08d38b-cbf5-fa88-812f-8f843932c902"), "General preparation" },
                    { new Guid("20e298fd-e875-24aa-2aba-f154dae969cf"), new Guid("25ef7a86-ce44-fd78-4f81-0688a9163dbd"), "General preparation" },
                    { new Guid("29ed97c5-1abf-6a8d-3bf8-7850cbe9da48"), new Guid("4061d256-6119-a453-dbc5-d5a0f20b3914"), "General preparation" },
                    { new Guid("2fc9f791-98f8-764e-425b-2db2e475e097"), new Guid("cf651407-313d-497d-b71b-a47fcb56f32d"), "General preparation" },
                    { new Guid("33aaa431-3499-88ad-ec42-3e68799bbc41"), new Guid("115d0ce3-91ec-9925-32d0-e67eb97f8c1a"), "General preparation" },
                    { new Guid("3db7f32f-cbf5-62ce-5b13-082a0040630d"), new Guid("14d36c47-2010-4872-8481-64301f48d37b"), "General preparation" },
                    { new Guid("4b998bea-363d-ad7a-0762-1ef391ab3879"), new Guid("28cfeb5b-0dbc-836e-224d-3545a0691cd0"), "General preparation" },
                    { new Guid("53039780-1877-af50-8ae1-cd0d62ac09cb"), new Guid("1e7bd3d6-2c6b-d230-cf4d-329d5a046bdc"), "General preparation" },
                    { new Guid("573971e6-54de-cf2b-01b1-6b00fbd3beac"), new Guid("bd68ec8d-6664-8323-4263-deb347b82226"), "General preparation" },
                    { new Guid("5bedb96b-fbf0-3cd3-75dc-d3d098b39ecd"), new Guid("f01c3e41-20ab-a1ba-beaf-d411c1fb3892"), "General preparation" },
                    { new Guid("617afb6b-29bd-edd9-013b-2ede179e112c"), new Guid("b6fcc5c7-6540-dbe3-3cab-9fc71e72a0ed"), "General preparation" },
                    { new Guid("660bf600-82b9-241d-6d7b-92ebc1a6c74b"), new Guid("0f8155b3-856e-b417-a59c-469ffb8110f4"), "General preparation" },
                    { new Guid("75477ff3-5e3a-f204-bfaf-867f5e8b8c8e"), new Guid("4210a105-d2c7-523e-4aa8-5007da4903c7"), "General preparation" },
                    { new Guid("7aa0a8e6-fc98-de28-1ded-553cac8eb425"), new Guid("01ed612f-d0e1-b799-58fe-04272c052166"), "General preparation" },
                    { new Guid("7e81c179-2df8-07f4-0e53-8a622b5f17b3"), new Guid("0d2e5bed-b211-5add-f025-3ae3d5f1a50e"), "General preparation" },
                    { new Guid("9002fb31-c048-fed1-a656-82924be87ad4"), new Guid("d79f3259-1cf2-646d-15f9-e8f314a9d9d1"), "General preparation" },
                    { new Guid("94e66efc-b100-6412-161e-6f98e932dcd1"), new Guid("1c6e410e-80e8-20b3-b612-ad966de94f48"), "General preparation" },
                    { new Guid("955330fb-c23a-6121-f52c-5148ae7c8a9d"), new Guid("cee1596f-d5c8-6eb8-e2a7-214ea5e3e7f0"), "General preparation" },
                    { new Guid("a3ea00f3-2d2f-4107-d4dc-8e01a1a7c1e8"), new Guid("3ad9ae3e-8f52-79e6-d8ce-b3e21dce6576"), "General preparation" },
                    { new Guid("a44acf4a-c0ef-33bc-6991-fbb580912bb9"), new Guid("831881f3-0c28-8ca0-f835-9deb2c83b2cd"), "General preparation" },
                    { new Guid("a54ba431-50d9-5d6b-c28c-172bd8196258"), new Guid("e9dab7c0-bbf4-8798-a1ca-62959c568082"), "General preparation" },
                    { new Guid("a74e2eea-17d5-0f84-16b6-e8c20028cf92"), new Guid("eef5343d-befb-84e2-8d32-f0eb54c0d00f"), "General preparation" },
                    { new Guid("ad1dc0fa-deb6-660f-b109-411e8cd8c182"), new Guid("e52c40c3-ffda-ecef-2f71-9461cca27782"), "General preparation" },
                    { new Guid("ae84e8e7-10d0-b1d1-aee9-a180a547d38d"), new Guid("44c9aadb-ad67-1fcc-9b26-2c683fc5405c"), "General preparation" },
                    { new Guid("af15cbbe-31cf-9016-fc81-b4e041d39025"), new Guid("52112b11-75c4-8710-6598-7d5d740b3118"), "General preparation" },
                    { new Guid("b40cd678-0fda-0b80-f6cb-da6f67528f17"), new Guid("5173fd10-a78d-0ee0-1d05-1dcbe924c5ec"), "General preparation" },
                    { new Guid("b4394a85-b058-c2fb-8065-d60b8829474c"), new Guid("5a36f5c8-4696-4467-4c72-fb8e04fdce5d"), "General preparation" },
                    { new Guid("bc843aea-4f35-9b06-3f66-97614da52ecc"), new Guid("90f766dc-b2a5-d960-5d49-bb218e703ad6"), "General preparation" },
                    { new Guid("bc909a90-7e7b-96fe-431f-9e4e073f5371"), new Guid("6446a1ca-308d-799f-3d14-19e396b0501e"), "General preparation" },
                    { new Guid("beb716b4-7120-d231-cb9f-033720fc5ba4"), new Guid("ceadd86e-83ff-96ca-3fab-129c16982e63"), "General preparation" },
                    { new Guid("c294a04b-502d-6d18-177e-795410ff4b91"), new Guid("22ea299e-04da-0a03-4cc8-d138b621b9d1"), "General preparation" },
                    { new Guid("ce94cedb-a7c1-dc26-b3e2-58e090c73790"), new Guid("d6c5dd45-1a1f-9337-c1ad-603bfe687b03"), "General preparation" },
                    { new Guid("cf818614-e920-ee64-2bfd-df896886f307"), new Guid("d951e176-1ad3-6ae4-7547-31cb15aa0501"), "General preparation" },
                    { new Guid("d95a0b78-89e3-faff-2356-5f568460e1e4"), new Guid("f1e36386-fdd0-eb23-5904-6abd9742bb93"), "General preparation" },
                    { new Guid("dc88fe6e-6268-f8f7-5347-9c8466574da4"), new Guid("17b2cbae-241d-31a2-915e-620bf7ad08c0"), "General preparation" },
                    { new Guid("eb0bce1d-2230-8142-e37b-120396c1167d"), new Guid("d4298507-6165-c070-fdff-b9532bb9ff81"), "General preparation" },
                    { new Guid("fb34e05b-9dd2-11d1-509f-35ff00f7b2fd"), new Guid("0fae0d7d-dd59-4346-3bf3-f682d2de291b"), "General preparation" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "ExamStageId", "Name" },
                values: new object[,]
                {
                    { new Guid("0426bd63-650d-39e6-ba91-32a1bf2c0188"), new Guid("4b998bea-363d-ad7a-0762-1ef391ab3879"), "English" },
                    { new Guid("04e69404-9b8a-d643-b48a-8710d2f73d29"), new Guid("c294a04b-502d-6d18-177e-795410ff4b91"), "Quantitative Aptitude" },
                    { new Guid("0886696f-48c8-891e-a7d5-8550fb0d7021"), new Guid("a74e2eea-17d5-0f84-16b6-e8c20028cf92"), "Reasoning" },
                    { new Guid("096355bb-6cb5-8220-abc3-6fe02899a819"), new Guid("617afb6b-29bd-edd9-013b-2ede179e112c"), "General Awareness" },
                    { new Guid("0aefdf79-fe7f-9bdc-e978-fed568e3b1d4"), new Guid("29ed97c5-1abf-6a8d-3bf8-7850cbe9da48"), "English" },
                    { new Guid("0b52a18c-3ced-19d8-b9d3-dc053e587ad4"), new Guid("a44acf4a-c0ef-33bc-6991-fbb580912bb9"), "Quantitative Aptitude" },
                    { new Guid("0c484af1-d5d5-453c-06a7-067c5d5a87fb"), new Guid("a74e2eea-17d5-0f84-16b6-e8c20028cf92"), "Quantitative Aptitude" },
                    { new Guid("0dc1832f-ee53-b526-2499-2645419c24e7"), new Guid("a54ba431-50d9-5d6b-c28c-172bd8196258"), "General Awareness" },
                    { new Guid("0dfd2625-89ac-f13e-6b38-e3c0d8d0a2c4"), new Guid("ce94cedb-a7c1-dc26-b3e2-58e090c73790"), "Quantitative Aptitude" },
                    { new Guid("0e77bff5-deff-a943-0610-fd871552ad7b"), new Guid("20e298fd-e875-24aa-2aba-f154dae969cf"), "English" },
                    { new Guid("0ed4b1a8-e05d-fa4c-423a-150036463aff"), new Guid("2fc9f791-98f8-764e-425b-2db2e475e097"), "General Awareness" },
                    { new Guid("0edd4df9-10c6-ab16-4384-f3cc03c77e3e"), new Guid("ad1dc0fa-deb6-660f-b109-411e8cd8c182"), "General Awareness" },
                    { new Guid("0f9aae8b-5730-91c1-4eba-dcab68ef87d4"), new Guid("bc843aea-4f35-9b06-3f66-97614da52ecc"), "English" },
                    { new Guid("1a1e72e3-c129-2580-72a0-51e0c5e8c51a"), new Guid("dc88fe6e-6268-f8f7-5347-9c8466574da4"), "Reasoning" },
                    { new Guid("1fba6d2c-20e5-6bb6-6119-cfefd37d187a"), new Guid("3db7f32f-cbf5-62ce-5b13-082a0040630d"), "Quantitative Aptitude" },
                    { new Guid("205fdf02-2719-c1b7-be92-666228b6e3e7"), new Guid("573971e6-54de-cf2b-01b1-6b00fbd3beac"), "General Awareness" },
                    { new Guid("21b78efb-e9e3-8cc5-1d58-8cc5510842a7"), new Guid("5bedb96b-fbf0-3cd3-75dc-d3d098b39ecd"), "Quantitative Aptitude" },
                    { new Guid("22ed3e15-caf6-1025-37b9-8459eaae6fe4"), new Guid("ad1dc0fa-deb6-660f-b109-411e8cd8c182"), "Quantitative Aptitude" },
                    { new Guid("239d45ea-f0cb-4232-5fea-bd7c5cbe3ab3"), new Guid("33aaa431-3499-88ad-ec42-3e68799bbc41"), "English" },
                    { new Guid("2414d6e1-c8bd-053f-5c2e-20a4c5c3a12c"), new Guid("5bedb96b-fbf0-3cd3-75dc-d3d098b39ecd"), "Reasoning" },
                    { new Guid("248f5cbe-71c8-ca4a-44fd-37b5fe0b5f4d"), new Guid("cf818614-e920-ee64-2bfd-df896886f307"), "Reasoning" },
                    { new Guid("26aeaf8e-cccf-0ceb-5682-ff5da0b6d543"), new Guid("53039780-1877-af50-8ae1-cd0d62ac09cb"), "Quantitative Aptitude" },
                    { new Guid("2922679e-6588-e91d-2617-07abd23a9823"), new Guid("94e66efc-b100-6412-161e-6f98e932dcd1"), "General Awareness" },
                    { new Guid("29cbfd96-bd12-d1e4-10ad-43a1fecbfe56"), new Guid("beb716b4-7120-d231-cb9f-033720fc5ba4"), "Quantitative Aptitude" },
                    { new Guid("2b57ea8f-d5a6-227e-b079-b81ae68d0267"), new Guid("02649287-aaf7-1427-3199-e9c77d3d995c"), "English" },
                    { new Guid("2df66c1d-c139-3764-c529-61c645d4245b"), new Guid("ae84e8e7-10d0-b1d1-aee9-a180a547d38d"), "Quantitative Aptitude" },
                    { new Guid("30ae9e12-3ceb-88c9-5e54-a00e67f69df4"), new Guid("0109ec90-48c8-fe15-3bbd-b0d0aea6002a"), "English" },
                    { new Guid("30b331ed-e430-7bae-e7ce-f589f35517eb"), new Guid("94e66efc-b100-6412-161e-6f98e932dcd1"), "English" },
                    { new Guid("30c33b7b-9e1f-487b-fd59-2872a64fe595"), new Guid("9002fb31-c048-fed1-a656-82924be87ad4"), "English" },
                    { new Guid("3206020e-1e1a-d766-d66b-6d604baf5616"), new Guid("bc909a90-7e7b-96fe-431f-9e4e073f5371"), "English" },
                    { new Guid("322fb74b-b528-7a96-495c-39fc1d728a2e"), new Guid("b4394a85-b058-c2fb-8065-d60b8829474c"), "Reasoning" },
                    { new Guid("3249993c-ee08-0d52-5e33-5def559739ce"), new Guid("33aaa431-3499-88ad-ec42-3e68799bbc41"), "Quantitative Aptitude" },
                    { new Guid("3c40893b-b1da-0e77-c6e6-27559f0200bb"), new Guid("c294a04b-502d-6d18-177e-795410ff4b91"), "English" },
                    { new Guid("3c7bd5f1-989b-5d61-d6e4-f5af3a101227"), new Guid("fb34e05b-9dd2-11d1-509f-35ff00f7b2fd"), "Quantitative Aptitude" },
                    { new Guid("3e6077cf-a585-a5b4-8058-0ab46cef9880"), new Guid("ce94cedb-a7c1-dc26-b3e2-58e090c73790"), "English" },
                    { new Guid("3ebe7357-d069-0030-d517-fbd60e30a263"), new Guid("33aaa431-3499-88ad-ec42-3e68799bbc41"), "General Awareness" },
                    { new Guid("3f9b3521-896b-6a86-c006-f74c596b124d"), new Guid("5bedb96b-fbf0-3cd3-75dc-d3d098b39ecd"), "General Awareness" },
                    { new Guid("42599220-bbb4-8c25-dfa1-f63ee080c8dd"), new Guid("cf818614-e920-ee64-2bfd-df896886f307"), "General Awareness" },
                    { new Guid("42cdbe8c-d2ae-443c-1d7d-586a0e8f42f5"), new Guid("29ed97c5-1abf-6a8d-3bf8-7850cbe9da48"), "General Awareness" },
                    { new Guid("43cc05df-90fb-d3a9-3329-e42168f33a77"), new Guid("beb716b4-7120-d231-cb9f-033720fc5ba4"), "English" },
                    { new Guid("4773c0d2-b3ca-c94d-9849-c745b4f3dba8"), new Guid("ae84e8e7-10d0-b1d1-aee9-a180a547d38d"), "English" },
                    { new Guid("47bc02e3-9998-79d2-ca9c-e4c3b751689e"), new Guid("beb716b4-7120-d231-cb9f-033720fc5ba4"), "General Awareness" },
                    { new Guid("482cd3d1-1df4-67f3-a3bd-b273d9892ab8"), new Guid("b4394a85-b058-c2fb-8065-d60b8829474c"), "General Awareness" },
                    { new Guid("48edbf68-6220-167c-9c06-21954ca206e9"), new Guid("a3ea00f3-2d2f-4107-d4dc-8e01a1a7c1e8"), "English" },
                    { new Guid("48f0f198-01af-1a80-8f50-aacffbb6440e"), new Guid("53039780-1877-af50-8ae1-cd0d62ac09cb"), "General Awareness" },
                    { new Guid("49d1d6ba-8541-3024-5413-135b5663003c"), new Guid("573971e6-54de-cf2b-01b1-6b00fbd3beac"), "Quantitative Aptitude" },
                    { new Guid("49d6e9c7-e86b-daf0-0a22-16cfa49fc212"), new Guid("af15cbbe-31cf-9016-fc81-b4e041d39025"), "General Awareness" },
                    { new Guid("4c110e73-7f94-2f66-ba13-eb0bd5733804"), new Guid("29ed97c5-1abf-6a8d-3bf8-7850cbe9da48"), "Quantitative Aptitude" },
                    { new Guid("4d5881bc-d0b4-0d9e-4975-1dd816dfd3e5"), new Guid("9002fb31-c048-fed1-a656-82924be87ad4"), "Quantitative Aptitude" },
                    { new Guid("4eeb5fcc-8d45-97a3-9f21-8838d2db2c9b"), new Guid("d95a0b78-89e3-faff-2356-5f568460e1e4"), "General Awareness" },
                    { new Guid("5006ddb3-b59d-627a-0a54-9567cad71a31"), new Guid("b4394a85-b058-c2fb-8065-d60b8829474c"), "English" },
                    { new Guid("5392ef0e-3dbf-d6d5-8adc-9f074c7b0f3e"), new Guid("c294a04b-502d-6d18-177e-795410ff4b91"), "General Awareness" },
                    { new Guid("55e88ad4-b31c-ecca-8211-a5d39afca9c1"), new Guid("bc909a90-7e7b-96fe-431f-9e4e073f5371"), "Quantitative Aptitude" },
                    { new Guid("59a1391d-7a39-5360-9fb3-832bdb2cac79"), new Guid("b4394a85-b058-c2fb-8065-d60b8829474c"), "Quantitative Aptitude" },
                    { new Guid("5ab6da55-694a-0a7f-bd80-a63079edf15b"), new Guid("a44acf4a-c0ef-33bc-6991-fbb580912bb9"), "English" },
                    { new Guid("5c4b02f0-6369-076a-31b2-4e64db0b316f"), new Guid("ce94cedb-a7c1-dc26-b3e2-58e090c73790"), "General Awareness" },
                    { new Guid("5c99f7b3-af3e-e99e-0824-9d55cbc055a1"), new Guid("a54ba431-50d9-5d6b-c28c-172bd8196258"), "English" },
                    { new Guid("60150aea-1845-3cb3-002d-8c8b1e3eddd7"), new Guid("a3ea00f3-2d2f-4107-d4dc-8e01a1a7c1e8"), "Reasoning" },
                    { new Guid("60d5154d-3d43-60dd-b21d-0943056545bd"), new Guid("2fc9f791-98f8-764e-425b-2db2e475e097"), "Quantitative Aptitude" },
                    { new Guid("61f44871-630e-0b34-8fad-ca4cc042320b"), new Guid("20e298fd-e875-24aa-2aba-f154dae969cf"), "Reasoning" },
                    { new Guid("633b28ab-6d5a-dd92-ce09-65145ac09269"), new Guid("140935c2-6291-87af-2235-b79257104223"), "Reasoning" },
                    { new Guid("64eda4df-4f57-2a64-b542-f9d8d2463c0d"), new Guid("dc88fe6e-6268-f8f7-5347-9c8466574da4"), "English" },
                    { new Guid("65e36679-c743-faf2-6191-8a6c5f266f04"), new Guid("02649287-aaf7-1427-3199-e9c77d3d995c"), "General Awareness" },
                    { new Guid("6850b500-1bb1-e162-189f-045a15d9a0f2"), new Guid("955330fb-c23a-6121-f52c-5148ae7c8a9d"), "Reasoning" },
                    { new Guid("68d95e07-877c-0cb9-0c71-381742807ddb"), new Guid("ad1dc0fa-deb6-660f-b109-411e8cd8c182"), "Reasoning" },
                    { new Guid("68dd4f56-21f3-72c9-4a78-6ac785f4ac6e"), new Guid("0109ec90-48c8-fe15-3bbd-b0d0aea6002a"), "Quantitative Aptitude" },
                    { new Guid("69c2b215-e4dd-c22f-48ba-6affabc79966"), new Guid("20e298fd-e875-24aa-2aba-f154dae969cf"), "Quantitative Aptitude" },
                    { new Guid("6b36ab48-4a04-61ec-0b7f-57884fea536a"), new Guid("eb0bce1d-2230-8142-e37b-120396c1167d"), "English" },
                    { new Guid("6cfc7e4b-94e6-d2cd-da48-a516fe9aa24b"), new Guid("7aa0a8e6-fc98-de28-1ded-553cac8eb425"), "Quantitative Aptitude" },
                    { new Guid("6d81ff1c-ca17-baed-b320-3fe9b11eb2f5"), new Guid("29ed97c5-1abf-6a8d-3bf8-7850cbe9da48"), "Reasoning" },
                    { new Guid("6dfdb9f3-fcc7-d8cd-9bb7-4b366d335b84"), new Guid("a44acf4a-c0ef-33bc-6991-fbb580912bb9"), "Reasoning" },
                    { new Guid("6e5381c2-aa75-e19e-eac4-a983be83afd5"), new Guid("bc843aea-4f35-9b06-3f66-97614da52ecc"), "Reasoning" },
                    { new Guid("74afdb9b-0e1c-94b9-765e-d720199d450d"), new Guid("dc88fe6e-6268-f8f7-5347-9c8466574da4"), "General Awareness" },
                    { new Guid("77075acf-29eb-d1e7-f247-9b164a9d9b87"), new Guid("573971e6-54de-cf2b-01b1-6b00fbd3beac"), "Reasoning" },
                    { new Guid("79200f88-1d99-cfa8-0aa9-2b2077af2f6a"), new Guid("a74e2eea-17d5-0f84-16b6-e8c20028cf92"), "English" },
                    { new Guid("7ae414ef-143b-4d3e-20b1-ea9fcc461547"), new Guid("a54ba431-50d9-5d6b-c28c-172bd8196258"), "Quantitative Aptitude" },
                    { new Guid("7b2364df-0c2b-26f8-281d-af4d6518681b"), new Guid("75477ff3-5e3a-f204-bfaf-867f5e8b8c8e"), "Quantitative Aptitude" },
                    { new Guid("7bd4bbc7-818f-47aa-ea61-1894b73d2186"), new Guid("02649287-aaf7-1427-3199-e9c77d3d995c"), "Quantitative Aptitude" },
                    { new Guid("7dac52a4-15f6-e83e-3143-62647c5adf4c"), new Guid("eb0bce1d-2230-8142-e37b-120396c1167d"), "General Awareness" },
                    { new Guid("7e4344c7-802a-1dbd-6a3e-d8b0b12e7666"), new Guid("cf818614-e920-ee64-2bfd-df896886f307"), "Quantitative Aptitude" },
                    { new Guid("7f5ddba9-3e27-eeb8-eb18-e306a8d57622"), new Guid("75477ff3-5e3a-f204-bfaf-867f5e8b8c8e"), "Reasoning" },
                    { new Guid("7fdd795a-6994-63d6-fd5a-9372678e5143"), new Guid("955330fb-c23a-6121-f52c-5148ae7c8a9d"), "General Awareness" },
                    { new Guid("82df9511-fd16-f5b9-3da2-d877a07ace7c"), new Guid("617afb6b-29bd-edd9-013b-2ede179e112c"), "Reasoning" },
                    { new Guid("85986333-45ff-1990-65d6-97248deb6d08"), new Guid("955330fb-c23a-6121-f52c-5148ae7c8a9d"), "English" },
                    { new Guid("867f98d1-22e1-c9e6-7eb9-a2030b743d9b"), new Guid("ae84e8e7-10d0-b1d1-aee9-a180a547d38d"), "Reasoning" },
                    { new Guid("8910836e-bf37-7b12-dfcd-fc7cb4ef7495"), new Guid("a54ba431-50d9-5d6b-c28c-172bd8196258"), "Reasoning" },
                    { new Guid("8ac4ada6-4775-eeb0-d87f-32383572fbe6"), new Guid("ad1dc0fa-deb6-660f-b109-411e8cd8c182"), "English" },
                    { new Guid("8dd5c1f6-3406-e8a6-de08-130071a44989"), new Guid("0716af56-8176-1a84-63ff-9c6a32622a47"), "Reasoning" },
                    { new Guid("8eebe33a-8ce2-3bed-d6d7-55d057d7fff4"), new Guid("7aa0a8e6-fc98-de28-1ded-553cac8eb425"), "English" },
                    { new Guid("8fc32dcf-6b85-d7b7-c9ee-dfeb8347435a"), new Guid("5bedb96b-fbf0-3cd3-75dc-d3d098b39ecd"), "English" },
                    { new Guid("91eebe88-16a5-addf-707d-06768eed2e82"), new Guid("af15cbbe-31cf-9016-fc81-b4e041d39025"), "Reasoning" },
                    { new Guid("92f3676e-2bdc-7c39-31ba-7e057faaac57"), new Guid("fb34e05b-9dd2-11d1-509f-35ff00f7b2fd"), "Reasoning" },
                    { new Guid("93b5cded-e275-4edf-d41c-341fb7954bda"), new Guid("af15cbbe-31cf-9016-fc81-b4e041d39025"), "Quantitative Aptitude" },
                    { new Guid("93dedde1-bc85-8d54-8ab5-0f407da7fd28"), new Guid("c294a04b-502d-6d18-177e-795410ff4b91"), "Reasoning" },
                    { new Guid("94994903-a722-d664-d90f-c27454228d9a"), new Guid("3db7f32f-cbf5-62ce-5b13-082a0040630d"), "English" },
                    { new Guid("94ae42f3-35b8-aa97-694c-77c3df885c64"), new Guid("7e81c179-2df8-07f4-0e53-8a622b5f17b3"), "Quantitative Aptitude" },
                    { new Guid("99a755c8-4233-88d1-880c-20d737ea5468"), new Guid("ce94cedb-a7c1-dc26-b3e2-58e090c73790"), "Reasoning" },
                    { new Guid("99d290b8-8163-35de-4061-d29f52183491"), new Guid("b40cd678-0fda-0b80-f6cb-da6f67528f17"), "English" },
                    { new Guid("9bc50977-c5e3-4eea-99fd-100bcca73458"), new Guid("617afb6b-29bd-edd9-013b-2ede179e112c"), "Quantitative Aptitude" },
                    { new Guid("9c860696-a5de-ec1a-83b1-d00dc21e8195"), new Guid("94e66efc-b100-6412-161e-6f98e932dcd1"), "Reasoning" },
                    { new Guid("9ffa0b37-7671-52d4-b662-9254099ae239"), new Guid("bc843aea-4f35-9b06-3f66-97614da52ecc"), "Quantitative Aptitude" },
                    { new Guid("a0d8924d-729a-94ff-b85a-f00daaf488fe"), new Guid("d95a0b78-89e3-faff-2356-5f568460e1e4"), "Reasoning" },
                    { new Guid("a6f8ba35-9571-9bb2-c4e6-9f3f32950dba"), new Guid("7aa0a8e6-fc98-de28-1ded-553cac8eb425"), "General Awareness" },
                    { new Guid("a8310bb0-30f7-59aa-4793-e850695b69bd"), new Guid("140935c2-6291-87af-2235-b79257104223"), "General Awareness" },
                    { new Guid("a8debe0a-0d78-d1f1-bcd3-3b60b0dced1b"), new Guid("fb34e05b-9dd2-11d1-509f-35ff00f7b2fd"), "English" },
                    { new Guid("acca5304-c9b6-1e28-83e3-9418b80c1e6b"), new Guid("bc843aea-4f35-9b06-3f66-97614da52ecc"), "General Awareness" },
                    { new Guid("af6317d8-4f29-7dfd-c1c5-a4049d275f5e"), new Guid("660bf600-82b9-241d-6d7b-92ebc1a6c74b"), "English" },
                    { new Guid("b16535b0-614d-56cf-6642-6a923a5dd392"), new Guid("4b998bea-363d-ad7a-0762-1ef391ab3879"), "General Awareness" },
                    { new Guid("b4166db5-ba06-8de6-35b8-16212d900202"), new Guid("fb34e05b-9dd2-11d1-509f-35ff00f7b2fd"), "General Awareness" },
                    { new Guid("b490c3cf-581e-aeff-e211-ff42df02629d"), new Guid("660bf600-82b9-241d-6d7b-92ebc1a6c74b"), "General Awareness" },
                    { new Guid("b4f4bfc1-87b3-db9d-6a37-509f32357111"), new Guid("9002fb31-c048-fed1-a656-82924be87ad4"), "General Awareness" },
                    { new Guid("b52fdd07-25c4-2456-59fb-fe754430b45d"), new Guid("0109ec90-48c8-fe15-3bbd-b0d0aea6002a"), "General Awareness" },
                    { new Guid("b6ca55cb-52d9-e35b-dfa2-31a59bccc342"), new Guid("bc909a90-7e7b-96fe-431f-9e4e073f5371"), "Reasoning" },
                    { new Guid("bb1e9892-8176-5fb1-bf07-97c46b34100a"), new Guid("2fc9f791-98f8-764e-425b-2db2e475e097"), "English" },
                    { new Guid("bd15903d-a370-057d-93a0-e5a6d37c5da2"), new Guid("eb0bce1d-2230-8142-e37b-120396c1167d"), "Quantitative Aptitude" },
                    { new Guid("be180ffa-7d82-cf64-ef60-02f431e2a51b"), new Guid("53039780-1877-af50-8ae1-cd0d62ac09cb"), "English" },
                    { new Guid("bf8a799a-ce3c-e1be-0d3c-dee172a73ab7"), new Guid("573971e6-54de-cf2b-01b1-6b00fbd3beac"), "English" },
                    { new Guid("bfee3ae6-d5d6-961b-60fb-6bf1d5b8710f"), new Guid("02649287-aaf7-1427-3199-e9c77d3d995c"), "Reasoning" },
                    { new Guid("c20816a0-b9cf-d402-831c-ba498ec540f9"), new Guid("2fc9f791-98f8-764e-425b-2db2e475e097"), "Reasoning" },
                    { new Guid("c32886cb-768d-7fd9-6835-1ffdb5acc3fe"), new Guid("75477ff3-5e3a-f204-bfaf-867f5e8b8c8e"), "English" },
                    { new Guid("c36cae96-b91f-e4cb-756c-1fd7d8fb219d"), new Guid("7aa0a8e6-fc98-de28-1ded-553cac8eb425"), "Reasoning" },
                    { new Guid("c3e0cacd-ad3d-f8bd-26e3-9c10f6ee4640"), new Guid("7e81c179-2df8-07f4-0e53-8a622b5f17b3"), "General Awareness" },
                    { new Guid("c49953ee-92c5-9df9-4540-39edf8bfac34"), new Guid("b40cd678-0fda-0b80-f6cb-da6f67528f17"), "Quantitative Aptitude" },
                    { new Guid("c5d5cec4-aac8-73c0-3c4c-8dbd6c3c9648"), new Guid("0716af56-8176-1a84-63ff-9c6a32622a47"), "English" },
                    { new Guid("c61f716f-e12b-56f1-0c31-327b68a1d74e"), new Guid("140935c2-6291-87af-2235-b79257104223"), "English" },
                    { new Guid("c63a3fe7-693e-4566-3760-572ad7dca8af"), new Guid("94e66efc-b100-6412-161e-6f98e932dcd1"), "Quantitative Aptitude" },
                    { new Guid("c67a192c-593e-5ad9-5b5a-d04633886269"), new Guid("a74e2eea-17d5-0f84-16b6-e8c20028cf92"), "General Awareness" },
                    { new Guid("c6e37724-21e2-8f38-96a9-202971a82ef3"), new Guid("660bf600-82b9-241d-6d7b-92ebc1a6c74b"), "Quantitative Aptitude" },
                    { new Guid("c9d07602-cc8d-5173-33d0-77c500336aad"), new Guid("3db7f32f-cbf5-62ce-5b13-082a0040630d"), "Reasoning" },
                    { new Guid("c9e63985-7226-db07-0a90-f2afff9e4532"), new Guid("955330fb-c23a-6121-f52c-5148ae7c8a9d"), "Quantitative Aptitude" },
                    { new Guid("cab2a243-5606-3f5c-4a75-bdc6bd08b177"), new Guid("3db7f32f-cbf5-62ce-5b13-082a0040630d"), "General Awareness" },
                    { new Guid("cba95b28-a792-279a-eebf-0931e088750d"), new Guid("d95a0b78-89e3-faff-2356-5f568460e1e4"), "English" },
                    { new Guid("cc6f069c-72e0-2320-6097-e8d0a82c0af3"), new Guid("75477ff3-5e3a-f204-bfaf-867f5e8b8c8e"), "General Awareness" },
                    { new Guid("cdf661da-b824-ace3-cdc1-9226ffa65710"), new Guid("bc909a90-7e7b-96fe-431f-9e4e073f5371"), "General Awareness" },
                    { new Guid("cf0ceb04-8549-49d0-9e2a-0d3c138d8a3b"), new Guid("33aaa431-3499-88ad-ec42-3e68799bbc41"), "Reasoning" },
                    { new Guid("cf43bf94-c993-8ac9-c2fa-2107009d615b"), new Guid("eb0bce1d-2230-8142-e37b-120396c1167d"), "Reasoning" },
                    { new Guid("cf584bf4-6877-b592-3b0e-cf2de432ab68"), new Guid("a3ea00f3-2d2f-4107-d4dc-8e01a1a7c1e8"), "Quantitative Aptitude" },
                    { new Guid("cfe68c14-de43-9914-1f1b-5cfec539a364"), new Guid("0109ec90-48c8-fe15-3bbd-b0d0aea6002a"), "Reasoning" },
                    { new Guid("d2f289ff-4632-f941-4428-3be85994e398"), new Guid("d95a0b78-89e3-faff-2356-5f568460e1e4"), "Quantitative Aptitude" },
                    { new Guid("d4e88339-8242-61dd-1109-5c9b8c3239ec"), new Guid("a3ea00f3-2d2f-4107-d4dc-8e01a1a7c1e8"), "General Awareness" },
                    { new Guid("d82ee6fe-0ae5-5559-d306-2dc93913f7b9"), new Guid("20e298fd-e875-24aa-2aba-f154dae969cf"), "General Awareness" },
                    { new Guid("d941bee7-d351-3aba-8b25-87c9f3a990ec"), new Guid("617afb6b-29bd-edd9-013b-2ede179e112c"), "English" },
                    { new Guid("da9b8a63-d9d3-3904-7359-949ef4d1459d"), new Guid("9002fb31-c048-fed1-a656-82924be87ad4"), "Reasoning" },
                    { new Guid("dedccbd1-0495-d064-9dd2-9bb2e272e966"), new Guid("ae84e8e7-10d0-b1d1-aee9-a180a547d38d"), "General Awareness" },
                    { new Guid("df3fcbcc-92eb-58b6-723d-d8b54fa4ab6c"), new Guid("a44acf4a-c0ef-33bc-6991-fbb580912bb9"), "General Awareness" },
                    { new Guid("df7000e9-4ce7-789f-6611-ea0bbc407cbe"), new Guid("7e81c179-2df8-07f4-0e53-8a622b5f17b3"), "Reasoning" },
                    { new Guid("e539d4f4-ed7f-2082-a12c-b64118823689"), new Guid("53039780-1877-af50-8ae1-cd0d62ac09cb"), "Reasoning" },
                    { new Guid("e6059fdb-adc3-7728-fa78-d089919f5f5e"), new Guid("7e81c179-2df8-07f4-0e53-8a622b5f17b3"), "English" },
                    { new Guid("e646bc26-8933-29ad-2167-39b0c2be0883"), new Guid("0716af56-8176-1a84-63ff-9c6a32622a47"), "Quantitative Aptitude" },
                    { new Guid("e8adf44e-22ea-249b-42e4-2f7855ddf194"), new Guid("4b998bea-363d-ad7a-0762-1ef391ab3879"), "Quantitative Aptitude" },
                    { new Guid("eafbcc0a-5071-d42c-677a-58fd0374cfba"), new Guid("cf818614-e920-ee64-2bfd-df896886f307"), "English" },
                    { new Guid("f13e3c99-cdf7-5843-f4b3-90e1c409402b"), new Guid("b40cd678-0fda-0b80-f6cb-da6f67528f17"), "General Awareness" },
                    { new Guid("f1c3656d-72d3-0b64-1882-cd41023be0eb"), new Guid("0716af56-8176-1a84-63ff-9c6a32622a47"), "General Awareness" },
                    { new Guid("f2814268-ca6c-b23b-582b-d2698cd4c0ec"), new Guid("af15cbbe-31cf-9016-fc81-b4e041d39025"), "English" },
                    { new Guid("f528c47d-c50a-1954-7b5b-647d28e075f2"), new Guid("b40cd678-0fda-0b80-f6cb-da6f67528f17"), "Reasoning" },
                    { new Guid("f7d81f71-e69b-1efd-a0dc-7600bce80b21"), new Guid("beb716b4-7120-d231-cb9f-033720fc5ba4"), "Reasoning" },
                    { new Guid("f8eb512b-7061-90ee-218d-f79f85013f49"), new Guid("140935c2-6291-87af-2235-b79257104223"), "Quantitative Aptitude" },
                    { new Guid("fb9a1cab-1799-1c4f-6d6b-8ddcfff1099d"), new Guid("4b998bea-363d-ad7a-0762-1ef391ab3879"), "Reasoning" },
                    { new Guid("fc9e5144-f3e5-f26c-a6b1-3a80f0684fc2"), new Guid("660bf600-82b9-241d-6d7b-92ebc1a6c74b"), "Reasoning" },
                    { new Guid("fd19c7c8-1b31-50de-adc4-97bca17deb85"), new Guid("dc88fe6e-6268-f8f7-5347-9c8466574da4"), "Quantitative Aptitude" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Name", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("0020557e-b5f1-6bb6-fee4-2f5b4968a363"), "Reading Comprehension", new Guid("99d290b8-8163-35de-4061-d29f52183491") },
                    { new Guid("00547c11-344b-0af2-52a5-3be310ba9d26"), "Coding-Decoding", new Guid("c20816a0-b9cf-d402-831c-ba498ec540f9") },
                    { new Guid("016f0210-152a-6759-bd94-81314f14afaf"), "Vocabulary", new Guid("43cc05df-90fb-d3a9-3329-e42168f33a77") },
                    { new Guid("023f9096-fd2e-f857-e2cc-9a880c2c296e"), "Logical Reasoning", new Guid("c9d07602-cc8d-5173-33d0-77c500336aad") },
                    { new Guid("02bf3d63-bebc-c5b0-03a8-d0532bed47db"), "Geometry", new Guid("3249993c-ee08-0d52-5e33-5def559739ce") },
                    { new Guid("0388eb3d-bffc-1410-8276-7424daf2e142"), "Error Detection", new Guid("239d45ea-f0cb-4232-5fea-bd7c5cbe3ab3") },
                    { new Guid("039b78ac-dd85-2bba-25c3-14dbe08bd925"), "Vocabulary", new Guid("64eda4df-4f57-2a64-b542-f9d8d2463c0d") },
                    { new Guid("0417b97f-83f2-b62a-d1b8-021962a1b755"), "Coding-Decoding", new Guid("82df9511-fd16-f5b9-3da2-d877a07ace7c") },
                    { new Guid("04d062ed-6b33-b892-beb1-05e9ce3f77e0"), "Geography", new Guid("42599220-bbb4-8c25-dfa1-f63ee080c8dd") },
                    { new Guid("05252edf-ad98-2a1c-19af-8441bfd62c98"), "History", new Guid("b4f4bfc1-87b3-db9d-6a37-509f32357111") },
                    { new Guid("05d7968c-c16f-dde5-c9e5-38c457b84203"), "Static GK", new Guid("a6f8ba35-9571-9bb2-c4e6-9f3f32950dba") },
                    { new Guid("05e64683-c8fc-a6f2-87fc-7b5edab79cc1"), "History", new Guid("b490c3cf-581e-aeff-e211-ff42df02629d") },
                    { new Guid("06afd08b-21c4-6e0d-c2b2-245263d4ea29"), "Geometry", new Guid("f8eb512b-7061-90ee-218d-f79f85013f49") },
                    { new Guid("070156af-351e-7bee-cb82-c3985ae8095b"), "Series", new Guid("61f44871-630e-0b34-8fad-ca4cc042320b") },
                    { new Guid("070af5f2-5203-ddab-1d55-67b08af245c8"), "Percentage", new Guid("c63a3fe7-693e-4566-3760-572ad7dca8af") },
                    { new Guid("076c80d2-4cb3-b427-3ac7-a0ab9f712e5c"), "Coding-Decoding", new Guid("8dd5c1f6-3406-e8a6-de08-130071a44989") },
                    { new Guid("076d451f-955a-6e67-f800-555d26e44e96"), "Vocabulary", new Guid("5c99f7b3-af3e-e99e-0824-9d55cbc055a1") },
                    { new Guid("07a17c1a-fc7e-1112-f9ac-b943fe7df2f0"), "Percentage", new Guid("7ae414ef-143b-4d3e-20b1-ea9fcc461547") },
                    { new Guid("08ff95e0-498c-aeaa-c925-b42fe45c30a1"), "Error Detection", new Guid("94994903-a722-d664-d90f-c27454228d9a") },
                    { new Guid("0900e16e-3be5-7654-d6e0-1cccee63a58e"), "Error Detection", new Guid("f2814268-ca6c-b23b-582b-d2698cd4c0ec") },
                    { new Guid("0a40b888-a101-9d09-104e-9951227f3927"), "Reading Comprehension", new Guid("bb1e9892-8176-5fb1-bf07-97c46b34100a") },
                    { new Guid("0b516c63-1284-eade-7ef0-3b51d5e29e91"), "Coding-Decoding", new Guid("77075acf-29eb-d1e7-f247-9b164a9d9b87") },
                    { new Guid("0bd26f1f-f089-6bfd-2b9c-2c7be96bf00e"), "Coding-Decoding", new Guid("f7d81f71-e69b-1efd-a0dc-7600bce80b21") },
                    { new Guid("0c1669e4-9edd-25e2-9f42-7e1dc7194e33"), "Static GK", new Guid("acca5304-c9b6-1e28-83e3-9418b80c1e6b") },
                    { new Guid("0cc7cecf-db30-2ccc-045d-72290f703606"), "Vocabulary", new Guid("3e6077cf-a585-a5b4-8058-0ab46cef9880") },
                    { new Guid("0d057670-4728-0b13-5614-fd0f65f08aa0"), "Vocabulary", new Guid("c5d5cec4-aac8-73c0-3c4c-8dbd6c3c9648") },
                    { new Guid("0d703533-000d-571a-73b9-c703c4e2dfa8"), "Geometry", new Guid("cf584bf4-6877-b592-3b0e-cf2de432ab68") },
                    { new Guid("0df8fafc-cac3-be62-19b6-8d3b433eff07"), "Vocabulary", new Guid("af6317d8-4f29-7dfd-c1c5-a4049d275f5e") },
                    { new Guid("0ecd4150-776d-900a-7364-84d2b0a5775c"), "Geometry", new Guid("49d1d6ba-8541-3024-5413-135b5663003c") },
                    { new Guid("0ffec92c-fc83-93ea-e118-9781baad8a95"), "Geometry", new Guid("69c2b215-e4dd-c22f-48ba-6affabc79966") },
                    { new Guid("10f93e20-c92d-dee1-218a-b3fd666b3cf1"), "Static GK", new Guid("48f0f198-01af-1a80-8f50-aacffbb6440e") },
                    { new Guid("11ece3ee-ba8d-7dfa-9010-496044a83bc8"), "Coding-Decoding", new Guid("6dfdb9f3-fcc7-d8cd-9bb7-4b366d335b84") },
                    { new Guid("12bdd0b0-f774-9ef6-fdae-aeef606d13fc"), "Static GK", new Guid("47bc02e3-9998-79d2-ca9c-e4c3b751689e") },
                    { new Guid("13a241bb-9571-c186-4fee-b655921ff308"), "Coding-Decoding", new Guid("e539d4f4-ed7f-2082-a12c-b64118823689") },
                    { new Guid("13b7a905-9bb7-57d4-e846-17f1ebafe873"), "Series", new Guid("fb9a1cab-1799-1c4f-6d6b-8ddcfff1099d") },
                    { new Guid("14711777-0e05-f8b0-9306-ea7b2717069d"), "History", new Guid("b16535b0-614d-56cf-6642-6a923a5dd392") },
                    { new Guid("148e2f56-266b-d84f-3b55-bd0a320dff18"), "Geography", new Guid("7dac52a4-15f6-e83e-3143-62647c5adf4c") },
                    { new Guid("1701ff6a-ccf2-150b-914c-cb0a12668d04"), "Reading Comprehension", new Guid("0aefdf79-fe7f-9bdc-e978-fed568e3b1d4") },
                    { new Guid("1794dff6-7046-449b-608a-20c7971d2730"), "Vocabulary", new Guid("30ae9e12-3ceb-88c9-5e54-a00e67f69df4") },
                    { new Guid("17f504a3-6345-90ea-dcaa-7984e94bd901"), "Geometry", new Guid("94ae42f3-35b8-aa97-694c-77c3df885c64") },
                    { new Guid("1839ce22-a7de-a5d9-635c-4386ee086755"), "Series", new Guid("60150aea-1845-3cb3-002d-8c8b1e3eddd7") },
                    { new Guid("18f70d11-99e6-364f-12cb-493618fa6857"), "Static GK", new Guid("5392ef0e-3dbf-d6d5-8adc-9f074c7b0f3e") },
                    { new Guid("18f865b3-ca5d-ebb3-04a1-1838edb3a527"), "Static GK", new Guid("dedccbd1-0495-d064-9dd2-9bb2e272e966") },
                    { new Guid("19102d4b-8230-9a43-cda3-176b9f23527e"), "Percentage", new Guid("0dfd2625-89ac-f13e-6b38-e3c0d8d0a2c4") },
                    { new Guid("1984d93e-8494-5993-a8a5-08fa1ab91422"), "Error Detection", new Guid("3e6077cf-a585-a5b4-8058-0ab46cef9880") },
                    { new Guid("198ddd1b-e34e-92c7-eb6e-625f894f58bc"), "Reading Comprehension", new Guid("0f9aae8b-5730-91c1-4eba-dcab68ef87d4") },
                    { new Guid("19cbdf30-fbd5-0ab9-9ca9-95e1a74e4ada"), "Percentage", new Guid("c6e37724-21e2-8f38-96a9-202971a82ef3") },
                    { new Guid("19fbb948-d4d9-d8ac-ad31-b13a9984ec6a"), "Coding-Decoding", new Guid("68d95e07-877c-0cb9-0c71-381742807ddb") },
                    { new Guid("1a28d6ef-62de-3aa3-6c3c-3e01f0c72ea5"), "Geography", new Guid("0edd4df9-10c6-ab16-4384-f3cc03c77e3e") },
                    { new Guid("1b563258-3a47-7bb0-4412-fe4c14fa6ffa"), "Percentage", new Guid("cf584bf4-6877-b592-3b0e-cf2de432ab68") },
                    { new Guid("1b671202-02e1-5a2f-ce13-92046503480f"), "Series", new Guid("b6ca55cb-52d9-e35b-dfa2-31a59bccc342") },
                    { new Guid("1bbeaaef-1ba5-d95b-b66f-9ac92773952c"), "Percentage", new Guid("d2f289ff-4632-f941-4428-3be85994e398") },
                    { new Guid("1bd8688f-8563-56b2-1882-c7daa80a8ffc"), "Series", new Guid("6e5381c2-aa75-e19e-eac4-a983be83afd5") },
                    { new Guid("1d8dfb5d-b6cf-284b-5fbd-19f95a0b0b72"), "Geometry", new Guid("1fba6d2c-20e5-6bb6-6119-cfefd37d187a") },
                    { new Guid("1e887bad-89b5-11de-2fa0-056bf52b6391"), "Percentage", new Guid("c9e63985-7226-db07-0a90-f2afff9e4532") },
                    { new Guid("1ebdd1ac-f40a-317b-bc32-12f99ee147e4"), "History", new Guid("cdf661da-b824-ace3-cdc1-9226ffa65710") },
                    { new Guid("1f0a75d3-3b3d-b92a-68d3-8d1dfb324f06"), "History", new Guid("7dac52a4-15f6-e83e-3143-62647c5adf4c") },
                    { new Guid("1f2ccbde-2e10-f388-7da9-fe8f6e9c6164"), "Geometry", new Guid("26aeaf8e-cccf-0ceb-5682-ff5da0b6d543") },
                    { new Guid("1f4672c4-9cfd-c589-0191-00dbc13fd485"), "Logical Reasoning", new Guid("fc9e5144-f3e5-f26c-a6b1-3a80f0684fc2") },
                    { new Guid("206e379a-e110-5339-ab8c-91c79c18fc1e"), "Coding-Decoding", new Guid("c36cae96-b91f-e4cb-756c-1fd7d8fb219d") },
                    { new Guid("2148a4f8-f077-633a-41c6-4ac1f367e046"), "Vocabulary", new Guid("0e77bff5-deff-a943-0610-fd871552ad7b") },
                    { new Guid("21ceccef-bf23-3bf4-a5d5-ac5afc49a82d"), "Percentage", new Guid("2df66c1d-c139-3764-c529-61c645d4245b") },
                    { new Guid("2233ea37-6693-1d5b-44b0-1850f1c92415"), "Geography", new Guid("5c4b02f0-6369-076a-31b2-4e64db0b316f") },
                    { new Guid("223b73ed-a165-125e-5054-b8a7600774fc"), "Profit and Loss", new Guid("1fba6d2c-20e5-6bb6-6119-cfefd37d187a") },
                    { new Guid("2297b959-b0c8-9497-93db-f2d669b7f93a"), "Percentage", new Guid("4d5881bc-d0b4-0d9e-4975-1dd816dfd3e5") },
                    { new Guid("230bd032-c13f-4759-c6b1-42e062c7993a"), "Series", new Guid("68d95e07-877c-0cb9-0c71-381742807ddb") },
                    { new Guid("23670d1d-8303-2b69-60cb-dbac6d52bf48"), "Geography", new Guid("4eeb5fcc-8d45-97a3-9f21-8838d2db2c9b") },
                    { new Guid("23eb6b63-8cdd-f2e1-a76d-396f74847377"), "Logical Reasoning", new Guid("92f3676e-2bdc-7c39-31ba-7e057faaac57") },
                    { new Guid("25f92134-e11b-f21c-7af2-70644fc42de4"), "Error Detection", new Guid("bf8a799a-ce3c-e1be-0d3c-dee172a73ab7") },
                    { new Guid("262c0186-dd70-44b2-1e38-a91523ca2786"), "Vocabulary", new Guid("8eebe33a-8ce2-3bed-d6d7-55d057d7fff4") },
                    { new Guid("26655a58-b4c6-4440-91a5-a46b4929f13a"), "Logical Reasoning", new Guid("f528c47d-c50a-1954-7b5b-647d28e075f2") },
                    { new Guid("2671146a-2491-22d0-9bb4-b0cd933aa880"), "Profit and Loss", new Guid("29cbfd96-bd12-d1e4-10ad-43a1fecbfe56") },
                    { new Guid("26cd18da-e3c7-6aa6-5217-f878240ddb5f"), "History", new Guid("74afdb9b-0e1c-94b9-765e-d720199d450d") },
                    { new Guid("2761aba9-01e7-8b00-e019-e888dc919892"), "Percentage", new Guid("1fba6d2c-20e5-6bb6-6119-cfefd37d187a") },
                    { new Guid("284fb7e8-0ef0-2b69-e5a3-c29f91c4d5cc"), "Static GK", new Guid("a8310bb0-30f7-59aa-4793-e850695b69bd") },
                    { new Guid("28afcbc1-4187-8994-432a-565cc1f34e13"), "Error Detection", new Guid("0426bd63-650d-39e6-ba91-32a1bf2c0188") },
                    { new Guid("28f095f8-9209-3243-653f-92ab380eced7"), "Percentage", new Guid("93b5cded-e275-4edf-d41c-341fb7954bda") },
                    { new Guid("29eea3a2-3ec8-77e7-78ce-4df6b8edce53"), "Percentage", new Guid("e646bc26-8933-29ad-2167-39b0c2be0883") },
                    { new Guid("2abf6ded-d3ee-8ad6-a714-8301f059b975"), "Percentage", new Guid("60d5154d-3d43-60dd-b21d-0943056545bd") },
                    { new Guid("2b11c80f-a526-cbd8-869e-2e046aee50c8"), "Vocabulary", new Guid("94994903-a722-d664-d90f-c27454228d9a") },
                    { new Guid("2b2076c7-1515-3c2d-db52-246842e624b5"), "Error Detection", new Guid("0e77bff5-deff-a943-0610-fd871552ad7b") },
                    { new Guid("2bf10c64-d854-3017-fee4-cd9a406b5e51"), "Coding-Decoding", new Guid("7f5ddba9-3e27-eeb8-eb18-e306a8d57622") },
                    { new Guid("2ef53bca-7e40-dce1-d325-d7c64174fdef"), "Reading Comprehension", new Guid("79200f88-1d99-cfa8-0aa9-2b2077af2f6a") },
                    { new Guid("2f4852df-cfa0-bf58-8cfb-89cd7a03403e"), "Logical Reasoning", new Guid("a0d8924d-729a-94ff-b85a-f00daaf488fe") },
                    { new Guid("2f547303-89e6-50ea-b714-3683f4288b55"), "Profit and Loss", new Guid("0dfd2625-89ac-f13e-6b38-e3c0d8d0a2c4") },
                    { new Guid("2f61f563-9925-3be3-0e83-b3e528501517"), "Profit and Loss", new Guid("4d5881bc-d0b4-0d9e-4975-1dd816dfd3e5") },
                    { new Guid("30e6f736-8354-9404-5edc-e99cb072a148"), "Geometry", new Guid("59a1391d-7a39-5360-9fb3-832bdb2cac79") },
                    { new Guid("310db2c4-282d-346e-cb7e-d963a2da2bdc"), "Series", new Guid("da9b8a63-d9d3-3904-7359-949ef4d1459d") },
                    { new Guid("311422fe-4b39-4e72-3690-91891772c0c7"), "Reading Comprehension", new Guid("4773c0d2-b3ca-c94d-9849-c745b4f3dba8") },
                    { new Guid("31d74296-2f88-7948-399e-27f2c004796f"), "Geometry", new Guid("2df66c1d-c139-3764-c529-61c645d4245b") },
                    { new Guid("323829d9-3aea-4ec3-8318-5fdd60e8d5f2"), "Static GK", new Guid("3ebe7357-d069-0030-d517-fbd60e30a263") },
                    { new Guid("32d35706-374b-3367-6f74-3aa97a51e3a3"), "History", new Guid("482cd3d1-1df4-67f3-a3bd-b273d9892ab8") },
                    { new Guid("32e3afa1-0b66-12de-f986-e64aecb89cc8"), "Static GK", new Guid("b490c3cf-581e-aeff-e211-ff42df02629d") },
                    { new Guid("33c34be1-a238-6051-bace-6370a7e8441b"), "Static GK", new Guid("49d6e9c7-e86b-daf0-0a22-16cfa49fc212") },
                    { new Guid("3531c4ff-8e05-fe7d-3d51-c7e8c15771d9"), "Percentage", new Guid("0c484af1-d5d5-453c-06a7-067c5d5a87fb") },
                    { new Guid("35be9b6d-668e-95a0-180c-b032dc82dd01"), "Logical Reasoning", new Guid("8910836e-bf37-7b12-dfcd-fc7cb4ef7495") },
                    { new Guid("35c3d728-b008-383a-02ac-ddd1048e7127"), "Static GK", new Guid("b4f4bfc1-87b3-db9d-6a37-509f32357111") },
                    { new Guid("36fef8c4-63b6-c0c0-ea66-9c3c6c6bf3a8"), "Error Detection", new Guid("8fc32dcf-6b85-d7b7-c9ee-dfeb8347435a") },
                    { new Guid("37595b12-00d3-f632-94ba-2123a9ac8a42"), "Geography", new Guid("b52fdd07-25c4-2456-59fb-fe754430b45d") },
                    { new Guid("37a4d06e-f4cf-50f2-1d64-750617551c03"), "Static GK", new Guid("cdf661da-b824-ace3-cdc1-9226ffa65710") },
                    { new Guid("3960d4f3-0854-f87f-77d2-c76e0da61ea4"), "Geometry", new Guid("55e88ad4-b31c-ecca-8211-a5d39afca9c1") },
                    { new Guid("3979a51e-a640-28d1-fa46-4f1b63829cea"), "Coding-Decoding", new Guid("8910836e-bf37-7b12-dfcd-fc7cb4ef7495") },
                    { new Guid("39e92cbe-4a6e-03b8-fb42-9ebd025146d9"), "Profit and Loss", new Guid("22ed3e15-caf6-1025-37b9-8459eaae6fe4") },
                    { new Guid("3b6ec320-289b-b75f-d1c2-8eb99c18210f"), "Geography", new Guid("df3fcbcc-92eb-58b6-723d-d8b54fa4ab6c") },
                    { new Guid("3baa284d-bac9-d7cb-c8ce-f89d52a046b6"), "Reading Comprehension", new Guid("c61f716f-e12b-56f1-0c31-327b68a1d74e") },
                    { new Guid("3d1ebe89-ba46-d97b-ff5d-eddaa164de02"), "Series", new Guid("77075acf-29eb-d1e7-f247-9b164a9d9b87") },
                    { new Guid("3dc2f1b9-abab-71ec-93a8-d5cba8f4b8b0"), "History", new Guid("3ebe7357-d069-0030-d517-fbd60e30a263") },
                    { new Guid("3f3c8e1e-16c6-9a61-d2b3-16be64a0e0eb"), "Logical Reasoning", new Guid("7f5ddba9-3e27-eeb8-eb18-e306a8d57622") },
                    { new Guid("4037bed9-8995-7325-4292-79b468e15b05"), "Vocabulary", new Guid("3206020e-1e1a-d766-d66b-6d604baf5616") },
                    { new Guid("4082415b-2834-6ee9-ec19-c7f18661e746"), "Error Detection", new Guid("0aefdf79-fe7f-9bdc-e978-fed568e3b1d4") },
                    { new Guid("40a9a61c-ebe0-9d28-23be-1f2d944eab47"), "Geography", new Guid("b16535b0-614d-56cf-6642-6a923a5dd392") },
                    { new Guid("41e6ee2b-e131-3815-a87a-78a7970602c7"), "Geography", new Guid("d82ee6fe-0ae5-5559-d306-2dc93913f7b9") },
                    { new Guid("42dc6353-17ab-6fa9-4ceb-d9f601cbfd75"), "Profit and Loss", new Guid("7ae414ef-143b-4d3e-20b1-ea9fcc461547") },
                    { new Guid("438fc7b3-cd6d-8403-f243-804c09e9e6ad"), "Series", new Guid("0886696f-48c8-891e-a7d5-8550fb0d7021") },
                    { new Guid("4454109a-447d-076b-033c-64c56705ac45"), "Profit and Loss", new Guid("3249993c-ee08-0d52-5e33-5def559739ce") },
                    { new Guid("449914f8-fe2e-e4d2-0045-3b38245f48ee"), "Static GK", new Guid("c67a192c-593e-5ad9-5b5a-d04633886269") },
                    { new Guid("451418c0-88ab-7e31-625e-766ac0d86f35"), "Static GK", new Guid("0edd4df9-10c6-ab16-4384-f3cc03c77e3e") },
                    { new Guid("45cf0714-da47-5256-8013-7cf73376dab7"), "History", new Guid("df3fcbcc-92eb-58b6-723d-d8b54fa4ab6c") },
                    { new Guid("45f3bc3a-70cd-a8cf-0f47-9681fe9ae2fb"), "Static GK", new Guid("7fdd795a-6994-63d6-fd5a-9372678e5143") },
                    { new Guid("468376f5-6597-b1c1-9463-8d0d39c873f7"), "Error Detection", new Guid("c5d5cec4-aac8-73c0-3c4c-8dbd6c3c9648") },
                    { new Guid("4779d2f3-f910-30ec-daf3-4b26c7622237"), "Reading Comprehension", new Guid("0426bd63-650d-39e6-ba91-32a1bf2c0188") },
                    { new Guid("48014a2e-25a7-149c-5485-a3dd2ec9e54e"), "Vocabulary", new Guid("e6059fdb-adc3-7728-fa78-d089919f5f5e") },
                    { new Guid("486f0008-a23e-5d47-13b0-0c67d8d09e75"), "Static GK", new Guid("cc6f069c-72e0-2320-6097-e8d0a82c0af3") },
                    { new Guid("48b7fb14-9557-e6ba-210e-3fb578572e3d"), "Vocabulary", new Guid("d941bee7-d351-3aba-8b25-87c9f3a990ec") },
                    { new Guid("49f0b26a-dd5c-2e1c-7c67-160dbacac9a4"), "Vocabulary", new Guid("99d290b8-8163-35de-4061-d29f52183491") },
                    { new Guid("49fe9985-def7-833f-a1a1-9e24dc9dfe77"), "Error Detection", new Guid("99d290b8-8163-35de-4061-d29f52183491") },
                    { new Guid("4b00ac95-425b-37aa-733e-e5438f1da1be"), "Percentage", new Guid("21b78efb-e9e3-8cc5-1d58-8cc5510842a7") },
                    { new Guid("4c3fbb73-e972-1335-b63a-51dfb458ef9a"), "Logical Reasoning", new Guid("f7d81f71-e69b-1efd-a0dc-7600bce80b21") },
                    { new Guid("4ca39f7f-ad2d-fd8a-3b41-7ba82d6d5857"), "Geography", new Guid("7fdd795a-6994-63d6-fd5a-9372678e5143") },
                    { new Guid("4cace7fb-31ca-d19d-814b-e4273b66b21a"), "Series", new Guid("df7000e9-4ce7-789f-6611-ea0bbc407cbe") },
                    { new Guid("4cb37820-828c-f5a1-ad9b-96a798069449"), "Percentage", new Guid("04e69404-9b8a-d643-b48a-8710d2f73d29") },
                    { new Guid("4e6f9ce2-c712-cd86-1ee4-3bfacd55d428"), "Percentage", new Guid("69c2b215-e4dd-c22f-48ba-6affabc79966") },
                    { new Guid("4ea4ec87-9da7-055a-4b7b-06fdc5fc9e01"), "Static GK", new Guid("b52fdd07-25c4-2456-59fb-fe754430b45d") },
                    { new Guid("4ee520bf-11fb-00c8-4322-65266ee4394a"), "Error Detection", new Guid("85986333-45ff-1990-65d6-97248deb6d08") },
                    { new Guid("4f23ff9e-a5af-d4bd-f20d-e4fff6d094d2"), "Series", new Guid("cfe68c14-de43-9914-1f1b-5cfec539a364") },
                    { new Guid("4f8b33d8-2c1c-366b-1549-a62f5f24a15a"), "Error Detection", new Guid("3206020e-1e1a-d766-d66b-6d604baf5616") },
                    { new Guid("500fe9fb-1828-0563-010d-daacd496bbbb"), "Reading Comprehension", new Guid("e6059fdb-adc3-7728-fa78-d089919f5f5e") },
                    { new Guid("5040e040-7952-6808-4bf0-0e0317db20cb"), "Geometry", new Guid("22ed3e15-caf6-1025-37b9-8459eaae6fe4") },
                    { new Guid("504f2fd3-31fc-135e-00eb-46937330b1b0"), "History", new Guid("205fdf02-2719-c1b7-be92-666228b6e3e7") },
                    { new Guid("513bbc7a-a2f2-c2fb-755b-16ec9b22f34b"), "Percentage", new Guid("9bc50977-c5e3-4eea-99fd-100bcca73458") },
                    { new Guid("51b42134-f203-6da5-94bc-1b34903e8015"), "Logical Reasoning", new Guid("77075acf-29eb-d1e7-f247-9b164a9d9b87") },
                    { new Guid("52698647-866e-e682-b9c2-930c65b83d0c"), "Logical Reasoning", new Guid("cfe68c14-de43-9914-1f1b-5cfec539a364") },
                    { new Guid("532ea53d-1a89-84c8-4393-0bf54bff705d"), "Reading Comprehension", new Guid("c32886cb-768d-7fd9-6835-1ffdb5acc3fe") },
                    { new Guid("534f19a8-186f-ab39-fbb6-4dd7992c1a21"), "Logical Reasoning", new Guid("633b28ab-6d5a-dd92-ce09-65145ac09269") },
                    { new Guid("53afed7d-878e-7c84-85a9-8db50acadf02"), "Error Detection", new Guid("48edbf68-6220-167c-9c06-21954ca206e9") },
                    { new Guid("542d4779-0685-3b1e-94df-9cd2fd431dec"), "Vocabulary", new Guid("c32886cb-768d-7fd9-6835-1ffdb5acc3fe") },
                    { new Guid("5485136f-ec4f-7bdf-ec2f-1c053ef69073"), "Vocabulary", new Guid("0426bd63-650d-39e6-ba91-32a1bf2c0188") },
                    { new Guid("54c1179c-e6e4-cd62-3da6-0830f1e60958"), "Coding-Decoding", new Guid("cf43bf94-c993-8ac9-c2fa-2107009d615b") },
                    { new Guid("55110bc5-8813-bdd4-834c-1b4032be4aa5"), "Coding-Decoding", new Guid("a0d8924d-729a-94ff-b85a-f00daaf488fe") },
                    { new Guid("554ced57-e75a-316c-e8a7-daef8b827010"), "Geography", new Guid("d4e88339-8242-61dd-1109-5c9b8c3239ec") },
                    { new Guid("575ea5f9-9a38-042d-fde8-c6fc0f2d7214"), "Error Detection", new Guid("8ac4ada6-4775-eeb0-d87f-32383572fbe6") },
                    { new Guid("5767400a-ad0c-7dac-fe48-47e68f474255"), "Static GK", new Guid("096355bb-6cb5-8220-abc3-6fe02899a819") },
                    { new Guid("57742d5c-f9ed-798e-acbd-35fcbff1b42a"), "Vocabulary", new Guid("eafbcc0a-5071-d42c-677a-58fd0374cfba") },
                    { new Guid("5775200e-efa7-d1e2-51b1-78e38357afb1"), "Series", new Guid("6dfdb9f3-fcc7-d8cd-9bb7-4b366d335b84") },
                    { new Guid("5801c4ad-fa9b-a470-b6d1-005c9df064c3"), "Geography", new Guid("b490c3cf-581e-aeff-e211-ff42df02629d") },
                    { new Guid("58517477-c68c-8160-efff-f26d801bc616"), "Reading Comprehension", new Guid("85986333-45ff-1990-65d6-97248deb6d08") },
                    { new Guid("58755b7b-fa9e-0700-c12f-28f3e1ac565a"), "Geometry", new Guid("e646bc26-8933-29ad-2167-39b0c2be0883") },
                    { new Guid("58ea3e73-7466-7145-3f70-9bf7352f170d"), "Logical Reasoning", new Guid("e539d4f4-ed7f-2082-a12c-b64118823689") },
                    { new Guid("596c6e87-4b89-eca6-7ffa-db955ceda47a"), "Coding-Decoding", new Guid("60150aea-1845-3cb3-002d-8c8b1e3eddd7") },
                    { new Guid("59bb4c41-6567-6d60-522b-e195fdc9b0ed"), "Reading Comprehension", new Guid("bf8a799a-ce3c-e1be-0d3c-dee172a73ab7") },
                    { new Guid("5a9b577b-7d37-9579-dbb5-d3221f4c50e1"), "Vocabulary", new Guid("30c33b7b-9e1f-487b-fd59-2872a64fe595") },
                    { new Guid("5b7d0fc1-2865-47e5-ca8f-6de4f11f9e71"), "Reading Comprehension", new Guid("f2814268-ca6c-b23b-582b-d2698cd4c0ec") },
                    { new Guid("5bb82a83-c754-29d8-e50b-4660cc3469b4"), "Geometry", new Guid("7e4344c7-802a-1dbd-6a3e-d8b0b12e7666") },
                    { new Guid("5c2f9bfc-292e-40ea-29fc-73e541b21de8"), "Series", new Guid("633b28ab-6d5a-dd92-ce09-65145ac09269") },
                    { new Guid("5ce25ca0-0551-7fae-6a4b-2ffdfa68db42"), "Geography", new Guid("cab2a243-5606-3f5c-4a75-bdc6bd08b177") },
                    { new Guid("5e913ded-9b28-13b3-e1b3-e921909be1da"), "Series", new Guid("93dedde1-bc85-8d54-8ab5-0f407da7fd28") },
                    { new Guid("5f1a9b72-274f-3067-8f24-f553ff282184"), "Profit and Loss", new Guid("e8adf44e-22ea-249b-42e4-2f7855ddf194") },
                    { new Guid("5f689989-a450-f5a4-7ef2-82abe4c0409b"), "Logical Reasoning", new Guid("322fb74b-b528-7a96-495c-39fc1d728a2e") },
                    { new Guid("5f7f1646-fd70-33a1-93f5-5382e4952d02"), "Error Detection", new Guid("8eebe33a-8ce2-3bed-d6d7-55d057d7fff4") },
                    { new Guid("6008d83a-9222-6d06-ae25-a1108012d2eb"), "Reading Comprehension", new Guid("5c99f7b3-af3e-e99e-0824-9d55cbc055a1") },
                    { new Guid("601295d0-a8d8-1c6b-f008-50dea8c4c284"), "Logical Reasoning", new Guid("1a1e72e3-c129-2580-72a0-51e0c5e8c51a") },
                    { new Guid("60276173-13a7-5eb0-28ac-f18c56f96052"), "Reading Comprehension", new Guid("6b36ab48-4a04-61ec-0b7f-57884fea536a") },
                    { new Guid("6064287d-fdfd-c9dc-5526-5f7d40b4a103"), "Geography", new Guid("5392ef0e-3dbf-d6d5-8adc-9f074c7b0f3e") },
                    { new Guid("60930aa4-2100-3f38-ac7f-2306a29afe93"), "Static GK", new Guid("7dac52a4-15f6-e83e-3143-62647c5adf4c") },
                    { new Guid("61f9b508-c846-f39a-21ba-89ddab9ff9ce"), "Geography", new Guid("47bc02e3-9998-79d2-ca9c-e4c3b751689e") },
                    { new Guid("624853aa-8d76-0537-bff3-eb9a19a36c04"), "Error Detection", new Guid("2b57ea8f-d5a6-227e-b079-b81ae68d0267") },
                    { new Guid("62da6521-920e-f77a-9ea9-16671b206fa2"), "Vocabulary", new Guid("f2814268-ca6c-b23b-582b-d2698cd4c0ec") },
                    { new Guid("63708290-96a6-0622-3b81-9d0b21cded94"), "Vocabulary", new Guid("4773c0d2-b3ca-c94d-9849-c745b4f3dba8") },
                    { new Guid("63ddee5b-cf95-3973-bb85-e03759b90b10"), "Logical Reasoning", new Guid("68d95e07-877c-0cb9-0c71-381742807ddb") },
                    { new Guid("64424e9a-e470-3316-a64b-7f98a9d2e156"), "Geometry", new Guid("c9e63985-7226-db07-0a90-f2afff9e4532") },
                    { new Guid("64748b13-ea73-0081-d8d4-3fe196ad4a71"), "Reading Comprehension", new Guid("30b331ed-e430-7bae-e7ce-f589f35517eb") },
                    { new Guid("64798e77-f459-5e76-9480-ad4e9a93d5ee"), "History", new Guid("d4e88339-8242-61dd-1109-5c9b8c3239ec") },
                    { new Guid("65da7b33-7a31-b7b2-82f5-88790496825a"), "Reading Comprehension", new Guid("c5d5cec4-aac8-73c0-3c4c-8dbd6c3c9648") },
                    { new Guid("663ebfc6-ea0e-0aa9-9da0-beec2b5b0b9d"), "Vocabulary", new Guid("0aefdf79-fe7f-9bdc-e978-fed568e3b1d4") },
                    { new Guid("66567745-b21d-c8e8-b3cb-277750ca568b"), "Vocabulary", new Guid("a8debe0a-0d78-d1f1-bcd3-3b60b0dced1b") },
                    { new Guid("68988e3c-4233-1592-320b-ad50095635d7"), "Reading Comprehension", new Guid("3c40893b-b1da-0e77-c6e6-27559f0200bb") },
                    { new Guid("68af1f8b-98f4-d183-fd20-d538d7466d29"), "Coding-Decoding", new Guid("92f3676e-2bdc-7c39-31ba-7e057faaac57") },
                    { new Guid("69636dcf-cc6b-4a9b-0db1-7e6f4045e6a4"), "Geometry", new Guid("c49953ee-92c5-9df9-4540-39edf8bfac34") },
                    { new Guid("699688bb-d10b-e6c4-8a52-a33ed36ee932"), "Percentage", new Guid("22ed3e15-caf6-1025-37b9-8459eaae6fe4") },
                    { new Guid("6a3e0cc2-e69e-523e-ede0-8ec258ae1406"), "Profit and Loss", new Guid("55e88ad4-b31c-ecca-8211-a5d39afca9c1") },
                    { new Guid("6a979cc5-030b-e559-1c68-25240e2f4da3"), "Series", new Guid("92f3676e-2bdc-7c39-31ba-7e057faaac57") },
                    { new Guid("6b4f3203-edac-410c-cb9e-61305f89afd0"), "Percentage", new Guid("55e88ad4-b31c-ecca-8211-a5d39afca9c1") },
                    { new Guid("6b5a4bf7-8bcc-3ab3-e5c0-3bd9836774b7"), "Profit and Loss", new Guid("c9e63985-7226-db07-0a90-f2afff9e4532") },
                    { new Guid("6b6bf92b-1df5-864d-97d1-88739f074ba8"), "Profit and Loss", new Guid("7e4344c7-802a-1dbd-6a3e-d8b0b12e7666") },
                    { new Guid("6c1e090e-2074-be30-6c7b-9ec9429f9791"), "Logical Reasoning", new Guid("82df9511-fd16-f5b9-3da2-d877a07ace7c") },
                    { new Guid("6c2fdb82-16fe-bbaa-8fbe-7fb318189c5a"), "History", new Guid("49d6e9c7-e86b-daf0-0a22-16cfa49fc212") },
                    { new Guid("6d0da312-6ddc-e263-eba1-9b22fdb879ce"), "Logical Reasoning", new Guid("9c860696-a5de-ec1a-83b1-d00dc21e8195") },
                    { new Guid("6d41176f-a0eb-7077-be93-d48b31059c9b"), "History", new Guid("3f9b3521-896b-6a86-c006-f74c596b124d") },
                    { new Guid("6f9a0306-bed4-ea69-1da1-70eca90b33e3"), "Series", new Guid("1a1e72e3-c129-2580-72a0-51e0c5e8c51a") },
                    { new Guid("6ff3112c-8d6c-d72c-009c-63959105f8a4"), "Static GK", new Guid("65e36679-c743-faf2-6191-8a6c5f266f04") },
                    { new Guid("701b4ae9-c09f-ff8f-f224-39910fcdec0d"), "Reading Comprehension", new Guid("30c33b7b-9e1f-487b-fd59-2872a64fe595") },
                    { new Guid("72f33926-d7dc-9879-245e-6d1770f766a3"), "Profit and Loss", new Guid("26aeaf8e-cccf-0ceb-5682-ff5da0b6d543") },
                    { new Guid("743b32f3-9cf1-1a12-d59b-75fda739ad79"), "Profit and Loss", new Guid("68dd4f56-21f3-72c9-4a78-6ac785f4ac6e") },
                    { new Guid("744159b6-4154-4565-bbcd-ba643cbda6b5"), "Static GK", new Guid("0dc1832f-ee53-b526-2499-2645419c24e7") },
                    { new Guid("7454f1bb-edd9-3ce7-063f-569f2e2d8a9d"), "Geometry", new Guid("0c484af1-d5d5-453c-06a7-067c5d5a87fb") },
                    { new Guid("75153855-9434-ee77-f028-f59b199c0fb9"), "Geography", new Guid("0dc1832f-ee53-b526-2499-2645419c24e7") },
                    { new Guid("7557a853-225b-4795-d608-3dc3658a8a80"), "Series", new Guid("99a755c8-4233-88d1-880c-20d737ea5468") },
                    { new Guid("75f79c06-4a50-75de-2b8b-08942cd3159b"), "Logical Reasoning", new Guid("b6ca55cb-52d9-e35b-dfa2-31a59bccc342") },
                    { new Guid("7675e3ca-8a2e-c820-d7f0-f2482accf6d6"), "Reading Comprehension", new Guid("a8debe0a-0d78-d1f1-bcd3-3b60b0dced1b") },
                    { new Guid("7697886b-bae9-2745-62b7-5038b990b686"), "Geometry", new Guid("0dfd2625-89ac-f13e-6b38-e3c0d8d0a2c4") },
                    { new Guid("76b10981-9c40-8cec-cc94-6996d82d0694"), "Series", new Guid("cf0ceb04-8549-49d0-9e2a-0d3c138d8a3b") },
                    { new Guid("76e62b47-557a-05c6-0830-b3b88f9d9c86"), "Error Detection", new Guid("5006ddb3-b59d-627a-0a54-9567cad71a31") },
                    { new Guid("773397b0-a622-1a43-f1a5-950546ff897e"), "History", new Guid("dedccbd1-0495-d064-9dd2-9bb2e272e966") },
                    { new Guid("78a7a905-7c01-0ff4-f8ab-2627cc6fe307"), "Profit and Loss", new Guid("04e69404-9b8a-d643-b48a-8710d2f73d29") },
                    { new Guid("792c00bb-f6d1-14f8-8434-a61471181648"), "Coding-Decoding", new Guid("61f44871-630e-0b34-8fad-ca4cc042320b") },
                    { new Guid("7989c441-afd5-8dd4-a766-cb9ec9a9f403"), "Geography", new Guid("acca5304-c9b6-1e28-83e3-9418b80c1e6b") },
                    { new Guid("7a5a599f-a967-f20d-c1e3-ae933a0b3e19"), "Error Detection", new Guid("be180ffa-7d82-cf64-ef60-02f431e2a51b") },
                    { new Guid("7ac2285e-711f-520c-6eb3-ec44c1935e66"), "Percentage", new Guid("26aeaf8e-cccf-0ceb-5682-ff5da0b6d543") },
                    { new Guid("7ac95e74-3874-59b0-5ac9-fbd35045e8d7"), "Geography", new Guid("a8310bb0-30f7-59aa-4793-e850695b69bd") },
                    { new Guid("7ba6a030-77fd-70e0-7b43-abec2fc6cc0a"), "Geography", new Guid("cc6f069c-72e0-2320-6097-e8d0a82c0af3") },
                    { new Guid("7c49e47e-895e-b8dc-a86e-2d75b85da15d"), "Error Detection", new Guid("af6317d8-4f29-7dfd-c1c5-a4049d275f5e") },
                    { new Guid("7cba2f9a-c22a-e6ac-0262-71ff5f443a7f"), "Static GK", new Guid("42cdbe8c-d2ae-443c-1d7d-586a0e8f42f5") },
                    { new Guid("7cf6c3e0-3103-766b-aa2f-69f5769861d0"), "Series", new Guid("cf43bf94-c993-8ac9-c2fa-2107009d615b") },
                    { new Guid("7d3138df-3223-d4d1-29e4-3fb5a6fd688f"), "Geometry", new Guid("bd15903d-a370-057d-93a0-e5a6d37c5da2") },
                    { new Guid("7d363073-3ac3-72b8-0299-afaecc863853"), "Profit and Loss", new Guid("69c2b215-e4dd-c22f-48ba-6affabc79966") },
                    { new Guid("7dd98435-db8a-3da4-7fe0-9628853f7f8b"), "Error Detection", new Guid("e6059fdb-adc3-7728-fa78-d089919f5f5e") },
                    { new Guid("7e6ec37b-c48a-44c4-1554-38e727056369"), "Logical Reasoning", new Guid("bfee3ae6-d5d6-961b-60fb-6bf1d5b8710f") },
                    { new Guid("7ee884ca-a4f1-fedc-c6e6-f1e5d7c9e9dd"), "Geography", new Guid("48f0f198-01af-1a80-8f50-aacffbb6440e") },
                    { new Guid("7f046fe9-f01c-8382-a470-84e3e449f9f7"), "Profit and Loss", new Guid("94ae42f3-35b8-aa97-694c-77c3df885c64") },
                    { new Guid("7f90e50e-af04-b3e9-e109-9af453875a53"), "Series", new Guid("7f5ddba9-3e27-eeb8-eb18-e306a8d57622") },
                    { new Guid("800e8387-a6e2-49e6-1aa2-97d9f6b51f13"), "Static GK", new Guid("df3fcbcc-92eb-58b6-723d-d8b54fa4ab6c") },
                    { new Guid("805da04a-7d64-56f6-0db3-44e51d48d263"), "Static GK", new Guid("cab2a243-5606-3f5c-4a75-bdc6bd08b177") },
                    { new Guid("80ddf5cd-8e64-3a46-0ad6-6bda643e54ab"), "Geometry", new Guid("6cfc7e4b-94e6-d2cd-da48-a516fe9aa24b") },
                    { new Guid("817cabab-4316-dfc1-7f51-92b27a9e0aae"), "Error Detection", new Guid("64eda4df-4f57-2a64-b542-f9d8d2463c0d") },
                    { new Guid("81a741a6-9749-098a-4ac3-303c839b7ab1"), "Vocabulary", new Guid("5ab6da55-694a-0a7f-bd80-a63079edf15b") },
                    { new Guid("81b67df5-7b3c-04ab-4975-0290b53f571b"), "Geography", new Guid("f1c3656d-72d3-0b64-1882-cd41023be0eb") },
                    { new Guid("81ed545f-9123-6b9c-d5e1-26be6d2afd7a"), "Static GK", new Guid("3f9b3521-896b-6a86-c006-f74c596b124d") },
                    { new Guid("826abcac-7cf5-4a3f-7a8e-b4d08eea9ca6"), "History", new Guid("f13e3c99-cdf7-5843-f4b3-90e1c409402b") },
                    { new Guid("82878b41-5efa-4c80-55a7-48681fc2dfb2"), "Error Detection", new Guid("cba95b28-a792-279a-eebf-0931e088750d") },
                    { new Guid("82af4e24-5d45-8afa-ffc3-49398bb67c96"), "Geography", new Guid("482cd3d1-1df4-67f3-a3bd-b273d9892ab8") },
                    { new Guid("82b8ff6a-8bec-b923-0f0a-d178070498de"), "Logical Reasoning", new Guid("6e5381c2-aa75-e19e-eac4-a983be83afd5") },
                    { new Guid("82bcefd9-c571-29b2-396b-909bed09bc28"), "Logical Reasoning", new Guid("2414d6e1-c8bd-053f-5c2e-20a4c5c3a12c") },
                    { new Guid("82d10738-39ac-898b-4ca3-843aad211618"), "Series", new Guid("248f5cbe-71c8-ca4a-44fd-37b5fe0b5f4d") },
                    { new Guid("8356a1dc-1dfa-8b5d-129c-bc13b3ad4db5"), "Vocabulary", new Guid("3c40893b-b1da-0e77-c6e6-27559f0200bb") },
                    { new Guid("847d2c4d-ca56-2df0-5331-b68615d2c4e5"), "Error Detection", new Guid("43cc05df-90fb-d3a9-3329-e42168f33a77") },
                    { new Guid("8529c55f-a3c8-e612-d0b2-b4842e25ceb5"), "Geography", new Guid("c67a192c-593e-5ad9-5b5a-d04633886269") },
                    { new Guid("8543bc05-79fa-2d31-1da7-afd7b4394d7e"), "History", new Guid("d82ee6fe-0ae5-5559-d306-2dc93913f7b9") },
                    { new Guid("85478605-acdf-22a3-7c78-c6db618bb127"), "Geometry", new Guid("3c7bd5f1-989b-5d61-d6e4-f5af3a101227") },
                    { new Guid("8559a7b4-bc77-78a4-40df-ad697118f0ad"), "Coding-Decoding", new Guid("cf0ceb04-8549-49d0-9e2a-0d3c138d8a3b") },
                    { new Guid("85b73170-71d6-8fe0-37a8-81347c409796"), "Series", new Guid("c20816a0-b9cf-d402-831c-ba498ec540f9") },
                    { new Guid("85b82ae0-3d2f-6764-b28a-c7f21ecba623"), "Profit and Loss", new Guid("f8eb512b-7061-90ee-218d-f79f85013f49") },
                    { new Guid("861b8330-160f-2740-64b8-c8ba35ba33d1"), "Percentage", new Guid("7b2364df-0c2b-26f8-281d-af4d6518681b") },
                    { new Guid("864c204f-0d31-e469-a27e-adb2e95b15f6"), "Profit and Loss", new Guid("7bd4bbc7-818f-47aa-ea61-1894b73d2186") },
                    { new Guid("868ecb21-8a70-1ee6-b27f-841e3b5d5597"), "History", new Guid("42599220-bbb4-8c25-dfa1-f63ee080c8dd") },
                    { new Guid("86985d3b-d3af-d50c-44e7-2c3c41eb0766"), "Percentage", new Guid("3249993c-ee08-0d52-5e33-5def559739ce") },
                    { new Guid("88ce1a88-a323-9c7d-0260-1a4298944032"), "Reading Comprehension", new Guid("5006ddb3-b59d-627a-0a54-9567cad71a31") },
                    { new Guid("8949b762-e2a4-04b2-1a59-2e6dfb1c3fbf"), "Coding-Decoding", new Guid("0886696f-48c8-891e-a7d5-8550fb0d7021") },
                    { new Guid("89554737-ed3a-f41f-2f91-0546af998fa4"), "Error Detection", new Guid("d941bee7-d351-3aba-8b25-87c9f3a990ec") },
                    { new Guid("89a7ab43-b1bb-7a60-3ef7-224ea0e2590f"), "History", new Guid("cab2a243-5606-3f5c-4a75-bdc6bd08b177") },
                    { new Guid("89de6fb0-5f38-09b9-5cf8-bcca2638be13"), "Reading Comprehension", new Guid("3e6077cf-a585-a5b4-8058-0ab46cef9880") },
                    { new Guid("89ffbf7b-711e-fd60-89a1-7cada4db1684"), "Geometry", new Guid("60d5154d-3d43-60dd-b21d-0943056545bd") },
                    { new Guid("8a94fa4c-687c-6805-6d10-41defc58d108"), "Profit and Loss", new Guid("60d5154d-3d43-60dd-b21d-0943056545bd") },
                    { new Guid("8aa4a345-a5bc-6e84-003b-2560f1b3396a"), "Vocabulary", new Guid("2b57ea8f-d5a6-227e-b079-b81ae68d0267") },
                    { new Guid("8aad03d1-a6da-07b7-039e-70aab61eeee7"), "Coding-Decoding", new Guid("b6ca55cb-52d9-e35b-dfa2-31a59bccc342") },
                    { new Guid("8aec6c46-20bf-f664-2862-3a65fcb70edf"), "History", new Guid("c3e0cacd-ad3d-f8bd-26e3-9c10f6ee4640") },
                    { new Guid("8b18648c-e699-76ff-f361-e6cc7add0c83"), "Profit and Loss", new Guid("0c484af1-d5d5-453c-06a7-067c5d5a87fb") },
                    { new Guid("8c4b87e9-c5fd-a7ec-806b-3fcf88dfbea3"), "Error Detection", new Guid("30ae9e12-3ceb-88c9-5e54-a00e67f69df4") },
                    { new Guid("8c61df07-a889-b6a1-2f8b-ccbd538ff8a5"), "Coding-Decoding", new Guid("6d81ff1c-ca17-baed-b320-3fe9b11eb2f5") },
                    { new Guid("8c8c3cc8-524d-e5dc-490f-c311bfe7cdec"), "Geometry", new Guid("7b2364df-0c2b-26f8-281d-af4d6518681b") },
                    { new Guid("8c9f58e3-782e-bdbf-05a3-68753a22bae1"), "History", new Guid("48f0f198-01af-1a80-8f50-aacffbb6440e") },
                    { new Guid("8cb25289-4f29-be31-8e46-ca04a2cb9a29"), "Coding-Decoding", new Guid("99a755c8-4233-88d1-880c-20d737ea5468") },
                    { new Guid("8cd3b601-d5a4-d069-a727-bd6bb7bec9f7"), "Reading Comprehension", new Guid("d941bee7-d351-3aba-8b25-87c9f3a990ec") },
                    { new Guid("8ce1e170-5103-51e7-2f1e-478853f55d36"), "Coding-Decoding", new Guid("248f5cbe-71c8-ca4a-44fd-37b5fe0b5f4d") },
                    { new Guid("8cec00b6-9482-6cfa-2de0-a2cb81bd1301"), "Profit and Loss", new Guid("9ffa0b37-7671-52d4-b662-9254099ae239") },
                    { new Guid("8f159b85-07d5-2bcc-48ed-bce349bc5137"), "Profit and Loss", new Guid("21b78efb-e9e3-8cc5-1d58-8cc5510842a7") },
                    { new Guid("8f26edd8-a4f9-2a74-26e5-117f686e7f9e"), "Static GK", new Guid("74afdb9b-0e1c-94b9-765e-d720199d450d") },
                    { new Guid("8f349c27-9981-57cd-f78b-624a48998847"), "Geography", new Guid("3f9b3521-896b-6a86-c006-f74c596b124d") },
                    { new Guid("8fd13f26-afd0-7596-7760-d273a550f701"), "Geometry", new Guid("04e69404-9b8a-d643-b48a-8710d2f73d29") },
                    { new Guid("8ff44779-b809-d9b3-5d16-47622fe4f5ef"), "Reading Comprehension", new Guid("5ab6da55-694a-0a7f-bd80-a63079edf15b") },
                    { new Guid("90010247-3771-66ff-efa8-3b770c1fc935"), "Geometry", new Guid("29cbfd96-bd12-d1e4-10ad-43a1fecbfe56") },
                    { new Guid("90a367d1-20f8-e9ed-6ffe-d402cdd9a833"), "Geometry", new Guid("c6e37724-21e2-8f38-96a9-202971a82ef3") },
                    { new Guid("90b877a7-d70f-d30b-55ae-d7b56d5aeac5"), "Error Detection", new Guid("c32886cb-768d-7fd9-6835-1ffdb5acc3fe") },
                    { new Guid("916f27e4-2a2d-726d-19e5-6b090f5821c2"), "Logical Reasoning", new Guid("93dedde1-bc85-8d54-8ab5-0f407da7fd28") },
                    { new Guid("92ec3797-b5ce-c8b3-1499-e5d49c9fd222"), "Series", new Guid("91eebe88-16a5-addf-707d-06768eed2e82") },
                    { new Guid("933d1f57-828d-ced7-cd52-1dbbbff3d8fb"), "Reading Comprehension", new Guid("3206020e-1e1a-d766-d66b-6d604baf5616") },
                    { new Guid("93a69d90-1186-aced-a996-d8ca600d3537"), "Profit and Loss", new Guid("4c110e73-7f94-2f66-ba13-eb0bd5733804") },
                    { new Guid("93afc40b-d6ac-a257-c5ff-c7df56800f19"), "Logical Reasoning", new Guid("60150aea-1845-3cb3-002d-8c8b1e3eddd7") },
                    { new Guid("93c73661-0d02-882f-1249-4c17bbbcc5a6"), "Series", new Guid("322fb74b-b528-7a96-495c-39fc1d728a2e") },
                    { new Guid("9421a447-3109-64a0-50e7-3c56177decf4"), "Percentage", new Guid("bd15903d-a370-057d-93a0-e5a6d37c5da2") },
                    { new Guid("94e07f45-e1d1-d7ff-7895-69fe22acb13c"), "Geography", new Guid("b4f4bfc1-87b3-db9d-6a37-509f32357111") },
                    { new Guid("95d2f30b-0387-db58-9c6f-45a46cbec322"), "Error Detection", new Guid("c61f716f-e12b-56f1-0c31-327b68a1d74e") },
                    { new Guid("9632dbbe-c56b-1786-21c4-79f72dedfc4d"), "Static GK", new Guid("0ed4b1a8-e05d-fa4c-423a-150036463aff") },
                    { new Guid("967df2e2-539f-2504-f24d-52087d0ecd6c"), "Coding-Decoding", new Guid("f528c47d-c50a-1954-7b5b-647d28e075f2") },
                    { new Guid("96b039ca-ee22-212b-6f96-c7cf582631f0"), "Geometry", new Guid("9ffa0b37-7671-52d4-b662-9254099ae239") },
                    { new Guid("97176a60-c400-3bd1-32c0-c4758fdeb01b"), "Static GK", new Guid("f13e3c99-cdf7-5843-f4b3-90e1c409402b") },
                    { new Guid("972e6bc8-2e47-0472-f2b0-abda3ef74e62"), "Geometry", new Guid("4d5881bc-d0b4-0d9e-4975-1dd816dfd3e5") },
                    { new Guid("9762c454-fc7c-da6e-5ef9-ac9f64e52528"), "Logical Reasoning", new Guid("df7000e9-4ce7-789f-6611-ea0bbc407cbe") },
                    { new Guid("978e8b45-bb9e-d5b9-c15f-e6be4944537e"), "Reading Comprehension", new Guid("eafbcc0a-5071-d42c-677a-58fd0374cfba") },
                    { new Guid("97fba8b6-e647-08f6-41d6-0a71a2c3d870"), "Logical Reasoning", new Guid("cf43bf94-c993-8ac9-c2fa-2107009d615b") },
                    { new Guid("98944fdf-c487-1267-d5e1-81f87c6f89c9"), "Series", new Guid("8dd5c1f6-3406-e8a6-de08-130071a44989") },
                    { new Guid("98a08964-55f3-d2dc-1683-9dc9863a2552"), "History", new Guid("4eeb5fcc-8d45-97a3-9f21-8838d2db2c9b") },
                    { new Guid("998fce9f-e457-c6f3-af90-25a4dcf141ae"), "Logical Reasoning", new Guid("6850b500-1bb1-e162-189f-045a15d9a0f2") },
                    { new Guid("9a17f511-7b57-8bf3-a648-ecea7b91c667"), "Coding-Decoding", new Guid("df7000e9-4ce7-789f-6611-ea0bbc407cbe") },
                    { new Guid("9afc98a7-c1f5-b576-a539-93b2f0012afd"), "Vocabulary", new Guid("cba95b28-a792-279a-eebf-0931e088750d") },
                    { new Guid("9c289e6e-067e-2e45-5073-67a5ff38a40d"), "Geography", new Guid("a6f8ba35-9571-9bb2-c4e6-9f3f32950dba") },
                    { new Guid("9c6e46e2-6de3-3b67-a6a9-9a71b0a548aa"), "Geography", new Guid("49d6e9c7-e86b-daf0-0a22-16cfa49fc212") },
                    { new Guid("9c93f4d0-b195-884f-1748-68ca647a31f7"), "Error Detection", new Guid("30c33b7b-9e1f-487b-fd59-2872a64fe595") },
                    { new Guid("9db98070-4eb9-643a-c513-8c2f886547c7"), "History", new Guid("f1c3656d-72d3-0b64-1882-cd41023be0eb") },
                    { new Guid("9dbb88e3-6f85-9c6e-855f-fa9fd3e773a0"), "Coding-Decoding", new Guid("da9b8a63-d9d3-3904-7359-949ef4d1459d") },
                    { new Guid("9df17a8c-7214-6773-c963-bbed9a525be3"), "Geography", new Guid("205fdf02-2719-c1b7-be92-666228b6e3e7") },
                    { new Guid("9f5f0974-72e7-ef38-c15b-b6a76028fcc6"), "History", new Guid("096355bb-6cb5-8220-abc3-6fe02899a819") },
                    { new Guid("a05ff79f-689f-f33b-d5ed-f4f4ed5b3d6d"), "Profit and Loss", new Guid("6cfc7e4b-94e6-d2cd-da48-a516fe9aa24b") },
                    { new Guid("a0cbb04b-ff44-72fb-6326-ba15c2acc211"), "Logical Reasoning", new Guid("61f44871-630e-0b34-8fad-ca4cc042320b") },
                    { new Guid("a0ef1fc2-d996-3f08-5874-c6fdd471a2a8"), "Coding-Decoding", new Guid("fb9a1cab-1799-1c4f-6d6b-8ddcfff1099d") },
                    { new Guid("a2aab89d-be0f-9a8b-ab8d-caa133a11ab5"), "Geometry", new Guid("7ae414ef-143b-4d3e-20b1-ea9fcc461547") },
                    { new Guid("a2e7aaf7-8134-e9ab-243c-37eb46b05272"), "Reading Comprehension", new Guid("8ac4ada6-4775-eeb0-d87f-32383572fbe6") },
                    { new Guid("a3fb0573-ca59-eb7e-a393-9f2865e4521c"), "Percentage", new Guid("68dd4f56-21f3-72c9-4a78-6ac785f4ac6e") },
                    { new Guid("a453a8fd-79cb-0148-e0c3-b7d7fc5da149"), "History", new Guid("2922679e-6588-e91d-2617-07abd23a9823") },
                    { new Guid("a453d9cd-e79e-36a4-7075-9d040bae8b14"), "Coding-Decoding", new Guid("633b28ab-6d5a-dd92-ce09-65145ac09269") },
                    { new Guid("a5d82258-d4c2-499a-3d35-64222c70db6f"), "Coding-Decoding", new Guid("6e5381c2-aa75-e19e-eac4-a983be83afd5") },
                    { new Guid("a63c350b-7c16-4878-70d1-ef4d91f71026"), "Reading Comprehension", new Guid("8fc32dcf-6b85-d7b7-c9ee-dfeb8347435a") },
                    { new Guid("a66d2914-663f-74be-b7aa-1aef606d15f7"), "Series", new Guid("bfee3ae6-d5d6-961b-60fb-6bf1d5b8710f") },
                    { new Guid("a6e5644c-630b-57da-b7fb-344e770ae5ae"), "Static GK", new Guid("482cd3d1-1df4-67f3-a3bd-b273d9892ab8") },
                    { new Guid("a6ee14c6-ad07-d275-1e12-42251f9a2e70"), "Geometry", new Guid("21b78efb-e9e3-8cc5-1d58-8cc5510842a7") },
                    { new Guid("a7440dfb-628e-36d4-f3a3-d66bf34e6de0"), "Coding-Decoding", new Guid("9c860696-a5de-ec1a-83b1-d00dc21e8195") },
                    { new Guid("a7aa5c72-ecab-1dea-4e40-7ff80b1bef2c"), "Geography", new Guid("cdf661da-b824-ace3-cdc1-9226ffa65710") },
                    { new Guid("a7f64c46-233f-7e3c-fd01-22a0b6c74c36"), "History", new Guid("65e36679-c743-faf2-6191-8a6c5f266f04") },
                    { new Guid("a85d9bc6-99b6-44ce-c13f-152c15aa0184"), "Geometry", new Guid("c63a3fe7-693e-4566-3760-572ad7dca8af") },
                    { new Guid("a97863c4-762a-698a-2c3a-cae0c6fb0896"), "Series", new Guid("8910836e-bf37-7b12-dfcd-fc7cb4ef7495") },
                    { new Guid("a98ba9f2-5d96-df55-5a0a-4016ecfa6141"), "Series", new Guid("f7d81f71-e69b-1efd-a0dc-7600bce80b21") },
                    { new Guid("a99ffca4-6289-df84-0364-bc9e14e5e2f5"), "Logical Reasoning", new Guid("867f98d1-22e1-c9e6-7eb9-a2030b743d9b") },
                    { new Guid("a9ed3972-ccfd-eeaa-fd7f-01cc50e1a2fa"), "Geometry", new Guid("7bd4bbc7-818f-47aa-ea61-1894b73d2186") },
                    { new Guid("aa61c146-24c0-04a7-3ab0-5add6c9e305e"), "Vocabulary", new Guid("0f9aae8b-5730-91c1-4eba-dcab68ef87d4") },
                    { new Guid("ab1574b3-dd32-1a3d-0aa7-3355567abae3"), "Error Detection", new Guid("4773c0d2-b3ca-c94d-9849-c745b4f3dba8") },
                    { new Guid("abc940f4-bf7f-2894-9516-ddadb77e7990"), "Geometry", new Guid("0b52a18c-3ced-19d8-b9d3-dc053e587ad4") },
                    { new Guid("ac5d9328-ff0b-3b51-4cbe-745907bf1869"), "Logical Reasoning", new Guid("91eebe88-16a5-addf-707d-06768eed2e82") },
                    { new Guid("ad6ac6b4-9518-8320-6738-682d3e4ccaec"), "Profit and Loss", new Guid("d2f289ff-4632-f941-4428-3be85994e398") },
                    { new Guid("afb9f2fb-3ff6-6fe8-e3e1-9b80264a8822"), "Geometry", new Guid("93b5cded-e275-4edf-d41c-341fb7954bda") },
                    { new Guid("b0543335-78f6-b2e7-f07a-a0d4103c8afd"), "Percentage", new Guid("3c7bd5f1-989b-5d61-d6e4-f5af3a101227") },
                    { new Guid("b0e81daa-a8ce-1546-987c-823925765c5c"), "Geometry", new Guid("d2f289ff-4632-f941-4428-3be85994e398") },
                    { new Guid("b0f51de7-805e-951b-f33d-1bb6f9cb6a6d"), "Series", new Guid("f528c47d-c50a-1954-7b5b-647d28e075f2") },
                    { new Guid("b15a4f40-64b7-cdc4-3d67-0ed315720f07"), "Profit and Loss", new Guid("c49953ee-92c5-9df9-4540-39edf8bfac34") },
                    { new Guid("b1872c04-cac8-7192-10a0-419c215ef0ea"), "Reading Comprehension", new Guid("8eebe33a-8ce2-3bed-d6d7-55d057d7fff4") },
                    { new Guid("b22b241c-d2d9-eb2e-4560-d28d3d372f97"), "Logical Reasoning", new Guid("cf0ceb04-8549-49d0-9e2a-0d3c138d8a3b") },
                    { new Guid("b2cff68f-c7f1-078a-c426-2255247ff62c"), "Geography", new Guid("dedccbd1-0495-d064-9dd2-9bb2e272e966") },
                    { new Guid("b301e02b-df3d-225f-952c-7812c598c586"), "Geometry", new Guid("68dd4f56-21f3-72c9-4a78-6ac785f4ac6e") },
                    { new Guid("b51c48c4-2ac5-aecc-68a4-215d7eee98cf"), "Profit and Loss", new Guid("bd15903d-a370-057d-93a0-e5a6d37c5da2") },
                    { new Guid("b573951d-057e-4fa5-7803-f0b9213b39ef"), "History", new Guid("5392ef0e-3dbf-d6d5-8adc-9f074c7b0f3e") },
                    { new Guid("b5966155-13c8-b1ec-372d-861fcb75a043"), "Geography", new Guid("b4166db5-ba06-8de6-35b8-16212d900202") },
                    { new Guid("b6b68070-a1fc-da72-250f-973b111d67b4"), "Profit and Loss", new Guid("2df66c1d-c139-3764-c529-61c645d4245b") },
                    { new Guid("b778a206-66c7-d38e-5b44-c533ef008f32"), "Percentage", new Guid("9ffa0b37-7671-52d4-b662-9254099ae239") },
                    { new Guid("b803ad16-62e1-92ff-e850-92d351ae2bf6"), "Series", new Guid("867f98d1-22e1-c9e6-7eb9-a2030b743d9b") },
                    { new Guid("b851b628-d727-44cf-6da3-56f7b53b5f37"), "Geometry", new Guid("fd19c7c8-1b31-50de-adc4-97bca17deb85") },
                    { new Guid("b86008e1-e587-fb09-5829-95c89098d472"), "Error Detection", new Guid("6b36ab48-4a04-61ec-0b7f-57884fea536a") },
                    { new Guid("b8bfe2ba-de0a-91dd-9c3d-0801933275d7"), "Coding-Decoding", new Guid("bfee3ae6-d5d6-961b-60fb-6bf1d5b8710f") },
                    { new Guid("ba973e5a-d689-ccbc-d39f-c2be964585e4"), "Vocabulary", new Guid("be180ffa-7d82-cf64-ef60-02f431e2a51b") },
                    { new Guid("bac7075e-a2ca-a3e4-1bee-8057c411e1a9"), "Geography", new Guid("65e36679-c743-faf2-6191-8a6c5f266f04") },
                    { new Guid("bb5a75ef-4f26-bfe9-35b0-28a5682c66c4"), "Reading Comprehension", new Guid("30ae9e12-3ceb-88c9-5e54-a00e67f69df4") },
                    { new Guid("bba90cfa-50bd-fa42-3834-e041aac5f9ce"), "Vocabulary", new Guid("5006ddb3-b59d-627a-0a54-9567cad71a31") },
                    { new Guid("bbc48ed6-cc8a-b358-1278-03f42057b87d"), "Percentage", new Guid("29cbfd96-bd12-d1e4-10ad-43a1fecbfe56") },
                    { new Guid("bbe703fb-744d-3726-a754-74d232d4627f"), "Profit and Loss", new Guid("0b52a18c-3ced-19d8-b9d3-dc053e587ad4") },
                    { new Guid("bc238d01-1088-8448-17f6-148add5f29e4"), "Error Detection", new Guid("3c40893b-b1da-0e77-c6e6-27559f0200bb") },
                    { new Guid("bc6d2402-52ed-5d19-d9ff-4984e04240f8"), "Error Detection", new Guid("30b331ed-e430-7bae-e7ce-f589f35517eb") },
                    { new Guid("bcd237f7-b385-1be9-44d3-ed9a4e2e7f0d"), "Percentage", new Guid("f8eb512b-7061-90ee-218d-f79f85013f49") },
                    { new Guid("bce0c8e5-eda8-498b-b3bb-89bb04cc7614"), "Series", new Guid("6d81ff1c-ca17-baed-b320-3fe9b11eb2f5") },
                    { new Guid("bdbd1027-9d26-6428-7f18-7d3912563dfd"), "Static GK", new Guid("5c4b02f0-6369-076a-31b2-4e64db0b316f") },
                    { new Guid("bebc7b5e-5c99-11c1-4299-5844f59f6e51"), "Series", new Guid("c36cae96-b91f-e4cb-756c-1fd7d8fb219d") },
                    { new Guid("bed93e55-0d8f-ccb4-b215-0187ce215137"), "Profit and Loss", new Guid("fd19c7c8-1b31-50de-adc4-97bca17deb85") },
                    { new Guid("bf8436e4-524b-7cf2-6c46-36b35929f027"), "Series", new Guid("82df9511-fd16-f5b9-3da2-d877a07ace7c") },
                    { new Guid("bfcea2e1-4f52-0f2b-60bb-47a30386fad5"), "Reading Comprehension", new Guid("64eda4df-4f57-2a64-b542-f9d8d2463c0d") },
                    { new Guid("c0340117-0739-670b-2350-05ed5f3030a2"), "Profit and Loss", new Guid("9bc50977-c5e3-4eea-99fd-100bcca73458") },
                    { new Guid("c098975f-d9d5-22cf-8fa0-f24052e7f0f8"), "Geometry", new Guid("9bc50977-c5e3-4eea-99fd-100bcca73458") },
                    { new Guid("c131ea07-ae5b-0ccc-fd8a-0a8d2bb0c6a0"), "History", new Guid("0dc1832f-ee53-b526-2499-2645419c24e7") },
                    { new Guid("c146764c-f056-5d31-ac33-9f11d51a5035"), "Vocabulary", new Guid("85986333-45ff-1990-65d6-97248deb6d08") },
                    { new Guid("c18f9071-d486-6d52-669a-083c9bc9fff1"), "Series", new Guid("6850b500-1bb1-e162-189f-045a15d9a0f2") },
                    { new Guid("c31fa477-49f0-bf5b-636f-7b061e16e113"), "Reading Comprehension", new Guid("af6317d8-4f29-7dfd-c1c5-a4049d275f5e") },
                    { new Guid("c3292938-a522-ab51-9bcf-ac412691e12a"), "Error Detection", new Guid("5c99f7b3-af3e-e99e-0824-9d55cbc055a1") },
                    { new Guid("c33a5f83-20fd-1e38-2547-2ee265faf7da"), "Geography", new Guid("0ed4b1a8-e05d-fa4c-423a-150036463aff") },
                    { new Guid("c48c119d-469a-2765-e868-40a9ca65a097"), "Logical Reasoning", new Guid("da9b8a63-d9d3-3904-7359-949ef4d1459d") },
                    { new Guid("c5ce6c11-7617-37df-d4f1-0c5c41cc3a7a"), "Coding-Decoding", new Guid("1a1e72e3-c129-2580-72a0-51e0c5e8c51a") },
                    { new Guid("c783d86e-972f-da4c-fabd-43017e307d48"), "Series", new Guid("c9d07602-cc8d-5173-33d0-77c500336aad") },
                    { new Guid("c8078c89-52d1-e7c7-9715-56ce76dfd7fa"), "Series", new Guid("9c860696-a5de-ec1a-83b1-d00dc21e8195") },
                    { new Guid("c908d7cc-dee9-d914-cf26-e4ce0890ecd0"), "Geometry", new Guid("4c110e73-7f94-2f66-ba13-eb0bd5733804") },
                    { new Guid("ca50d26a-7d20-1fc9-ba69-09d4ecbd831f"), "Logical Reasoning", new Guid("6dfdb9f3-fcc7-d8cd-9bb7-4b366d335b84") },
                    { new Guid("cb8c2d51-c236-0a94-de6d-90a93d7e1f96"), "Error Detection", new Guid("0f9aae8b-5730-91c1-4eba-dcab68ef87d4") },
                    { new Guid("ccd719d1-0d27-f153-6128-c85c8f44b57e"), "Coding-Decoding", new Guid("91eebe88-16a5-addf-707d-06768eed2e82") },
                    { new Guid("ccdcb5c2-d14d-f804-8fc8-c415aa058b7f"), "Series", new Guid("e539d4f4-ed7f-2082-a12c-b64118823689") },
                    { new Guid("cd0e5dcb-b639-4473-b37a-e02ab984b77f"), "History", new Guid("42cdbe8c-d2ae-443c-1d7d-586a0e8f42f5") },
                    { new Guid("cdf71698-0faf-16d1-a54c-028e47f0b679"), "Static GK", new Guid("c3e0cacd-ad3d-f8bd-26e3-9c10f6ee4640") },
                    { new Guid("ceb131f3-4546-54ca-6176-709a0276d7c0"), "Coding-Decoding", new Guid("867f98d1-22e1-c9e6-7eb9-a2030b743d9b") },
                    { new Guid("cec2ce7c-db27-48ac-322d-f0430b184c87"), "Logical Reasoning", new Guid("c36cae96-b91f-e4cb-756c-1fd7d8fb219d") },
                    { new Guid("ced36879-a42f-d02e-2d73-baed492e8e5f"), "Profit and Loss", new Guid("e646bc26-8933-29ad-2167-39b0c2be0883") },
                    { new Guid("cf359ec8-8985-280f-e942-69949cfe5268"), "Geography", new Guid("74afdb9b-0e1c-94b9-765e-d720199d450d") },
                    { new Guid("cf67d021-95ff-f399-4e99-69860a206747"), "Reading Comprehension", new Guid("be180ffa-7d82-cf64-ef60-02f431e2a51b") },
                    { new Guid("d06cd634-70e7-d19e-fb28-5cba4c5e03ab"), "Reading Comprehension", new Guid("0e77bff5-deff-a943-0610-fd871552ad7b") },
                    { new Guid("d1360cd6-af75-b88d-09c7-1cddd615e662"), "Coding-Decoding", new Guid("2414d6e1-c8bd-053f-5c2e-20a4c5c3a12c") },
                    { new Guid("d252ea34-a7d1-c8a1-4daf-6997a51851b4"), "Series", new Guid("fc9e5144-f3e5-f26c-a6b1-3a80f0684fc2") },
                    { new Guid("d2bd111e-f284-0cf6-b774-620cf28d18d1"), "Percentage", new Guid("94ae42f3-35b8-aa97-694c-77c3df885c64") },
                    { new Guid("d2d4e09a-19e6-6c64-7985-0c1573027446"), "Profit and Loss", new Guid("cf584bf4-6877-b592-3b0e-cf2de432ab68") },
                    { new Guid("d30cc72c-11d6-89ff-a2d0-cb1f3136d9f0"), "History", new Guid("cc6f069c-72e0-2320-6097-e8d0a82c0af3") },
                    { new Guid("d32ed2e7-9600-5e4a-9beb-e30c75f6ebfa"), "Vocabulary", new Guid("8ac4ada6-4775-eeb0-d87f-32383572fbe6") },
                    { new Guid("d352b1af-2df6-fcb3-275f-b57f733754ee"), "Vocabulary", new Guid("bf8a799a-ce3c-e1be-0d3c-dee172a73ab7") },
                    { new Guid("d61bce4a-82a7-1aaf-a796-14e377da4420"), "Reading Comprehension", new Guid("cba95b28-a792-279a-eebf-0931e088750d") },
                    { new Guid("d6c38060-a21b-b877-3674-89ca499c0126"), "Profit and Loss", new Guid("59a1391d-7a39-5360-9fb3-832bdb2cac79") },
                    { new Guid("d6f708f6-e94b-ec80-b4b1-5899f0bc6eaa"), "Geography", new Guid("c3e0cacd-ad3d-f8bd-26e3-9c10f6ee4640") },
                    { new Guid("d7db6ea3-22a8-18d4-8b41-b2ff426168c2"), "Vocabulary", new Guid("239d45ea-f0cb-4232-5fea-bd7c5cbe3ab3") },
                    { new Guid("d902122c-fad9-3eb6-14bc-725781b49966"), "Percentage", new Guid("7bd4bbc7-818f-47aa-ea61-1894b73d2186") },
                    { new Guid("d950adda-b328-7c68-4b72-1009e2945dfb"), "Percentage", new Guid("7e4344c7-802a-1dbd-6a3e-d8b0b12e7666") },
                    { new Guid("d97c8251-70d8-e38b-c30f-36753967a123"), "Static GK", new Guid("205fdf02-2719-c1b7-be92-666228b6e3e7") },
                    { new Guid("da116b38-179e-8abd-9b7e-804a7abac8cc"), "Vocabulary", new Guid("30b331ed-e430-7bae-e7ce-f589f35517eb") },
                    { new Guid("da13d411-832b-e88d-4df3-030dd09e6666"), "Error Detection", new Guid("79200f88-1d99-cfa8-0aa9-2b2077af2f6a") },
                    { new Guid("da51763d-aecb-6e0f-47db-1be3d1d71118"), "Logical Reasoning", new Guid("8dd5c1f6-3406-e8a6-de08-130071a44989") },
                    { new Guid("da55eb1f-5742-6a11-46cd-ce969c05e999"), "Static GK", new Guid("42599220-bbb4-8c25-dfa1-f63ee080c8dd") },
                    { new Guid("dbc8df23-d9f0-11cf-7cdd-5eae9445358d"), "Vocabulary", new Guid("bb1e9892-8176-5fb1-bf07-97c46b34100a") },
                    { new Guid("dc88a1b7-68f5-9425-e677-1a2e3ee6533e"), "Error Detection", new Guid("a8debe0a-0d78-d1f1-bcd3-3b60b0dced1b") },
                    { new Guid("dcba5d04-f337-967a-070d-f79a9859b06a"), "Percentage", new Guid("59a1391d-7a39-5360-9fb3-832bdb2cac79") },
                    { new Guid("dd62db5b-9105-bcf8-1979-47b414ad95af"), "Geography", new Guid("096355bb-6cb5-8220-abc3-6fe02899a819") },
                    { new Guid("ddfccc8f-3bd2-d903-6a19-c687b358bd33"), "Profit and Loss", new Guid("c63a3fe7-693e-4566-3760-572ad7dca8af") },
                    { new Guid("de53b09a-ca73-620f-4e2a-175256bceee6"), "Profit and Loss", new Guid("c6e37724-21e2-8f38-96a9-202971a82ef3") },
                    { new Guid("def58ca2-1a5f-a9ab-8272-79fb054ea20f"), "Vocabulary", new Guid("6b36ab48-4a04-61ec-0b7f-57884fea536a") },
                    { new Guid("df050cb1-0611-4116-91ca-129196a030c0"), "Coding-Decoding", new Guid("93dedde1-bc85-8d54-8ab5-0f407da7fd28") },
                    { new Guid("df1f0485-ffcd-fdd4-ea51-93523fd0638d"), "Coding-Decoding", new Guid("cfe68c14-de43-9914-1f1b-5cfec539a364") },
                    { new Guid("df5b785e-1631-b224-0e5a-8b2da2afdb96"), "History", new Guid("7fdd795a-6994-63d6-fd5a-9372678e5143") },
                    { new Guid("e0c69ee8-b33e-5db2-7523-42ca280623b8"), "Percentage", new Guid("fd19c7c8-1b31-50de-adc4-97bca17deb85") },
                    { new Guid("e104424c-70af-2768-b47f-9281dff0d1a1"), "Error Detection", new Guid("bb1e9892-8176-5fb1-bf07-97c46b34100a") },
                    { new Guid("e15377e7-de81-0701-9fd0-19fb0545fdc3"), "Vocabulary", new Guid("48edbf68-6220-167c-9c06-21954ca206e9") },
                    { new Guid("e33da27e-f6ae-ed56-933d-8e045e58eeeb"), "Vocabulary", new Guid("8fc32dcf-6b85-d7b7-c9ee-dfeb8347435a") },
                    { new Guid("e34bcce9-d0c4-b84d-e783-8dc4f5587b8b"), "Geography", new Guid("3ebe7357-d069-0030-d517-fbd60e30a263") },
                    { new Guid("e368f200-5387-dd5c-fa28-825c00227ee8"), "Percentage", new Guid("4c110e73-7f94-2f66-ba13-eb0bd5733804") },
                    { new Guid("e3a00dac-80f1-9b7c-6a88-1048db765d7a"), "History", new Guid("acca5304-c9b6-1e28-83e3-9418b80c1e6b") },
                    { new Guid("e4996695-5fb3-0bb1-106a-18aa3719e3cd"), "Profit and Loss", new Guid("93b5cded-e275-4edf-d41c-341fb7954bda") },
                    { new Guid("e513f51f-c010-b2d3-c403-e2bc314cedd6"), "Profit and Loss", new Guid("3c7bd5f1-989b-5d61-d6e4-f5af3a101227") },
                    { new Guid("e5518f58-b8b8-f7f2-ccfc-c20641917dd7"), "Coding-Decoding", new Guid("322fb74b-b528-7a96-495c-39fc1d728a2e") },
                    { new Guid("e59b69ae-c704-4ad0-6532-a3ddcdbc41fe"), "Static GK", new Guid("2922679e-6588-e91d-2617-07abd23a9823") },
                    { new Guid("e64efba8-1209-19e4-470b-a9178f3b7cda"), "Reading Comprehension", new Guid("43cc05df-90fb-d3a9-3329-e42168f33a77") },
                    { new Guid("e7237bd3-ff4a-4e1b-977d-511d366ecc95"), "Coding-Decoding", new Guid("6850b500-1bb1-e162-189f-045a15d9a0f2") },
                    { new Guid("e73d6a48-b0bf-df10-ea89-3ce89c260486"), "Geography", new Guid("f13e3c99-cdf7-5843-f4b3-90e1c409402b") },
                    { new Guid("e91acc5c-3e35-ccc3-c8b7-e4d69168d747"), "History", new Guid("5c4b02f0-6369-076a-31b2-4e64db0b316f") },
                    { new Guid("ea79b14d-05d9-3a71-50c9-5b58b03a2a58"), "Logical Reasoning", new Guid("fb9a1cab-1799-1c4f-6d6b-8ddcfff1099d") },
                    { new Guid("ec593206-ddb7-a3c0-b0bb-e7f7e90a0910"), "History", new Guid("a6f8ba35-9571-9bb2-c4e6-9f3f32950dba") },
                    { new Guid("ed1220dc-b674-256c-633a-033de90bf0cf"), "History", new Guid("b52fdd07-25c4-2456-59fb-fe754430b45d") },
                    { new Guid("ed515b45-1900-b65a-2314-c400b2d81a34"), "Percentage", new Guid("e8adf44e-22ea-249b-42e4-2f7855ddf194") },
                    { new Guid("ed739b92-4366-dd26-ea0e-06b507689406"), "Coding-Decoding", new Guid("c9d07602-cc8d-5173-33d0-77c500336aad") },
                    { new Guid("ee1efa0d-e9c9-59f5-a493-9fb54343eb54"), "Logical Reasoning", new Guid("6d81ff1c-ca17-baed-b320-3fe9b11eb2f5") },
                    { new Guid("ee40e0af-b271-7912-8150-259764cd94cf"), "Reading Comprehension", new Guid("239d45ea-f0cb-4232-5fea-bd7c5cbe3ab3") },
                    { new Guid("eeaf3755-7e4f-1af0-b010-7fae87386b40"), "Percentage", new Guid("49d1d6ba-8541-3024-5413-135b5663003c") },
                    { new Guid("ef1a1201-0c59-93e5-b366-b5763bbb1c88"), "Coding-Decoding", new Guid("fc9e5144-f3e5-f26c-a6b1-3a80f0684fc2") },
                    { new Guid("ef3a7600-d4c3-9e83-c15b-70983450c9d4"), "Error Detection", new Guid("5ab6da55-694a-0a7f-bd80-a63079edf15b") },
                    { new Guid("ef4c12a0-703d-70cc-7c89-1abb0416753c"), "Reading Comprehension", new Guid("48edbf68-6220-167c-9c06-21954ca206e9") },
                    { new Guid("f0184b8f-24c8-c760-105f-ece29b8cfb4c"), "Logical Reasoning", new Guid("0886696f-48c8-891e-a7d5-8550fb0d7021") },
                    { new Guid("f085f01b-6208-b67b-ce90-5415f93ae82c"), "Reading Comprehension", new Guid("2b57ea8f-d5a6-227e-b079-b81ae68d0267") },
                    { new Guid("f259f83c-a8e8-e7f5-cc27-563c2ce0ac5b"), "Static GK", new Guid("d4e88339-8242-61dd-1109-5c9b8c3239ec") },
                    { new Guid("f26e15e8-c2df-e872-aa95-ea633ae0e298"), "Percentage", new Guid("6cfc7e4b-94e6-d2cd-da48-a516fe9aa24b") },
                    { new Guid("f281b20d-efcc-3790-391a-ca68fb4f945d"), "Geography", new Guid("42cdbe8c-d2ae-443c-1d7d-586a0e8f42f5") },
                    { new Guid("f32fe25f-0949-4b60-57ba-ed4afdad03b6"), "History", new Guid("0edd4df9-10c6-ab16-4384-f3cc03c77e3e") },
                    { new Guid("f3907dfd-6c63-70e7-53f1-a80d1a1a614a"), "Vocabulary", new Guid("79200f88-1d99-cfa8-0aa9-2b2077af2f6a") },
                    { new Guid("f3c17282-bcd9-1055-05a3-df044bd95927"), "Geometry", new Guid("e8adf44e-22ea-249b-42e4-2f7855ddf194") },
                    { new Guid("f4818d27-436c-1c8b-1dc9-d3f5a42eac9b"), "Profit and Loss", new Guid("49d1d6ba-8541-3024-5413-135b5663003c") },
                    { new Guid("f5402942-7490-45a7-365e-15cfef247169"), "Static GK", new Guid("4eeb5fcc-8d45-97a3-9f21-8838d2db2c9b") },
                    { new Guid("f56a66a5-d309-c856-b122-41c051852d22"), "History", new Guid("a8310bb0-30f7-59aa-4793-e850695b69bd") },
                    { new Guid("f7af568f-ebad-a5e7-aaf4-8c5192d13f07"), "History", new Guid("0ed4b1a8-e05d-fa4c-423a-150036463aff") },
                    { new Guid("f8171cb0-8f67-beac-4009-9adfa7f73ffa"), "History", new Guid("b4166db5-ba06-8de6-35b8-16212d900202") },
                    { new Guid("f8327dc3-3875-2b07-c02d-cd58db6183ec"), "Percentage", new Guid("0b52a18c-3ced-19d8-b9d3-dc053e587ad4") },
                    { new Guid("f886413d-ef38-f5a7-1ab7-69ed21d0c6e9"), "Geography", new Guid("2922679e-6588-e91d-2617-07abd23a9823") },
                    { new Guid("f88644d0-8897-29fc-a063-873ced623c34"), "Reading Comprehension", new Guid("94994903-a722-d664-d90f-c27454228d9a") },
                    { new Guid("f9833804-323c-5faa-8b19-c4d1ce455cad"), "Static GK", new Guid("d82ee6fe-0ae5-5559-d306-2dc93913f7b9") },
                    { new Guid("f98d103c-4959-63de-ba2a-aec4a8346d21"), "Profit and Loss", new Guid("7b2364df-0c2b-26f8-281d-af4d6518681b") },
                    { new Guid("f9d82d04-b341-d310-9f7f-142ee398b5cb"), "History", new Guid("c67a192c-593e-5ad9-5b5a-d04633886269") },
                    { new Guid("f9f3e468-d15b-29bf-a586-1506f520080c"), "History", new Guid("47bc02e3-9998-79d2-ca9c-e4c3b751689e") },
                    { new Guid("faa96723-68ea-a03c-acb7-8f98bbdf800a"), "Error Detection", new Guid("eafbcc0a-5071-d42c-677a-58fd0374cfba") },
                    { new Guid("fab63582-9134-0ac3-a4ce-7116b6df7545"), "Percentage", new Guid("c49953ee-92c5-9df9-4540-39edf8bfac34") },
                    { new Guid("facc3403-a59a-0c68-3479-5089a59416f9"), "Static GK", new Guid("b4166db5-ba06-8de6-35b8-16212d900202") },
                    { new Guid("fb2d5cd4-7ac4-3014-6f2e-91a1837d8a5f"), "Logical Reasoning", new Guid("248f5cbe-71c8-ca4a-44fd-37b5fe0b5f4d") },
                    { new Guid("fb41a7e1-5ab0-2b0c-0301-3503400fa1ef"), "Vocabulary", new Guid("c61f716f-e12b-56f1-0c31-327b68a1d74e") },
                    { new Guid("fc3afb88-0b11-95c7-ffbc-883a444b81c9"), "Series", new Guid("2414d6e1-c8bd-053f-5c2e-20a4c5c3a12c") },
                    { new Guid("ff04fa70-ac75-2e19-2e4d-13d728d8d7d0"), "Logical Reasoning", new Guid("99a755c8-4233-88d1-880c-20d737ea5468") },
                    { new Guid("ff2f4384-7149-5be8-3fa7-5de676117758"), "Series", new Guid("a0d8924d-729a-94ff-b85a-f00daaf488fe") },
                    { new Guid("ff3e61df-eeef-122f-0e7b-2270fcac4c69"), "Static GK", new Guid("b16535b0-614d-56cf-6642-6a923a5dd392") },
                    { new Guid("ff9c6170-e5dc-298f-4f6c-602f09cd1d58"), "Static GK", new Guid("f1c3656d-72d3-0b64-1882-cd41023be0eb") },
                    { new Guid("ffb02d4a-2a6b-2b50-832a-0d3dcda3f90c"), "Logical Reasoning", new Guid("c20816a0-b9cf-d402-831c-ba498ec540f9") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0020557e-b5f1-6bb6-fee4-2f5b4968a363"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("00547c11-344b-0af2-52a5-3be310ba9d26"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("016f0210-152a-6759-bd94-81314f14afaf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("023f9096-fd2e-f857-e2cc-9a880c2c296e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("02bf3d63-bebc-c5b0-03a8-d0532bed47db"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0388eb3d-bffc-1410-8276-7424daf2e142"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("039b78ac-dd85-2bba-25c3-14dbe08bd925"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0417b97f-83f2-b62a-d1b8-021962a1b755"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("04d062ed-6b33-b892-beb1-05e9ce3f77e0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("05252edf-ad98-2a1c-19af-8441bfd62c98"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("05d7968c-c16f-dde5-c9e5-38c457b84203"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("05e64683-c8fc-a6f2-87fc-7b5edab79cc1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("06afd08b-21c4-6e0d-c2b2-245263d4ea29"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("070156af-351e-7bee-cb82-c3985ae8095b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("070af5f2-5203-ddab-1d55-67b08af245c8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("076c80d2-4cb3-b427-3ac7-a0ab9f712e5c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("076d451f-955a-6e67-f800-555d26e44e96"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("07a17c1a-fc7e-1112-f9ac-b943fe7df2f0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("08ff95e0-498c-aeaa-c925-b42fe45c30a1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0900e16e-3be5-7654-d6e0-1cccee63a58e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0a40b888-a101-9d09-104e-9951227f3927"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0b516c63-1284-eade-7ef0-3b51d5e29e91"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0bd26f1f-f089-6bfd-2b9c-2c7be96bf00e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0c1669e4-9edd-25e2-9f42-7e1dc7194e33"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0cc7cecf-db30-2ccc-045d-72290f703606"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0d057670-4728-0b13-5614-fd0f65f08aa0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0d703533-000d-571a-73b9-c703c4e2dfa8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0df8fafc-cac3-be62-19b6-8d3b433eff07"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0ecd4150-776d-900a-7364-84d2b0a5775c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0ffec92c-fc83-93ea-e118-9781baad8a95"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10f93e20-c92d-dee1-218a-b3fd666b3cf1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("11ece3ee-ba8d-7dfa-9010-496044a83bc8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("12bdd0b0-f774-9ef6-fdae-aeef606d13fc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("13a241bb-9571-c186-4fee-b655921ff308"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("13b7a905-9bb7-57d4-e846-17f1ebafe873"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("14711777-0e05-f8b0-9306-ea7b2717069d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("148e2f56-266b-d84f-3b55-bd0a320dff18"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1701ff6a-ccf2-150b-914c-cb0a12668d04"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1794dff6-7046-449b-608a-20c7971d2730"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("17f504a3-6345-90ea-dcaa-7984e94bd901"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1839ce22-a7de-a5d9-635c-4386ee086755"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("18f70d11-99e6-364f-12cb-493618fa6857"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("18f865b3-ca5d-ebb3-04a1-1838edb3a527"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("19102d4b-8230-9a43-cda3-176b9f23527e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1984d93e-8494-5993-a8a5-08fa1ab91422"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("198ddd1b-e34e-92c7-eb6e-625f894f58bc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("19cbdf30-fbd5-0ab9-9ca9-95e1a74e4ada"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("19fbb948-d4d9-d8ac-ad31-b13a9984ec6a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1a28d6ef-62de-3aa3-6c3c-3e01f0c72ea5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1b563258-3a47-7bb0-4412-fe4c14fa6ffa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1b671202-02e1-5a2f-ce13-92046503480f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1bbeaaef-1ba5-d95b-b66f-9ac92773952c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1bd8688f-8563-56b2-1882-c7daa80a8ffc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1d8dfb5d-b6cf-284b-5fbd-19f95a0b0b72"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1e887bad-89b5-11de-2fa0-056bf52b6391"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1ebdd1ac-f40a-317b-bc32-12f99ee147e4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1f0a75d3-3b3d-b92a-68d3-8d1dfb324f06"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1f2ccbde-2e10-f388-7da9-fe8f6e9c6164"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1f4672c4-9cfd-c589-0191-00dbc13fd485"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("206e379a-e110-5339-ab8c-91c79c18fc1e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2148a4f8-f077-633a-41c6-4ac1f367e046"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("21ceccef-bf23-3bf4-a5d5-ac5afc49a82d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2233ea37-6693-1d5b-44b0-1850f1c92415"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("223b73ed-a165-125e-5054-b8a7600774fc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2297b959-b0c8-9497-93db-f2d669b7f93a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("230bd032-c13f-4759-c6b1-42e062c7993a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("23670d1d-8303-2b69-60cb-dbac6d52bf48"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("23eb6b63-8cdd-f2e1-a76d-396f74847377"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25f92134-e11b-f21c-7af2-70644fc42de4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("262c0186-dd70-44b2-1e38-a91523ca2786"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("26655a58-b4c6-4440-91a5-a46b4929f13a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2671146a-2491-22d0-9bb4-b0cd933aa880"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("26cd18da-e3c7-6aa6-5217-f878240ddb5f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2761aba9-01e7-8b00-e019-e888dc919892"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("284fb7e8-0ef0-2b69-e5a3-c29f91c4d5cc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("28afcbc1-4187-8994-432a-565cc1f34e13"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("28f095f8-9209-3243-653f-92ab380eced7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("29eea3a2-3ec8-77e7-78ce-4df6b8edce53"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2abf6ded-d3ee-8ad6-a714-8301f059b975"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2b11c80f-a526-cbd8-869e-2e046aee50c8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2b2076c7-1515-3c2d-db52-246842e624b5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2bf10c64-d854-3017-fee4-cd9a406b5e51"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2ef53bca-7e40-dce1-d325-d7c64174fdef"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2f4852df-cfa0-bf58-8cfb-89cd7a03403e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2f547303-89e6-50ea-b714-3683f4288b55"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2f61f563-9925-3be3-0e83-b3e528501517"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("30e6f736-8354-9404-5edc-e99cb072a148"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("310db2c4-282d-346e-cb7e-d963a2da2bdc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("311422fe-4b39-4e72-3690-91891772c0c7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("31d74296-2f88-7948-399e-27f2c004796f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("323829d9-3aea-4ec3-8318-5fdd60e8d5f2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("32d35706-374b-3367-6f74-3aa97a51e3a3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("32e3afa1-0b66-12de-f986-e64aecb89cc8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("33c34be1-a238-6051-bace-6370a7e8441b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3531c4ff-8e05-fe7d-3d51-c7e8c15771d9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("35be9b6d-668e-95a0-180c-b032dc82dd01"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("35c3d728-b008-383a-02ac-ddd1048e7127"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("36fef8c4-63b6-c0c0-ea66-9c3c6c6bf3a8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("37595b12-00d3-f632-94ba-2123a9ac8a42"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("37a4d06e-f4cf-50f2-1d64-750617551c03"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3960d4f3-0854-f87f-77d2-c76e0da61ea4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3979a51e-a640-28d1-fa46-4f1b63829cea"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("39e92cbe-4a6e-03b8-fb42-9ebd025146d9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3b6ec320-289b-b75f-d1c2-8eb99c18210f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3baa284d-bac9-d7cb-c8ce-f89d52a046b6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3d1ebe89-ba46-d97b-ff5d-eddaa164de02"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3dc2f1b9-abab-71ec-93a8-d5cba8f4b8b0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3f3c8e1e-16c6-9a61-d2b3-16be64a0e0eb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4037bed9-8995-7325-4292-79b468e15b05"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4082415b-2834-6ee9-ec19-c7f18661e746"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("40a9a61c-ebe0-9d28-23be-1f2d944eab47"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("41e6ee2b-e131-3815-a87a-78a7970602c7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("42dc6353-17ab-6fa9-4ceb-d9f601cbfd75"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("438fc7b3-cd6d-8403-f243-804c09e9e6ad"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4454109a-447d-076b-033c-64c56705ac45"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("449914f8-fe2e-e4d2-0045-3b38245f48ee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("451418c0-88ab-7e31-625e-766ac0d86f35"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("45cf0714-da47-5256-8013-7cf73376dab7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("45f3bc3a-70cd-a8cf-0f47-9681fe9ae2fb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("468376f5-6597-b1c1-9463-8d0d39c873f7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4779d2f3-f910-30ec-daf3-4b26c7622237"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("48014a2e-25a7-149c-5485-a3dd2ec9e54e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("486f0008-a23e-5d47-13b0-0c67d8d09e75"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("48b7fb14-9557-e6ba-210e-3fb578572e3d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("49f0b26a-dd5c-2e1c-7c67-160dbacac9a4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("49fe9985-def7-833f-a1a1-9e24dc9dfe77"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4b00ac95-425b-37aa-733e-e5438f1da1be"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4c3fbb73-e972-1335-b63a-51dfb458ef9a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4ca39f7f-ad2d-fd8a-3b41-7ba82d6d5857"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4cace7fb-31ca-d19d-814b-e4273b66b21a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4cb37820-828c-f5a1-ad9b-96a798069449"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4e6f9ce2-c712-cd86-1ee4-3bfacd55d428"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4ea4ec87-9da7-055a-4b7b-06fdc5fc9e01"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4ee520bf-11fb-00c8-4322-65266ee4394a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4f23ff9e-a5af-d4bd-f20d-e4fff6d094d2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4f8b33d8-2c1c-366b-1549-a62f5f24a15a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("500fe9fb-1828-0563-010d-daacd496bbbb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5040e040-7952-6808-4bf0-0e0317db20cb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("504f2fd3-31fc-135e-00eb-46937330b1b0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("513bbc7a-a2f2-c2fb-755b-16ec9b22f34b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("51b42134-f203-6da5-94bc-1b34903e8015"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("52698647-866e-e682-b9c2-930c65b83d0c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("532ea53d-1a89-84c8-4393-0bf54bff705d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("534f19a8-186f-ab39-fbb6-4dd7992c1a21"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("53afed7d-878e-7c84-85a9-8db50acadf02"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("542d4779-0685-3b1e-94df-9cd2fd431dec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5485136f-ec4f-7bdf-ec2f-1c053ef69073"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("54c1179c-e6e4-cd62-3da6-0830f1e60958"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("55110bc5-8813-bdd4-834c-1b4032be4aa5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("554ced57-e75a-316c-e8a7-daef8b827010"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("575ea5f9-9a38-042d-fde8-c6fc0f2d7214"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5767400a-ad0c-7dac-fe48-47e68f474255"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("57742d5c-f9ed-798e-acbd-35fcbff1b42a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5775200e-efa7-d1e2-51b1-78e38357afb1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5801c4ad-fa9b-a470-b6d1-005c9df064c3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("58517477-c68c-8160-efff-f26d801bc616"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("58755b7b-fa9e-0700-c12f-28f3e1ac565a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("58ea3e73-7466-7145-3f70-9bf7352f170d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("596c6e87-4b89-eca6-7ffa-db955ceda47a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("59bb4c41-6567-6d60-522b-e195fdc9b0ed"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5a9b577b-7d37-9579-dbb5-d3221f4c50e1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5b7d0fc1-2865-47e5-ca8f-6de4f11f9e71"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5bb82a83-c754-29d8-e50b-4660cc3469b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5c2f9bfc-292e-40ea-29fc-73e541b21de8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5ce25ca0-0551-7fae-6a4b-2ffdfa68db42"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5e913ded-9b28-13b3-e1b3-e921909be1da"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5f1a9b72-274f-3067-8f24-f553ff282184"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5f689989-a450-f5a4-7ef2-82abe4c0409b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5f7f1646-fd70-33a1-93f5-5382e4952d02"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6008d83a-9222-6d06-ae25-a1108012d2eb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("601295d0-a8d8-1c6b-f008-50dea8c4c284"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("60276173-13a7-5eb0-28ac-f18c56f96052"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6064287d-fdfd-c9dc-5526-5f7d40b4a103"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("60930aa4-2100-3f38-ac7f-2306a29afe93"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("61f9b508-c846-f39a-21ba-89ddab9ff9ce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("624853aa-8d76-0537-bff3-eb9a19a36c04"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("62da6521-920e-f77a-9ea9-16671b206fa2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("63708290-96a6-0622-3b81-9d0b21cded94"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("63ddee5b-cf95-3973-bb85-e03759b90b10"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("64424e9a-e470-3316-a64b-7f98a9d2e156"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("64748b13-ea73-0081-d8d4-3fe196ad4a71"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("64798e77-f459-5e76-9480-ad4e9a93d5ee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("65da7b33-7a31-b7b2-82f5-88790496825a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("663ebfc6-ea0e-0aa9-9da0-beec2b5b0b9d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("66567745-b21d-c8e8-b3cb-277750ca568b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("68988e3c-4233-1592-320b-ad50095635d7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("68af1f8b-98f4-d183-fd20-d538d7466d29"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("69636dcf-cc6b-4a9b-0db1-7e6f4045e6a4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("699688bb-d10b-e6c4-8a52-a33ed36ee932"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6a3e0cc2-e69e-523e-ede0-8ec258ae1406"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6a979cc5-030b-e559-1c68-25240e2f4da3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6b4f3203-edac-410c-cb9e-61305f89afd0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6b5a4bf7-8bcc-3ab3-e5c0-3bd9836774b7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6b6bf92b-1df5-864d-97d1-88739f074ba8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6c1e090e-2074-be30-6c7b-9ec9429f9791"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6c2fdb82-16fe-bbaa-8fbe-7fb318189c5a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6d0da312-6ddc-e263-eba1-9b22fdb879ce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6d41176f-a0eb-7077-be93-d48b31059c9b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6f9a0306-bed4-ea69-1da1-70eca90b33e3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6ff3112c-8d6c-d72c-009c-63959105f8a4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("701b4ae9-c09f-ff8f-f224-39910fcdec0d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("72f33926-d7dc-9879-245e-6d1770f766a3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("743b32f3-9cf1-1a12-d59b-75fda739ad79"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("744159b6-4154-4565-bbcd-ba643cbda6b5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7454f1bb-edd9-3ce7-063f-569f2e2d8a9d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("75153855-9434-ee77-f028-f59b199c0fb9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7557a853-225b-4795-d608-3dc3658a8a80"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("75f79c06-4a50-75de-2b8b-08942cd3159b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7675e3ca-8a2e-c820-d7f0-f2482accf6d6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7697886b-bae9-2745-62b7-5038b990b686"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("76b10981-9c40-8cec-cc94-6996d82d0694"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("76e62b47-557a-05c6-0830-b3b88f9d9c86"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("773397b0-a622-1a43-f1a5-950546ff897e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("78a7a905-7c01-0ff4-f8ab-2627cc6fe307"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("792c00bb-f6d1-14f8-8434-a61471181648"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7989c441-afd5-8dd4-a766-cb9ec9a9f403"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7a5a599f-a967-f20d-c1e3-ae933a0b3e19"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7ac2285e-711f-520c-6eb3-ec44c1935e66"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7ac95e74-3874-59b0-5ac9-fbd35045e8d7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7ba6a030-77fd-70e0-7b43-abec2fc6cc0a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7c49e47e-895e-b8dc-a86e-2d75b85da15d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7cba2f9a-c22a-e6ac-0262-71ff5f443a7f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7cf6c3e0-3103-766b-aa2f-69f5769861d0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7d3138df-3223-d4d1-29e4-3fb5a6fd688f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7d363073-3ac3-72b8-0299-afaecc863853"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7dd98435-db8a-3da4-7fe0-9628853f7f8b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7e6ec37b-c48a-44c4-1554-38e727056369"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7ee884ca-a4f1-fedc-c6e6-f1e5d7c9e9dd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7f046fe9-f01c-8382-a470-84e3e449f9f7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7f90e50e-af04-b3e9-e109-9af453875a53"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("800e8387-a6e2-49e6-1aa2-97d9f6b51f13"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("805da04a-7d64-56f6-0db3-44e51d48d263"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("80ddf5cd-8e64-3a46-0ad6-6bda643e54ab"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("817cabab-4316-dfc1-7f51-92b27a9e0aae"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("81a741a6-9749-098a-4ac3-303c839b7ab1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("81b67df5-7b3c-04ab-4975-0290b53f571b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("81ed545f-9123-6b9c-d5e1-26be6d2afd7a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("826abcac-7cf5-4a3f-7a8e-b4d08eea9ca6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("82878b41-5efa-4c80-55a7-48681fc2dfb2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("82af4e24-5d45-8afa-ffc3-49398bb67c96"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("82b8ff6a-8bec-b923-0f0a-d178070498de"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("82bcefd9-c571-29b2-396b-909bed09bc28"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("82d10738-39ac-898b-4ca3-843aad211618"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8356a1dc-1dfa-8b5d-129c-bc13b3ad4db5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("847d2c4d-ca56-2df0-5331-b68615d2c4e5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8529c55f-a3c8-e612-d0b2-b4842e25ceb5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8543bc05-79fa-2d31-1da7-afd7b4394d7e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("85478605-acdf-22a3-7c78-c6db618bb127"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8559a7b4-bc77-78a4-40df-ad697118f0ad"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("85b73170-71d6-8fe0-37a8-81347c409796"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("85b82ae0-3d2f-6764-b28a-c7f21ecba623"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("861b8330-160f-2740-64b8-c8ba35ba33d1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("864c204f-0d31-e469-a27e-adb2e95b15f6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("868ecb21-8a70-1ee6-b27f-841e3b5d5597"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("86985d3b-d3af-d50c-44e7-2c3c41eb0766"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("88ce1a88-a323-9c7d-0260-1a4298944032"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8949b762-e2a4-04b2-1a59-2e6dfb1c3fbf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("89554737-ed3a-f41f-2f91-0546af998fa4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("89a7ab43-b1bb-7a60-3ef7-224ea0e2590f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("89de6fb0-5f38-09b9-5cf8-bcca2638be13"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("89ffbf7b-711e-fd60-89a1-7cada4db1684"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8a94fa4c-687c-6805-6d10-41defc58d108"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8aa4a345-a5bc-6e84-003b-2560f1b3396a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8aad03d1-a6da-07b7-039e-70aab61eeee7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8aec6c46-20bf-f664-2862-3a65fcb70edf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8b18648c-e699-76ff-f361-e6cc7add0c83"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c4b87e9-c5fd-a7ec-806b-3fcf88dfbea3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c61df07-a889-b6a1-2f8b-ccbd538ff8a5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c8c3cc8-524d-e5dc-490f-c311bfe7cdec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c9f58e3-782e-bdbf-05a3-68753a22bae1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8cb25289-4f29-be31-8e46-ca04a2cb9a29"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8cd3b601-d5a4-d069-a727-bd6bb7bec9f7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8ce1e170-5103-51e7-2f1e-478853f55d36"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8cec00b6-9482-6cfa-2de0-a2cb81bd1301"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8f159b85-07d5-2bcc-48ed-bce349bc5137"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8f26edd8-a4f9-2a74-26e5-117f686e7f9e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8f349c27-9981-57cd-f78b-624a48998847"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8fd13f26-afd0-7596-7760-d273a550f701"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8ff44779-b809-d9b3-5d16-47622fe4f5ef"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("90010247-3771-66ff-efa8-3b770c1fc935"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("90a367d1-20f8-e9ed-6ffe-d402cdd9a833"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("90b877a7-d70f-d30b-55ae-d7b56d5aeac5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("916f27e4-2a2d-726d-19e5-6b090f5821c2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("92ec3797-b5ce-c8b3-1499-e5d49c9fd222"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("933d1f57-828d-ced7-cd52-1dbbbff3d8fb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("93a69d90-1186-aced-a996-d8ca600d3537"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("93afc40b-d6ac-a257-c5ff-c7df56800f19"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("93c73661-0d02-882f-1249-4c17bbbcc5a6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9421a447-3109-64a0-50e7-3c56177decf4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("94e07f45-e1d1-d7ff-7895-69fe22acb13c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("95d2f30b-0387-db58-9c6f-45a46cbec322"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9632dbbe-c56b-1786-21c4-79f72dedfc4d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("967df2e2-539f-2504-f24d-52087d0ecd6c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("96b039ca-ee22-212b-6f96-c7cf582631f0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("97176a60-c400-3bd1-32c0-c4758fdeb01b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("972e6bc8-2e47-0472-f2b0-abda3ef74e62"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9762c454-fc7c-da6e-5ef9-ac9f64e52528"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("978e8b45-bb9e-d5b9-c15f-e6be4944537e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("97fba8b6-e647-08f6-41d6-0a71a2c3d870"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("98944fdf-c487-1267-d5e1-81f87c6f89c9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("98a08964-55f3-d2dc-1683-9dc9863a2552"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("998fce9f-e457-c6f3-af90-25a4dcf141ae"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9a17f511-7b57-8bf3-a648-ecea7b91c667"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9afc98a7-c1f5-b576-a539-93b2f0012afd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9c289e6e-067e-2e45-5073-67a5ff38a40d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9c6e46e2-6de3-3b67-a6a9-9a71b0a548aa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9c93f4d0-b195-884f-1748-68ca647a31f7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9db98070-4eb9-643a-c513-8c2f886547c7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9dbb88e3-6f85-9c6e-855f-fa9fd3e773a0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9df17a8c-7214-6773-c963-bbed9a525be3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9f5f0974-72e7-ef38-c15b-b6a76028fcc6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a05ff79f-689f-f33b-d5ed-f4f4ed5b3d6d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a0cbb04b-ff44-72fb-6326-ba15c2acc211"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a0ef1fc2-d996-3f08-5874-c6fdd471a2a8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a2aab89d-be0f-9a8b-ab8d-caa133a11ab5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a2e7aaf7-8134-e9ab-243c-37eb46b05272"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a3fb0573-ca59-eb7e-a393-9f2865e4521c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a453a8fd-79cb-0148-e0c3-b7d7fc5da149"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a453d9cd-e79e-36a4-7075-9d040bae8b14"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a5d82258-d4c2-499a-3d35-64222c70db6f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a63c350b-7c16-4878-70d1-ef4d91f71026"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a66d2914-663f-74be-b7aa-1aef606d15f7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a6e5644c-630b-57da-b7fb-344e770ae5ae"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a6ee14c6-ad07-d275-1e12-42251f9a2e70"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a7440dfb-628e-36d4-f3a3-d66bf34e6de0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a7aa5c72-ecab-1dea-4e40-7ff80b1bef2c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a7f64c46-233f-7e3c-fd01-22a0b6c74c36"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a85d9bc6-99b6-44ce-c13f-152c15aa0184"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a97863c4-762a-698a-2c3a-cae0c6fb0896"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a98ba9f2-5d96-df55-5a0a-4016ecfa6141"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a99ffca4-6289-df84-0364-bc9e14e5e2f5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a9ed3972-ccfd-eeaa-fd7f-01cc50e1a2fa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("aa61c146-24c0-04a7-3ab0-5add6c9e305e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ab1574b3-dd32-1a3d-0aa7-3355567abae3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("abc940f4-bf7f-2894-9516-ddadb77e7990"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ac5d9328-ff0b-3b51-4cbe-745907bf1869"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ad6ac6b4-9518-8320-6738-682d3e4ccaec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("afb9f2fb-3ff6-6fe8-e3e1-9b80264a8822"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0543335-78f6-b2e7-f07a-a0d4103c8afd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0e81daa-a8ce-1546-987c-823925765c5c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0f51de7-805e-951b-f33d-1bb6f9cb6a6d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b15a4f40-64b7-cdc4-3d67-0ed315720f07"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b1872c04-cac8-7192-10a0-419c215ef0ea"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b22b241c-d2d9-eb2e-4560-d28d3d372f97"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b2cff68f-c7f1-078a-c426-2255247ff62c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b301e02b-df3d-225f-952c-7812c598c586"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b51c48c4-2ac5-aecc-68a4-215d7eee98cf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b573951d-057e-4fa5-7803-f0b9213b39ef"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b5966155-13c8-b1ec-372d-861fcb75a043"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b6b68070-a1fc-da72-250f-973b111d67b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b778a206-66c7-d38e-5b44-c533ef008f32"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b803ad16-62e1-92ff-e850-92d351ae2bf6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b851b628-d727-44cf-6da3-56f7b53b5f37"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b86008e1-e587-fb09-5829-95c89098d472"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b8bfe2ba-de0a-91dd-9c3d-0801933275d7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ba973e5a-d689-ccbc-d39f-c2be964585e4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bac7075e-a2ca-a3e4-1bee-8057c411e1a9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bb5a75ef-4f26-bfe9-35b0-28a5682c66c4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bba90cfa-50bd-fa42-3834-e041aac5f9ce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bbc48ed6-cc8a-b358-1278-03f42057b87d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bbe703fb-744d-3726-a754-74d232d4627f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bc238d01-1088-8448-17f6-148add5f29e4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bc6d2402-52ed-5d19-d9ff-4984e04240f8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bcd237f7-b385-1be9-44d3-ed9a4e2e7f0d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bce0c8e5-eda8-498b-b3bb-89bb04cc7614"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bdbd1027-9d26-6428-7f18-7d3912563dfd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bebc7b5e-5c99-11c1-4299-5844f59f6e51"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bed93e55-0d8f-ccb4-b215-0187ce215137"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bf8436e4-524b-7cf2-6c46-36b35929f027"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bfcea2e1-4f52-0f2b-60bb-47a30386fad5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c0340117-0739-670b-2350-05ed5f3030a2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c098975f-d9d5-22cf-8fa0-f24052e7f0f8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c131ea07-ae5b-0ccc-fd8a-0a8d2bb0c6a0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c146764c-f056-5d31-ac33-9f11d51a5035"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c18f9071-d486-6d52-669a-083c9bc9fff1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c31fa477-49f0-bf5b-636f-7b061e16e113"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c3292938-a522-ab51-9bcf-ac412691e12a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c33a5f83-20fd-1e38-2547-2ee265faf7da"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c48c119d-469a-2765-e868-40a9ca65a097"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c5ce6c11-7617-37df-d4f1-0c5c41cc3a7a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c783d86e-972f-da4c-fabd-43017e307d48"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c8078c89-52d1-e7c7-9715-56ce76dfd7fa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c908d7cc-dee9-d914-cf26-e4ce0890ecd0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ca50d26a-7d20-1fc9-ba69-09d4ecbd831f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cb8c2d51-c236-0a94-de6d-90a93d7e1f96"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ccd719d1-0d27-f153-6128-c85c8f44b57e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ccdcb5c2-d14d-f804-8fc8-c415aa058b7f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cd0e5dcb-b639-4473-b37a-e02ab984b77f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cdf71698-0faf-16d1-a54c-028e47f0b679"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ceb131f3-4546-54ca-6176-709a0276d7c0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cec2ce7c-db27-48ac-322d-f0430b184c87"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ced36879-a42f-d02e-2d73-baed492e8e5f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cf359ec8-8985-280f-e942-69949cfe5268"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cf67d021-95ff-f399-4e99-69860a206747"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d06cd634-70e7-d19e-fb28-5cba4c5e03ab"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d1360cd6-af75-b88d-09c7-1cddd615e662"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d252ea34-a7d1-c8a1-4daf-6997a51851b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d2bd111e-f284-0cf6-b774-620cf28d18d1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d2d4e09a-19e6-6c64-7985-0c1573027446"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d30cc72c-11d6-89ff-a2d0-cb1f3136d9f0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d32ed2e7-9600-5e4a-9beb-e30c75f6ebfa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d352b1af-2df6-fcb3-275f-b57f733754ee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d61bce4a-82a7-1aaf-a796-14e377da4420"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d6c38060-a21b-b877-3674-89ca499c0126"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d6f708f6-e94b-ec80-b4b1-5899f0bc6eaa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d7db6ea3-22a8-18d4-8b41-b2ff426168c2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d902122c-fad9-3eb6-14bc-725781b49966"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d950adda-b328-7c68-4b72-1009e2945dfb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d97c8251-70d8-e38b-c30f-36753967a123"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("da116b38-179e-8abd-9b7e-804a7abac8cc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("da13d411-832b-e88d-4df3-030dd09e6666"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("da51763d-aecb-6e0f-47db-1be3d1d71118"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("da55eb1f-5742-6a11-46cd-ce969c05e999"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dbc8df23-d9f0-11cf-7cdd-5eae9445358d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dc88a1b7-68f5-9425-e677-1a2e3ee6533e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dcba5d04-f337-967a-070d-f79a9859b06a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dd62db5b-9105-bcf8-1979-47b414ad95af"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ddfccc8f-3bd2-d903-6a19-c687b358bd33"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("de53b09a-ca73-620f-4e2a-175256bceee6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("def58ca2-1a5f-a9ab-8272-79fb054ea20f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df050cb1-0611-4116-91ca-129196a030c0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df1f0485-ffcd-fdd4-ea51-93523fd0638d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df5b785e-1631-b224-0e5a-8b2da2afdb96"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e0c69ee8-b33e-5db2-7523-42ca280623b8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e104424c-70af-2768-b47f-9281dff0d1a1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e15377e7-de81-0701-9fd0-19fb0545fdc3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e33da27e-f6ae-ed56-933d-8e045e58eeeb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e34bcce9-d0c4-b84d-e783-8dc4f5587b8b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e368f200-5387-dd5c-fa28-825c00227ee8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e3a00dac-80f1-9b7c-6a88-1048db765d7a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e4996695-5fb3-0bb1-106a-18aa3719e3cd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e513f51f-c010-b2d3-c403-e2bc314cedd6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e5518f58-b8b8-f7f2-ccfc-c20641917dd7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e59b69ae-c704-4ad0-6532-a3ddcdbc41fe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e64efba8-1209-19e4-470b-a9178f3b7cda"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e7237bd3-ff4a-4e1b-977d-511d366ecc95"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e73d6a48-b0bf-df10-ea89-3ce89c260486"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e91acc5c-3e35-ccc3-c8b7-e4d69168d747"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ea79b14d-05d9-3a71-50c9-5b58b03a2a58"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ec593206-ddb7-a3c0-b0bb-e7f7e90a0910"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ed1220dc-b674-256c-633a-033de90bf0cf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ed515b45-1900-b65a-2314-c400b2d81a34"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ed739b92-4366-dd26-ea0e-06b507689406"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ee1efa0d-e9c9-59f5-a493-9fb54343eb54"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ee40e0af-b271-7912-8150-259764cd94cf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("eeaf3755-7e4f-1af0-b010-7fae87386b40"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ef1a1201-0c59-93e5-b366-b5763bbb1c88"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ef3a7600-d4c3-9e83-c15b-70983450c9d4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ef4c12a0-703d-70cc-7c89-1abb0416753c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f0184b8f-24c8-c760-105f-ece29b8cfb4c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f085f01b-6208-b67b-ce90-5415f93ae82c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f259f83c-a8e8-e7f5-cc27-563c2ce0ac5b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f26e15e8-c2df-e872-aa95-ea633ae0e298"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f281b20d-efcc-3790-391a-ca68fb4f945d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f32fe25f-0949-4b60-57ba-ed4afdad03b6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f3907dfd-6c63-70e7-53f1-a80d1a1a614a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f3c17282-bcd9-1055-05a3-df044bd95927"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f4818d27-436c-1c8b-1dc9-d3f5a42eac9b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f5402942-7490-45a7-365e-15cfef247169"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f56a66a5-d309-c856-b122-41c051852d22"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f7af568f-ebad-a5e7-aaf4-8c5192d13f07"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f8171cb0-8f67-beac-4009-9adfa7f73ffa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f8327dc3-3875-2b07-c02d-cd58db6183ec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f886413d-ef38-f5a7-1ab7-69ed21d0c6e9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f88644d0-8897-29fc-a063-873ced623c34"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f9833804-323c-5faa-8b19-c4d1ce455cad"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f98d103c-4959-63de-ba2a-aec4a8346d21"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f9d82d04-b341-d310-9f7f-142ee398b5cb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f9f3e468-d15b-29bf-a586-1506f520080c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("faa96723-68ea-a03c-acb7-8f98bbdf800a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fab63582-9134-0ac3-a4ce-7116b6df7545"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("facc3403-a59a-0c68-3479-5089a59416f9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fb2d5cd4-7ac4-3014-6f2e-91a1837d8a5f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fb41a7e1-5ab0-2b0c-0301-3503400fa1ef"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fc3afb88-0b11-95c7-ffbc-883a444b81c9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ff04fa70-ac75-2e19-2e4d-13d728d8d7d0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ff2f4384-7149-5be8-3fa7-5de676117758"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ff3e61df-eeef-122f-0e7b-2270fcac4c69"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ff9c6170-e5dc-298f-4f6c-602f09cd1d58"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ffb02d4a-2a6b-2b50-832a-0d3dcda3f90c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0426bd63-650d-39e6-ba91-32a1bf2c0188"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("04e69404-9b8a-d643-b48a-8710d2f73d29"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0886696f-48c8-891e-a7d5-8550fb0d7021"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("096355bb-6cb5-8220-abc3-6fe02899a819"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0aefdf79-fe7f-9bdc-e978-fed568e3b1d4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0b52a18c-3ced-19d8-b9d3-dc053e587ad4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0c484af1-d5d5-453c-06a7-067c5d5a87fb"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0dc1832f-ee53-b526-2499-2645419c24e7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0dfd2625-89ac-f13e-6b38-e3c0d8d0a2c4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0e77bff5-deff-a943-0610-fd871552ad7b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0ed4b1a8-e05d-fa4c-423a-150036463aff"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0edd4df9-10c6-ab16-4384-f3cc03c77e3e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0f9aae8b-5730-91c1-4eba-dcab68ef87d4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("1a1e72e3-c129-2580-72a0-51e0c5e8c51a"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("1fba6d2c-20e5-6bb6-6119-cfefd37d187a"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("205fdf02-2719-c1b7-be92-666228b6e3e7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("21b78efb-e9e3-8cc5-1d58-8cc5510842a7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("22ed3e15-caf6-1025-37b9-8459eaae6fe4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("239d45ea-f0cb-4232-5fea-bd7c5cbe3ab3"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("2414d6e1-c8bd-053f-5c2e-20a4c5c3a12c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("248f5cbe-71c8-ca4a-44fd-37b5fe0b5f4d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("26aeaf8e-cccf-0ceb-5682-ff5da0b6d543"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("2922679e-6588-e91d-2617-07abd23a9823"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("29cbfd96-bd12-d1e4-10ad-43a1fecbfe56"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("2b57ea8f-d5a6-227e-b079-b81ae68d0267"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("2df66c1d-c139-3764-c529-61c645d4245b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("30ae9e12-3ceb-88c9-5e54-a00e67f69df4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("30b331ed-e430-7bae-e7ce-f589f35517eb"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("30c33b7b-9e1f-487b-fd59-2872a64fe595"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3206020e-1e1a-d766-d66b-6d604baf5616"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("322fb74b-b528-7a96-495c-39fc1d728a2e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3249993c-ee08-0d52-5e33-5def559739ce"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3c40893b-b1da-0e77-c6e6-27559f0200bb"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3c7bd5f1-989b-5d61-d6e4-f5af3a101227"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3e6077cf-a585-a5b4-8058-0ab46cef9880"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3ebe7357-d069-0030-d517-fbd60e30a263"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3f9b3521-896b-6a86-c006-f74c596b124d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("42599220-bbb4-8c25-dfa1-f63ee080c8dd"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("42cdbe8c-d2ae-443c-1d7d-586a0e8f42f5"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("43cc05df-90fb-d3a9-3329-e42168f33a77"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4773c0d2-b3ca-c94d-9849-c745b4f3dba8"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("47bc02e3-9998-79d2-ca9c-e4c3b751689e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("482cd3d1-1df4-67f3-a3bd-b273d9892ab8"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("48edbf68-6220-167c-9c06-21954ca206e9"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("48f0f198-01af-1a80-8f50-aacffbb6440e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("49d1d6ba-8541-3024-5413-135b5663003c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("49d6e9c7-e86b-daf0-0a22-16cfa49fc212"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4c110e73-7f94-2f66-ba13-eb0bd5733804"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4d5881bc-d0b4-0d9e-4975-1dd816dfd3e5"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4eeb5fcc-8d45-97a3-9f21-8838d2db2c9b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("5006ddb3-b59d-627a-0a54-9567cad71a31"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("5392ef0e-3dbf-d6d5-8adc-9f074c7b0f3e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("55e88ad4-b31c-ecca-8211-a5d39afca9c1"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("59a1391d-7a39-5360-9fb3-832bdb2cac79"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("5ab6da55-694a-0a7f-bd80-a63079edf15b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("5c4b02f0-6369-076a-31b2-4e64db0b316f"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("5c99f7b3-af3e-e99e-0824-9d55cbc055a1"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("60150aea-1845-3cb3-002d-8c8b1e3eddd7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("60d5154d-3d43-60dd-b21d-0943056545bd"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("61f44871-630e-0b34-8fad-ca4cc042320b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("633b28ab-6d5a-dd92-ce09-65145ac09269"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("64eda4df-4f57-2a64-b542-f9d8d2463c0d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("65e36679-c743-faf2-6191-8a6c5f266f04"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6850b500-1bb1-e162-189f-045a15d9a0f2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("68d95e07-877c-0cb9-0c71-381742807ddb"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("68dd4f56-21f3-72c9-4a78-6ac785f4ac6e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("69c2b215-e4dd-c22f-48ba-6affabc79966"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6b36ab48-4a04-61ec-0b7f-57884fea536a"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6cfc7e4b-94e6-d2cd-da48-a516fe9aa24b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6d81ff1c-ca17-baed-b320-3fe9b11eb2f5"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6dfdb9f3-fcc7-d8cd-9bb7-4b366d335b84"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6e5381c2-aa75-e19e-eac4-a983be83afd5"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("74afdb9b-0e1c-94b9-765e-d720199d450d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("77075acf-29eb-d1e7-f247-9b164a9d9b87"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("79200f88-1d99-cfa8-0aa9-2b2077af2f6a"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7ae414ef-143b-4d3e-20b1-ea9fcc461547"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7b2364df-0c2b-26f8-281d-af4d6518681b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7bd4bbc7-818f-47aa-ea61-1894b73d2186"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7dac52a4-15f6-e83e-3143-62647c5adf4c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7e4344c7-802a-1dbd-6a3e-d8b0b12e7666"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7f5ddba9-3e27-eeb8-eb18-e306a8d57622"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7fdd795a-6994-63d6-fd5a-9372678e5143"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("82df9511-fd16-f5b9-3da2-d877a07ace7c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("85986333-45ff-1990-65d6-97248deb6d08"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("867f98d1-22e1-c9e6-7eb9-a2030b743d9b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8910836e-bf37-7b12-dfcd-fc7cb4ef7495"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8ac4ada6-4775-eeb0-d87f-32383572fbe6"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8dd5c1f6-3406-e8a6-de08-130071a44989"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8eebe33a-8ce2-3bed-d6d7-55d057d7fff4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8fc32dcf-6b85-d7b7-c9ee-dfeb8347435a"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("91eebe88-16a5-addf-707d-06768eed2e82"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("92f3676e-2bdc-7c39-31ba-7e057faaac57"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("93b5cded-e275-4edf-d41c-341fb7954bda"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("93dedde1-bc85-8d54-8ab5-0f407da7fd28"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("94994903-a722-d664-d90f-c27454228d9a"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("94ae42f3-35b8-aa97-694c-77c3df885c64"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("99a755c8-4233-88d1-880c-20d737ea5468"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("99d290b8-8163-35de-4061-d29f52183491"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("9bc50977-c5e3-4eea-99fd-100bcca73458"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("9c860696-a5de-ec1a-83b1-d00dc21e8195"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("9ffa0b37-7671-52d4-b662-9254099ae239"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a0d8924d-729a-94ff-b85a-f00daaf488fe"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a6f8ba35-9571-9bb2-c4e6-9f3f32950dba"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a8310bb0-30f7-59aa-4793-e850695b69bd"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a8debe0a-0d78-d1f1-bcd3-3b60b0dced1b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("acca5304-c9b6-1e28-83e3-9418b80c1e6b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("af6317d8-4f29-7dfd-c1c5-a4049d275f5e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b16535b0-614d-56cf-6642-6a923a5dd392"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b4166db5-ba06-8de6-35b8-16212d900202"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b490c3cf-581e-aeff-e211-ff42df02629d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b4f4bfc1-87b3-db9d-6a37-509f32357111"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b52fdd07-25c4-2456-59fb-fe754430b45d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b6ca55cb-52d9-e35b-dfa2-31a59bccc342"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("bb1e9892-8176-5fb1-bf07-97c46b34100a"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("bd15903d-a370-057d-93a0-e5a6d37c5da2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("be180ffa-7d82-cf64-ef60-02f431e2a51b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("bf8a799a-ce3c-e1be-0d3c-dee172a73ab7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("bfee3ae6-d5d6-961b-60fb-6bf1d5b8710f"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c20816a0-b9cf-d402-831c-ba498ec540f9"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c32886cb-768d-7fd9-6835-1ffdb5acc3fe"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c36cae96-b91f-e4cb-756c-1fd7d8fb219d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c3e0cacd-ad3d-f8bd-26e3-9c10f6ee4640"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c49953ee-92c5-9df9-4540-39edf8bfac34"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c5d5cec4-aac8-73c0-3c4c-8dbd6c3c9648"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c61f716f-e12b-56f1-0c31-327b68a1d74e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c63a3fe7-693e-4566-3760-572ad7dca8af"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c67a192c-593e-5ad9-5b5a-d04633886269"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c6e37724-21e2-8f38-96a9-202971a82ef3"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c9d07602-cc8d-5173-33d0-77c500336aad"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c9e63985-7226-db07-0a90-f2afff9e4532"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cab2a243-5606-3f5c-4a75-bdc6bd08b177"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cba95b28-a792-279a-eebf-0931e088750d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cc6f069c-72e0-2320-6097-e8d0a82c0af3"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cdf661da-b824-ace3-cdc1-9226ffa65710"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cf0ceb04-8549-49d0-9e2a-0d3c138d8a3b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cf43bf94-c993-8ac9-c2fa-2107009d615b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cf584bf4-6877-b592-3b0e-cf2de432ab68"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cfe68c14-de43-9914-1f1b-5cfec539a364"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("d2f289ff-4632-f941-4428-3be85994e398"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("d4e88339-8242-61dd-1109-5c9b8c3239ec"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("d82ee6fe-0ae5-5559-d306-2dc93913f7b9"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("d941bee7-d351-3aba-8b25-87c9f3a990ec"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("da9b8a63-d9d3-3904-7359-949ef4d1459d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("dedccbd1-0495-d064-9dd2-9bb2e272e966"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("df3fcbcc-92eb-58b6-723d-d8b54fa4ab6c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("df7000e9-4ce7-789f-6611-ea0bbc407cbe"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e539d4f4-ed7f-2082-a12c-b64118823689"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e6059fdb-adc3-7728-fa78-d089919f5f5e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e646bc26-8933-29ad-2167-39b0c2be0883"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e8adf44e-22ea-249b-42e4-2f7855ddf194"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("eafbcc0a-5071-d42c-677a-58fd0374cfba"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f13e3c99-cdf7-5843-f4b3-90e1c409402b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f1c3656d-72d3-0b64-1882-cd41023be0eb"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f2814268-ca6c-b23b-582b-d2698cd4c0ec"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f528c47d-c50a-1954-7b5b-647d28e075f2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f7d81f71-e69b-1efd-a0dc-7600bce80b21"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f8eb512b-7061-90ee-218d-f79f85013f49"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("fb9a1cab-1799-1c4f-6d6b-8ddcfff1099d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("fc9e5144-f3e5-f26c-a6b1-3a80f0684fc2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("fd19c7c8-1b31-50de-adc4-97bca17deb85"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("0109ec90-48c8-fe15-3bbd-b0d0aea6002a"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("02649287-aaf7-1427-3199-e9c77d3d995c"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("0716af56-8176-1a84-63ff-9c6a32622a47"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("140935c2-6291-87af-2235-b79257104223"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("20e298fd-e875-24aa-2aba-f154dae969cf"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("29ed97c5-1abf-6a8d-3bf8-7850cbe9da48"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("2fc9f791-98f8-764e-425b-2db2e475e097"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("33aaa431-3499-88ad-ec42-3e68799bbc41"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("3db7f32f-cbf5-62ce-5b13-082a0040630d"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("4b998bea-363d-ad7a-0762-1ef391ab3879"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("53039780-1877-af50-8ae1-cd0d62ac09cb"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("573971e6-54de-cf2b-01b1-6b00fbd3beac"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("5bedb96b-fbf0-3cd3-75dc-d3d098b39ecd"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("617afb6b-29bd-edd9-013b-2ede179e112c"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("660bf600-82b9-241d-6d7b-92ebc1a6c74b"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("75477ff3-5e3a-f204-bfaf-867f5e8b8c8e"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("7aa0a8e6-fc98-de28-1ded-553cac8eb425"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("7e81c179-2df8-07f4-0e53-8a622b5f17b3"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("9002fb31-c048-fed1-a656-82924be87ad4"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("94e66efc-b100-6412-161e-6f98e932dcd1"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("955330fb-c23a-6121-f52c-5148ae7c8a9d"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("a3ea00f3-2d2f-4107-d4dc-8e01a1a7c1e8"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("a44acf4a-c0ef-33bc-6991-fbb580912bb9"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("a54ba431-50d9-5d6b-c28c-172bd8196258"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("a74e2eea-17d5-0f84-16b6-e8c20028cf92"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("ad1dc0fa-deb6-660f-b109-411e8cd8c182"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("ae84e8e7-10d0-b1d1-aee9-a180a547d38d"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("af15cbbe-31cf-9016-fc81-b4e041d39025"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("b40cd678-0fda-0b80-f6cb-da6f67528f17"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("b4394a85-b058-c2fb-8065-d60b8829474c"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("bc843aea-4f35-9b06-3f66-97614da52ecc"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("bc909a90-7e7b-96fe-431f-9e4e073f5371"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("beb716b4-7120-d231-cb9f-033720fc5ba4"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("c294a04b-502d-6d18-177e-795410ff4b91"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("ce94cedb-a7c1-dc26-b3e2-58e090c73790"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("cf818614-e920-ee64-2bfd-df896886f307"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("d95a0b78-89e3-faff-2356-5f568460e1e4"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("dc88fe6e-6268-f8f7-5347-9c8466574da4"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("eb0bce1d-2230-8142-e37b-120396c1167d"));

            migrationBuilder.DeleteData(
                table: "ExamStages",
                keyColumn: "Id",
                keyValue: new Guid("fb34e05b-9dd2-11d1-509f-35ff00f7b2fd"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("01ed612f-d0e1-b799-58fe-04272c052166"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("0d2e5bed-b211-5add-f025-3ae3d5f1a50e"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("0f8155b3-856e-b417-a59c-469ffb8110f4"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("0fae0d7d-dd59-4346-3bf3-f682d2de291b"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("115d0ce3-91ec-9925-32d0-e67eb97f8c1a"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("14d36c47-2010-4872-8481-64301f48d37b"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("17b2cbae-241d-31a2-915e-620bf7ad08c0"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("1c6e410e-80e8-20b3-b612-ad966de94f48"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("1e7bd3d6-2c6b-d230-cf4d-329d5a046bdc"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("22ea299e-04da-0a03-4cc8-d138b621b9d1"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("25ef7a86-ce44-fd78-4f81-0688a9163dbd"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("28cfeb5b-0dbc-836e-224d-3545a0691cd0"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("2f6c21bb-9f0d-d631-4b38-948b63d5ac96"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("3ad9ae3e-8f52-79e6-d8ce-b3e21dce6576"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("4061d256-6119-a453-dbc5-d5a0f20b3914"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("4210a105-d2c7-523e-4aa8-5007da4903c7"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("44c9aadb-ad67-1fcc-9b26-2c683fc5405c"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("4a3c4aff-a8a0-15e0-2475-bcc12de9d71b"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("5173fd10-a78d-0ee0-1d05-1dcbe924c5ec"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("52112b11-75c4-8710-6598-7d5d740b3118"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("5a36f5c8-4696-4467-4c72-fb8e04fdce5d"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("6446a1ca-308d-799f-3d14-19e396b0501e"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("831881f3-0c28-8ca0-f835-9deb2c83b2cd"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("90f766dc-b2a5-d960-5d49-bb218e703ad6"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("af08d38b-cbf5-fa88-812f-8f843932c902"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("b6fcc5c7-6540-dbe3-3cab-9fc71e72a0ed"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("bd68ec8d-6664-8323-4263-deb347b82226"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("ceadd86e-83ff-96ca-3fab-129c16982e63"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("cee1596f-d5c8-6eb8-e2a7-214ea5e3e7f0"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("cf651407-313d-497d-b71b-a47fcb56f32d"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("d4298507-6165-c070-fdff-b9532bb9ff81"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("d6c5dd45-1a1f-9337-c1ad-603bfe687b03"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("d79f3259-1cf2-646d-15f9-e8f314a9d9d1"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("d951e176-1ad3-6ae4-7547-31cb15aa0501"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("e52c40c3-ffda-ecef-2f71-9461cca27782"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("e9dab7c0-bbf4-8798-a1ca-62959c568082"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("eef5343d-befb-84e2-8d32-f0eb54c0d00f"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("f01c3e41-20ab-a1ba-beaf-d411c1fb3892"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("f1e36386-fdd0-eb23-5904-6abd9742bb93"));

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: new Guid("fe61ec0a-b7d5-eb45-1e2c-9a8d40cfce21"));
        }
    }
}
