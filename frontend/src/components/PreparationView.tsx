import { useLocation, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";

export const PreparationView = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const interviewData = location.state;

  if (!interviewData) {
    navigate("/setup");
    return null;
  }

  return (
    <div className="animate-fade-in space-y-8">
      <Card className="p-6 space-y-4">
        <h2 className="text-2xl font-bold text-primary">Interview Summary</h2>
        <div className="space-y-2">
          <p><span className="font-semibold">Role:</span> {interviewData.role}</p>
          <p><span className="font-semibold">Company:</span> {interviewData.company}</p>
          <p><span className="font-semibold">Type:</span> {interviewData.type}</p>
          <p><span className="font-semibold">Duration:</span> {interviewData.duration} minutes</p>
        </div>
      </Card>

      <Card className="p-6 space-y-4">
        <h2 className="text-2xl font-bold text-primary">Preparation Tips</h2>
        <ul className="list-disc list-inside space-y-2">
          <li>Find a quiet space without distractions</li>
          <li>Test your microphone and camera</li>
          <li>Have a glass of water nearby</li>
          <li>Keep your resume handy for reference</li>
          <li>Take deep breaths and stay calm</li>
        </ul>
      </Card>

      <Button 
        className="w-full"
        onClick={() => navigate("/interview", { state: interviewData })}
      >
        Begin Interview
      </Button>
    </div>
  );
};