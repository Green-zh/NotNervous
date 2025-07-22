import { useLocation, useNavigate } from "react-router-dom";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Button } from "@/components/ui/button";
import { CalendarPlus } from "lucide-react";

interface QuestionTime {
  question: string;
  timeSpent: number;
}

interface PerformanceScore {
  technicalProficiency: number;
  communication: number;
  roleSpecificRequirements: number;
}

interface ReportData {
  jobTitle: string;
  company: string;
  questionsAndTime: QuestionTime[];
  performanceScore: PerformanceScore;
  improvementSuggestions: string[];
}

const Report = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const reportData = location.state as ReportData;

  if (!reportData) {
    navigate("/setup");
    return null;
  }

  const calculateAverageScore = () => {
    const { technicalProficiency, communication, roleSpecificRequirements } =
      reportData.performanceScore;
    return (
      (technicalProficiency + communication + roleSpecificRequirements) / 3
    ).toFixed(1);
  };

  const getPerformancePercentile = () => {
    const score = parseFloat(calculateAverageScore());
    const percentile = Math.min(Math.round((score / 5) * 100), 95);
    return percentile;
  };

  const renderScoreBadge = (score: number) => {
    let color = "default";
    if (score >= 4) color = "default";
    else if (score >= 3) color = "secondary";
    else color = "destructive";

    return (
      <Badge variant={color as "default" | "secondary" | "destructive"}>
        {score.toFixed(1)}/5.0
      </Badge>
    );
  };

  return (
    <div className="container mx-auto py-8 max-w-4xl space-y-6 animate-fade-in bg-gray-50">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-800">Interview Report</h1>
        <Button 
          onClick={() => navigate("/setup")} 
          className="gap-2 bg-primary text-white hover:bg-primary-light transition-colors"
        >
          <CalendarPlus className="h-4 w-4" />
          Schedule New Interview
        </Button>
      </div>
      <Card className="shadow-sm border-gray-100">
        <CardHeader className="bg-white">
          <CardTitle className="text-2xl text-gray-800">Interview Report</CardTitle>
          <p className="text-gray-600">
            {reportData.jobTitle} at {reportData.company}
          </p>
        </CardHeader>
        <CardContent className="space-y-6">
          <div>
            <h3 className="text-lg font-semibold mb-3 text-gray-800">Questions Summary</h3>
            <ScrollArea className="h-[200px] rounded-md border border-gray-100 p-4 bg-white">
              {reportData.questionsAndTime.map((item, index) => (
                <div
                  key={index}
                  className="flex justify-between items-center py-2 border-b border-gray-100 last:border-0"
                >
                  <span className="text-sm text-gray-700">{item.question}</span>
                  <Badge variant="secondary">{item.timeSpent} min</Badge>
                </div>
              ))}
            </ScrollArea>
          </div>

          <div>
            <h3 className="text-lg font-semibold mb-3 text-gray-800">Performance Score</h3>
            <div className="grid gap-4 md:grid-cols-2">
              <Card className="shadow-sm border-gray-100">
                <CardHeader>
                  <CardTitle className="text-lg text-gray-800">Overall Score</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-2">
                    <div className="text-3xl font-bold text-primary">
                      {calculateAverageScore()}/5.0
                    </div>
                    <p className="text-sm text-gray-600">
                      You have outperformed {getPerformancePercentile()}% of candidates 
                      interviewing for {reportData.jobTitle} positions
                    </p>
                  </div>
                </CardContent>
              </Card>
              <Card className="shadow-sm border-gray-100">
                <CardContent className="pt-6">
                  <div className="space-y-2">
                    <div className="flex justify-between items-center">
                      <span className="text-gray-700">Technical Proficiency</span>
                      {renderScoreBadge(reportData.performanceScore.technicalProficiency)}
                    </div>
                    <div className="flex justify-between items-center">
                      <span className="text-gray-700">Communication</span>
                      {renderScoreBadge(reportData.performanceScore.communication)}
                    </div>
                    <div className="flex justify-between items-center">
                      <span className="text-gray-700">Role-Specific Requirements</span>
                      {renderScoreBadge(
                        reportData.performanceScore.roleSpecificRequirements
                      )}
                    </div>
                  </div>
                </CardContent>
              </Card>
            </div>
          </div>

          <div>
            <h3 className="text-lg font-semibold mb-3 text-gray-800">
              Improvement Suggestions
            </h3>
            <Card className="shadow-sm border-gray-100">
              <CardContent className="pt-6">
                <ul className="list-disc pl-5 space-y-2">
                  {reportData.improvementSuggestions.map((suggestion, index) => (
                    <li key={index} className="text-gray-600">
                      {suggestion}
                    </li>
                  ))}
                </ul>
              </CardContent>
            </Card>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default Report;