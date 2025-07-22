import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";

const Index = () => {
  const navigate = useNavigate();

  return (
    <div className="max-w-4xl mx-auto text-center space-y-8 animate-fade-in">
      <h1 className="text-5xl font-bold text-primary">
        Master Your Interviews with AI
      </h1>
      
      <p className="text-xl text-gray-600">
        Practice makes perfect. Get ready for your dream job with our AI-powered interview simulator.
      </p>

      <div className="grid gap-4 md:grid-cols-3 py-8">
        <div className="p-6 bg-white rounded-lg shadow-md">
          <h3 className="text-lg font-semibold text-primary mb-2">Personalized Practice</h3>
          <p className="text-gray-600">Tailored interviews based on your target role and company</p>
        </div>
        <div className="p-6 bg-white rounded-lg shadow-md">
          <h3 className="text-lg font-semibold text-primary mb-2">Real-time Feedback</h3>
          <p className="text-gray-600">Get instant insights on your performance</p>
        </div>
        <div className="p-6 bg-white rounded-lg shadow-md">
          <h3 className="text-lg font-semibold text-primary mb-2">Multiple Formats</h3>
          <p className="text-gray-600">Practice technical, behavioral, and product interviews</p>
        </div>
      </div>

      <Button 
        size="lg"
        onClick={() => navigate("/setup")}
        className="animate-slide-up"
      >
        Start Your Practice Interview
      </Button>
    </div>
  );
};

export default Index;