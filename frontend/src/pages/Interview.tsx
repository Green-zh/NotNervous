import { useLocation, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";

const Interview = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const interviewData = location.state;

  if (!interviewData) {
    navigate("/setup");
    return null;
  }

  // This is a mock function to simulate ending the interview and generating a report
  const handleEndInterview = () => {
    // Mock report data - in a real application, this would come from the AI's analysis
    const mockReportData = {
      jobTitle: interviewData.role,
      company: interviewData.company,
      questionsAndTime: [
        { question: "Tell me about your experience with React", timeSpent: 5 },
        { question: "How do you handle state management?", timeSpent: 7 },
        { question: "Describe a challenging project", timeSpent: 8 },
      ],
      performanceScore: {
        technicalProficiency: 4.2,
        communication: 3.8,
        roleSpecificRequirements: 4.0,
      },
      improvementSuggestions: [
        "Provide more specific examples when discussing past projects",
        "Consider elaborating more on your problem-solving approach",
        "Include more metrics and quantifiable results in your answers",
      ],
    };

    navigate("/report", { state: mockReportData });
  };

  return (
    <div className="max-w-4xl mx-auto animate-fade-in">
      <Card className="p-8">
        <h1 className="text-3xl font-bold text-primary mb-8 text-center">
          Interview in Progress
        </h1>
        <p className="text-center text-gray-600 mb-8">
          This feature is coming soon! We're working on integrating real-time AI
          interactions.
        </p>
        <div className="flex justify-center">
          <Button onClick={handleEndInterview}>End Interview & View Report</Button>
        </div>
      </Card>
    </div>
  );
};

export default Interview;