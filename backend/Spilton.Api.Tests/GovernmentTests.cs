using System.Text.Json;
using Spilton.Api.Government;
using Spilton.Api.Documents;
namespace Spilton.Api.Tests;
public class GovernmentTests
{
 [Theory][InlineData("javascript:alert(1)")][InlineData("http://ssc.gov.in")][InlineData("https://127.0.0.1/a")][InlineData("https://user:password@example.com/a")][InlineData("https://localhost/a")][InlineData("https://server.internal/a")]
 public void Unsafe_URL_rejected(string url)=>Assert.False(SourcePolicy.SafeUrl(url));
 [Fact]public void Source_ranking_is_transparent(){Assert.True(SourcePolicy.Rank("OFFICIAL")>SourcePolicy.Rank("TRUSTED_SECONDARY"));Assert.True(SourcePolicy.Rank("TRUSTED_SECONDARY")>SourcePolicy.Rank("USER_UPLOADED"));Assert.True(SourcePolicy.Rank("USER_UPLOADED")>SourcePolicy.Rank("UNVERIFIED"));}
 [Fact]public void Eligibility_missing_ambiguous_and_complete_rules(){var fields=new List<NotificationField>{new(){Name="Age",Quote="Age: 21 to 30 years",Page=1},new(){Name="Qualification",Quote="Qualification: bachelor degree.",Page=2}};
 string Status(EligibilityInput input)=>JsonSerializer.SerializeToElement(EligibilityEngine.Check(fields,input)).GetProperty("overall").GetString()!;
 Assert.Equal("INSUFFICIENT_INFORMATION",Status(new(null,"BACHELOR_OR_HIGHER",true)));Assert.Equal("LIKELY_ELIGIBLE",Status(new(25,"BACHELOR_OR_HIGHER",true)));Assert.Equal("LIKELY_NOT_ELIGIBLE",Status(new(45,"BACHELOR_OR_HIGHER",true)));Assert.Equal("NEEDS_MANUAL_REVIEW",Status(new(25,"BACHELOR_OR_HIGHER",false)));Assert.Equal("NEEDS_MANUAL_REVIEW",Status(new(25,"BACHELOR_OR_HIGHER",true,true)));
 fields.Add(new(){Name="Age",Quote="Age: 18 to 27 years"});Assert.Equal("NEEDS_MANUAL_REVIEW",Status(new(25,"BACHELOR_OR_HIGHER",true)));
 }
 [Fact]public void Field_quotes_retain_real_source_and_paper_counts_use_actual_lines(){var chunk=new DocumentChunk{Content="Qualification: bachelor degree.\nAge: 21 to 30 years\n1. What is 25 percent of 480?\n2. Find the triangle angle.",PageNumber=7};var fields=EvidenceAnalysis.Extract([chunk]);Assert.All(fields,f=>{Assert.Equal(chunk.Id,f.ChunkId);Assert.Equal(7,f.Page);Assert.Contains(f.Quote,chunk.Content);});var paper=JsonSerializer.SerializeToElement(EvidenceAnalysis.Paper([chunk]));Assert.Equal(2,paper.GetProperty("detectedQuestionCount").GetInt32());}
}
